//------------------------------------------------------------------------------
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
    using System.Collections.Generic;
    
    public partial class Photo
    {
        public int Id { get; set; }
        public byte[] PPath { get; set; }
        public int CollectionID { get; set; }
    
        public virtual Items Items { get; set; }
        public virtual Items Items1 { get; set; }
    }
}