export class NotificationDTO {
    ID: number;
    Message: string;
    AdditionalParams: string;
    Title: string;
    CreationDate: Date;

    // Proprietà client per la visualizzazione della notifica con i placeholder
    CompleteMessage: string;
}