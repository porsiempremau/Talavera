using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TalaveraWeb.Models;

namespace TalaveraWeb.Services
{
    public class TalaveraServices
    {
        TalaveraEntities db = new TalaveraEntities();

        //PROVEDORES
        public List<Provedores> getProvedores()
        {
            try
            {
                return db.Provedores.ToList();
            }
            catch (Exception ex)
            {
                return null;
            }            
        }

        public int newProvedores(Provedores pProv)
        {
            db.Provedores.Add(pProv);
            int res = db.SaveChanges();
            return res;
        }

        public Provedores getProvedor(int? pId)
        {
            Provedores tmpProv = db.Provedores.Where(x => x.id == pId).FirstOrDefault();
            return tmpProv;
        }

        public int editProvedores(int pId, Provedores pProv)
        {
            db.Entry(pProv).State = EntityState.Modified;
            int res = db.SaveChanges();
            return res;
        }

        public int deleteProvedores(int pId, Provedores pProv)
        {
            try
            {
                Provedores prov = db.Provedores.Find(pId);
                db.Provedores.Remove(prov);
                int res = db.SaveChanges();
                return res;
            }
            catch(Exception ex)
            {                
                return -1;
            }            
        }

        //BARROMAESTRA
        public List<BarroMaestra> getBarroMestra()
        {
            return db.BarroMaestra.ToList();
        }

        public BarroMaestra getBarroMaestra(int pId)
        {
            try
            {
                BarroMaestra res = db.BarroMaestra.Where(x => x.Id == pId).FirstOrDefault();
                return res;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public int addBarroMaestra(BarroMaestra pBarro)
        {
            try
            {
                db.BarroMaestra.Add(pBarro);
                int res = db.SaveChanges();
                return res;
            }
            catch(Exception ex)
            {
                return -1;
            }            
        }

        //Valida si ya existe un producto similar, para no duplicar productos de barro
        public BarroMaestra validaExistenciaBarroMaestra(BarroMaestra pBarro)
        {
            try
            {
                BarroMaestra res = db.BarroMaestra.Where(x => x.CodigoProducto == pBarro.CodigoProducto).FirstOrDefault();                
                return res;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public int deleteBarroMaestra(int pId)
        {
            try
            {
                BarroMaestra barro = db.BarroMaestra.Find(pId);
                db.BarroMaestra.Remove(barro);
                int res = db.SaveChanges();
                return res;
            }
            catch (Exception ex)
            {
                return -1;
            }
        }

        //MOVIMIENTOS BARRO
        public List<ReservaBarro> getReservasFrom(string pLocacion)
        {
            //Obtengo los registros segun el total de ingresos y egresos
            var lst = db.MovimientosBarro
                .Where(x => x.Locacion == pLocacion)
                .GroupBy(y => new { y.TipoMovimiento, y.CodigoProducto })
                .Select(z => new { TipoMovimiento = z.Key, Unidades = z.Sum(s => s.Unidades) })
                .ToList();

            //Obtengo los distintos tipos de productos para hacer las sumas correspondientes.
            var lstTiposBarro = lst.GroupBy(x => x.TipoMovimiento.CodigoProducto).Select(y => y.First());

            var lstBarros = db.BarroMaestra.ToList();

            List<ReservaBarro> lstReservas = new List<ReservaBarro>();
            foreach(var item in lstTiposBarro)
            {
                var Positivo = lst.Where(x => x.TipoMovimiento.CodigoProducto == item.TipoMovimiento.CodigoProducto && x.TipoMovimiento.TipoMovimiento == "In").Select(y => y.Unidades).FirstOrDefault();
                var Negativo = lst.Where(x => x.TipoMovimiento.CodigoProducto == item.TipoMovimiento.CodigoProducto && x.TipoMovimiento.TipoMovimiento == "Eg").Select(y => y.Unidades).FirstOrDefault();
                var tmBarro = lstBarros.Where(x => x.CodigoProducto == item.TipoMovimiento.CodigoProducto).Select(y => new { CodigoProducto = y.CodigoProducto, Tipo = y.Tipo, Capacidad = y.Capacidad }).FirstOrDefault();

                int? Total = (Positivo != null ? Positivo : 0) - (Negativo != null ? Negativo : 0);
                ReservaBarro RecBar = new ReservaBarro() { CodigoBarro = tmBarro.CodigoProducto , Tipo = tmBarro.Tipo, Capacidad = tmBarro.Capacidad, Unidades = Total, TotalKg = tmBarro.Capacidad * Total };
                                
                lstReservas.Add(RecBar);            
            }        

            return lstReservas;
        }

        public int addMovimientosBarro(MovimientosBarro pMovB)
        {
            try
            {
                db.MovimientosBarro.Add(pMovB);
                int res = db.SaveChanges();
                return res;
            }
            catch(Exception ex)
            {
                return -1;
            } 
        }
        public int addMovimientosBarro(List<MovimientosBarro> plstMovB)
        {
            try
            {
                db.MovimientosBarro.AddRange(plstMovB);
                int res = db.SaveChanges();
                return res;
            }
            catch (Exception ex)
            {
                return -1;
            }
        }
        //Catalogos para Movimientos de barro
        public List<SelectListItem> obtenerProductos()
        {
            List<SelectListItem> lst = db.BarroMaestra.Select(x => new SelectListItem() { Text = "Barro " + x.Tipo + " de " + x.Capacidad + " Kg", Value = x.CodigoProducto }).ToList();
            return lst;
        }
        public List<SelectListItem> obtenerProvedores()
        {
            List<SelectListItem> lst = db.Provedores.Select(x => new SelectListItem() { Text = x.Nombre, Value = x.id.ToString() }).ToList();
            return lst;
        }
        //Se hizo de esta manera pues no se considero necesario crear un catalogo en BD para 2 locaciones
        public List<SelectListItem> obtenerLocaciones()        
        {
            List<SelectListItem> lst = new List<SelectListItem>() {
                new SelectListItem() { Text = "34 pte", Value = "34 pte" },
                new SelectListItem() { Text = "La Luz", Value = "La Luz" }
            };
            return lst;
        }
                

    }
}