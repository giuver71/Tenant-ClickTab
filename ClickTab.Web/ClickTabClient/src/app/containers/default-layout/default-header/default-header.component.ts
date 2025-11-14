import { ClassToggleService } from '@eqproject/eqp-ui';
import { HeaderComponent } from '@eqproject/eqp-ui';
import { ActivatedRoute, Router } from '@angular/router';
import { NotificationDetailDTO } from './../../../models/notification-center/notificationDetail.model';
import { UserDTO } from './../../../models/generics/user.model';
import { Component, Input, Output, EventEmitter, OnInit, ViewChild, ElementRef } from '@angular/core';
import { RoleService } from '../../../services/generics/role.services';
import { RoleDTO } from '../../../models/generics/role.model';
import { DialogService } from '../../../services/dialog.service';
import { UserService } from '../../../services/user.service';
import { RoleRuleDTO } from '../../../models/generics/rolerule.model';
import { AuthService } from '../../../services/auth.service';
import { MatSelect } from '@angular/material/select';

@Component({
  selector: 'app-default-header',
  templateUrl: './default-header.component.html',
  styleUrls: ['./default-header.component.scss']
})
export class DefaultHeaderComponent extends HeaderComponent implements OnInit {
  @Input() manageableRoles: Array<RoleDTO> = new Array<RoleDTO>();
  @Input() hashedRoles: Array<string> = new Array<string>();
  @Input() sidebarId: string = "sidebar";
  @Input() currentUser: UserDTO;
  @Input() selectedNotification: NotificationDetailDTO;
  @Input() notificationList: Array<NotificationDetailDTO> = new Array<NotificationDetailDTO>();
  @Input() notificationCount: number;
  @Output() readNotificationEvent: EventEmitter<any> = new EventEmitter<any>();
  @Output() goToNotificationListEvent: EventEmitter<any> = new EventEmitter<any>();
  @Output() selectedRoleEvent: EventEmitter<RoleDTO> = new EventEmitter<RoleDTO>();

  selectedRole: RoleDTO = new RoleDTO();
  loaded: boolean = false;
  selectedRoleId!: number;

  collapsed: boolean = true; 

@ViewChild('roleSelect') roleSelect?: MatSelect;
@ViewChild('roleTrigger', { read: ElementRef }) roleTrigger?: ElementRef;

  constructor(
    private classToggler: ClassToggleService,
    private activatedRoute: ActivatedRoute,
    private router: Router,
    private userService: UserService,
    private roleService: RoleService,
    public authService: AuthService
  ) {
    super();
  }


  ngOnInit(): void {
    this.loaded = false;
    if (this.currentUser == null) {
      this.currentUser = this.authService.getCurrentUser();
    }
    this.selectedRoleId = this.authService.getCurrentRole().ID;
    this.selectedRole=this.authService.getCurrentRole();
    setTimeout(() => {
      this.loaded = true;
    }, 50);
  }

  // 👉 toggle del collasso header
  toggleHeader(): void {
    this.collapsed = !this.collapsed;
  }

  //#region Gestione notifiche
  readNotification(notification: NotificationDetailDTO) {
    this.readNotificationEvent.emit(notification);
  }

  goToNotificationList() {
    this.goToNotificationListEvent.emit();
  }
  //#endregion

  viewProfile() {
    this.router.navigate(['/profile'], { relativeTo: this.activatedRoute });
  }

  logout() {
    this.authService.logout();
  }

  changeRole(role: RoleDTO) {
    debugger;
    this.selectedRole = role;
    this.selectedRoleId = role.ID;
    this.selectedRoleEvent.emit(this.selectedRole);
  }
  compareRoles(r1: RoleDTO, r2: RoleDTO): boolean {
    return r1 && r2 ? r1.ID === r2.ID : r1 === r2;
  }
}
