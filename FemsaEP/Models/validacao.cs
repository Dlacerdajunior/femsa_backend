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
    
    public partial class validacao
    {
        public validacao()
        {
            this.manutencao = new HashSet<manutencao>();
        }
    
        public int id { get; set; }
        public string codigo { get; set; }
        public string nome { get; set; }
        public string mandante { get; set; }
    
        public virtual ICollection<manutencao> manutencao { get; set; }
    }
}
