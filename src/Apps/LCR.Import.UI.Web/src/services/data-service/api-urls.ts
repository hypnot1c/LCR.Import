import { IApiUrls } from "./abstractions";

const apiUrls: IApiUrls = {
    file: {
        upload: "fileupload"
    },
    import: {
        statusCheck: (historyId: number, userId: number) => `import/${historyId}/user/${userId}/status`,
        result: (historyId: number, userId: number) => `import/${historyId}/user/${userId}/result`
    },
    switch: {
        list: "switch"
    }
}

export default apiUrls;
