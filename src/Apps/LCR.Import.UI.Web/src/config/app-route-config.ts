import { PLATFORM } from "aurelia-pal";
import { RouteConfig } from "aurelia-router";

let config: RouteConfig[] = [
    {
        route: ["", "page=:page?"],
        name: "root",
        moduleId: PLATFORM.moduleName("pages/main"),
        nav: false,
        title: "Welcome"
    },
    {
        route: "upload-file",
        name: "upload-file",
        moduleId: PLATFORM.moduleName("pages/upload-file-page"),
        nav: false,
        title: "Upload"
    },

    {
        route: "process-file",
        name: "process-file",
        moduleId: PLATFORM.moduleName("pages/process-file-page"),
        nav: false,
        title: "Proccess"
    }
];

export default { routes: config };
export { config as routes };
