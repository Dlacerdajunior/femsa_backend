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
    
    public partial class enquete_resposta
    {
        public int id { get; set; }
        public Nullable<int> id_enquete { get; set; }
        public Nullable<System.DateTime> data_resposta { get; set; }
        public Nullable<int> id_pergunta { get; set; }
        public string resposta { get; set; }
        public string mandante { get; set; }
        public Nullable<decimal> ZCODCOLAB { get; set; }
    
        public virtual colaborador_SAP colaborador_SAP { get; set; }
        public virtual enquete enquete { get; set; }
    }
}
