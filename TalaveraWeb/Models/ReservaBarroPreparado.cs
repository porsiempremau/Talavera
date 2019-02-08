using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace TalaveraWeb.Models
{
    public class ReservaBarroPreparado : ReservaBarro
    {
        public int Id { get; set; }
        public double BarroUsado { get; set; }

        public ReservaBarroPreparado()
        {
            this.Id = 1;
        }
        
    }
}