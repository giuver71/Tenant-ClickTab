import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { AuthService } from "../auth.service";
import { RoleDTO } from "../../models/generics/role.model";
import { environment } from "../../../environments/environment";

@Injectable({ providedIn: 'root' })
export class RoleService{

    constructor(
        private http: HttpClient,
        private authService: AuthService,
    ) {
    }

    getAll(){
         return this.http.get<Array<RoleDTO>>(environment.apiFullUrl + '/Role/GetAll/').toPromise();
    }

}
