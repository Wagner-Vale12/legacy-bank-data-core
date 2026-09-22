import { HttpClient, HttpParams } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';

import { Importacao } from '../../models/importacao';
import { ImportacaoPaginadoResponse } from '../../models/importacao-paginado-response';

interface ReprocessarImportacaoResponse {
  Id: number;
  Status: string;
  Mensagem: string;
}

@Injectable({
  providedIn: 'root',
})
export class ImportacoesService {
  private readonly http = inject(HttpClient);

  listar(): Observable<Importacao[]> {
    return this.http.get<Importacao[]>('/api/importacoes');
  }

  listarPaginado(
    termo: string,
    status: string,
    pagina: number,
    tamanhoPagina: number,
  ): Observable<ImportacaoPaginadoResponse> {
    let params = new HttpParams().set('pagina', pagina).set('tamanhoPagina', tamanhoPagina);

    if (termo.trim()) {
      params = params.set('termo', termo.trim());
    }

    if (status !== 'TODOS') {
      params = params.set('status', status);
    }

    return this.http.get<ImportacaoPaginadoResponse>('/api/importacoes/paginado', { params });
  }

  reprocessar(id: number): Observable<ReprocessarImportacaoResponse> {
    return this.http.put<ReprocessarImportacaoResponse>(`/api/importacoes/${id}/reprocessar`, null);
  }
}
