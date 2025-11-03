import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { AuthService } from "../auth.service";
import { RoleDTO } from "../../models/generics/role.model";
import { environment } from "../../../environments/environment";
import { RuleDTO } from "../../models/generics/rule.model";

@Injectable({ providedIn: 'root' })
export class RuleService{

    constructor(
        private http: HttpClient,
        private authService: AuthService,
    ) {
    }

    getAll(){
         return this.http.get<Array<RuleDTO>>(environment.apiFullUrl + '/Rule/GetAll/').toPromise();
    }



}
