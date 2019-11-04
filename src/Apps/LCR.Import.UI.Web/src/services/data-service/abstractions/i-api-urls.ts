export interface IApiUrls {
    file: {
        upload: string
    },
    import: {
        statusCheck: (historyId: number, userId: number) => string
        result: (historyId: number, userId: number, queryParams: any) => string
        history: (queryParams: any) => string
    },
    switch: {
        list: string
    }
}
