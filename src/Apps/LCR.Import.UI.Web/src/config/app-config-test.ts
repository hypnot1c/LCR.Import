import { Environment } from "./environment";

const appHost = "http://10.31.65.27/lcr-import";

const appConfig = {
  Environment: Environment.TEST,
  AppHost: appHost,

  root: "/lcr-import",

  ApiHost: "http://10.31.65.27/lcr-import-api"
};

export default appConfig;
