export class CategoryDTO {
    public ID: number=0;
    public Description: string='';
    public Code: string='';
    public Fee: number=0;
    public IsFiscal: boolean=false;
    public Department: string | null=null;
    public Negative: boolean=false;
    public FeeOnPurchasePrice: boolean=false;
    public FK_UpdateUser: number=0;
    public UpdateDate: Date | null=null;
}

