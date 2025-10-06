import { EventEmitter, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { TranslateService } from '@ngx-translate/core';
import { AuthService } from '../auth.service';
import { MenuDTO } from '../../models/generics/menu.model';
import { environment } from '../../../environments/environment';
import { SystemRole } from '../../models/generics/user.model';
// import { RoleDimensionEnum } from '../models/enums/roleDimensioEnum';


@Injectable({ providedIn: 'root' })

/**
 * Servizio di autenticazione
 */
export class MenuService {

  onChangePermissions: EventEmitter<any> = new EventEmitter();

  constructor(
    private http: HttpClient,
    private translate: TranslateService,
    private authService: AuthService,
  ) {
  }

  getAll(): Promise<Array<MenuDTO>> {
    return this.http.get<Array<MenuDTO>>(environment.apiFullUrl + '/Menu/GetAll/').toPromise();
  }

  getMenuByRole(systemRole: SystemRole, roleId?: number): Promise<Array<MenuDTO>> {
    if (roleId != null) {
      return this.http.get<Array<MenuDTO>>(environment.apiFullUrl + '/Menu/GetMenuByRole/' + systemRole + '/' + roleId).toPromise();
    } else {
      return this.http.get<Array<MenuDTO>>(environment.apiFullUrl + '/Menu/GetMenuByRole/' + systemRole).toPromise();
    }
  }



}
