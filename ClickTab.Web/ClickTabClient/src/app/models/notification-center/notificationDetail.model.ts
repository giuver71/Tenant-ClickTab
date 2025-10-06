import { NotificationDTO } from "./notification.model";

export class NotificationDetailDTO {
    ID: number;
    FK_Notification: number;
    Notification: NotificationDTO;
    SendDate: Date;
    ReadDate: Date;
    IsRead: boolean;
    FK_User: number;
    ReceiverEmail: string;
}