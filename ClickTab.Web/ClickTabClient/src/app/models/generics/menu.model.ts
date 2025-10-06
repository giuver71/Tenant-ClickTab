export class MenuDTO {
  ID: number;
  Label: string;
  ClientCode: string;
  ParentClientCode: string;
  Url: string;
  Icon: string;
  Name: string;
  Order: string;
  isExternalPage: boolean;
  children: Array<MenuDTO>;
}

export class MenuConfig {
  public side: MenuSide;
  public rows: Array<MenuDTO>;
  public status: MenuStatus;
  public platform: string;
  public mode: MenuMode;
}

export class MenuRow {

}

export enum MenuSide {
  LEFT = 1,
  RIGHT = 2
}

export enum MenuStatus {
  OPENED = 1,
  CLOSED = 2
}

export enum MenuMode {
  NORMAL = 1,
  SHRINKED = 2
}


export enum ExternalActionEnum {
  ROUTE = 0,
  NEWPAGE = 1
}
