import { IAppState } from "./abstractions";

const initialState: IAppState = {
  userId: 1,
  import: {
    //currentHistoryId: 18
    currentHistoryId: undefined
  }
};

export { initialState };
