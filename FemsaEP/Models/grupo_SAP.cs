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
    
    public partial class grupo_SAP
    {
        public grupo_SAP()
        {
            this.colaborador_SAP = new HashSet<colaborador_SAP>();
            this.mensagem = new HashSet<mensagem>();
        }
    
        public Nullable<decimal> MANDT { get; set; }
        public decimal ZCODGRUPO { get; set; }
        public string ZNOMGRUPO { get; set; }
        public string descricao { get; set; }
    
        public virtual ICollection<colaborador_SAP> colaborador_SAP { get; set; }
        public virtual ICollection<mensagem> mensagem { get; set; }
    }
}