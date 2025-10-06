import { LookupDTO } from "../lookup.model";
import { RulesDescriptionEnum } from "./rule.model";

export class RoleRuleDTO {
  public ID: number;
  public FK_Rule: number;
  public Rule?: LookupDTO;
  public RuleUrlRoutes: string;
  public RuleDescriptionEnum: RulesDescriptionEnum;
  public CanCreate: boolean = true;
  public CanEdit: boolean = true;
  public CanDelete: boolean = true;
}