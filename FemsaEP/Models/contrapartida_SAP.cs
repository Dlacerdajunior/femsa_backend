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
    
    public partial class contrapartida_SAP
    {
        public contrapartida_SAP()
        {
            this.contrapartida_ativacoes = new HashSet<contrapartida_ativacoes>();
            this.contrapartida_projeto = new HashSet<contrapartida_projeto>();
        }
    
        public Nullable<decimal> MANDT { get; set; }
        public decimal ZCODCONPA { get; set; }
        public string ZNOMCONPA { get; set; }
        public string obrigatorio { get; set; }
        public string descricao { get; set; }
    
        public virtual ICollection<contrapartida_ativacoes> contrapartida_ativacoes { get; set; }
        public virtual ICollection<contrapartida_projeto> contrapartida_projeto { get; set; }
    }
}
