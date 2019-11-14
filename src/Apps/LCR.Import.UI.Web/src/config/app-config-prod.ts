import { Environment } from "./environment";

const appHost = "http://10.31.65.23/lcr-import";

const appConfig = {
  Environment: Environment.PROD,
  AppHost: appHost,

  root: "/lcr-import",

  ApiHost: "http://10.31.65.23/lcr-import-api"
};

export default appConfig;
