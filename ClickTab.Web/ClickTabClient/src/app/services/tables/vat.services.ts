import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { AuthService } from "../auth.service";
import { RoleDTO } from "../../models/generics/role.model";
import { environment } from "../../../environments/environment";
import { VatDTO } from "../../models/tables/vat.model";

@Injectable({ providedIn: 'root' })
export class VatService{

    constructor(
        private http: HttpClient,
        private authService: AuthService,
    ) {
    }

    getAll():Promise<Array<VatDTO>>{
         return this.http.get<Array<VatDTO>>(environment.apiFullUrl + '/Vat/GetAll/').toPromise();
    }

   get(id: number): Promise<VatDTO> {
       return this.http.get<VatDTO>(environment.apiFullUrl + "/Vat/" + id).toPromise();
    }

    save(role: VatDTO): Promise<any> {
        return this.http.post<any>(environment.apiFullUrl + "/Vat", role).toPromise();
      }

}
