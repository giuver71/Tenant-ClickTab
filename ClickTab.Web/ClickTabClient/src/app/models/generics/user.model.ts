import { UserRoleDTO } from "./userRole.model";

/**
 * Modello per memorizzare i dati dell'utente
 */
export class UserDTO {
  ID: number;
  Name: string;
  Surname: string;
  Email: string;
  Password: string;
  ChangedPassword: boolean;
  SubscriptionDate: Date;
  Status:UserStatusEnum;
  UserRoles:Array<UserRoleDTO>;
}

export enum SystemRoleEnum {
  ADMIN = 1,
  USER = 2
}

export enum UserStatusEnum
{
    Abilitato=1,
    Disabilitato=2
}


