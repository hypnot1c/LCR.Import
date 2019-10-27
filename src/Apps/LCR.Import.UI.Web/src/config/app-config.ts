import { Environment } from "./environment";

const appHost = "http://localhost:3000";

const appConfig = {
    Environment: Environment.DEV,
    AppHost: appHost,

    ApiHost: "https://localhost:11111"
};

export default appConfig;
