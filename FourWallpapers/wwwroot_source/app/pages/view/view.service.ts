import { Injectable } from '@angular/core';
import { Http, Response } from '@angular/http';
import { Observable } from 'rxjs/Observable';
import 'rxjs/add/operator/catch';
import 'rxjs/add/operator/map';

import { Image, UpdateRequest } from '@app/core';

@Injectable()
export class ViewService {

    private baseUrl = '/api/image/';

    constructor(private http: Http) {}

    get(imageId: string) {
        // assemble url
        const url = this.baseUrl + imageId;
        // send get request
        return this.http.get(url)
            .map(response => response.json() as Image)
            .catch(this.handleError);
    }

    report(imageId: string) {
        // assemble url
        const url = this.getUpdateUrl(imageId);

        // assemble request body
        const requestBody = new UpdateRequest('Report', 'Reported');

        // send post request
        return this.http.post(url, requestBody)
            .map(response => response.json() as Image)
            .catch(this.handleError);
    }

    updateKeywords(imageId: string, keywords: string) {
        // assemble url
        const url = this.getUpdateUrl(imageId);

        // assemble request body
        const requestBody = new UpdateRequest('Keywords', keywords);

        // send post request
        return this.http.post(url, requestBody)
            .map(response => response.json())
            .catch(this.handleError);
    }

    updateClass(imageId: string, classification: string) {
        // setup the url
        const url = this.getUpdateUrl(imageId);

        // assemble the request object
        const requestBody = new UpdateRequest('Class', classification);

        // send post request
        return this.http.post(url, requestBody)
            .map(response => response.json())
            .catch(this.handleError);
    }

    private getUpdateUrl(imageId: string) {

        return this.baseUrl + imageId + '/update';
    }

    private handleError(error: Response) {
        console.error(error);
        return Observable.throw(error.json().error || 'Server Error');
    }
}
