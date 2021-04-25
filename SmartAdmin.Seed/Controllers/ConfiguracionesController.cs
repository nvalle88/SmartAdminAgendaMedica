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
    public class ConfiguracionesController : Controller
    {
        public IConfiguration Configuration { get; }

        public ConfiguracionesController(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        #region Pedidos Actuales

        [Authorize(Policy = "Administrador")]
        public IActionResult General()
        {
            return View();
        }

        #endregion
    }
}
