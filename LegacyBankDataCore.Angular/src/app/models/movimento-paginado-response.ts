import { Movimento } from './movimento';

export interface MovimentoPaginadoResponse {
  Itens: Movimento[];
  TotalRegistros: number;
  Pagina: number;
  TamanhoPagina: number;
  TotalPaginas: number;
}
