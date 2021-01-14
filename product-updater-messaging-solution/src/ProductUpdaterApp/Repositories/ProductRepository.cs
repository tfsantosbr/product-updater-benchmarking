using System;
using System.Data.SqlClient;
using Dapper;

namespace ProductUpdaterApp.Repositories
{
    public class ProductRepository
    {
        private readonly string _connectionString = "Data Source=(LocalDb)\\MSSQLLocalDB;Initial Catalog=SiteMercadoIntegracao;Integrated Security=SSPI";

        public bool AtualizaProduto(int idLoja, string[] valores)
        {
            var sql = @"UPDATE [Produtos]
                           SET [nome] = @nome
                              ,[sku] = @sku
                              ,[preco] = @preco
                              ,[estoque] = @estoque
                              ,[data_sincronizacao] = @data_sincronizacao
                         WHERE [id_loja] = @id_loja 
                           AND [id_produto] = @id_produto";

            using var connection = new SqlConnection(_connectionString);

            var rowsAffected = connection.Execute(sql, new
            {
                nome = valores[2],
                preco = valores[3],
                estoque = valores[4],
                data_sincronizacao = DateTime.Now,
                id_loja = idLoja,
                sku = valores[1],
                id_produto = valores[0]
            });

            return rowsAffected > 0;
        }
    }
}