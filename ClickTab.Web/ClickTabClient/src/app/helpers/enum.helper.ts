import { Injectable } from '@angular/core';

@Injectable({
    providedIn: 'root'
})

export class EnumHelper {

    //Contiene i singoli valori key value dell'enumeratore separati
    indexEnumerator;

    //Rec
    arrayToCompare;

    //Array dove sarà ricostruito l'enumeratore e dopo essere clonato sarà restituito per ricavare la Label interessata
    arrayIterated = [];

    constructor() { }

    getEnumArray<T extends { [name: string]: any }>(enumObject: T) {
        this.indexEnumerator = Object.keys(enumObject);
        this.arrayToCompare = this.indexEnumerator.filter(x => !isNaN(x));

        this.arrayToCompare.forEach(index => {
            let object = { key: parseInt(index), value: enumObject[index] }
            this.arrayIterated.push(object);
        });

        //Dovendo svuotare l'arrayIterated creo un clone da passare come oggetto finale
        let clonedArray = JSON.parse(JSON.stringify(this.arrayIterated));

        this.arrayIterated = [];

        return clonedArray;
    }

    // Dato il tipo di enumeratore e il valore restituisco l'etichetta
    getEnumLabel<T extends { [name: string]: any }>(enumObject: T, value?: number) {
        let enumArray = this.getEnumArray(enumObject);
        if (value)
            return enumArray.find(x => x.key == value).value;
        else
            return enumArray;
    }

}

