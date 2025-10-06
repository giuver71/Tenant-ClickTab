import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../environments/environment';
import { NotificationDetailDTO } from '../models/notification-center/notificationDetail.model';
import moment from 'moment';

@Injectable({ providedIn: 'root' })

export class NotificationService {

    constructor(
        private http: HttpClient,
    ) { }

    getNotifications(onlyUnread: boolean): Promise<Array<NotificationDetailDTO>> {
        return this.http.get<Array<NotificationDetailDTO>>(environment.apiFullUrl + '/Notification/GetNotifications/' + onlyUnread).toPromise();
    }

    markAsRead(id: number): Promise<Date> {
        return this.http.get<Date>(environment.apiFullUrl + '/Notification/MarkAsRead/' + id).toPromise();
    }

    markListAsRead(IDs: number[]): Promise<any> {
        return this.http.post<any>(environment.apiFullUrl + '/Notification/MarkListAsRead', IDs).toPromise();
    }

    /**
     * Dato un array di notifiche traduce il messaggio e sostituisce i parametri.
     * Nel caso uno dei parametri è di tipo Date lo trasforma in stringa in base alla lingua con cui ci si è loggati.
     * @param notificationList
     * @returns
     */
    replaceNotificationsPlaceholders(notificationDetailList: Array<NotificationDetailDTO>): Array<NotificationDetailDTO> {
        notificationDetailList.forEach(nd => {
            nd.Notification.CompleteMessage = nd.Notification.Message;
            if (nd.Notification.AdditionalParams) {
                let params = JSON.parse(nd.Notification.AdditionalParams);
                let paramNames = Object.keys(params);
                paramNames.forEach(p => {
                    if ((moment(params[p], 'YYYY-MM-DDTHH:mm:ssZ', true).isValid() == true || moment(params[p], 'YYYY-MM-DDTHH:mm:ss', true).isValid() == true) && !this.isValidHttpUrl(params[p])) {
                        let localeMoment = moment().locale('it-IT');
                        nd.Notification.CompleteMessage = nd.Notification.CompleteMessage.replace("{" + p + "}", moment(params[p]).format(localeMoment["_locale"]["_longDateFormat"]["L"]));
                    } else {
                        nd.Notification.CompleteMessage = nd.Notification.CompleteMessage.replace("{" + p + "}", (params[p] ?? ""));
                    }
                });
            }
        });
        return notificationDetailList;
    }

    isValidHttpUrl(param: string) {
        let url;
        try { url = new URL(param); }
        catch (_) { return false; }
        return true;
    }
}
