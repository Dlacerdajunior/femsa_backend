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
    
    public partial class provedor_SAP
    {
        public Nullable<decimal> MANDT { get; set; }
        public decimal ZCODPROVE { get; set; }
        public string ZNOMBRE { get; set; }
        public string ZRAZONSOC { get; set; }
        public string ZCNPJ { get; set; }
        public string ZCONTRATO { get; set; }
        public string ZTELEFONO { get; set; }
        public string ZEMAIL { get; set; }
        public string ZTERRITOR { get; set; }
        public Nullable<decimal> ZCODSAP { get; set; }
        public Nullable<decimal> ZVALLEITO { get; set; }
        public Nullable<decimal> ZVALORPER { get; set; }
        public string ZDESACTIV { get; set; }
        public Nullable<decimal> VALORM2 { get; set; }
    
        public virtual territorio_SAP territorio_SAP { get; set; }
    }
}