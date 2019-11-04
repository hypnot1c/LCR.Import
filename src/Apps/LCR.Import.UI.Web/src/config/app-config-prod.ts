import { Environment } from "./environment";

const appHost = "http://localhost:8082";

const appConfig = {
    Environment: Environment.PROD,
    AppHost: appHost,

    ApiHost: "http://localhost:8083"
};

export default appConfig;
