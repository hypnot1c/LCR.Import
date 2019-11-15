import * as Bluebird from "bluebird";
import { Aurelia, LogManager, PLATFORM } from "aurelia-framework";
import { ConsoleAppender } from "aurelia-logging-console";
import moment from "moment";
import UIkit from 'uikit';
import Icons from 'uikit/dist/js/uikit-icons';
import "whatwg-fetch";

import { initialState } from "config/state/initial-state";

import appConfig from "config/app-config";
import { Environment } from "config/environment";

//Configure Bluebird Promises.
//Note: You may want to use environment-specific configuration.
Bluebird.config({ warnings: false });
UIkit.use(Icons);

export async function configure(aurelia: Aurelia) {
  const locale = (window.navigator as any).userLanguage || window.navigator.language;
  moment.locale(locale);
  let aureliaConfig = aurelia.use
    .standardConfiguration()
    .plugin(PLATFORM.moduleName("aurelia-validation"))
    .plugin(PLATFORM.moduleName("aurelia-store"), { initialState });

  if (appConfig.Environment < Environment.PROD) {
    aureliaConfig = aureliaConfig.developmentLogging();
  }
  if (appConfig.Environment > Environment.DEV) {
  }
  if (appConfig.Environment == Environment.PROD) {
    LogManager.addAppender(new ConsoleAppender());
    LogManager.setLevel(LogManager.logLevel.warn);
  }

  await aurelia.start();
  await aurelia.setRoot(PLATFORM.moduleName("app"));
}
