using Ecommerce2021a.Data;
using Ecommerce2021a.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using static Ecommerce2021a.Controllers.ClienteController;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Ecommerce2021a.Controllers
{
    public class CarrinhoController : Controller
    {
        public IActionResult Index(int id)
        {
            Pedido pedido;
            try
            {
                pedido = JsonConvert.DeserializeObject<Pedido>(HttpContext.Session.GetString("Carrinho"));
            }
            catch
            {
                pedido = new Pedido();
            }

            using (var data = new ClienteData())
                ViewBag.Clientes = data.Read();

            ViewBag.SubTotal = pedido.Total;

            //ViewBag.Frete = pedido.Frete;

            //ViewBag.Total = pedido.Total + pedido.Frete;

            //ViewBag.QtdProduto = pedido.QtdProduto;

            return View(pedido);

            // List<Item> lista = new List<Item>();

            // var carrinho = HttpContext.Session.GetString("Carrinho");

            // if (carrinho != null)
            // {
            //     //TODO Converter String para Lista(Json)
            //     lista = System.Text.Json.JsonSerializer.Deserialize<List<Item>>(carrinho);
            // }

            // return View(lista);
        }

        [HttpGet]
        public IActionResult Comprar(int id)
        {
            Pedido pedido;
            try
            {
                // JSON é basicamente um formato leve de troca de informações/dados
                // entre sistemas.
                // Deserialize faz a Conversão a cadeia de caracteres JSON especificada
                // em um grafo de objeto.
                // Estado de sessão usa um armazenamento mantido pelo aplicativo para
                // que os dados persistam entre solicitações feitas.

                pedido = JsonConvert.DeserializeObject<Pedido>(HttpContext.Session.GetString("Carrinho"));
            }
            catch
            {
                pedido = new Pedido();
            }

            // consulta em bd e lógica de programação

            using (var data = new ProdutoData())
            {
                Produto produto = data.Read(id);

                Item item = pedido.Itens.SingleOrDefault(i => i.Produto.IdProduto == id);

                if (item == null)
                {
                    pedido.Itens.Add(new Item
                    {
                        Produto = produto,
                        Quantidade = 1,
                        Valor = produto.Valor
                    });
                }
                else
                {
                    item.Quantidade++;
                }
            }

            HttpContext.Session.SetString("Carrinho", JsonConvert.SerializeObject(pedido));

            return RedirectToAction("Index");

            // List<Item> lista = new List<Item>();

            // var carrinho = HttpContext.Session.GetString("Carrinho");

            // if (carrinho != null)
            // {
            //     //TODO Converter String para Lista(Json)
            //     lista = System.Text.Json.JsonSerializer.Deserialize<List<Item>>(carrinho);
            // }

            // using (var data = new ProdutoData())
            // {
            //     var item = lista.SingleOrDefault(i => i.Produto.IdProduto == id);

            //     if (item == null)
            //     {
            //         Produto produto = data.Read(id);

            //         item = new Item();
            //         item.Produto = produto;
            //         item.Quantidade = 1;
            //         item.Valor = item.Produto.Valor;
            //         lista.Add(item);
            //     }
            //     else
            //     {
            //         item.Quantidade++;
            //     }

            //     //TODO Converter Lista para String (Json)
            //     carrinho = System.Text.Json.JsonSerializer.Serialize<List<Item>>(lista);

            //     HttpContext.Session.SetString("Carrinho", carrinho);

            //     return RedirectToAction("Index");
            // }
        }

        [HttpGet]
        public IActionResult Remover(int id)
        {
            //List<Item> lista = new List<Item>();

            Pedido pedido = new Pedido();

            var carrinho = HttpContext.Session.GetString("Carrinho");

            if (carrinho != null)
            {
                //TODO Converter String para Lista(Json)
                pedido = System.Text.Json.JsonSerializer.Deserialize<Pedido>(carrinho);
            }

            using (var data = new ProdutoData())
            {
                var item = pedido.Itens.SingleOrDefault(i => i.Produto.IdProduto == id);

                pedido.Itens.Remove(item);

                //TODO Converter Lista para String (Json)
                carrinho = System.Text.Json.JsonSerializer.Serialize<Pedido>(pedido);

                HttpContext.Session.SetString("Carrinho", carrinho);

                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public IActionResult Finalizar(IFormCollection cliente)
        {
                /*pega o usuário logado*/
                Cliente user = JsonConvert.DeserializeObject<Cliente>(HttpContext.Session.GetString("user"));


                int id = Convert.ToInt32(user.IdCliente);

                /*pega os itens do carrinho*/
                Pedido pedido = JsonConvert.DeserializeObject<Pedido>(HttpContext.Session.GetString("Carrinho"));

                pedido.IdCliente = id;

                /*salva o pedido*/
                using (var data = new PedidoData())
                    data.Create(pedido);

                HttpContext.Session.Remove("Carrinho");

                return RedirectToAction("Index", "Pedido", new { id = pedido.IdCliente });
        }
    }
}

            
               //var carrinho = HttpContext.Session.GetString("Carrinho");
            // if (carrinho != null)
            // {
            //     //TODO Converter String para Lista(Json)
            //     lista = System.Text.Json.JsonSerializer.Deserialize<List<Item>>(carrinho);
            // }

            // using (var data = new PedidoData())
            //     data.Create(lista);