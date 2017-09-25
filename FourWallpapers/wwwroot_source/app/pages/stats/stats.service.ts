import { Injectable } from '@angular/core';
import { Http, Response } from '@angular/http';
import { Observable } from 'rxjs/Observable';
import 'rxjs/add/operator/catch';
import 'rxjs/add/operator/map';

import { StatsResponse } from '@app/core';

@Injectable()
export class StatsService {
    private baseUrl = '/api/general/stats/';

    constructor(private http: Http) {}

    get() {
        // send get request
        return this.http.get(this.baseUrl)
            .map(response => response.json() as StatsResponse)
            .catch(this.handleError);
    }

    private handleError(error: Response) {
        console.error(error);
        return Observable.throw(error.json().error || 'Server Error');
    }
}
