import { LookupDTO } from "../lookup.model";
import { RoleDTO } from "./role.model";
// import { RoleDimensionEnum } from "../enums/roleDimensioEnum";

export class UserRoleDTO {
  public ID: number;
  public FK_User: number;
  public FK_Role: number;
  public Role: RoleDTO;
  public FK_InsertUser: number;
  public InsertDate: Date;
  public FK_UpdateUser: number;
  public UpdateDate: Date;
  public Deleted: boolean;
  public FK_DeletedUser: number;
  public DeletedDate: Date;
  public IsChecked:boolean=false;

}

export class vUserRole 
{
  public ID: number ;
  public Name: string ;
  // public RoleDimension: RoleDimensionEnum ; 
}
