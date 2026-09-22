import { DatePipe } from '@angular/common';
import { Component, ElementRef, inject, OnInit, signal, viewChild } from '@angular/core';

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

  readonly importacaoSelecionada = signal<Importacao | null>(null);

  readonly reprocessandoId = signal<number | null>(null);

  readonly mensagemSucesso = signal<string | null>(null);

  readonly erroReprocessamento = signal<string | null>(null);

  readonly detalhesCard = viewChild<ElementRef<HTMLElement>>('detalhesCard');

  ngOnInit(): void {
    this.carregarImportacoes();
  }

  private carregarImportacoes(): void {
    this.carregando.set(true);
    this.erro.set(null);

    this.importacoesService.listar().subscribe({
      next: (importacoes) => {
        this.importacoes.set(importacoes);
        this.carregando.set(false);
      },
      error: (erro) => {
        console.error('Erro ao carregar importações:', erro);

        this.erro.set('Não foi possível carregar as importações.');

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
