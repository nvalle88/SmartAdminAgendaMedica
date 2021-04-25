#region Using

using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using SistemaPedidos.Utilidades;
using SistemaPedidos.Utils;

#endregion

namespace SmartAdminSaludsa.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        public IConfiguration Configuration { get; }

        public HomeController(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        #region Pedidos Actuales

        //[Authorize(Policy = "Administrador")]
        [Authorize]
        public IActionResult Index()
        {
            
            return View();
        }

        #endregion
    }
}
