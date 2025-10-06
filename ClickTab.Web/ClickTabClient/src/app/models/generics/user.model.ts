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
}

export enum SystemRole {
  ADMIN = 1,
  USER = 2
}


