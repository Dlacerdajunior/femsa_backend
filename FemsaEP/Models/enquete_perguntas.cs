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
    
    public partial class enquete_perguntas
    {
        public int id { get; set; }
        public string nome { get; set; }
        public Nullable<int> id_enquete { get; set; }
        public string mandante { get; set; }
    
        public virtual enquete enquete { get; set; }
    }
}
