import { Container } from "aurelia-framework";
import { Store } from "aurelia-store";
import cloneDeep from "clone-deep";

import { IAppState } from "config/state/abstractions";

export function setCurrentHistoryId(state: IAppState, currentHistoryId: number)
  : IAppState {
  const newState: IAppState = cloneDeep(state);

  newState.import.currentHistoryId = currentHistoryId;

  return newState;
}

const store: Store<IAppState> = Container.instance.get<Store<IAppState>>(Store);

const actions = {
  "import:setCurrentHistoryId": setCurrentHistoryId
};

for (let action in actions) {
  store.registerAction(action, actions[action]);
}
