using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TalaveraWeb.Services;
using TalaveraWeb.Models;
using TalaveraWeb.Models.MiBD;

namespace TalaveraWeb.Controllers
{
    public class SolicitarPellasController : Controller
    {
        TalaveraServices tsvc = new TalaveraServices();
        // GET: SolicitarPellas
        public ActionResult Solicitar(int pLoc)
        {
            HttpContext.Application["Locacion"] = pLoc;
            ViewBag.Loc = pLoc;
            ViewBag.NombreLocActual = tsvc.getNombreSucursal(pLoc);
            ViewBag.lstLocaciones = tsvc.obtenerSucursalesExcepto(pLoc);

            int IdOtraSucursal = pLoc == 1 ? 2:1;
            ViewBag.lstRecervasPellas = tsvc.getPellasPorCargaFrom(IdOtraSucursal);

            ReservaBarroListaSolicitud EP = new ReservaBarroListaSolicitud();
            EP.FechaMovimiento = DateTime.Today;
            EP.lstReservas = tsvc.getPellasPorCargaFrom(IdOtraSucursal);
            return View(EP);
        }

        [HttpPost]
        public ActionResult Solicitar(ReservaBarroListaSolicitud pRBLS)
        {
            ViewBag.Loc = (int)HttpContext.Application["Locacion"];
            int? tmpResponsable = int.Parse(pRBLS.Responsable); //Id de la sucursal a la que se le pidio los recursos.
            string NombreResponsable = tsvc.getNombreSucursal((int)tmpResponsable);
            
            if (ModelState.IsValid)
            {
                List<EntregaPellas> lst = CalculaMovimientos(pRBLS.lstReservas, ViewBag.Loc, (int)tmpResponsable);
                lst.ForEach(x => x.Observacion = pRBLS.Observacion);
                
                //EntregaPellas pEnPe = new EntregaPellas();
                //pEnPe.Responsable = NombreResponsable;
                //pEnPe.TipoMovimiento = "I";
                //pEnPe.Editor = User.Identity.Name;
                //pEnPe.FechaEdicion = DateTime.Now;
                //pEnPe.Locacion = ViewBag.Loc;

                int res = tsvc.addEntregaPellas(lst);                
                if (res >= 1)
                {                    
                    //EntregaPellas EgresoEnPe = new EntregaPellas()
                    //{
                    //    FechaMovimiento = DateTime.Now,
                    //    Responsable = tsvc.getNombreSucursal(ViewBag.Loc),
                    //    TipoMovimiento = "E",
                    //    CantidadPellas = pEnPe.CantidadPellas,
                    //    NumCarga = pEnPe.NumCarga,
                    //    Editor = User.Identity.Name,
                    //    FechaEdicion = DateTime.Now,
                    //    Locacion = tmpResponsable
                    //};
                    //int res2 = tsvc.addEntregaPellas(EgresoEnPe);

                    if(ViewBag.Loc == 1)
                        return RedirectToAction("Index", "LaLuz");
                    else
                        return RedirectToAction("Index", "TreintaYcuatroPte");
                }
            }
            return new HttpStatusCodeResult(404, "No se pudo registrar la solicitud de pellas");
        }

        public List<EntregaPellas> CalculaMovimientos(List<PellasDisponibles> pLst, int pSolicitante, int pProvedor )
        {
            List<EntregaPellas> tmp = new List<EntregaPellas>();

            foreach(var item in pLst)
            {
                if (item.UnidadesSolicitadas != null && item.UnidadesSolicitadas > 0)
                {
                    //Ingresos
                    EntregaPellas EPIngresos = new EntregaPellas();
                    EPIngresos.FechaMovimiento = DateTime.Now;
                    EPIngresos.Responsable = tsvc.getNombreSucursal((int)pProvedor);
                    EPIngresos.CantidadPellas = item.UnidadesSolicitadas;
                    EPIngresos.NumCarga = item.NumeroCarga;
                    EPIngresos.TipoMovimiento = "I";
                    EPIngresos.Locacion = pSolicitante;
                    EPIngresos.Editor = User.Identity.Name;
                    EPIngresos.FechaEdicion = DateTime.Now;
                    tmp.Add(EPIngresos);

                    //Egresos
                    EntregaPellas EgresoEnPe = new EntregaPellas()
                    {
                        FechaMovimiento = DateTime.Now,
                        Responsable = tsvc.getNombreSucursal(pSolicitante),
                        TipoMovimiento = "E",
                        CantidadPellas = item.UnidadesSolicitadas,
                        NumCarga = item.NumeroCarga,
                        Locacion = pProvedor,
                        Editor = User.Identity.Name,
                        FechaEdicion = DateTime.Now
                    };
                    tmp.Add(EgresoEnPe);
                }                   
            }

            return tmp;
        }

        //Retorna un historial de movimientos en las pellas
        public ActionResult Movimientos(int pLoc)
        {
            ViewBag.Loc = pLoc;
            ViewBag.NombreLocActual = tsvc.getNombreSucursal(pLoc);

            List<EntregaPellas> lst = tsvc.calculaMovimientosEntregaPellas(pLoc);
            int? Positivo = lst.Where(y => y.TipoMovimiento == "I").Sum(x => x.CantidadPellas);
            int? Negativo = lst.Where(y => y.TipoMovimiento == "E").Sum(x => x.CantidadPellas);
            @ViewBag.Total = (Positivo != null ? Positivo : 0) - (Negativo != null ? Negativo : 0);
            return View(lst);
        }


        //Entrega de pella a trabajadores para crear jahuetes.
        public ActionResult Entregar(int pLoc)
        {
            HttpContext.Application["Locacion"] = pLoc;
            ViewBag.Loc = pLoc;
            ViewBag.NombreLocActual = tsvc.getNombreSucursal(pLoc);

            //ViewBag.lstRecervasPellas = tsvc.getReservasPellasFrom(pLoc);

            ReservaBarroListaSolicitud EP = new ReservaBarroListaSolicitud();
            EP.FechaMovimiento = DateTime.Today;
            EP.lstReservas = tsvc.getPellasPorCargaFrom(pLoc);

            return View(EP);
        }

        [HttpPost]
        public ActionResult Entregar(ReservaBarroListaSolicitud pRBLS)
        {
            ViewBag.Loc = (int)HttpContext.Application["Locacion"];
                        
            if (ModelState.IsValid)
            {
                List<EntregaPellas> tmp = new List<EntregaPellas>();
                foreach (var item in pRBLS.lstReservas)
                {
                    if (item.UnidadesSolicitadas != null && item.UnidadesSolicitadas > 0)
                    {
                        //Egresos
                        EntregaPellas EgresoEnPe = new EntregaPellas()
                        {
                            FechaMovimiento = DateTime.Now,
                            Responsable = pRBLS.Responsable,
                            TipoMovimiento = "E",
                            CantidadPellas = item.UnidadesSolicitadas,
                            NumCarga = item.NumeroCarga,
                            Locacion = ViewBag.Loc,
                            Editor = User.Identity.Name,
                            FechaEdicion = DateTime.Now,
                            Observacion = pRBLS.Observacion
                        };
                        tmp.Add(EgresoEnPe);
                    }                    
                }
                
                int res = tsvc.addEntregaPellas(tmp);
                if (res >= 1)
                {
                    if (ViewBag.Loc == 1)
                        return RedirectToAction("Index", "LaLuz");
                    else
                        return RedirectToAction("Index", "TreintaYcuatroPte");
                }
            }
            return new HttpStatusCodeResult(404, "No se pudo registrar la solicitud de pellas");
        }
        
        public ActionResult ReporteEntrega(int pLoc)
        {
            ViewBag.Loc = pLoc;
            ViewBag.lstEntrega = tsvc.listEngregaPellas(pLoc);
            return View();
        }
    }
}