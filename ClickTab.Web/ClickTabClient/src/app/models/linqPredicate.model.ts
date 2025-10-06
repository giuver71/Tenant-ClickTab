import { LinqPredicateDTO } from "@eqproject/eqp-common";

//Modelli per la definizione dei filtri dinamici con predicati LINQ
export class ComplexLinqPredicateDTO {
  public Predicates: Array<LinqPredicateDTO>;

  /**
   * Ricostruisce l'array di predicati complessi a partire dalla matrice di LinqPredicateDTO passata nel parametro.
   * Ogni riga della matrice diventerà un elemento della lista restituita.
   * Ogni elemento delle lista conterrà i dati della riga della matrice.
   * @param complexPredicates Restituisce un array di oggetti di tipo ComplexLinqPredicateDTO
   */
  static CreateComplexPredicate(complexPredicates: Array<Array<LinqPredicateDTO>>) : Array<ComplexLinqPredicateDTO> {
    let results: Array<ComplexLinqPredicateDTO> = new Array<ComplexLinqPredicateDTO>();

    complexPredicates.forEach(cp => {
      let complexPredicate: ComplexLinqPredicateDTO = new ComplexLinqPredicateDTO();
      complexPredicate.Predicates = cp;
      results.push(complexPredicate);
    });

    return results;
  }
}
