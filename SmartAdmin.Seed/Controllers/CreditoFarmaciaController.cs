#region Using

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using SistemaPedidos.Utilidades;
using SistemaPedidos.Utils;
using SmartAdmin.Seed.BaseDatos.ContextoBaseDatos;
using SmartAdmin.Seed.Models.DTO;
using SmartAdmin.Seed.ModelsSaludsa;
using SmartAdminSaludsa.Extensores;
using SmartAdminSaludsa.Models;

#endregion

namespace SmartAdminSaludsa.Controllers
{
    [Authorize]
    public class CreditoFarmaciaController : Controller
    {
        public IConfiguration Configuration { get; }
        private readonly UserManager<ApplicationUser> _userManager;


        private readonly SmartAdmin.Seed.BaseDatos.ContextoBaseDatos.SaludsaContext db;

        public CreditoFarmaciaController(IConfiguration configuration, SmartAdmin.Seed.BaseDatos.ContextoBaseDatos.SaludsaContext context, UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
            Configuration = configuration;
            this.db = context;
        }

        public async Task<IActionResult> Creditos()
        {
            return View();
        }

        public async Task<IActionResult> Negativas()
        {
            return View();
        }

        public async Task<JsonResult> ListaNegativas(string numeroDocumento)
        {
            try
            {
                var listaDb = await db.CfsolicitudRechazo.Where(x => x.NumeroDocumento.Equals(numeroDocumento)).Include(x => x.CfdiagnosticoRechazo).Include(x => x.CfmedicamentoRechazo).OrderByDescending(x => x.Id).ToListAsync();
                var listaSalidsa = listaDb.Select(x => new CfsolicitudRechazo
                {
                    Producto = x.Producto,
                    NumeroDocumento = x.NumeroDocumento,
                    FechaCreacion = x.FechaCreacion,
                    CodigoPlan = x.CodigoPlan,
                    NumeroContrato = x.NumeroContrato,
                    VersionPlan = x.VersionPlan,
                    Region = x.Region,
                    Nivel = x.Nivel,
                    NombrePaciente = x.NombrePaciente,
                    NumeroConvenio = x.NumeroConvenio,
                    RechazoGeneral = x.RechazoGeneral,
                    IdSolicitud = x.IdSolicitud,
                    CodigoContrato = x.CodigoContrato,
                    Id = x.Id,
                    NumeroPersona = x.NumeroPersona,
                    Observacion = x.Observacion,
                    TipoDocumento = x.TipoDocumento,
                    CfdiagnosticoRechazo = x.CfdiagnosticoRechazo.Select(y => new CfdiagnosticoRechazo { CodigoDiagnostico = y.CodigoDiagnostico, DescripcionDiagnostico = y.DescripcionDiagnostico }).ToList(),
                    CfmedicamentoRechazo = x.CfmedicamentoRechazo.Select(y => new CfmedicamentoRechazo { CodigoProducto = y.CodigoProducto, CodigoProductoSalud = y.CodigoProductoSalud, DescripcionProducto = y.DescripcionProducto, ReglaAplicada = y.ReglaAplicada }).ToList(),
                }).ToList();




                return new JsonResult(new RespuestaGenericaPaginada
                {
                    Estado = Respuesta.OK,
                    Resultado = listaSalidsa == null ? new List<CfsolicitudRechazo>() : listaSalidsa,
                    TotalRegistros = listaSalidsa.Count(),
                    Mensaje = "",

                });

            }
            catch (Exception ex)
            {
                Log.WriteLog(TypeError.Error, $"Detalle error:{ex.Serializar()}");

                return new JsonResult(new RespuestaGenericaPaginada
                {
                    Estado = Respuesta.Error,
                    TotalRegistros = 0,
                    Mensaje = ex.Message,
                    Resultado = null,
                });
            }
        }

        public async Task<JsonResult> ListaCreditos(string numeroDocumento)
        {
            try
            {
                var usuario = await _userManager.GetUserAsync(User);

                if (usuario == null)
                {
                    return new JsonResult(new RespuestaGenericaPaginada
                    {
                        Estado = Respuesta.Error,
                        Resultado = new List<Cfingreso>(),
                        TotalRegistros = 0,
                        Mensaje = "No existe el usuario.",

                    });

                }
                var query = db.Cfingreso.Where(x => x.IdSolicitudNavigation.NumeroDocumento.Equals(numeroDocumento));

                if (usuario.NumeroConvenio > 0 && (usuario.Externo ?? true))
                   query= query.Where(x => x.IdSolicitudNavigation.NumeroConvenio.Equals(usuario.NumeroConvenio));

                query = query.OrderByDescending(x => x.Id);
                var listaSalidsa = await query.ToListAsync();

                return new JsonResult(new RespuestaGenericaPaginada
                {
                    Estado = Respuesta.OK,
                    Resultado = listaSalidsa == null ? new List<Cfingreso>() : listaSalidsa,
                    TotalRegistros = listaSalidsa.Count(),
                    Mensaje = "",

                });

            }
            catch (Exception ex)
            {
                Log.WriteLog(TypeError.Error, $"Detalle error:{ex.Serializar()}");

                return new JsonResult(new RespuestaGenericaPaginada
                {
                    Estado = Respuesta.Error,
                    TotalRegistros = 0,
                    Mensaje = ex.Message,
                    Resultado = null,
                });
            }
        }

        public async Task<JsonResult> EliminarCredito(int id)
        {
            try
            {
                if (id <= 0)
                {
                    return new JsonResult(new RespuestaGenerica
                    {
                        Estado = Respuesta.Error,
                        Resultado = "El ID de crédito no es válido.",
                        Mensaje = "El ID de crédito no es válido.",
                    });
                }

                var usuario = await _userManager.GetUserAsync(User);

                if (usuario == null)
                {
                    return new JsonResult(new RespuestaGenerica
                    {
                        Estado = Respuesta.Error,
                        Resultado = "No existe el usuario.",
                        Mensaje = "No existe el usuario.",

                    });

                };

                if (usuario.Externo ?? true)
                {
                    if (usuario.NumeroConvenio == null || usuario.NumeroConvenio <= 0)
                    {
                        return new JsonResult(new RespuestaGenerica
                        {
                            Estado = Respuesta.Error,
                            Resultado = "El usuario externo no tiene asociado un número de convenio. Debe adicionar el número de convenio que corresponde con el prestador con el usuario actual.",
                            Mensaje = "El usuario externo no tiene asociado un número de convenio. Debe adicionar el número de convenio que corresponde con el prestador con el usuario actual.",

                        });
                    }
                    var credito = await db.Cfingreso.Where(x => x.Id.Equals(id) && x.IdSolicitudNavigation.NumeroConvenio.Equals(usuario.NumeroConvenio)).FirstOrDefaultAsync();

                    if (credito == null)
                    {
                        return new JsonResult(new RespuestaGenerica
                        {
                            Estado = Respuesta.Error,
                            Resultado = $"No existe el registro con id [{id}].",
                            Mensaje = $"No existe el registro con id [{id}].",

                        });
                    }
                    credito.Estado = "Anulado-Prestador";
                    credito.FechaAnulacion = DateTime.Now;
                    credito.ObservacionAnulacion = $"Anulado por el portal de Saludsa, usuario: {usuario.Serializar()}";

                    await db.SaveChangesAsync();

                    return new JsonResult(new RespuestaGenerica
                    {
                        Estado = Respuesta.OK,
                        Resultado = $"Registro anulado correctamente.",
                        Mensaje = $"Registro anulado correctamente.",
                    });
                }
                else 
                {
                    return new JsonResult(new RespuestaGenerica
                    {
                        Estado = Respuesta.Error,
                        Resultado = "El usuario no está habilitado como externo. Para realizar esta opción debe habilitar el usuario como externo.",
                        Mensaje = "El usuario no está habilitado como externo. Para realizar esta opción debe habilitar el usuario como externo.",

                    });
                }
            }
            catch (Exception ex)
            {

                return new JsonResult(new RespuestaGenerica
                {
                    Estado = Respuesta.Error,
                    Resultado = $"Error al anular el id de crédito [{id}].({ex.Message})",
                    Mensaje = $"Error al anular el id de crédito [{id}].({ex.Message})",
                });
            }
        }
    }
}
