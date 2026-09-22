import { CurrencyPipe, DatePipe } from '@angular/common';
import { Component, computed, inject, OnInit, signal } from '@angular/core';
import { forkJoin } from 'rxjs';

import { ImportacoesService } from '../../core/services/importacoes.service';
import { MovimentosService } from '../../core/services/movimentos.service';
import { Importacao } from '../../models/importacao';
import { Movimento } from '../../models/movimento';

@Component({
  selector: 'app-dashboard',
  imports: [CurrencyPipe, DatePipe],
  templateUrl: './dashboard.html',
  styleUrl: './dashboard.css',
})
export class Dashboard implements OnInit {
  private readonly importacoesService = inject(ImportacoesService);

  private readonly movimentosService = inject(MovimentosService);

  readonly importacoes = signal<Importacao[]>([]);
  readonly movimentos = signal<Movimento[]>([]);

  readonly carregando = signal(true);
  readonly erro = signal<string | null>(null);

  readonly totalImportacoes = computed(() => this.importacoes().length);

  readonly importacoesConcluidas = computed(
    () => this.importacoes().filter((importacao) => importacao.Status === 'CONCLUIDA').length,
  );

  readonly importacoesComErro = computed(
    () => this.importacoes().filter((importacao) => importacao.Status === 'ERRO').length,
  );

  readonly importacoesProcessando = computed(
    () => this.importacoes().filter((importacao) => importacao.Status === 'PROCESSANDO').length,
  );

  readonly totalMovimentos = computed(() => this.movimentos().length);

  readonly totalEntradas = computed(
    () => this.movimentos().filter((movimento) => movimento.Tipo === 'ENTRADA').length,
  );

  readonly totalSaidas = computed(
    () => this.movimentos().filter((movimento) => movimento.Tipo === 'SAIDA').length,
  );

  readonly valorEntradas = computed(() =>
    this.movimentos()
      .filter((movimento) => movimento.Tipo === 'ENTRADA')
      .reduce((total, movimento) => total + movimento.Valor, 0),
  );

  readonly valorSaidas = computed(() =>
    this.movimentos()
      .filter((movimento) => movimento.Tipo === 'SAIDA')
      .reduce((total, movimento) => total + movimento.Valor, 0),
  );

  readonly ultimasImportacoes = computed(() =>
    [...this.importacoes()].sort((a, b) => b.Id - a.Id).slice(0, 5),
  );

  ngOnInit(): void {
    this.carregarDashboard();
  }

  private carregarDashboard(): void {
    this.carregando.set(true);
    this.erro.set(null);

    forkJoin({
      importacoes: this.importacoesService.listar(),

      movimentos: this.movimentosService.listar(),
    }).subscribe({
      next: ({ importacoes, movimentos }) => {
        this.importacoes.set(importacoes);
        this.movimentos.set(movimentos);

        this.carregando.set(false);
      },

      error: (erro) => {
        console.error('Erro ao carregar dashboard:', erro);

        this.erro.set('Não foi possível carregar os dados do dashboard.');

        this.carregando.set(false);
      },
    });
  }

  classeStatus(status: string): string {
    switch (status) {
      case 'CONCLUIDA':
        return 'bg-success';

      case 'ERRO':
        return 'bg-danger';

      case 'PROCESSANDO':
        return 'bg-warning text-dark';

      case 'RECEBIDA':
        return 'bg-secondary';

      default:
        return 'bg-secondary';
    }
  }
}
