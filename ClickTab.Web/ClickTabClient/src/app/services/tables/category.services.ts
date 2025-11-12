import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { AuthService } from "../auth.service";
import { RoleDTO } from "../../models/generics/role.model";
import { environment } from "../../../environments/environment";
import { CategoryDTO } from "../../models/tables/category.model";

@Injectable({ providedIn: 'root' })
export class CategoryService{

    constructor(
        private http: HttpClient,
        private authService: AuthService,
    ) {
    }

    getAll():Promise<Array<CategoryDTO>>{
         return this.http.get<Array<CategoryDTO>>(environment.apiFullUrl + '/Category/GetAll/').toPromise();
    }

   get(id: number): Promise<CategoryDTO> {
       return this.http.get<CategoryDTO>(environment.apiFullUrl + "/Category" + id).toPromise();
    }

    save(role: CategoryDTO): Promise<any> {
        return this.http.post<any>(environment.apiFullUrl + "/Category", role).toPromise();
      }

}
