using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TalaveraWeb.Models;
using TalaveraWeb.Models.MiBD;
using TalaveraWeb.Services;

namespace TalaveraWeb.Controllers
{
    public class PreparacionBarroController : Controller
    {
        TalaveraServices tsvc = new TalaveraServices();
        // GET: PreparacionBarro
        public ActionResult Index(int pLoc = 1)
        {
            HttpContext.Application["Locacion"] = pLoc;
            ViewBag.Loc = pLoc;
            List<PreparacionBarro> lstPreBar = tsvc.obtenerPreparacionBarro(pLoc);
            ViewBag.NombreLocacion = tsvc.getNombreSucursal(pLoc);            
            return View(lstPreBar);
        }

        // GET: PreparacionBarro/Details/5
        public ActionResult Details(int id)
        {
            ViewBag.Loc = (int)HttpContext.Application["Locacion"];
            PreparacionBarro PreBar = tsvc.detallePreparacionBarro(id);            
            return View(PreBar);
        }

        // GET: PreparacionBarro/Create
        public ActionResult Create()
        {
            ViewBag.Loc = (int)HttpContext.Application["Locacion"];
            ViewBag.NombreLocacion = tsvc.getNombreSucursal(ViewBag.Loc);
            List<ReservaBarro> lstReservas = tsvc.getReservasFrom(ViewBag.Loc);
            ViewBag.lstLuz = lstReservas;            
            ViewBag.lstLocaciones = tsvc.obtenerSucursales(ViewBag.Loc);
            
            PreparacionBarroConsumo PreBar = new PreparacionBarroConsumo();
            PreBar.FechaPreparacion = DateTime.Today;
            PreBar.NumPreparado = tsvc.getNumeroPreparado(ViewBag.Loc);
            PreBar.BarroBlanco = 105;
            PreBar.BarroNegro = 150;
            PreBar.Locacion = ViewBag.Loc;
            PreBar.lstConsumoBarroNegro = lstReservas.Where(x => x.Tipo == "Negro")
                                    .Select(y => new ReservaBarroPreparado()
                                    {
                                        CodigoBarro = y.CodigoBarro,
                                        Tipo = y.Tipo,
                                        Capacidad = y.Capacidad,
                                        Unidades = y.Unidades,
                                        TotalKg = y.TotalKg
                                    }).ToList();

            PreBar.lstConsumoBarroBlanco = lstReservas.Where(x => x.Tipo == "Blanco")
                                    .Select(y => new ReservaBarroPreparado()
                                    {
                                        CodigoBarro = y.CodigoBarro,
                                        Tipo = y.Tipo,
                                        Capacidad = y.Capacidad,
                                        Unidades = y.Unidades,
                                        TotalKg = y.TotalKg
                                    }).ToList();

            return View(PreBar);
        }

        // POST: PreparacionBarro/Create
        [HttpPost]
        public ActionResult Create(PreparacionBarroConsumo pPreBar)
        {
            ViewBag.Loc = (int)HttpContext.Application["Locacion"];
            if (ModelState.IsValid)
            {                
                double? tmpBarNeg = pPreBar.BarroNegro;
                double? SumBN = pPreBar.lstConsumoBarroNegro.Select(x => new { TotalSolicitado = x.Capacidad * x.BarroUsado }).Sum(x => x.TotalSolicitado);
                double? SumBB = pPreBar.lstConsumoBarroBlanco.Select(x => new { TotalSolicitado = x.Capacidad * x.BarroUsado }).Sum(x => x.TotalSolicitado);

                List<BarroMovimientos> lst = new List<BarroMovimientos>();
                pPreBar.Estado = "Disponible";
                PreparacionBarro PB = new PreparacionBarro()
                {
                    Id = pPreBar.Id,
                    FechaPreparacion = pPreBar.FechaPreparacion,
                    NumPreparado = pPreBar.NumPreparado,
                    BarroNegro = pPreBar.BarroNegro,
                    BarroBlanco = pPreBar.BarroBlanco,
                    Recuperado = pPreBar.Recuperado,
                    EnPiedra = pPreBar.EnPiedra,
                    TiempoAgitacion = pPreBar.TiempoAgitacion,
                    NumTambos = pPreBar.NumTambos,
                    DesperdicioMojado = pPreBar.DesperdicioMojado,
                    Comentario = pPreBar.Comentario,
                    Estado = pPreBar.Estado,
                    Locacion = pPreBar.Locacion,
                    Editor = User.Identity.Name,
                    FechaEdicion = DateTime.Now
                };
                
                //Si las recercas asignadas para cubrir el barro Negro solicitado son mayores, se cubre el pedido, en caso contrario se solicita asignar mas recervas.
                if (pPreBar.BarroNegro <= SumBN)
                {
                    //Si las recercas asignadas para cubrir el barro Blanco solicitado son mayores, se cubre el pedido, en caso contrario se solicita asignar mas recervas.
                    if (pPreBar.BarroBlanco <= SumBB)
                    {                        
                        int idPreparacion = tsvc.addPreparacionBarro(PB);
                        if (idPreparacion != -1)
                        {
                            var lstBarMovNegro = calculaBarroMovimientos(pPreBar.lstConsumoBarroNegro, (int)pPreBar.BarroNegro, PB, idPreparacion);
                            lst.AddRange(lstBarMovNegro);

                            var lstBarMovBlanco = calculaBarroMovimientos(pPreBar.lstConsumoBarroBlanco, (int)pPreBar.BarroBlanco, PB, idPreparacion);
                            lst.AddRange(lstBarMovBlanco);

                            List<BarroMovimientos> lstVariaciones = calculaBarroMovimientosVariaciones(PB, idPreparacion);

                            //Por ultimo se registran los movimientos en BD.
                            int res2 = tsvc.addMovimientosBarro(lst);
                            if (res2 > 0)
                            {
                                if (lstVariaciones.Count > 0)
                                {
                                    int res3 = tsvc.addMovimientosBarro(lstVariaciones);
                                    if(res3 > 0)
                                        return RedirectToAction("Index", new { pLoc = ViewBag.Loc });
                                }
                                else
                                {
                                    return RedirectToAction("Index", new { pLoc = ViewBag.Loc });
                                }
                            }
                        }
                        else
                        {
                            ViewBag.txtMensaje = "No se pudo crear la preparación, vuelva a intentarlo más tarde.";
                        }                        
                    }
                    else //Como las recervas son menores al barro solicitado, se pide una cantidad mayor de recervas.
                    {
                        ViewBag.txtMensaje = "Es necesario que asignes reservas de barro blanco mayores que el barro blanco solicitado para la preparación.";
                    }
                }
                else //Como las recervas son menores al barro solicitado, se pide una cantidad mayor de recervas.
                {
                    ViewBag.txtMensaje = "Es necesario que asignes reservas de barro mayores que el barro negro solicitado para la preparación.";                    
                }                
            }

            List<ReservaBarro> lstReservas = tsvc.getReservasFrom((int)pPreBar.Locacion);
            ViewBag.lstLuz = lstReservas;
            ViewBag.lstLocaciones = tsvc.obtenerSucursales(ViewBag.Loc);
            return View(pPreBar);
        }

        public List<BarroMovimientos> calculaBarroMovimientos(List<ReservaBarroPreparado> plstBarroReservaAsignado, double pBarroSolicitado, PreparacionBarro pPreBar, int idPreparacion)
        {
            List<BarroMovimientos> lst = new List<BarroMovimientos>();
            double? SumBN = plstBarroReservaAsignado.Select(x => new { TotalSolicitado = x.Capacidad * x.BarroUsado }).Sum(x => x.TotalSolicitado);

            //foreach (var item in pPreBar.lstConsumoBarroNegro.OrderBy(x => x.CodigoBarro))
            for (int a = 0; a < plstBarroReservaAsignado.Count; a++)
            {
                var item = plstBarroReservaAsignado[a];
                if (item.BarroUsado > 0)
                {
                    //En caso de ser a granel, se hace una resta directa
                    if (item.Capacidad == 1)
                    {
                        double TotalKgUsados = item.BarroUsado * (double)item.Capacidad;
                        pBarroSolicitado = pBarroSolicitado - TotalKgUsados;

                        //Se hace el egreso de barro a granel.
                        //Nota: en el barro a granel "NO" es nesario hacer un ingreso con el sobrante del barro asignado.
                        BarroMovimientos tmpMovB_egNegro = new BarroMovimientos()
                        {
                            CodigoProducto = item.CodigoBarro,
                            FechaMovimiento = DateTime.Today,
                            TipoMovimiento = "Eg",
                            Unidades = item.BarroUsado,
                            Locacion = pPreBar.Locacion,
                            OrigenTransferencia = idPreparacion,
                            OrigenTabla = "PreparacionBarro",
                            PesoTotal = TotalKgUsados,
                            Editor = pPreBar.Editor,
                            FechaEdicion = pPreBar.FechaEdicion
                    };
                        lst.Add(tmpMovB_egNegro);
                    }
                    else  //Para los paquete se va haciendo gradual la resta, para determinar cuantos paquetes son necesrios.
                    {
                        double empaquetadosNecesarios = 0;
                        double TotalKgUsados = 0;
                        //Se ira sumando paquete a paquete hasta cubrir el barro solicitado o agotar las recervas asignadas(BarroUsado).
                        for (int i = 1; i <= item.BarroUsado; i++)
                        {
                            empaquetadosNecesarios = i;
                            pBarroSolicitado = pBarroSolicitado - (double)item.Capacidad;
                            if (pBarroSolicitado <= 0)
                                break;
                        }
                        TotalKgUsados = empaquetadosNecesarios * (double)item.Capacidad;

                        //Se genera el egreso de estas empaquetados.
                        BarroMovimientos tmpMovB_egNegro = new BarroMovimientos()
                        {
                            CodigoProducto = item.CodigoBarro,
                            FechaMovimiento = DateTime.Today,
                            TipoMovimiento = "Eg",
                            Unidades = empaquetadosNecesarios,
                            Locacion = pPreBar.Locacion,
                            OrigenTransferencia = idPreparacion,
                            OrigenTabla = "PreparacionBarro",
                            PesoTotal = TotalKgUsados,
                            Editor = pPreBar.Editor,
                            FechaEdicion = pPreBar.FechaEdicion
                        };
                        lst.Add(tmpMovB_egNegro);

                        //Cuando tmpBarNegro es negativo y dado que estamos en un segmento validador de paquetes
                        //se asume que el restante es debido a que un paquete tubo que ser habierto para cubrir el barro solicitado
                        //por esta razon, el restante sera un ingreso a granel en barromovimientos para no peder el restante del paquete habierto.
                        if (pBarroSolicitado < 0)
                        {
                            BarroMovimientos tmpMovB_inNegro = new BarroMovimientos()
                            {
                                CodigoProducto = tsvc.obtenerCodigoDeProducto(item.Tipo, 1),
                                FechaMovimiento = DateTime.Now,
                                TipoMovimiento = "In",
                                Unidades = pBarroSolicitado * -1,
                                Locacion = pPreBar.Locacion,
                                OrigenTransferencia = idPreparacion,
                                OrigenTabla = "PreparacionBarro",
                                PesoTotal = pBarroSolicitado * -1,
                                Editor = pPreBar.Editor,
                                FechaEdicion = pPreBar.FechaEdicion                                
                            };
                            lst.Add(tmpMovB_inNegro);
                        }
                    }
                }
            }

            return lst;
        }

        public List<BarroMovimientos> calculaBarroMovimientosVariaciones(PreparacionBarro pPB, int idPreparacion)
        {
            List<BarroMovimientos> tmpPB = new List<BarroMovimientos>();
            
            if (!string.IsNullOrEmpty(pPB.EnPiedra))
            {
                tmpPB.Add(new BarroMovimientos()
                {
                    CodigoProducto = "B1",
                    FechaMovimiento = DateTime.Now,
                    TipoMovimiento = "Eg",
                    Unidades = Double.Parse(pPB.EnPiedra),
                    Locacion = pPB.Locacion,
                    PesoTotal = Double.Parse(pPB.EnPiedra),
                    OrigenTransferencia = idPreparacion,
                    OrigenTabla = "PreparacionBarro",
                    OrigenVariacion = "EnPiedra",
                    Editor = pPB.Editor,
                    FechaEdicion = DateTime.Now
                });
            }
            if (!string.IsNullOrEmpty(pPB.DesperdicioMojado))
            {
                double tmpDesperdicio = double.Parse(pPB.DesperdicioMojado) / 2;
                tmpPB.Add(new BarroMovimientos()
                {
                    CodigoProducto = "N1",
                    FechaMovimiento = DateTime.Now,
                    TipoMovimiento = "Eg",
                    Unidades = tmpDesperdicio,
                    Locacion = pPB.Locacion,
                    PesoTotal = tmpDesperdicio,
                    OrigenTransferencia = idPreparacion,
                    OrigenTabla = "PreparacionBarro",
                    OrigenVariacion = "DesperdicioMojado",
                    Editor = pPB.Editor,
                    FechaEdicion = DateTime.Now
                });
                tmpPB.Add(new BarroMovimientos()
                {
                    CodigoProducto = "B1",
                    FechaMovimiento = DateTime.Now,
                    TipoMovimiento = "Eg",
                    Unidades = tmpDesperdicio,
                    Locacion = pPB.Locacion,
                    PesoTotal = tmpDesperdicio,
                    OrigenTransferencia = idPreparacion,
                    OrigenTabla = "PreparacionBarro",
                    OrigenVariacion = "DesperdicioMojado",
                    Editor = pPB.Editor,
                    FechaEdicion = DateTime.Now
                });
            }
            if (pPB.Recuperado != null || pPB.Recuperado != 0)
            {

            }

            return tmpPB;
        }

        [HttpPost]
        public JsonResult ActualizarRecervasFrom(int pLocacion, DateTime pFecha, int pId)
        {
            //Este accion es para consumo interno de Edit.
            List<ReservaBarro> lstRes = tsvc.getReservasEnKgFrom(pLocacion, pFecha, pId);
            return Json(lstRes);
        }

        // GET: PreparacionBarro/Edit/5
        public ActionResult Edit(int id)
        {
            ViewBag.Loc = (int)HttpContext.Application["Locacion"];
            PreparacionBarroConsumo PreBarCon = tsvc.detallePreparacionBarroConsumo(id);            
            ViewBag.lstRecervas = tsvc.getReservasFrom((int)PreBarCon.Locacion);
            ViewBag.lstLocaciones = tsvc.obtenerSucursales(ViewBag.Loc);
            return View(PreBarCon);
        }

        // POST: PreparacionBarro/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, PreparacionBarroConsumo pPreBar)
        {
            ViewBag.Loc = (int)HttpContext.Application["Locacion"];
            if (ModelState.IsValid)
            {
                ViewBag.Loc = (int)HttpContext.Application["Locacion"];
                double? SumBN = pPreBar.lstConsumoBarroNegro.Select(x => new { TotalSolicitado = x.Capacidad * x.BarroUsado }).Sum(x => x.TotalSolicitado);
                double? SumBB = pPreBar.lstConsumoBarroBlanco.Select(x => new { TotalSolicitado = x.Capacidad * x.BarroUsado }).Sum(x => x.TotalSolicitado);

                List<BarroMovimientos> lst = new List<BarroMovimientos>();
                pPreBar.Estado = "Disponible";
                PreparacionBarro PB = new PreparacionBarro()
                {
                    Id = pPreBar.Id,
                    FechaPreparacion = pPreBar.FechaPreparacion,
                    NumPreparado = pPreBar.NumPreparado,
                    BarroNegro = pPreBar.BarroNegro,
                    BarroBlanco = pPreBar.BarroBlanco,
                    Recuperado = pPreBar.Recuperado,
                    EnPiedra = pPreBar.EnPiedra,
                    TiempoAgitacion = pPreBar.TiempoAgitacion,
                    NumTambos = pPreBar.NumTambos,
                    DesperdicioMojado = pPreBar.DesperdicioMojado,
                    Comentario = pPreBar.Comentario,
                    Estado = pPreBar.Estado,
                    Locacion = pPreBar.Locacion,
                    Editor = User.Identity.Name,
                    FechaEdicion = DateTime.Now
                };

                //Si las recercas asignadas para cubrir el barro Negro solicitado son mayores, se cubre el pedido, en caso contrario se solicita asignar mas recervas.
                if (pPreBar.BarroNegro <= SumBN)
                {
                    //Si las recercas asignadas para cubrir el barro Blanco solicitado son mayores, se cubre el pedido, en caso contrario se solicita asignar mas recervas.
                    if (pPreBar.BarroBlanco <= SumBB)
                    {
                        int idPreparacion = tsvc.editPreparacionBarro(PB);
                        if (idPreparacion != -1)
                        {
                            int elemBorrados = tsvc.borrarMovimientosBarroDerivadosDePreparacionBarro(PB.Id);

                            var lstBarMovNegro = calculaBarroMovimientos(pPreBar.lstConsumoBarroNegro, (int)pPreBar.BarroNegro, PB, idPreparacion);
                            lst.AddRange(lstBarMovNegro);

                            var lstBarMovBlanco = calculaBarroMovimientos(pPreBar.lstConsumoBarroBlanco, (int)pPreBar.BarroBlanco, PB, idPreparacion);
                            lst.AddRange(lstBarMovBlanco);

                            List<BarroMovimientos> lstVariaciones = calculaBarroMovimientosVariaciones(PB, idPreparacion);

                            //Por ultimo se registran los movimientos en BD.
                            int res2 = tsvc.addMovimientosBarro(lst);
                            if (res2 > 0)
                            {
                                if (lstVariaciones.Count > 0)
                                {
                                    int res3 = tsvc.addMovimientosBarro(lstVariaciones);
                                    if (res3 > 0)
                                        return RedirectToAction("Index", new { pLoc = ViewBag.Loc });
                                }
                            }
                        }
                        else
                        {
                            ViewBag.txtMensaje = "No se pudo crear la preparación, vuelva a intentarlo más tarde.";
                        }
                    }
                    else //Como las recervas son menores al barro solicitado, se pide una cantidad mayor de recervas.
                    {
                        ViewBag.txtMensaje = "Es necesario que asignes reservas de barro blanco mayores que el barro blanco solicitado para la preparación.";
                    }
                }
                else //Como las recervas son menores al barro solicitado, se pide una cantidad mayor de recervas.
                {
                    ViewBag.txtMensaje = "Es necesario que asignes reservas de barro mayores que el barro negro solicitado para la preparación.";
                }              
            }

            ViewBag.lstRecervas = tsvc.getReservasFrom((int)pPreBar.Locacion);
            ViewBag.lstLocaciones = tsvc.obtenerSucursales(ViewBag.Loc);
            return View(pPreBar);
        }

        // GET: PreparacionBarro/Delete/5
        public ActionResult Delete(int id)
        {
            PreparacionBarro PreBar = tsvc.detallePreparacionBarro(id);
            return View(PreBar);
        }

        // POST: PreparacionBarro/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            ViewBag.Loc = (int)HttpContext.Application["Locacion"];
            int res = tsvc.deletePreparacionBarro(id);
            if(res > 0)
            {
                int elemBorrados = tsvc.borrarMovimientosBarroDerivadosDePreparacionBarro(id);
                return RedirectToAction("Index", new { pLoc = ViewBag.Loc });
            }
            else
            {
                return View(new HttpStatusCodeResult(202, "No se pudo realizar el borrado, intentelo mas tarde"));
            }
        }
    }
}
