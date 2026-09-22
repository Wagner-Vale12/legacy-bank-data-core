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

  readonly paginaAtual = signal(1);
  readonly totalRegistros = signal(0);
  readonly totalPaginas = signal(0);

  readonly itensPorPagina = 10;

  readonly paginas = computed(() =>
    Array.from({ length: this.totalPaginas() }, (_, indice) => indice + 1),
  );

  ngOnInit(): void {
    this.carregarMovimentos();
  }

  private carregarMovimentos(): void {
    this.carregando.set(true);
    this.erro.set(null);

    this.movimentosService
      .listarPaginado(this.busca(), this.tipoSelecionado(), this.paginaAtual(), this.itensPorPagina)
      .subscribe({
        next: (resposta) => {
          this.movimentos.set(resposta.Itens);
          this.totalRegistros.set(resposta.TotalRegistros);
          this.totalPaginas.set(resposta.TotalPaginas);
          this.paginaAtual.set(resposta.Pagina);

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

  pesquisar(): void {
    this.paginaAtual.set(1);
    this.carregarMovimentos();
  }

  atualizarTipo(event: Event): void {
    const select = event.target as HTMLSelectElement;

    this.tipoSelecionado.set(select.value);
    this.paginaAtual.set(1);

    this.carregarMovimentos();
  }

  limparFiltros(): void {
    this.busca.set('');
    this.tipoSelecionado.set('TODOS');
    this.paginaAtual.set(1);

    this.carregarMovimentos();
  }

  irParaPagina(pagina: number): void {
    if (pagina < 1 || pagina > this.totalPaginas() || pagina === this.paginaAtual()) {
      return;
    }

    this.paginaAtual.set(pagina);

    this.carregarMovimentos();
  }

  paginaAnterior(): void {
    this.irParaPagina(this.paginaAtual() - 1);
  }

  proximaPagina(): void {
    this.irParaPagina(this.paginaAtual() + 1);
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
