import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';

import { Movimento } from '../../models/movimento';

@Injectable({
  providedIn: 'root',
})
export class MovimentosService {
  private readonly http = inject(HttpClient);

  listar(): Observable<Movimento[]> {
    return this.http.get<Movimento[]>('/api/movimentos');
  }
}
