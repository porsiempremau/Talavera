using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TalaveraWeb.Models;

namespace TalaveraWeb.Models
{
    public class PreparacionBarroConsumo : PreparacionBarro
    {
        public List<ReservaBarroPreparado> lstConsumoBarroNegro { get; set; }
        public List<ReservaBarroPreparado> lstConsumoBarroBlanco { get; set; }
    }
}