//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace FemsaEP.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class alimento_SAP
    {
        public alimento_SAP()
        {
            this.caracteristica_de_peca_SAP = new HashSet<caracteristica_de_peca_SAP>();
        }
    
        public string MANDT { get; set; }
        public decimal ZCODIGOALIMENTO { get; set; }
        public string ZNOMBREALIMENTO { get; set; }
        public string ZDESACTIVADO { get; set; }
    
        public virtual ICollection<caracteristica_de_peca_SAP> caracteristica_de_peca_SAP { get; set; }
    }
}
