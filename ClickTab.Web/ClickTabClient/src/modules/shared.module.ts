import { EqpTimePickerComponent } from './../app/directives/eqp-time-picker/eqp-time-picker.component';
import { CsvImportExportComponent } from './../app/directives/csv-import-export/csv-import-export.component';
import { EqpMultiLanguageFieldComponent } from './../app/directives/eqp-multilanguage-field/eqp-multilanguage-field.component';
import { EqpTranslateEntityComponent } from './../app/directives/eqp-translate-entity/eqp-translate-entity.component';
import { DynamicLoaderDirective } from './../app/directives/dynamic-loader/dynamic-loader.directive';
import { NgModule, ModuleWithProviders } from '@angular/core';
import { EqpTableModule } from '@eqproject/eqp-table';
import { EqpSelectModule } from '@eqproject/eqp-select';
import { NgSelectModule } from '@ng-select/ng-select';
import { EqpLookupModule } from '@eqproject/eqp-lookup';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { MaterialModule } from './material.module';
import { CommonModule, registerLocaleData } from '@angular/common';
import { HttpClient } from '@angular/common/http';

import { ImageCropperModule } from 'ngx-image-cropper';
import { EqpDashboardModule } from '@eqproject/eqp-dashboard';
import { EqpAttachmentsModule } from '@eqproject/eqp-attachments';
import { EqpDatetimerangepickerModule } from '@eqproject/eqp-datetimerangepicker';
import { NgxMatDatetimePickerModule, NgxMatTimepickerModule } from '@angular-material-components/datetime-picker';
import { TranslateLoader, TranslateModule } from '@ngx-translate/core';
import { TranslateHttpLoader } from '@ngx-translate/http-loader';
import { DateAdapter } from '@angular/material/core';
import localeIT from '@angular/common/locales/it';
import { EqpFiltersModule } from '@eqproject/eqp-filters';
import { EqpNumericInputMode, EqpNumericModule, NumericMaskConfig } from "@eqproject/eqp-numeric";
import { NgxMaterialTimepickerModule } from 'ngx-material-timepicker';
import { PageHeaderComponent } from '../app/elements/page-header/page-header.component';
 

export const customNumericMaskConfig: NumericMaskConfig = {
  align: "right",
  allowNegative: true,
  allowZero: true,
  decimal: ",",
  precision: 5,
  prefix: "",
  suffix: "",
  thousands: "",
  nullable: true,
  min: null,
  max: null,
  inputMode: EqpNumericInputMode.NATURAL
};
@NgModule({
  imports: [
    FormsModule,
    ReactiveFormsModule,
    MaterialModule,
    CommonModule,
    EqpSelectModule,
    EqpTableModule,
    EqpLookupModule,
    NgSelectModule,
    ImageCropperModule,
    EqpDashboardModule,
    EqpAttachmentsModule,
    EqpDatetimerangepickerModule,
    NgxMatDatetimePickerModule,
    NgxMatTimepickerModule,
    TranslateModule.forRoot({
      loader: {
        provide: TranslateLoader,
        useFactory: HttpLoaderFactory,
        deps: [HttpClient]
      }
    }),
    EqpFiltersModule,
    EqpNumericModule.forRoot(customNumericMaskConfig),
    NgxMaterialTimepickerModule,
   
  ],
  exports: [
    FormsModule,
    MaterialModule,
    ReactiveFormsModule,
    CommonModule,
    EqpSelectModule,
    EqpTableModule,
    EqpLookupModule,
    NgSelectModule,
    ImageCropperModule,
    EqpDashboardModule,
    EqpAttachmentsModule,
    EqpDatetimerangepickerModule,
    NgxMatDatetimePickerModule,
    NgxMatTimepickerModule,
    TranslateModule,
    EqpTranslateEntityComponent,
    EqpMultiLanguageFieldComponent,
    CsvImportExportComponent,
    EqpTimePickerComponent,
    EqpFiltersModule,
    EqpNumericModule,
    NgxMaterialTimepickerModule,
    PageHeaderComponent,
  ],
  declarations: [
    DynamicLoaderDirective,
    EqpTranslateEntityComponent,
    EqpMultiLanguageFieldComponent,
    CsvImportExportComponent,
    EqpTimePickerComponent,
    PageHeaderComponent,
  ]
})

export class SharedModule {

  constructor(private adapter: DateAdapter<any>) {
    this.setDateAdapterLocale("it-IT");
    registerLocaleData(localeIT);
  }


  setDateAdapterLocale(localeString: string): void {
    this.adapter.setLocale(localeString);
    this.adapter.getFirstDayOfWeek = () => { return 1; }
  }

  static forRoot(): ModuleWithProviders<SharedModule> {
    return {
      ngModule: SharedModule,
      providers: []
    }
  }
}

export function HttpLoaderFactory(http: HttpClient): TranslateHttpLoader {
  return new TranslateHttpLoader(http);
}

export class COMPONENT_MAPPER {
  public static pool: { [key: string]: any } = {};
  static get(key: string) {
    let entity = COMPONENT_MAPPER.pool[key];
    if (!entity) {
      throw new Error(
        `No entity mapped for '${key}'. Please register Entity with component mapper service on your start up`
      );
    }
    return entity;
  }
}
