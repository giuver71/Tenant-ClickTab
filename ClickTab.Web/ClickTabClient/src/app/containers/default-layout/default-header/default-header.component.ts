import { ClassToggleService } from '@eqproject/eqp-ui';
import { HeaderComponent } from '@eqproject/eqp-ui';
import { ActivatedRoute, Router } from '@angular/router';
import { NotificationDetailDTO } from './../../../models/notification-center/notificationDetail.model';
import { UserDTO } from './../../../models/generics/user.model';
import { Component, Input, Output, EventEmitter } from '@angular/core';
import { AuthService } from 'src/app/services/auth.service';

@Component({
  selector: 'app-default-header',
  templateUrl: './default-header.component.html',
  styleUrls: ['./default-header.component.scss']
})
export class DefaultHeaderComponent extends HeaderComponent {

  @Input() sidebarId: string = "sidebar";
  @Input() currentUser: UserDTO;
  @Input() selectedNotification: NotificationDetailDTO;
  @Input() notificationList: Array<NotificationDetailDTO> = new Array<NotificationDetailDTO>();
  @Input() notificationCount: number;
  @Output() readNotificationEvent: EventEmitter<any> = new EventEmitter<any>();
  @Output() goToNotificationListEvent: EventEmitter<any> = new EventEmitter<any>();

  constructor(private classToggler: ClassToggleService, private activatedRoute: ActivatedRoute, private router: Router, private authService: AuthService) {
    super();
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

}
