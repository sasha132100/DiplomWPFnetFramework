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
    
    public partial class Template
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Template()
        {
            this.TemplateDocument = new HashSet<TemplateDocument>();
        }
    
        public int Id { get; set; }
        public string TName { get; set; }
        public string TStatus { get; set; }
        public System.DateTime TDate { get; set; }
        public Nullable<int> UserId { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TemplateDocument> TemplateDocument { get; set; }
        public virtual Users Users { get; set; }
    }
}
