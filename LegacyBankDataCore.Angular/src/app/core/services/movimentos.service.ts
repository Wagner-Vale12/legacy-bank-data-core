import { HttpClient, HttpParams } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';

import { Movimento } from '../../models/movimento';
import { MovimentoPaginadoResponse } from '../../models/movimento-paginado-response';

@Injectable({
  providedIn: 'root',
})
export class MovimentosService {
  private readonly http = inject(HttpClient);

  listar(): Observable<Movimento[]> {
    return this.http.get<Movimento[]>('/api/movimentos');
  }

  listarPaginado(
    termo: string,
    tipo: string,
    pagina: number,
    tamanhoPagina: number,
  ): Observable<MovimentoPaginadoResponse> {
    let params = new HttpParams().set('pagina', pagina).set('tamanhoPagina', tamanhoPagina);

    if (termo.trim()) {
      params = params.set('termo', termo.trim());
    }

    if (tipo !== 'TODOS') {
      params = params.set('tipo', tipo);
    }

    return this.http.get<MovimentoPaginadoResponse>('/api/movimentos/paginado', { params });
  }
}
