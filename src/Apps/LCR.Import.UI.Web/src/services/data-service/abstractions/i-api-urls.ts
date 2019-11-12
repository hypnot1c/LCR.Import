export interface IApiUrls {
  file: {
    upload: string
  },
  import: {
    statusCheck: (historyId: number, userId: number) => string
    result: (historyId: number, userId: number, queryParams: any) => string
    history: (queryParams: any) => string
    summary: (historyId: number) => string
    approveRow: (historyId: number, rowId: number) => string
    excludeRow: (historyId: number, rowId: number) => string
    isAllApproved: (historyId: number) => string
    lcrSave: (historyId: number) => string
    saveRow: (historyId: number, rowId: number) => string
  },
  operator: {
    list: string
  },
  switch: {
    list: string
  }
}
