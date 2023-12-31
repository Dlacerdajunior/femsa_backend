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
    
    public partial class colaborador_SAP
    {
        public colaborador_SAP()
        {
            this.chat_mensagem = new HashSet<chat_mensagem>();
            this.cliente_SAP = new HashSet<cliente_SAP>();
            this.enquete_resposta = new HashSet<enquete_resposta>();
            this.mensagem = new HashSet<mensagem>();
            this.rota_colaborador = new HashSet<rota_colaborador>();
        }
    
        public Nullable<decimal> MANDT { get; set; }
        public decimal ZCODCOLAB { get; set; }
        public string ZNOMBRE { get; set; }
        public Nullable<decimal> ZTELEFONO { get; set; }
        public string ZEMAIL { get; set; }
        public Nullable<decimal> ZFUNCARGO { get; set; }
        public string ZTERRITOR { get; set; }
        public string ZUNIDAD { get; set; }
        public string ZNUMPATRI { get; set; }
        public string ZNUMSERIE { get; set; }
        public string ZCONTRASE { get; set; }
        public Nullable<decimal> ZGRUPO { get; set; }
        public string ZLOGIN { get; set; }
        public string ZDESACTIV { get; set; }
        public string descricao { get; set; }
        public Nullable<int> id_usuario { get; set; }
        public Nullable<int> id_colaborador { get; set; }
        public string foto { get; set; }
        public Nullable<byte> idioma { get; set; }
        public Nullable<byte> acessoffline { get; set; }
        public Nullable<System.DateTime> ultimosinc { get; set; }
    
        public virtual ICollection<chat_mensagem> chat_mensagem { get; set; }
        public virtual ICollection<cliente_SAP> cliente_SAP { get; set; }
        public virtual grupo_SAP grupo_SAP { get; set; }
        public virtual territorio_SAP territorio_SAP { get; set; }
        public virtual unidade_SAP unidade_SAP { get; set; }
        public virtual ICollection<enquete_resposta> enquete_resposta { get; set; }
        public virtual ICollection<mensagem> mensagem { get; set; }
        public virtual ICollection<rota_colaborador> rota_colaborador { get; set; }
    }
}
