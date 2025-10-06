import { KeyValue } from '@angular/common';

export class TranslateEntityDTO {
    public MainEntityID: number;
    public MultiLanguageEntityType: string;
    public MultiLanguageEntityID: number;
    public FK_RequestedLanguage: number;
    public FK_DefaultLanguage: number;
    public RequestedTranslatedData: KeyValue<string, string>;
    public DefaultTranslatedData: KeyValue<string, string>;
    public TranslatedLabelFields: KeyValue<string,string>;
    public TranslatedDataRequired: KeyValue<string, boolean>;
}
