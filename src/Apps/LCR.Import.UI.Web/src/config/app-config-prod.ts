import { Environment } from "./environment";

const appHost = "http://10.31.65.27";

const appConfig = {
    Environment: Environment.PROD,
    AppHost: appHost,

    ApiHost: "http://10.31.65.27/lcr-api"
};

export default appConfig;
