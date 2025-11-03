export class CategoryDTO {
    public ID: number=0;
    public Description: string='';
    public Code: string='';
    public FK_UpdateUser: number=0;
    public UpdateDate: Date | null=null;
}