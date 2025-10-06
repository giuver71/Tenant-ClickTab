// import { RoleDimensionEnum } from "../enums/roleDimensioEnum";
import { MenuDTO } from "./menu.model";
import { RoleRuleDTO } from "./rolerule.model";

export class RoleDTO {
  public ID: number;
  public FK_Facilitie: number;
  public Description: string;
  public Menu: Array<MenuDTO>
  public RoleRules: Array<RoleRuleDTO>
  public IdsDeleted:Array<number>;
  // public RoleDimension: RoleDimensionEnum;
  public FK_InsertUser: number;
  public InsertDate: Date;
  public FK_UpdateUser: number;
  public UpdateDate: Date;
  public Deleted: boolean;
  public FK_DeletedUser: number;
  public DeletedDate: Date;
}

export enum RoleTypeEnum
    {
        ADMIN = 1,
        DOCTOR = 2,
        PATIENT = 3,
        TUTOR = 4
    }