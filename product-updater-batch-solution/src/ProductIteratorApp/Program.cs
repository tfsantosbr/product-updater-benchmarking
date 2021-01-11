using System;
using System.Diagnostics;
using System.IO;

namespace ProductIteratorApp
{
    class Program
    {
        static void Main(string[] args)
        {
            // Ler Arquivo CSV

            var caminhoArquivo = "C:\\Users\\tfsan\\Github\\product-updater-benchmarking\\data\\produtos.CSV";
            var watch = Stopwatch.StartNew();
            
            ProcessaArquivo(caminhoArquivo);

            watch.Stop();
            var elapsedMs = watch.Elapsed;
            Console.WriteLine($"- Tempo de Processamento: {elapsedMs.Hours}:{elapsedMs.Minutes}:{elapsedMs.Seconds}:{elapsedMs.Milliseconds}");
        }

        private static void ProcessaArquivo(string caminhoArquivo)
        {
            using var reader = new StreamReader(caminhoArquivo);

            reader.ReadLine();

            while (!reader.EndOfStream)
            {
                var linha = reader.ReadLine();

                ProcessarLinha(linha);
            }
        }

        private static void ProcessarLinha(string linha)
        {
            var valores = linha.Split(';');

            Console.WriteLine($"Propriedades {valores.Length} | {linha}");
        }
    }
}
