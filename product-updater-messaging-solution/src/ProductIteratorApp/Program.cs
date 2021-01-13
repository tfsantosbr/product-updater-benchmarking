using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using Confluent.Kafka;
using ProductIteratorApp.Repositories;

namespace ProductIteratorApp
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var idLoja = Convert.ToInt16(args[0]);
            var caminhoArquivo = $"C:/Users/tfsan/Projetos/Sitemercado/@arquivos/produtos-loja-{idLoja}.csv";
            var topic = $"topico-loja-{idLoja}";
            var config = new ProducerConfig
            {
                BootstrapServers = "localhost:9091,localhost:9092,localhost:9093"
            };
            using var producer = new ProducerBuilder<Null, string>(config).Build();

            var watch = Stopwatch.StartNew();

            Console.WriteLine($"Processando Loja: {idLoja}");

            await ProcessaArquivo(idLoja, producer, caminhoArquivo, topic);

            watch.Stop();
            var elapsedMs = watch.Elapsed;
            Console.WriteLine($"- Tempo de Processamento: {elapsedMs.Hours}:{elapsedMs.Minutes}:{elapsedMs.Seconds}:{elapsedMs.Milliseconds}");
        }

        private static async Task ProcessaArquivo(int idLoja, IProducer<Null, string> producer, string caminhoArquivo, string topic)
        {
            var linhasProcessadas = 0;
            var totalLinhas = 0;
            using var reader = new StreamReader(caminhoArquivo);

            reader.ReadLine();

            while (!reader.EndOfStream)
            {
                var linha = reader.ReadLine();

                await EnviaMensagem(idLoja, linha, producer, topic);

                totalLinhas++;
            }

            Console.WriteLine("Final do Processamento...");
            Console.WriteLine($"Total: {totalLinhas}");
            Console.WriteLine($"Processadas: {linhasProcessadas}");
        }

        private static async Task EnviaMensagem(int idLoja, string linha, IProducer<Null, string> producer, string topic)
        {
            // Envia Mensagem

            var result = await producer.ProduceAsync(
                topic,
                new Message<Null, string>
                { Value = linha });

            Console.WriteLine($"[Mensagem Enviada] \n" +
                $"Topico: {topic} \n" +
                $"Mensagem: {linha} \n" +
                $"Status: {result.Status}");
        }

        private static bool ValoresValidos(string[] valores)
        {
            return valores.Length == 7;
        }
    }
}
