﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Este código se generó a partir de una plantilla.
//
//     Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//     Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace TalaveraWeb.Models
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class TalaveraEntities : DbContext
    {
        public TalaveraEntities()
            : base("name=TalaveraEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<BarroMaestra> BarroMaestra { get; set; }
        public virtual DbSet<EntregaPellas> EntregaPellas { get; set; }
        public virtual DbSet<PreparacionPellas> PreparacionPellas { get; set; }
        public virtual DbSet<prepBarro_prepPellas> prepBarro_prepPellas { get; set; }
        public virtual DbSet<Provedores> Provedores { get; set; }
        public virtual DbSet<Recuperados> Recuperados { get; set; }
        public virtual DbSet<Sucursales> Sucursales { get; set; }
        public virtual DbSet<BarroMovimientos> BarroMovimientos { get; set; }
        public virtual DbSet<PreparacionBarro> PreparacionBarro { get; set; }
    }
}
