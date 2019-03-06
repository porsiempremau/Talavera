﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TalaveraWeb.Models;
using TalaveraWeb.Models.MiBD;
using TalaveraWeb.Services;

namespace TalaveraWeb.Controllers
{
    [Authorize(Roles = "Administrador, Usuario")]
    public class TreintaYCuatroPteController : Controller
    {
        TalaveraServices tsvc = new TalaveraServices();
        // GET: TreintaYCuatroPte
        public ActionResult Index()
        {
            HttpContext.Application["Locacion"] = 2;
            ViewBag.lstBarroGranel34pte = tsvc.getReservasBarroGranelFrom(2);
            ViewBag.lstBarroEmpaque34pte = tsvc.getReservasBarroEmpaqueFrom(2);
            ViewBag.lstPellas = tsvc.getReservasPellasFrom(2);

            //Para agregar la suma de granel y empaquetado en Kg.
            foreach (var item in ViewBag.lstBarroGranel34pte)
            {
                foreach (var elem in ViewBag.lstBarroEmpaque34pte)
                {
                    if (item.Tipo == elem.Tipo)
                    {
                        ViewData[item.Tipo] = item.TotalKg + elem.TotalKg;
                    }
                }
            }

            return View();
        }

        public ActionResult SolicitarBarroGranel()
        {            
            ViewBag.Productos = tsvc.obtenerProductos(1);
            ViewBag.Provedores = tsvc.obtenerProvedores();
            ViewBag.Locaciones = tsvc.obtenerSucursales(2);
            BarroMovimientos bm = new BarroMovimientos();
            bm.FechaMovimiento = DateTime.Today;
            return View(bm);
        }

        // POST: MovimientosBarro/Create
        [HttpPost]
        public ActionResult SolicitarBarroGranel(BarroMovimientos pMovB)
        {            
            if (ModelState.IsValid)
            {
                pMovB.TipoMovimiento = "In";
                pMovB.PesoTotal = pMovB.Unidades;
                pMovB.OrigenTabla = "Provedores";
                pMovB.Editor = User.Identity.Name;
                pMovB.FechaEdicion = DateTime.Now;

                int res = tsvc.addMovimientosBarro(pMovB);
                if (res == 1)
                {
                    return RedirectToAction("Index");
                }
            }
            return View(new HttpStatusCodeResult(202, "No pudo ser generada la solicitud."));
        }

        public ActionResult Historial()
        {
            ViewBag.Loc = (int)HttpContext.Application["Locacion"];

            List<BarroMovimientos> HistBarroGranel = tsvc.obtenerHistorialGranel(ViewBag.Loc);
            ViewBag.NombreLocActual = tsvc.getNombreSucursal(ViewBag.Loc);
            ViewBag.Provedores = tsvc.obtenerProvedores();

            return View(HistBarroGranel);
        }

        
        public ActionResult Edit(int id)
        {            
            BarroMovimientos bm = tsvc.getBarroMovimiento(id);
            ViewBag.Productos = tsvc.obtenerProductos(1);
            foreach(var item in ViewBag.Productos)
            {
                item.Selected = false;
                if (item.Value == bm.CodigoProducto)
                    item.Selected = true;
            }
            ViewBag.Provedores = tsvc.obtenerProvedores(bm.OrigenTransferencia);
            return View(bm);
        }

        [HttpPost]
        public ActionResult Edit(BarroMovimientos bm)
        {
            bm.PesoTotal = bm.Unidades;
            bm.Editor = User.Identity.Name;
            bm.FechaEdicion = DateTime.Now;
            
            int res = tsvc.editMovimientoBarro(bm);
            if (res > 0)
            {                
                return RedirectToAction("Index");
            }
            return new HttpStatusCodeResult(404, "No se pudo actualizar el registro, vuelva a intentarlo mas tarde");
        }

        public ActionResult Delete(int id)
        {
            ViewBag.Loc = (int)HttpContext.Application["Locacion"];            
            BarroMovimientos bm = tsvc.getBarroMovimiento(id);
            ViewBag.Provedores = tsvc.obtenerProvedores(bm.OrigenTransferencia);
            ViewBag.ProveedorNombre = ViewBag.Provedores[0].Text;
            return View(bm);
        }

        [HttpPost]
        public ActionResult Delete(int id, BarroMovimientos pPrePell)
        {
            int res = tsvc.deleteBarroMovimiento(id);
            if (res >= 1)
            {               
                return RedirectToAction("Index");
            }
            return new HttpStatusCodeResult(404, "No se pudo borrar el elemento, vuelva a interntar mas tarde.");
        }


        public ActionResult SolicitarBarroEmpaque()
        {
            ViewBag.lstBarroGranel34pte = tsvc.getReservasBarroGranelFrom(2);
            ViewData["Negro"] = tsvc.obtenerProductosDeTipo("Negro");
            ViewData["Blanco"] = tsvc.obtenerProductosDeTipo("Blanco");
            return View();
        }

        [HttpPost]
        public ActionResult SolicitarBarroEmpaque(string[] Tipo, int[] Capacidad, string[] CodigoProducto, string[] barroSolicitado)
        {
            List<BarroMovimientos> lst = new List<BarroMovimientos>();

            for (int i = 0; i < Tipo.Length; i++)
            {
                if(barroSolicitado[i] != null && barroSolicitado[i] != "")
                {
                    int PesoEmpaque = int.Parse(CodigoProducto[i].Replace("N", "").Replace("B", ""));
                    string codigoGranel = tsvc.obtenerCodigoDeProducto(Tipo[i], 1);
                    
                    BarroMovimientos bmEg = new BarroMovimientos()
                    {
                        CodigoProducto = codigoGranel, //Tipo[i].Substring(0, 1) + Capacidad[i],
                        FechaMovimiento = DateTime.Today,
                        TipoMovimiento = "Eg",
                        Unidades = PesoEmpaque * int.Parse(barroSolicitado[i]),
                        Locacion = 2,
                        OrigenTransferencia = 2,
                        OrigenTabla = "Sucursales",
                        PesoTotal = PesoEmpaque * int.Parse(barroSolicitado[i]),
                        Editor = User.Identity.Name,
                        FechaEdicion = DateTime.Now
                    };
                    lst.Add(bmEg);

                    BarroMovimientos bmIn = new BarroMovimientos()
                    {
                        CodigoProducto = CodigoProducto[i], //Tipo[i].Substring(0, 1) + Capacidad[i],
                        FechaMovimiento = DateTime.Today,
                        TipoMovimiento = "In",
                        Unidades = int.Parse(barroSolicitado[i]),
                        Locacion = 2,
                        OrigenTransferencia = 2,
                        OrigenTabla = "Sucursales",
                        PesoTotal = PesoEmpaque * int.Parse(barroSolicitado[i]),
                        Editor = User.Identity.Name,
                        FechaEdicion = DateTime.Now
                    };
                    lst.Add(bmIn);
                }                
            }

            int res = tsvc.addMovimientosBarro(lst);

            if (res > 1)
                return RedirectToAction("Index");
            else
                return View(new HttpStatusCodeResult(201, "No fue posible realizar la peticion."));
        }


    }
}