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
    
    public partial class kit_SAP
    {
        public kit_SAP()
        {
            this.cliente_SAP = new HashSet<cliente_SAP>();
        }
    
        public Nullable<decimal> MANDT { get; set; }
        public decimal ZCODKITS { get; set; }
        public string ZNOMKITS { get; set; }
        public string ZDESACTIV { get; set; }
        public string descricao { get; set; }
    
        public virtual ICollection<cliente_SAP> cliente_SAP { get; set; }
    }
}
