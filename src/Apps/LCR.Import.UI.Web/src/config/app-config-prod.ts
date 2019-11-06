import { Environment } from "./environment";

const appHost = "http://10.31.65.27:8082";

const appConfig = {
    Environment: Environment.PROD,
    AppHost: appHost,

    ApiHost: "http://10.31.65.27:8083"
};

export default appConfig;
