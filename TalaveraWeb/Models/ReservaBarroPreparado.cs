using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TalaveraWeb.Models
{
    public class ReservaBarroPreparado : ReservaBarro
    {
        public int BarroUsado { get; set; }
    }
}