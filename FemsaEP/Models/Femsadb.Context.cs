﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class MC1_DB_FEMSAEntities : DbContext
    {
        public MC1_DB_FEMSAEntities()
            : base("name=MC1_DB_FEMSAEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<acessorio_SAP> acessorio_SAP { get; set; }
        public virtual DbSet<administrador> administrador { get; set; }
        public virtual DbSet<alimento_SAP> alimento_SAP { get; set; }
        public virtual DbSet<ativacao> ativacao { get; set; }
        public virtual DbSet<ativacao_foto> ativacao_foto { get; set; }
        public virtual DbSet<ativacao_mensagem> ativacao_mensagem { get; set; }
        public virtual DbSet<ativacao_peca> ativacao_peca { get; set; }
        public virtual DbSet<book_online> book_online { get; set; }
        public virtual DbSet<canal_SAP> canal_SAP { get; set; }
        public virtual DbSet<caracteristica_de_peca_SAP> caracteristica_de_peca_SAP { get; set; }
        public virtual DbSet<cargo_SAP> cargo_SAP { get; set; }
        public virtual DbSet<carta> carta { get; set; }
        public virtual DbSet<chat_mensagem> chat_mensagem { get; set; }
        public virtual DbSet<cliente_cluster_SAP> cliente_cluster_SAP { get; set; }
        public virtual DbSet<cliente_projeto> cliente_projeto { get; set; }
        public virtual DbSet<cliente_SAP> cliente_SAP { get; set; }
        public virtual DbSet<cluster_peca_SAP> cluster_peca_SAP { get; set; }
        public virtual DbSet<cluster_SAP> cluster_SAP { get; set; }
        public virtual DbSet<colaborador_SAP> colaborador_SAP { get; set; }
        public virtual DbSet<contrapartida_ativacoes> contrapartida_ativacoes { get; set; }
        public virtual DbSet<contrapartida_projeto> contrapartida_projeto { get; set; }
        public virtual DbSet<contrapartida_SAP> contrapartida_SAP { get; set; }
        public virtual DbSet<coordenador> coordenador { get; set; }
        public virtual DbSet<email> email { get; set; }
        public virtual DbSet<embalagem_SAP> embalagem_SAP { get; set; }
        public virtual DbSet<enquete> enquete { get; set; }
        public virtual DbSet<enquete_perguntas> enquete_perguntas { get; set; }
        public virtual DbSet<enquete_resposta> enquete_resposta { get; set; }
        public virtual DbSet<executivo> executivo { get; set; }
        public virtual DbSet<familia> familia { get; set; }
        public virtual DbSet<foco> foco { get; set; }
        public virtual DbSet<fornecedor> fornecedor { get; set; }
        public virtual DbSet<franquia> franquia { get; set; }
        public virtual DbSet<grupo_de_usuario> grupo_de_usuario { get; set; }
        public virtual DbSet<grupo_SAP> grupo_SAP { get; set; }
        public virtual DbSet<kit_SAP> kit_SAP { get; set; }
        public virtual DbSet<localizacao_SAP> localizacao_SAP { get; set; }
        public virtual DbSet<manutencao> manutencao { get; set; }
        public virtual DbSet<manutencao_instalacao> manutencao_instalacao { get; set; }
        public virtual DbSet<manutencao_peca> manutencao_peca { get; set; }
        public virtual DbSet<marca_SAP> marca_SAP { get; set; }
        public virtual DbSet<mensagem> mensagem { get; set; }
        public virtual DbSet<mensagem_do_dia> mensagem_do_dia { get; set; }
        public virtual DbSet<motivo_de_manutencao> motivo_de_manutencao { get; set; }
        public virtual DbSet<motivo_de_recuso_SAP> motivo_de_recuso_SAP { get; set; }
        public virtual DbSet<nse_SAP> nse_SAP { get; set; }
        public virtual DbSet<ocasiao_de_consumo> ocasiao_de_consumo { get; set; }
        public virtual DbSet<peca_SAP> peca_SAP { get; set; }
        public virtual DbSet<peca_tipo> peca_tipo { get; set; }
        public virtual DbSet<planilhas> planilhas { get; set; }
        public virtual DbSet<projeto> projeto { get; set; }
        public virtual DbSet<projeto_cluster> projeto_cluster { get; set; }
        public virtual DbSet<prospector> prospector { get; set; }
        public virtual DbSet<provedor_SAP> provedor_SAP { get; set; }
        public virtual DbSet<rede_SAP> rede_SAP { get; set; }
        public virtual DbSet<rota_colaborador> rota_colaborador { get; set; }
        public virtual DbSet<rota_SAP> rota_SAP { get; set; }
        public virtual DbSet<shopping_SAP> shopping_SAP { get; set; }
        public virtual DbSet<status_ativacao> status_ativacao { get; set; }
        public virtual DbSet<subcanal_SAP> subcanal_SAP { get; set; }
        public virtual DbSet<territorio_SAP> territorio_SAP { get; set; }
        public virtual DbSet<tipo_de_manutencao> tipo_de_manutencao { get; set; }
        public virtual DbSet<uf> uf { get; set; }
        public virtual DbSet<unidade_SAP> unidade_SAP { get; set; }
        public virtual DbSet<validacao> validacao { get; set; }
        public virtual DbSet<zat_SAP> zat_SAP { get; set; }
    }
}
