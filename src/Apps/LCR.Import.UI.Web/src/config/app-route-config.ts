import { PLATFORM } from "aurelia-pal";
import { RouteConfig } from "aurelia-router";

let config: RouteConfig[] = [
    {
        route: [""],
        name: "root",
        moduleId: PLATFORM.moduleName("pages/main"),
        nav: false,
        title: "Welcome"
    }
];

export default { routes: config };
export { config as routes };
