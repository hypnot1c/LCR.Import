const path = require("path");

const {
  ProvidePlugin,
  SourceMapDevToolPlugin,
  DefinePlugin,
  NormalModuleReplacementPlugin,
  WatchIgnorePlugin
} = require("webpack");

const HtmlWebpackPlugin = require("html-webpack-plugin");
const SpriteLoaderPlugin = require("svg-sprite-loader/lib/plugin");
const TerserPlugin = require("terser-webpack-plugin");
const MomentLocalesPlugin = require("moment-locales-webpack-plugin");
const BrowserSyncPlugin = require("browser-sync-webpack-plugin");
const MiniCssExtractPlugin = require("mini-css-extract-plugin");
const { AureliaPlugin } = require("aurelia-webpack-plugin");
const { TsConfigPathsPlugin } = require("awesome-typescript-loader");
const BundleAnalyzerPlugin = require("webpack-bundle-analyzer")
  .BundleAnalyzerPlugin;

// primary config:
const outDir = path.resolve(__dirname, "wwwroot/dist");
const srcDir = path.resolve(__dirname, "src");
const nodeModulesDir = path.resolve(__dirname, "node_modules");

/**
 * @return {webpack.Configuration}
 */
module.exports = (env, argv) => {
  const isProd = argv.mode === "production";
  const isTest = process.env.NODE_ENV === "testing";
  const baseHref = isProd ? "/lcr-ui" : "/";
  const cssRules = [
    { loader: "css-loader" },
    {
      loader: "sass-loader",
      options: {
        sassOptions: {
          includePaths: [path.resolve(nodeModulesDir, "normalize-scss/sass")],
          sourceMap: !isProd && !isTest
        }
      }
    }
  ];

  let config = {
    mode: argv.mode || "development",

    resolve: {
      modules: [srcDir, "node_modules"],
      extensions: [".ts", ".js"],
      plugins: [new TsConfigPathsPlugin()]
    },

    entry: {
      app: ["aurelia-bootstrapper"]
    },

    output: {
      path: outDir,
      publicPath: "dist/",
      filename: isProd || isTest ? "[name].[chunkhash].js" : "[name].js",
      chunkFilename: isProd || isTest ? "[name].[chunkhash].js" : "[name].js"
    },
    devServer: {
      contentBase: outDir,
      // serve index.html for all 404 (required for push-state)
      historyApiFallback: true
    },
    module: {
      rules: [
        // CSS required in JS/TS files should use the style-loader that auto-injects it into the website
        // only when the issuer is a .js/.ts file, so the loaders are not applied inside html templates
        {
          test: /\.css$/i,
          issuer: [{ not: [{ test: /\.html$/i }] }],
          use: [
            {
              loader: MiniCssExtractPlugin.loader
            },
            "css-loader"
          ]
        },
        {
          test: /\.css$/i,
          issuer: [
            {
              test: /\.html$/i
            }
          ],
          // CSS required in templates cannot be extracted safely
          // because Aurelia would try to require it again in runtime
          use: cssRules
        },
        {
          test: /\.scss$/,
          issuer: /\.[tj]s$/i,
          use: [
            {
              loader: MiniCssExtractPlugin.loader
            },
            ...cssRules
          ]
        },
        {
          test: /\.scss$/,
          issuer: /\.html?$/i,
          use: cssRules
        },
        {
          test: /\.svg$/,
          loader: "svg-sprite-loader",
          options: {
            extract: true,
            spriteFilename: "icons.svg"
          }
        },
        {
          test: /\.html$/i,
          loader: "html-loader"
        },
        {
          test: /\.ts$/i,
          loader: "awesome-typescript-loader",
          exclude: nodeModulesDir
        },
        /*{ if not comment can't load local json file. Since webpack >= v2.0.0, importing of JSON files will work by default.
          test: /\.json$/i,
          loader: 'json-loader'
        },*/
        // use Bluebird as the global Promise implementation:
        {
          test: /[\/\\]node_modules[\/\\]bluebird[\/\\].+\.js$/,
          loader: "expose-loader?Promise"
        },
        // embed small images and fonts as Data Urls and larger ones as files:
        {
          test: /\.(png|gif|jpg|cur)$/i,
          loader: "url-loader",
          options: {
            limit: 8192
          }
        },
        // load these fonts normally, as files:
        {
          test: /\.(ttf|eot)(\?v=\d+\.\d+\.\d+)?$/,
          use: [
            {
              loader: "file-loader",
              options: {
                name: "[name].[ext]",
                outputPath: "fonts/"
              }
            }
          ]
        }
      ],
      noParse: [/oidc-client/]
    },
    plugins: [
      new WatchIgnorePlugin([/scss\.d\.ts$/, "/node_modules/"]),
      new NormalModuleReplacementPlugin(/(.*)app-config(\.*)/, resource => {
        let cfgFileName = "app-config";
        if (isTest) {
          cfgFileName = `${cfgFileName}-test`;
        } else if (isProd) {
          cfgFileName = `${cfgFileName}-prod`;
        }
        resource.request = resource.request.replace(/app-config/, cfgFileName);
      }),
      new MomentLocalesPlugin({
        localesToKeep: ["ru"]
      }),
      new MiniCssExtractPlugin({
        filename: "[id].css",
        allChunks: true
      }),
      new HtmlWebpackPlugin({
        inject: false,
        template: "Views/Templates/Home/Index.template.cshtml",
        filename: "../../Views/Home/Index.cshtml",
        metadata: { baseHref: baseHref },
        chunksSortMode: 'none' //Fix toposort cyclic dependency error
      }),
      new ProvidePlugin({
        Promise: "bluebird"
      }),
      new SpriteLoaderPlugin(),
      new AureliaPlugin({
        aureliaApp: "main"
      })
    ],
    optimization: {
      splitChunks: {
        chunks: "all",
        cacheGroups: {
          default: false,
          framework_plugins: {
            test(module, chunks) {
              let result = false;
              if (!!module.context) {
                var regexps = [
                  /[\\/]node_modules[\\/]aurelia-validation/,
                  /[\\/]node_modules[\\/]aurelia-fetch-client/,
                  /[\\/]node_modules[\\/]aurelia-store/
                ];
                result = regexps.some(reg => reg.test(module.context));
              }
              return result;
            },
            name: "framework-plugins",
            priority: 99,
            enforce: true
          },
          framework: {
            test: /[\\/]node_modules[\\/]aurelia-/,
            name: "framework",
            priority: 90,
            enforce: true
          },
          shared: {
            test: /[\\/]shared[\\/]/,
            name: "shared",
            priority: 10,
            enforce: true
          },
          vendors: {
            test: /[\\/]node_modules[\\/]/,
            name: "vendors",
            priority: 9,
            enforce: true
          },
          components: {
            test: /[\\/]v3[\\/]components[\\/]/,
            name: "components",
            priority: 8,
            enforce: true
          },
        }
      }
    }
  };

  if (isProd || isTest) {
    config.plugins = config.plugins.concat([
      new DefinePlugin({
        "process.env.NODE_ENV": JSON.stringify("production")
      })
    ]);
    config.optimization.minimizer = [
      new TerserPlugin({
        parallel: true
      })
    ];
  }
  else {
    config.plugins.push(
      new SourceMapDevToolPlugin({
        // Remove this line if you prefer inline source maps
        filename: "[file].map",
        // Point sourcemap entries to the original file locations on disk
        moduleFilenameTemplate: path.relative(outDir, "[resourcePath]")
      }),
      //new BundleAnalyzerPlugin()
      new BrowserSyncPlugin({
        host: "localhost",
        port: 3000,
        proxy: "http://localhost:52053/",
        open: false
      })
    );
  }
  return config;
};
