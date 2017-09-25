import { Injectable } from '@angular/core';
import { Http, Response } from '@angular/http';
import {Observable} from 'rxjs/Observable';
import 'rxjs/add/operator/catch';
import 'rxjs/add/operator/map';

import { SearchRequest, SearchResult } from '@app/core';

@Injectable()
export class SearchService {

    private baseUrl = '/api/search/';

    constructor(private http: Http) {}

    get(request: SearchRequest) {
        // send get request
        return this.http.post(this.baseUrl, request)
            .map(response => response.json() as SearchResult[])
            .catch(this.handleError);
    }

    random() {
        // send get request
        return this.http.get(this.baseUrl + 'random')
            .map(response => response.json() as SearchResult[])
            .catch(this.handleError);
    }

    private handleError(error: Response) {
        console.error(error);
        return Observable.throw(error.json().error || 'Server Error');
    }
}
