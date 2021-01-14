using System;
using System.Threading;
using Confluent.Kafka;
using ProductUpdaterApp.Repositories;

namespace ProductUpdaterApp
{
    class Program
    {
        static void Main(string[] args)
        {
            string topic = "product-updater";
            string groupId = "product-updater-1";

            var conf = new ConsumerConfig
            {
                GroupId = groupId,
                BootstrapServers = "localhost:9091,localhost:9092,localhost:9093",
                AutoOffsetReset = AutoOffsetReset.Latest
            };

            Console.WriteLine("Iniciando consumidor...");
            Console.WriteLine($"Tópico: {topic}");
            Console.WriteLine($"Grupo: {groupId}");

            var consumer = new ConsumerBuilder<Ignore, string>(conf).Build();
            consumer.Subscribe(topic);

            var cts = new CancellationTokenSource();
            Console.CancelKeyPress += (_, e) =>
            {
                e.Cancel = true;
                cts.Cancel();
            };

            try
            {
                while (true)
                {
                    try
                    {
                        var cr = consumer.Consume(cts.Token);
                        var linha = cr.Message.Value;

                        ProcessarLinha(linha);
                    }
                    catch (ConsumeException e)
                    {
                        Console.WriteLine($"Error occured: {e.Error.Reason}");
                    }
                }
            }
            catch (OperationCanceledException)
            {
                consumer.Close();
            }
        }

        private static bool ProcessarLinha(string linha)
        {
            var valores = linha.Split(';');

            // Valida linha

            if (!ValoresValidos(valores))
            {
                return false;
            }

            // Atualiza registro no banco

            var idLoja = Convert.ToInt32(valores[6]);
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
