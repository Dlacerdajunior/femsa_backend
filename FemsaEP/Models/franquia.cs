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
    
    public partial class franquia
    {
        public franquia()
        {
            this.cliente_SAP = new HashSet<cliente_SAP>();
        }
    
        public int id { get; set; }
        public string codigo { get; set; }
        public string descricao { get; set; }
        public string mandante { get; set; }
    
        public virtual ICollection<cliente_SAP> cliente_SAP { get; set; }
    }
}