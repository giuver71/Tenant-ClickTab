import { EventEmitter, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { TranslateService } from '@ngx-translate/core';
import { AuthService } from '../auth.service';
import { MenuDTO } from '../../models/generics/menu.model';
import { environment } from '../../../environments/environment';
import { SystemRole } from '../../models/generics/user.model';
import { INavData } from '@eqproject/eqp-ui';
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

  getMenuByRole(roleId: number): Promise<Array<any>> {
      return this.http.get<Array<any>>(environment.apiFullUrl + '/Menu/GetMenuByRole/' + roleId).toPromise();
  }



}
