import { LookupDTO } from "../lookup.model";
import { RulesDescriptionEnum } from "./rule.model";

export class RoleRuleDTO {
  public ID: number;
  public FK_Rule: number;
  public FK_Role:number;
  public Rule?: LookupDTO;
  public RuleUrlRoutes: string;
  public RuleDescriptionEnum: RulesDescriptionEnum;
  public CanCreate: boolean = true;
  public CanEdit: boolean = true;
  public CanDelete: boolean = true;
  public RuleDescription:string='';
  public IsChecked:boolean=false;
}