import { Importacao } from './importacao';

export interface ImportacaoPaginadoResponse {
  Itens: Importacao[];
  TotalRegistros: number;
  Pagina: number;
  TamanhoPagina: number;
  TotalPaginas: number;
}
