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
    
    public partial class projeto_cluster
    {
        public int id { get; set; }
        public Nullable<int> id_projeto { get; set; }
        public Nullable<decimal> ZCODCLUST { get; set; }
        public string mandante { get; set; }
    
        public virtual cluster_SAP cluster_SAP { get; set; }
        public virtual projeto projeto { get; set; }
    }
}
