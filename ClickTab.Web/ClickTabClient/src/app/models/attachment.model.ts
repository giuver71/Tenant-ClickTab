import { AttachmentType, IAttachmentDTO } from "@eqproject/eqp-attachments";

export class AttachmentDTO implements IAttachmentDTO {
    ID: number;
    FileName?: string;
    FileContentType?: string;
    FileExtension?: string;
    FilePath?: string;
    AttachmentType?: AttachmentType;
    FileDataBase64?: string;
    IsImage?: boolean;
}