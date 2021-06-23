using Ecommerce2021a.Data;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ecommerce2021a.Controllers
{
    public class ItemController : Controller
    {
         [HttpGet]
        public IActionResult Index(int id)
        {
            // consulta todos os items de um pedido pelo id
            using (var data = new ItemData())
            return View(data.Read(id));
           
        }
    }
}
