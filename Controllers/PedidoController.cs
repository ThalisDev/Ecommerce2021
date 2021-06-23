
using Ecommerce2021a.Data;
using Ecommerce2021a.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ecommerce2021a.Controllers
{
    public class PedidoController : Controller
    {
        public IActionResult Index(int id)
        {
            // consulta todos os pedidos de um cliente
            using (var data = new PedidoData())
            return View(data.Read(id));
        }
        public IActionResult TodosPedidos()
        {
            //consulta todos os pedidos de todos os clientes
            using (var data = new PedidoData())
                return View(data.Read());
        }
    }
}
