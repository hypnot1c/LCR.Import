export interface IAppState {
  userId: number;

  import: IImportState;
}

export interface IImportState {
  currentHistoryId: number;
}
