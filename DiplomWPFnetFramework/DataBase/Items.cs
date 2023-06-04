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
    
    public partial class Items
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Items()
        {
            this.Photo = new HashSet<Photo>();
        }
    
        public int Id { get; set; }
        public string Title { get; set; }
        public string IType { get; set; }
        public byte[] IImage { get; set; }
        public int IPriority { get; set; }
        public int IsHidden { get; set; }
        public int IsSelected { get; set; }
        public System.DateTime DateCreation { get; set; }
        public Nullable<int> FolderId { get; set; }
        public Nullable<int> UserId { get; set; }
    
        public virtual CreditCard CreditCard { get; set; }
        public virtual INN INN { get; set; }
        public virtual Users Users { get; set; }
        public virtual Passport Passport { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Photo> Photo { get; set; }
        public virtual Polis Polis { get; set; }
        public virtual SNILS SNILS { get; set; }
    }
}
