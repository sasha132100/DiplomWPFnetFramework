﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DiplomWPFnetFramework.DataBase
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class test123Entities1 : DbContext
    {
        public test123Entities1()
            : base("name=test123Entities1")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<CreditCard> CreditCard { get; set; }
        public virtual DbSet<INN> INN { get; set; }
        public virtual DbSet<Items> Items { get; set; }
        public virtual DbSet<LastLogginetUser> LastLogginetUser { get; set; }
        public virtual DbSet<Passport> Passport { get; set; }
        public virtual DbSet<Photo> Photo { get; set; }
        public virtual DbSet<Polis> Polis { get; set; }
        public virtual DbSet<SNILS> SNILS { get; set; }
        public virtual DbSet<Template> Template { get; set; }
        public virtual DbSet<TemplateDocument> TemplateDocument { get; set; }
        public virtual DbSet<TemplateDocumentData> TemplateDocumentData { get; set; }
        public virtual DbSet<TemplateObject> TemplateObject { get; set; }
        public virtual DbSet<Users> Users { get; set; }
    }
}
