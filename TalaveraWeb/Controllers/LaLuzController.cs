using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TalaveraWeb.Services;
using TalaveraWeb.Models;

namespace TalaveraWeb.Controllers
{
    public class LaLuzController : Controller
    {
        TalaveraServices tsvc = new TalaveraServices();
        // GET: LaLuz
        public ActionResult Index()
        {
            ViewBag.lstBarroGranelLuz = tsvc.getReservasBarroGranelFrom(1);
            ViewBag.lstBarroEmpaqueLuz = tsvc.getReservasBarroEmpaqueFrom(1);            
            return View();
        }

        public ActionResult SolicitarBarroEmpaque(int Sucursal)
        {
            ViewBag.lstBarroEmpaque34pte = tsvc.getReservasBarroEmpaqueFrom(2);            
            return View();
        }

        [HttpPost]
        public ActionResult SolicitarBarroEmpaque(string[] Tipo, int[] Capacidad, string[] barroSolicitado)
        {
            List<BarroMovimientos> lst = new List<BarroMovimientos>();

            for(int i = 0; i < Tipo.Length; i++)
            {
                BarroMovimientos bmEg = new BarroMovimientos() {
                    CodigoProducto = Tipo[i].Substring(0,1) + Capacidad[i],
                    FechaMovimiento = DateTime.Today,
                    TipoMovimiento = "Eg",
                    Unidades = int.Parse(barroSolicitado[i]),
                    Locacion = 2,
                    PesoTotal = Capacidad[i] * int.Parse(barroSolicitado[i])
                };
                lst.Add(bmEg);

                BarroMovimientos bmIn = new BarroMovimientos()
                {
                    CodigoProducto = Tipo[i].Substring(0, 1) + Capacidad[i],
                    FechaMovimiento = DateTime.Today,
                    TipoMovimiento = "In",
                    Unidades = int.Parse(barroSolicitado[i]),
                    Locacion = 1,
                    OrigenTransferencia = 2,
                    OrigenTabla = "Sucursales",
                    PesoTotal = Capacidad[i] * int.Parse(barroSolicitado[i])
                };
                lst.Add(bmIn);
            }

            int res = tsvc.addMovimientosBarro(lst);

            if (res > 1)
                return RedirectToAction("Index");
            else
                return View(new HttpStatusCodeResult(201, "No fue posible realizar la peticion."));
        }
    }
}