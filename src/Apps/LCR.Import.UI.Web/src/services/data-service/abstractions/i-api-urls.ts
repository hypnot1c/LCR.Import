export interface IApiUrls {
    file: {
        upload: string
    },
    import: {
        statusCheck: (historyId: number, userId: number) => string
    },
    switch: {
        list: string
    }
}
