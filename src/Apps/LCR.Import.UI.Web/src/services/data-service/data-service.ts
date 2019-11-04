import { autoinject } from "aurelia-framework";
import { HttpClient as FetchClient} from "aurelia-fetch-client";
import { HttpClient } from "aurelia-http-client";

import { BaseObject } from "shared/abstractions";
import apiUrls from "./api-urls";

@autoinject
export class DataService extends BaseObject {
    constructor(
        private fetchClient: FetchClient,
        private httpClient: HttpClient
    ) {
        super("DataService");
    }

    file = {
        upload: async (files: FormData, progressCallback: Function)
            : Promise<any> => {
            try {
                const resp = await this.httpClient.createRequest(apiUrls.file.upload)
                    .asPost()
                    .withContent(files)
                    .withProgressCallback(progressCallback)
                    .send();

                return JSON.parse(resp.response);
            }
            catch (e) {
                this.Logger.error("Error uploading file", e);
            }
        }
    };

    import = {
        checkStatus: async (historyId: number, userId: number) => {
            const resp = await this.fetchClient.fetch(apiUrls.import.statusCheck(historyId, userId));
            return resp.json();
        },
        getResult: async (historyId: number, userId: number, pagination: any) => {
            const resp = await this.fetchClient.fetch(apiUrls.import.result(historyId, userId, pagination));
            return resp.json();
        },
        getHistory: async (pagination: any) => {
            const resp = await this.fetchClient.fetch(apiUrls.import.history(pagination));
            return resp.json();
        }
    };

    switch = {
        getList: async () => {
            var res = await this.fetchClient.fetch(apiUrls.switch.list);
            return res.json();
        }
    }

}
