import { CurrencyPipe, DatePipe } from '@angular/common';
import { Component, computed, inject, OnInit, signal } from '@angular/core';

import { MovimentosService } from '../../core/services/movimentos.service';
import { Movimento } from '../../models/movimento';

@Component({
  selector: 'app-movimentos',
  imports: [CurrencyPipe, DatePipe],
  templateUrl: './movimentos.html',
  styleUrl: './movimentos.css',
})
export class Movimentos implements OnInit {
  private readonly movimentosService = inject(MovimentosService);

  readonly movimentos = signal<Movimento[]>([]);
  readonly carregando = signal(true);
  readonly erro = signal<string | null>(null);

  readonly busca = signal('');
  readonly tipoSelecionado = signal('TODOS');

  readonly movimentosFiltrados = computed(() => {
    const termo = this.busca().trim().toLowerCase();

    const tipo = this.tipoSelecionado();

    return this.movimentos().filter((movimento) => {
      const correspondeBusca =
        !termo ||
        movimento.IdExterno.toLowerCase().includes(termo) ||
        movimento.Conta.toLowerCase().includes(termo);

      const correspondeTipo = tipo === 'TODOS' || movimento.Tipo === tipo;

      return correspondeBusca && correspondeTipo;
    });
  });

  ngOnInit(): void {
    this.carregarMovimentos();
  }

  private carregarMovimentos(): void {
    this.carregando.set(true);
    this.erro.set(null);

    this.movimentosService.listar().subscribe({
      next: (movimentos) => {
        this.movimentos.set(movimentos);
        this.carregando.set(false);
      },

      error: (erro) => {
        console.error('Erro ao carregar movimentações:', erro);

        this.erro.set('Não foi possível carregar as movimentações.');

        this.carregando.set(false);
      },
    });
  }

  atualizarBusca(event: Event): void {
    const input = event.target as HTMLInputElement;

    this.busca.set(input.value);
  }

  atualizarTipo(event: Event): void {
    const select = event.target as HTMLSelectElement;

    this.tipoSelecionado.set(select.value);
  }

  limparFiltros(): void {
    this.busca.set('');
    this.tipoSelecionado.set('TODOS');
  }

  classeTipo(tipo: string): string {
    switch (tipo) {
      case 'ENTRADA':
        return 'bg-success';

      case 'SAIDA':
        return 'bg-danger';

      default:
        return 'bg-secondary';
    }
  }
}
