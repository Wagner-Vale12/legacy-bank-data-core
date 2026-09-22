import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';

import { Importacao } from '../../models/importacao';

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

  reprocessar(id: number): Observable<ReprocessarImportacaoResponse> {
    return this.http.put<ReprocessarImportacaoResponse>(`/api/importacoes/${id}/reprocessar`, null);
  }
}
