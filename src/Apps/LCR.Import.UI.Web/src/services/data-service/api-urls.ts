import { IApiUrls } from "./abstractions";

const apiUrls: IApiUrls = {
    file: {
        upload: "fileupload"
    },
    import: {
        statusCheck: (historyId: number, userId: number) => `import/${historyId}/user/${userId}/status`,
        result: (historyId: number, userId: number, queryParams: { page: number, pageSize: number }) => {
            let url = `import/${historyId}/user/${userId}/result`;
            const map = [];
            if (queryParams != null) {
                for (let i in queryParams) {
                    map.push(`${i}=${queryParams[i]}`);
                }
                url = `${url}?${map.join('&')}`;
            }
            return url;
        },
        history: (queryParams: { page: number, pageSize: number }) => {
            let url = "import/history";
            const map = [];
            if (queryParams != null) {
                for (let i in queryParams) {
                    map.push(`${i}=${queryParams[i]}`);
                }
                url = `${url}?${map.join('&')}`;
            }
            return url;
        }
    },
    switch: {
        list: "switch"
    }
}

export default apiUrls;
