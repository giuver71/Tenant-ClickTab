export class VatDTO {
    public ID: number=0;
    public Code: string='';
    public Description: string='';
    public Percentage: number=0;
    public FK_UpdateUser: number=0;
    public UpdateDate: Date | null=null;
}