import { autoinject } from "aurelia-framework";
import { HttpClient } from "aurelia-http-client";

import { BaseObject } from "shared/abstractions";
import apiUrls from "./api-urls";

@autoinject
export class DataService extends BaseObject {
    constructor(
        private httpClient: HttpClient
    ) {
        super("DataService");
    }
    file = {
        upload: async (id: number, files: FormData, progressCallback: Function)
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
    }
}
