using Ecommerce2021a.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace Ecommerce2021a.Data
{
    public class PedidoData : Data
    {
        public void Create(Pedido pedido)
        {
            SqlTransaction transaction = this.connectionDB.BeginTransaction();
            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = connectionDB;
                cmd.Transaction = transaction;
                cmd.CommandText = "INSERT INTO Pedido VALUES (@idCliente, @data); " + "SELECT @@IDENTITY";
                cmd.Parameters.AddWithValue("@idCliente", pedido.IdCliente);
                cmd.Parameters.AddWithValue("@data", DateTime.Now);

                //ExecuteScalar: executa a consulta e retorna a primeira coluna
                //da primeira linha no conjunto de resultados retornado pela consulta
                //Colunas ou linhas adicionais são ignoradas.
                int idPedido = Convert.ToInt32(cmd.ExecuteScalar());

                foreach (var item in pedido.Itens)
                {
                    SqlCommand cmdItem = new SqlCommand();
                    cmdItem.Connection = connectionDB;
                    cmdItem.Transaction = transaction;
                    cmdItem.CommandText = @"INSERT INTO ItemPedido VALUES (@idpedido, @idproduto, @quantidade, @preco)";
                    cmdItem.Parameters.AddWithValue("@idpedido", idPedido);
                    cmdItem.Parameters.AddWithValue("@idproduto", item.Produto.IdProduto);
                    cmdItem.Parameters.AddWithValue("@quantidade", item.Quantidade);
                    cmdItem.Parameters.AddWithValue("@preco", item.Valor);

                    cmdItem.ExecuteNonQuery();
                }
                //executa as inserções da transação nas tabelas
                transaction.Commit();
            }
            catch (Exception ex)
            {
                //desfaz as operações de insert caso dê algum problema e elas não
                //possam ser executadas
                transaction.Rollback();
            }
        }

        public List<Pedido> Read(int idCliente)
        {
            List<Pedido> lista = new List<Pedido>();
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = connectionDB;
            cmd.CommandText = "SELECT * FROM pedido WHERE IdCliente = @idCliente";
            cmd.Parameters.AddWithValue("@idCliente", idCliente);
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                Pedido p = new Pedido();
                p.IdPedido = (int)reader["IdPedido"];
                p.IdCliente = (int)reader["IdCliente"];
                p.Data = (DateTime)reader["Data"];
                lista.Add(p);
            }
            return lista;
        }

        public List<Pedido> Read()
        {
            List<Pedido> lista = new List<Pedido>();
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = connectionDB;
            cmd.CommandText = "SELECT * FROM pedido";
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                Pedido p = new Pedido();
                p.IdPedido = (int)reader["IdPedido"];
                p.IdCliente = (int)reader["IdCliente"];
                p.Data = (DateTime)reader["Data"];
                lista.Add(p);
            }
            return lista;
        }

        public void Delete_Pedido(int id)
        {
            //criado o cmd (comando SQl) para executar um comando SQL no Banco de Dados
            SqlCommand cmd = new SqlCommand();
            //conexão com o Banco de Dados
            cmd.Connection = base.connectionDB;
            //criada a String SQL
            cmd.CommandText = @"Delete from ItemPedido Where IdPedido = @id
                                Delete from Pedido Where IdPedido = @id";

            cmd.Parameters.AddWithValue("@id", id);

            //Execução do comando Delete no Banco de Dados
            cmd.ExecuteNonQuery();
        }


        // public void Create(Pedido pedido)
        // {
        //     SqlTransaction transaction = this.connectionDB.BeginTransaction();

        //     try
        //     {
        //         SqlCommand cmd = new SqlCommand();
        //         cmd.Connection = connectionDB;
        //         cmd.Transaction = transaction;
        //         cmd.CommandText = @"Insert into Pedidos values (@idCliente, @data);" + "Select @@IDENTITY";
        //         cmd.Parameters.AddWithValue("@idCliente", pedido.IdCliente);
        //         cmd.Parameters.AddWithValue("@data", DateTime.Now);

        //         //ExecuteScalar: executa a consula e retorna a primeira coluna da primeira linha
        //         // no conjunto de resultados retornado pela consulta
        //         //colunas ou linhas adicionais são ignorados

        //         int idPedido = Convert.ToInt32(cmd.ExecuteScalar());

        //         foreach (var item in pedido.Itens)
        //         {
        //             SqlCommand cmdItem = new SqlCommand();
        //             cmdItem.Connection = connectionDB;
        //             cmdItem.Transaction = transaction;
        //             cmdItem.CommandText = @"Insert into ItemPedido values" +
        //                                    "(@idpedido, @idproduto, @quantidade, @preco";

        //             cmdItem.Parameters.AddWithValue("@idpedido", item.IdPedido);
        //             cmdItem.Parameters.AddWithValue("@idproduto", item.Produto.IdProduto);
        //             cmdItem.Parameters.AddWithValue("@quantidade", item.Quantidade);
        //             cmdItem.Parameters.AddWithValue("@preco", item.Valor);

        //             cmdItem.ExecuteNonQuery();
        //         }

        //         //Executa as inserções da transação nas tabelas
        //         transaction.Commit();
        //     }
        //     catch(Exception ex)
        //     {
        //         //desfaz as operações de insert caso dê algum problema e elas não
        //         //possam ser executadas
        //         transaction.Rollback();
        //     }
        // }

        // public List<Pedido> Read(int idCliente)
        // {
        //     List<Pedido> lista = new List<Pedido>();

        //     SqlCommand cmd = new SqlCommand();
        //     cmd.Connection = connectionDB;
        //     cmd.CommandText = @"Select * from pedido where IdCliente = @idCliente";
        //     cmd.Parameters.AddWithValue("@idCliente", idCliente);

        //     SqlDataReader reader = cmd.ExecuteReader();

        //     while(reader.Read())
        //     {
        //         Pedido p = new Pedido();
        //         p.IdPedido = (int)reader["IdPedido"];
        //         p.IdCliente = (int)reader["IdCliente"];
        //         p.Data = (DateTime)reader["Data"];

        //         lista.Add(p);
        //     }
        //     return lista;
        // }

        //  public List<Pedido> Read()
        // {
        //     List<Pedido> lista = new List<Pedido>();
        //     SqlCommand cmd = new SqlCommand();
        //     cmd.Connection = connectionDB;
        //     cmd.CommandText = "SELECT * FROM pedido";
        //     SqlDataReader reader = cmd.ExecuteReader();
        //     while (reader.Read())
        //     {
        //         Pedido p = new Pedido();
        //         p.IdPedido = (int)reader["IdPedido"];
        //         p.IdCliente = (int)reader["IdCliente"];
        //         p.Data = (DateTime)reader["Data"];
        //         lista.Add(p);
        //     }
        //     return lista;
        // }
    }
}