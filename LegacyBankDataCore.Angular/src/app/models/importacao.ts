export interface Importacao {
  Id: number;
  NomeArquivo: string;
  Status: string;
  TotalRegistros: number;
  DataRecebimento: string;
  DataProcessamento: string | null;
  MensagemErro: string | null;
  HashArquivo: string | null;
}
