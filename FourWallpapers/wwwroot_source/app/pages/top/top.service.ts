import { Injectable } from '@angular/core';
import { Http, Response } from '@angular/http';
import { Observable } from 'rxjs/Observable';
import 'rxjs/add/operator/catch';
import 'rxjs/add/operator/map';

import { SearchResult } from '@app/core';

@Injectable()
export class TopService {
    private baseUrl = '/api/top/';

    constructor(private http: Http) {}

    get(by?: string, value?: string) {
        // assemble url
        let url = this.baseUrl;

        if (by != null) {
            url += by + '/' + value;
        }

        // send get request
        return this.http.get(url)
            .map(response => response.json() as SearchResult[])
            .catch(this.handleError);
    }

    private handleError(error: Response) {
        console.error(error);
        return Observable.throw(error.json().error || 'Server Error');
    }
}
