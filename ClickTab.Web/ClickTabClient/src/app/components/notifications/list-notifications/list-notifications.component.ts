import { Component, OnInit, TemplateRef, ViewChild } from '@angular/core';
import { MatDialog, MatDialogRef } from '@angular/material/dialog';
import { EqpTableComponent } from '@eqproject/eqp-table';
import { ConfigColumn, TypeColumn } from '@eqproject/eqp-common';
import { Subscription } from 'rxjs';
import { NotificationDetailDTO } from '../../../models/notification-center/notificationDetail.model';
import { DialogService } from '../../../services/dialog.service';
import { EventHandlerService } from '../../../services/eventHandler.service';
import { NotificationService } from '../../../services/notification.service';

@Component({
  selector: 'app-list-notifications',
  templateUrl: './list-notifications.component.html',
  styleUrls: ['./list-notifications.component.scss']
})
export class ListNotificationsComponent implements OnInit {


  notificationList: Array<NotificationDetailDTO> = new Array<NotificationDetailDTO>();
  columns: Array<ConfigColumn>;
  @ViewChild('table', { static: false }) table: EqpTableComponent;

  // Dialog per la visualizzazione di una notifica
  dialogViewNotificationRef: MatDialogRef<TemplateRef<any>>;
  @ViewChild('dialogViewNotification', { static: false }) dialogViewNotification: TemplateRef<any>;
  selectedNotification: NotificationDetailDTO;

  notificationUpdateSubscription: Subscription;
  notificationReadSubscription: Subscription;

  constructor(
    private notificationService: NotificationService,
    private dialog: MatDialog,
    private eventHandlerService: EventHandlerService
  ) {
  }

  /**
   * Qui viene eseguito il subcribe agli eventi "NotificationUpdateEvent" (recupera l'elenco delle notifiche e aggiorna la tabella)
   * e "NotificationReadEvent" (segna come letta la notifica relativa all'ID ricevuto senza dover ricaricare i dati)
   */
  ngOnInit(): void {
    this.configureColumns();
    this.loadNotification();
    this.notificationUpdateSubscription = this.eventHandlerService.subscribe(
      'NotificationUpdateEvent',
      (response) => { this.loadNotification(); }
    );
    this.notificationReadSubscription = this.eventHandlerService.subscribe(
      'NotificationReadEvent',
      (response) => {
        if (this.notificationList.find(n => n.ID == response.content.ID)) {
          let readNotification = this.notificationList.find(n => n.ID == response.content.ID);
          if (readNotification != null) {
            readNotification.ReadDate = response.content.ReadDate;
            readNotification.IsRead = true;
          }
        }
      }
    );
  }

  ngOnDestroy(): void {
    if (this.notificationUpdateSubscription)
      this.notificationUpdateSubscription.unsubscribe();
    if (this.notificationReadSubscription)
      this.notificationReadSubscription.unsubscribe();
  }

  configureColumns() {
    this.columns = [
      { key: "IsRead", display: "", type: TypeColumn.Boolean, booleanValues: { true: '<i class="fa fa-circle success-color"></i>', false: '<i class="fa fa-circle notification-icon"></i>' }, styles: { flex: "0 0 5%" } },
      {
        key: "action", display: "",
        type: TypeColumn.SimpleAction,
        actions: [
          { name: '', icon: "visibility", fn: (element, col, elementIndex) => this.readNotification(element) },
        ],
        styles: { flex: "0 0 5%" }
      },
      { key: "Notification.Title", display: "Titolo" },
      { key: "SendDate", display: "Data invio", type: TypeColumn.Date, styles: { flex: "0 0 20%" }, format: 'dd/MM/yyyy' }
    ];
  }

  loadNotification() {
    this.notificationService.getNotifications(false)
      .then(res => {
        this.notificationList = this.notificationService.replaceNotificationsPlaceholders(res);
        if (this.table)
          this.table.reloadDatatable();
      })
      .catch((err) => { DialogService.Error(err.message); });
  }

  readNotification(notification: NotificationDetailDTO) {
    if (!notification.ReadDate)
      this.notificationService.markAsRead(notification.ID)
        .then((res) => {
          notification.ReadDate = res;
          this.eventHandlerService.broadcast({ name: 'NotificationReadEvent', content: { ReadDate: res, ID: notification.ID } });
        })
        .catch((err) => { DialogService.Error(err.message); });
    this.selectedNotification = notification;
    this.dialogViewNotificationRef = this.dialog.open(this.dialogViewNotification, {
      disableClose: false,
      hasBackdrop: true,
      autoFocus: false,
      width: '40%',
    });
  }

  allNotifiesAsRead() {
    let notificationsToSend: Array<NotificationDetailDTO> = this.notificationList.filter(n => n.ReadDate == null);
    this.notificationService.markListAsRead(notificationsToSend.map(n => n.ID)).then(res => {
      notificationsToSend.forEach(notification => {
        this.eventHandlerService.broadcast({ name: 'NotificationReadEvent', content: { ReadDate: res, ID: notification.ID } });
      });
    }).catch(error => { DialogService.Error(error.message); });
  }
}
