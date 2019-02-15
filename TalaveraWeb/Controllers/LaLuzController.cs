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
    [Authorize(Roles = "Administrador, Usuario")]
    public class LaLuzController : Controller
    {
        TalaveraServices tsvc = new TalaveraServices();
        // GET: LaLuz
        public ActionResult Index()
        {
            HttpContext.Application["Locacion"] = 1;
            ViewBag.lstBarroGranelLuz = tsvc.getReservasBarroGranelFrom(1);
            ViewBag.lstBarroEmpaqueLuz = tsvc.getReservasBarroEmpaqueFrom(1);
            ViewBag.lstPellas = tsvc.getReservasPellasFrom(1);
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
                    OrigenTransferencia = 1,
                    OrigenTabla = "Sucursales",
                    PesoTotal = Capacidad[i] * int.Parse(barroSolicitado[i]),
                    Editor = User.Identity.Name,
                    FechaEdicion = DateTime.Now
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
                    PesoTotal = Capacidad[i] * int.Parse(barroSolicitado[i]),
                    Editor = User.Identity.Name,
                    FechaEdicion = DateTime.Now
            };
                lst.Add(bmIn);
            }

            int res = tsvc.addMovimientosBarro(lst);

            if (res > 1)
                return RedirectToAction("Index");
            else
                return View(new HttpStatusCodeResult(201, "No fue posible realizar la peticion."));
        }

        public ActionResult HistorialBarroEmpaque()
        {
            List<BarroMovimientos> HistBarroGranel = tsvc.obtenerHistorialEmpaquetados(1);
            ViewBag.NombreLocacion = tsvc.getNombreSucursal(1);
            ViewBag.Provedores = tsvc.getNombreSucursal(2);

            return View(HistBarroGranel);
        }

        public ActionResult EditarRegitro(int id)
        {
            BarroMovimientos bm = tsvc.getBarroMovimiento(id);
            ViewBag.Productos = tsvc.obtenerProductos(1);
            foreach (var item in ViewBag.Productos)
            {
                item.Selected = false;
                if (item.Value == bm.CodigoProducto)
                    item.Selected = true;
            }
            ViewBag.Provedores = tsvc.getNombreSucursal((int)bm.OrigenTransferencia);

            double merma = tsvc.obtenerMermaDePedido(bm);

            BarroMovimientosConMerma BMCM = new BarroMovimientosConMerma();
            BMCM.Merma = merma;
            BMCM.setBarroMovimientos(bm);
            return View(BMCM);
        }

        [HttpPost]
        public ActionResult EditarRegitro(BarroMovimientosConMerma pBMCM)
        {            
            pBMCM.Editor = User.Identity.Name;
            pBMCM.FechaEdicion = DateTime.Now;
            BarroMovimientos bm = pBMCM.getBarroMovimientos();

            int res = tsvc.editMovimientoBarro(bm);
            if (res > 0)
            {
                int res1 = tsvc.deleteMermaExistente(bm);
                if (pBMCM.Merma > 0)
                {
                    List<BarroMovimientos> lst = calculaMovimientosPorMerma(pBMCM);

                    string codigoEnGranel = pBMCM.CodigoProducto.Substring(0, 1) + "1";
                    BarroMovimientos exedenteRegistrado = new BarroMovimientos();
                    exedenteRegistrado.CodigoProducto = codigoEnGranel;
                    exedenteRegistrado.FechaMovimiento = DateTime.Now;
                    exedenteRegistrado.TipoMovimiento = "In";
                    exedenteRegistrado.Unidades = pBMCM.Merma;
                    exedenteRegistrado.Locacion = pBMCM.OrigenTransferencia;
                    exedenteRegistrado.PesoTotal = pBMCM.Merma;
                    exedenteRegistrado.OrigenTransferencia = bm.Id;
                    exedenteRegistrado.OrigenTabla = "BarroMovimiento";
                    exedenteRegistrado.OrigenVariacion = "Merma";
                    exedenteRegistrado.Editor = User.Identity.Name;
                    exedenteRegistrado.FechaEdicion = DateTime.Now;
                                        
                    int res2 = tsvc.addMovimientosBarro(lst);
                    int res3 = tsvc.addMovimientosBarro(exedenteRegistrado);
                }
                    
                return RedirectToAction("Index");
            }
            return new HttpStatusCodeResult(404, "No se pudo actualizar el registro, vuelva a intentarlo mas tarde");
        }

        
        public List<BarroMovimientos> calculaMovimientosPorMerma(BarroMovimientosConMerma pBMCM )
        {
            List<BarroMovimientos> lstTemp = new List<BarroMovimientos>();
            string codigoEnGranel = pBMCM.CodigoProducto.Substring(0, 1) + "1";
            double pesoCostal = double.Parse(pBMCM.CodigoProducto.Remove(0, 1));

            double entero = 0;
            if (pBMCM.Merma > pesoCostal)
                entero = (int)(pBMCM.Merma / pesoCostal);
            entero = entero + 1;   //se suma 1 pues es el costal que se debe descontar debido al residuo, osea el barro en granel que se descuenta a la carga.

            double residuo = pBMCM.Merma;
            residuo %= pesoCostal;
                        
            BarroMovimientos mermaEncostalada = new BarroMovimientos();
            mermaEncostalada.CodigoProducto = pBMCM.CodigoProducto;
            mermaEncostalada.FechaMovimiento = DateTime.Now;
            mermaEncostalada.TipoMovimiento = "Eg";
            mermaEncostalada.Unidades = entero;
            mermaEncostalada.Locacion = 1;
            mermaEncostalada.PesoTotal = entero * pesoCostal;
            mermaEncostalada.OrigenTransferencia = pBMCM.Id;
            mermaEncostalada.OrigenTabla = "BarroMovimiento";
            mermaEncostalada.OrigenVariacion = "Merma";
            mermaEncostalada.Editor = User.Identity.Name;
            mermaEncostalada.FechaEdicion = DateTime.Now;
            lstTemp.Add(mermaEncostalada);
            
            //pesoCostal - residuo => se hace esta resta debido a que el residuo es lo que se le restara a un costal completo para saber cuanto se tiene en realidad.
            BarroMovimientos merma = new BarroMovimientos();
            merma.CodigoProducto = codigoEnGranel;
            merma.FechaMovimiento = DateTime.Now;
            merma.TipoMovimiento = "In";
            merma.Unidades = pesoCostal - residuo;
            merma.Locacion = 1;
            merma.PesoTotal = pesoCostal - residuo;
            merma.OrigenTransferencia = pBMCM.Id;
            merma.OrigenTabla = "BarroMovimiento";
            merma.OrigenVariacion = "Merma";
            merma.Editor = User.Identity.Name;
            merma.FechaEdicion = DateTime.Now;
            lstTemp.Add(merma);

            return lstTemp;
        }

        public ActionResult Delete(int id)
        {
            ViewBag.Loc = (int)HttpContext.Application["Locacion"];
            ViewBag.NombreLocacion = tsvc.getNombreSucursal(ViewBag.Loc);
            BarroMovimientos bm = tsvc.getBarroMovimiento(id);
            ViewBag.Provedores = tsvc.obtenerSucursales(2);
            ViewBag.ProveedorNombre = ViewBag.Provedores[0].Text;
            return View(bm);
        }

        [HttpPost]
        public ActionResult Delete(int id, BarroMovimientos pPrePell)
        {
            BarroMovimientos bm = tsvc.getBarroMovimiento(id);
            int res2 = tsvc.deleteMermaExistente(bm);
            int res = tsvc.deleteBarroMovimiento(id);
            if (res >= 1)
            {
                return RedirectToAction("Index");
            }
            return new HttpStatusCodeResult(404, "No se pudo borrar el elemento, vuelva a interntar mas tarde.");
        }

    }
}