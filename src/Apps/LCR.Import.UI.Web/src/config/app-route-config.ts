import { PLATFORM } from "aurelia-pal";
import { RouteConfig } from "aurelia-router";

let config: RouteConfig[] = [
    {
        route: [""],
        name: "root",
        moduleId: PLATFORM.moduleName("pages/main"),
        nav: false,
        title: "Welcome"
    },
    {
        route: "proccess-file",
        name: "proccess-file",
        moduleId: PLATFORM.moduleName("pages/proccess-file-page"),
        nav: false,
        title: "Proccess"
    }
];

export default { routes: config };
export { config as routes };
