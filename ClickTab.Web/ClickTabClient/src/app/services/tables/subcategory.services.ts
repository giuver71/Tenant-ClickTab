import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { AuthService } from "../auth.service";
import { environment } from "../../../environments/environment";
import { CategoryDTO } from "../../models/tables/category.model";
import { SubCategoryDTO } from "../../models/tables/subcategory.model";

@Injectable({ providedIn: 'root' })
export class SubCategoryService{

    constructor(
        private http: HttpClient,
        private authService: AuthService,
    ) {
    }

    getAll():Promise<Array<SubCategoryDTO>>{
         return this.http.get<Array<SubCategoryDTO>>(environment.apiFullUrl + '/SubCategory/GetAll/').toPromise();
    }

   get(id: number): Promise<SubCategoryDTO> {
       return this.http.get<SubCategoryDTO>(environment.apiFullUrl + "/SubCategory/" + id).toPromise();
    }

    save(sub: SubCategoryDTO): Promise<any> {
        return this.http.post<any>(environment.apiFullUrl + "/SubCategory", sub).toPromise();
      }

}
