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
    
    public partial class chat_mensagem
    {
        public int id { get; set; }
        public Nullable<int> mensagem_id { get; set; }
        public Nullable<decimal> ZCODCOLAB { get; set; }
        public Nullable<int> remetente_id { get; set; }
        public string texto { get; set; }
        public Nullable<System.DateTime> hora { get; set; }
        public string tipo { get; set; }
        public string mandante { get; set; }
    
        public virtual administrador administrador { get; set; }
        public virtual mensagem mensagem { get; set; }
        public virtual colaborador_SAP colaborador_SAP { get; set; }
    }
}
