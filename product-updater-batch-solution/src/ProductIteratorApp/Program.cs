using System;
using System.Diagnostics;
using System.IO;
using ProductIteratorApp.Repositories;

namespace ProductIteratorApp
{
    class Program
    {
        static void Main(string[] args)
        {
            // Ler Arquivo CSV

            var idLoja = Convert.ToInt16(args[0]);
            var caminhoArquivo = $"C:/Users/tfsan/Projetos/Sitemercado/@arquivos/produtos-loja-{idLoja}.csv";
            var watch = Stopwatch.StartNew();

            Console.WriteLine($"Processando Loja: {idLoja}");

            ProcessaArquivo(idLoja, caminhoArquivo);

            watch.Stop();
            var elapsedMs = watch.Elapsed;
            Console.WriteLine($"- Tempo de Processamento: {elapsedMs.Hours}:{elapsedMs.Minutes}:{elapsedMs.Seconds}:{elapsedMs.Milliseconds}");
        }

        private static void ProcessaArquivo(int idLoja, string caminhoArquivo)
        {
            var linhasProcessadas = 0;
            var totalLinhas = 0;
            using var reader = new StreamReader(caminhoArquivo);

            reader.ReadLine();

            while (!reader.EndOfStream)
            {
                var linha = reader.ReadLine();

                if (ProcessarLinha(idLoja, linha))
                {
                    linhasProcessadas++;
                }

                totalLinhas++;
            }

            Console.WriteLine("Final do Processamento...");
            Console.WriteLine($"Total: {totalLinhas}");
            Console.WriteLine($"Processadas: {linhasProcessadas}");
        }

        private static bool ProcessarLinha(int idLoja, string linha)
        {
            var valores = linha.Split(';');

            // Valida linha

            if (!ValoresValidos(valores))
            {
                return false;
            }

            // Atualiza registro no banco

            var repository = new ProductRepository();
            if (!repository.AtualizaProduto(idLoja, valores))
            {
                return false;
            }

            Console.WriteLine($"Propriedades {valores.Length} | {linha}");

            return true;
        }

        private static bool ValoresValidos(string[] valores)
        {
            return valores.Length == 7;
        }
    }
}
