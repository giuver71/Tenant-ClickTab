import { EqpTranslateEntityComponent } from './../directives/eqp-translate-entity/eqp-translate-entity.component';
import { Injectable,EventEmitter } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../environments/environment';
import { AuthService } from './auth.service';
import { TranslateEntityDTO } from '../models/translateEntity.model';
import { MatDialog, MatDialogRef } from '@angular/material/dialog';
import { LookupDTO } from '../models/lookup.model';

@Injectable({
    providedIn: 'root'
})
export class MultiLanguageService {

    dialogRef: MatDialogRef<EqpTranslateEntityComponent>;
    translationEntitySaveResult: EventEmitter<boolean> = new EventEmitter();

    constructor(
        private http: HttpClient, private authService: AuthService, private dialog: MatDialog) {
    }

    /**
     * Recupera il json contenente le traduzioni nella lingua in cui è stata effettuata la login
     */
    getLocalizationJson(externalLanguageCode: string = null) : Promise<any> {
        let currentLanguageCode = externalLanguageCode == null || externalLanguageCode == undefined ? this.authService.getCurrentLanguage().Code : externalLanguageCode;
        return this.http.get<any>(environment.apiFullUrl + '/multilanguage/GetLocalizationJson/' + currentLanguageCode).toPromise()
    }

    /**
     * Restituisce un oggetto di tipo TranslateEntityDTO necessario alla gestione della form di traduzione delle varie entità
     * @param EntityID ID dell'entità per cui gestire la traduzione
     * @param EntityTypeString Nome della classe (type del modello espresso come stringa) per cui gestire la traduzione
     * @param FK_Language ID della lingua della traduzione da recuperare
     */
    getEntityTranslation(EntityID: number, EntityTypeString: string, FK_Language: number) : Promise<TranslateEntityDTO> {
        return this.http.get<TranslateEntityDTO>(environment.apiFullUrl + '/multilanguage/GetEntityTranslation/' + EntityID + '/' + EntityTypeString + '/' + FK_Language).toPromise()
    }

    /**
     * Memorizza la traduzione per una specifica entità.
     * Se la traduzione per l'entità esiste già allora la sovrascrive
     * @param translateEntity Oggetto TranslateEntityDTO contenente la traduzione da salvare
     */
    saveEntityTranslation(translateEntity: TranslateEntityDTO) : Promise<any> {
        return this.http.post<any>(environment.apiFullUrl + '/multilanguage/SaveEntityTranslation', translateEntity).toPromise();
    }

    openTranslationsEntity(EntityID: number, EntityType: string, EntityLanguages: Array<LookupDTO> = null) {
        this.dialogRef = this.dialog.open(EqpTranslateEntityComponent, {
            disableClose: true,
            hasBackdrop: true,
            minWidth: '50%',
            minHeight: '300px'
        });

        this.dialogRef.componentInstance.EntityID = EntityID;
        this.dialogRef.componentInstance.EntityType = EntityType;
        if(EntityLanguages != null)
            this.dialogRef.componentInstance.EntityLanguages = EntityLanguages;
    }

    closeModalTranslationEntity() {
        if(this.translationEntitySaveResult)
            this.translationEntitySaveResult.unsubscribe();

        if(this.dialogRef) {
            this.dialogRef.close();
        }
    }
}
