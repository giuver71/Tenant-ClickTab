import { LinqPredicateDTO } from '@eqproject/eqp-common';
import { ComplexLinqPredicateDTO } from './linqPredicate.model';

export class LookupDTO {
  ID: number;
  Label: string;
  FullObject: any;
}

/**
 * Modello per la configurazione della lookup, contiene le informazioni
 * sul tipo degli elementi da mostrare e gli eventuali filtri da applicare
 */
export class LookupConfigDTO {
  TypeName: string;
  Filters: Array<LinqPredicateDTO>;
  ComplexFilters: Array<ComplexLinqPredicateDTO>;
  CustomConfig: LookupCustomConfig;
}


export class LookupCustomConfig {
  public LabelProperties: Array<string>;
  public IncludeFullObject: boolean;
  public ApplyCoalesce: boolean = false;
}
