import { Environment } from "./environment";

const appHost = "http://localhost:3000";

const appConfig = {
  Environment: Environment.DEV,
  AppHost: appHost,

  root: "",

  ApiHost: "http://localhost:52414"
};

export default appConfig;
