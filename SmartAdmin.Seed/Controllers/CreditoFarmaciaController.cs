#region Using

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using SistemaPedidos.Utilidades;
using SistemaPedidos.Utils;
using SmartAdmin.Seed.BaseDatos.ContextoBaseDatos;
using SmartAdmin.Seed.Models.DTO;
using SmartAdmin.Seed.ModelsSaludsa;
using SmartAdminSaludsa.Extensores;

#endregion

namespace SmartAdminSaludsa.Controllers
{
    [Authorize]
    public class CreditoFarmaciaController : Controller
    {
        public IConfiguration Configuration { get; }

        private readonly SmartAdmin.Seed.BaseDatos.ContextoBaseDatos.SaludsaContext db;

        public CreditoFarmaciaController(IConfiguration configuration, SmartAdmin.Seed.BaseDatos.ContextoBaseDatos.SaludsaContext context)
        {
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
                var listaDb = await db.CfsolicitudRechazo.Where(x => x.NumeroDocumento.Equals(numeroDocumento)).Include(x=>x.CfdiagnosticoRechazo).Include(x=>x.CfmedicamentoRechazo).OrderByDescending(x => x.Id).ToListAsync();
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
                    IdSolicitud=x.IdSolicitud,
                    CodigoContrato=x.CodigoContrato,
                    Id=x.Id,
                    NumeroPersona=x.NumeroPersona,
                    Observacion=x.Observacion,
                    TipoDocumento=x.TipoDocumento,
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
                var listaSalidsa = await db.Cfingreso.Where(x =>  x.IdSolicitudNavigation.NumeroDocumento.Equals(numeroDocumento)).OrderByDescending(x => x.Id).ToListAsync();

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


                var credito = await db.Cfingreso.Where(x => x.Id.Equals(id) && x.IdSolicitudNavigation.NumeroConvenio.Equals(52876)).FirstOrDefaultAsync();
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
                credito.ObservacionAnulacion = "Anulado por el portal de Saludsa";

                await db.SaveChangesAsync();

                return new JsonResult(new RespuestaGenerica
                {
                    Estado = Respuesta.OK,
                    Resultado = $"Registro anulado correctamente.",
                    Mensaje = $"Registro anulado correctamente.",
                });
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
