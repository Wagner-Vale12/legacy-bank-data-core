import { DatePipe } from '@angular/common';
import { Component, computed, ElementRef, inject, OnInit, signal, viewChild } from '@angular/core';

import { ImportacoesService } from '../../core/services/importacoes.service';
import { Importacao } from '../../models/importacao';

@Component({
  selector: 'app-importacoes',
  imports: [DatePipe],
  templateUrl: './importacoes.html',
  styleUrl: './importacoes.css',
})
export class Importacoes implements OnInit {
  private readonly importacoesService = inject(ImportacoesService);

  readonly importacoes = signal<Importacao[]>([]);

  readonly carregando = signal(true);
  readonly erro = signal<string | null>(null);

  readonly busca = signal('');
  readonly statusSelecionado = signal('TODOS');

  readonly paginaAtual = signal(1);
  readonly totalRegistros = signal(0);
  readonly totalPaginas = signal(0);

  readonly itensPorPagina = 10;

  readonly importacaoSelecionada = signal<Importacao | null>(null);

  readonly reprocessandoId = signal<number | null>(null);

  readonly mensagemSucesso = signal<string | null>(null);

  readonly erroReprocessamento = signal<string | null>(null);

  readonly detalhesCard = viewChild<ElementRef<HTMLElement>>('detalhesCard');

  readonly paginas = computed(() =>
    Array.from({ length: this.totalPaginas() }, (_, indice) => indice + 1),
  );

  ngOnInit(): void {
    this.carregarImportacoes();
  }

  private carregarImportacoes(): void {
    this.carregando.set(true);
    this.erro.set(null);

    this.importacoesService
      .listarPaginado(
        this.busca(),
        this.statusSelecionado(),
        this.paginaAtual(),
        this.itensPorPagina,
      )
      .subscribe({
        next: (resposta) => {
          if (
            resposta.Itens.length === 0 &&
            resposta.TotalRegistros > 0 &&
            this.paginaAtual() > resposta.TotalPaginas
          ) {
            this.paginaAtual.set(resposta.TotalPaginas);

            this.carregarImportacoes();
            return;
          }

          this.importacoes.set(resposta.Itens);

          this.totalRegistros.set(resposta.TotalRegistros);

          this.totalPaginas.set(resposta.TotalPaginas);

          this.paginaAtual.set(resposta.Pagina);

          this.carregando.set(false);
        },

        error: (erro) => {
          console.error('Erro ao carregar importações:', erro);

          this.erro.set('Não foi possível carregar as importações.');

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
    this.importacaoSelecionada.set(null);

    this.carregarImportacoes();
  }

  atualizarStatus(event: Event): void {
    const select = event.target as HTMLSelectElement;

    this.statusSelecionado.set(select.value);

    this.paginaAtual.set(1);
    this.importacaoSelecionada.set(null);

    this.carregarImportacoes();
  }

  limparFiltros(): void {
    this.busca.set('');
    this.statusSelecionado.set('TODOS');
    this.paginaAtual.set(1);
    this.importacaoSelecionada.set(null);

    this.carregarImportacoes();
  }

  irParaPagina(pagina: number): void {
    if (pagina < 1 || pagina > this.totalPaginas() || pagina === this.paginaAtual()) {
      return;
    }

    this.paginaAtual.set(pagina);
    this.importacaoSelecionada.set(null);

    this.carregarImportacoes();
  }

  paginaAnterior(): void {
    this.irParaPagina(this.paginaAtual() - 1);
  }

  proximaPagina(): void {
    this.irParaPagina(this.paginaAtual() + 1);
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

  verDetalhes(importacao: Importacao): void {
    this.importacaoSelecionada.set(importacao);

    setTimeout(() => {
      this.detalhesCard()?.nativeElement.scrollIntoView({
        behavior: 'smooth',
        block: 'start',
      });
    });
  }

  fecharDetalhes(): void {
    this.importacaoSelecionada.set(null);
  }

  reprocessar(importacao: Importacao): void {
    const confirmado = window.confirm(`Deseja reprocessar o arquivo "${importacao.NomeArquivo}"?`);

    if (!confirmado) {
      return;
    }

    this.mensagemSucesso.set(null);
    this.erroReprocessamento.set(null);
    this.reprocessandoId.set(importacao.Id);

    this.importacoesService.reprocessar(importacao.Id).subscribe({
      next: (resposta) => {
        this.mensagemSucesso.set(resposta.Mensagem);

        this.reprocessandoId.set(null);
        this.importacaoSelecionada.set(null);

        this.carregarImportacoes();
      },

      error: (erro) => {
        console.error('Erro ao reprocessar importação:', erro);

        const mensagem =
          erro?.error?.Message ??
          erro?.error?.MessageDetail ??
          erro?.error ??
          'Não foi possível reprocessar a importação.';

        this.erroReprocessamento.set(
          typeof mensagem === 'string' ? mensagem : 'Não foi possível reprocessar a importação.',
        );

        this.reprocessandoId.set(null);

        this.carregarImportacoes();
      },
    });
  }
}
