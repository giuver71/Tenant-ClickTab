/**
 * Interfaccia necessaria da implementare per il funzionamento della lookup in aggiunta
 */
export interface IInjectLookup {
  /**
   * Gestione comportamento del componente al salvataggio (redirect da inserimento dati a lista)
   */
  DisableRedirectAfterSave: boolean;
}
