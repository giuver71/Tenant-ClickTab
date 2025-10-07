import { ClassToggleService } from '@eqproject/eqp-ui';
import { HeaderComponent } from '@eqproject/eqp-ui';
import { ActivatedRoute, Router } from '@angular/router';
import { NotificationDetailDTO } from './../../../models/notification-center/notificationDetail.model';
import { UserDTO } from './../../../models/generics/user.model';
import { Component, Input, Output, EventEmitter, OnInit } from '@angular/core';
import { AuthService } from 'src/app/services/auth.service';
import { RoleService } from '../../../services/generics/role.services';
import { RoleDTO } from '../../../models/generics/role.model';
import { DialogService } from '../../../services/dialog.service';
import { UserService } from '../../../services/user.service';
import { RoleRuleDTO } from '../../../models/generics/rolerule.model';

@Component({
  selector: 'app-default-header',
  templateUrl: './default-header.component.html',
  styleUrls: ['./default-header.component.scss']
})
export class DefaultHeaderComponent extends HeaderComponent implements OnInit {

  @Input() sidebarId: string = "sidebar";
  @Input() currentUser: UserDTO;
  @Input() selectedNotification: NotificationDetailDTO;
  @Input() notificationList: Array<NotificationDetailDTO> = new Array<NotificationDetailDTO>();
  @Input() notificationCount: number;
  @Output() readNotificationEvent: EventEmitter<any> = new EventEmitter<any>();
  @Output() goToNotificationListEvent: EventEmitter<any> = new EventEmitter<any>();

  manageableRoles:Array<RoleDTO>=new Array<RoleDTO>();
  currentRole:RoleDTO=new RoleDTO();
  hashedRoles: Array<string> = new Array<string>();
  constructor(
    private classToggler: ClassToggleService, 
    private activatedRoute: ActivatedRoute, 
    private router: Router, 
    private userService:UserService,
    private roleService:RoleService,
    private authService: AuthService) {
    super();
  }
  ngOnInit(): void {
    this.currentUser=this.authService.getCurrentUser();
   this.getRoles();
  }

  //#region Gestione emitter notifiche
  readNotification(notification: NotificationDetailDTO) {
    this.readNotificationEvent.emit(notification);
  }

  goToNotificationList() {
    this.goToNotificationListEvent.emit();
  }

  /**
   * Apre la sezione profilo utente
   */
  viewProfile() {
    this.router.navigate(['/profile'], { relativeTo: this.activatedRoute });
  }

  logout() {
    this.authService.logout();
  }
  //#endregion

   async getRoles(){
       await this.userService.getAllRolesUserFacilityHashed(this.currentUser.ID).then((hashedRoles) => {
      this.hashedRoles = hashedRoles;
      // Check del Ruolo
      var roleRules: Array<RoleRuleDTO> = new Array<RoleRuleDTO>();
      // res = res.filter(x => x.RoleDTO.FK_Facilitie == this.currentFacilitieId || x.RoleDTO.FK_Facilitie == null);
          this.hashedRoles.forEach((role) => {
            let decodedrole = this.authService.decodeToken(role);
            //todo tagliare l'oggetto user
            decodedrole.Role.RoleRules.forEach((roleRule) => {
              roleRules.push(roleRule);
            });
            this.manageableRoles.push(decodedrole.Role);
          });

          // this.currentRoleTypes = this.authService.getCurrentRoleTypes(roleRules);
          this.currentRole = this.authService.getCurrentRole();

          // se non trova il currentRole lo imposta
          if (this.currentRole == null && this.manageableRoles.length > 0) {
            this.currentRole = this.manageableRoles[0];
            this.authService.setCurrentRole(this.hashedRoles[0]);
          } else if (this.manageableRoles.length > 0) {
            let roleIndex = this.manageableRoles.findIndex((x) => x.ID == this.currentRole.ID);
            this.currentRole = this.manageableRoles[roleIndex];
            this.authService.setCurrentRole(this.hashedRoles[roleIndex]);
          }
      });   
  }


      changeRole(ev:RoleDTO){
        this.router.navigate(["/dashboard"]).then(() => {
        this.currentRole = ev;
        let roleiindex = this.manageableRoles.findIndex((x) => x.ID == ev.ID);
        this.authService.setCurrentRole(this.hashedRoles[roleiindex]);
        this.reloadComponent();
      });
      
    }

     reloadComponent() {
        let dashBoardUrl: string = "/dashboard";
        if (window.innerWidth >= 992) {
          this.router.routeReuseStrategy.shouldReuseRoute = () => false;
          this.router.onSameUrlNavigation = "reload";
          this.router.navigate([dashBoardUrl], { relativeTo: this.activatedRoute });
        } else {
          location.reload();
        }
  }

   


}
