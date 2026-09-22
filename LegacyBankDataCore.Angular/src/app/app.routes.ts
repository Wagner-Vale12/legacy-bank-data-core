import { Routes } from '@angular/router';

import { Dashboard } from './pages/dashboard/dashboard';
import { Importacoes } from './pages/importacoes/importacoes';
import { Movimentos } from './pages/movimentos/movimentos';

export const routes: Routes = [
  {
    path: '',
    redirectTo: 'dashboard',
    pathMatch: 'full',
  },
  {
    path: 'dashboard',
    component: Dashboard,
  },
  {
    path: 'importacoes',
    component: Importacoes,
  },
  {
    path: 'movimentos',
    component: Movimentos,
  },
  {
    path: '**',
    redirectTo: 'dashboard',
  },
];
