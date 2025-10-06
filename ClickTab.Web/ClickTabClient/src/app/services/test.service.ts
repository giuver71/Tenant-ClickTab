import { environment } from './../../environments/environment';
import { Router } from '@angular/router';
import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Injectable({
    providedIn: 'root'
})

export class TestService {

    constructor(
        private http: HttpClient,
        private router: Router) {

    }

    exampleApi(): Promise<any> {
        return this.http.get<any>(environment.apiFullUrl + '/test').toPromise();
    }

}
