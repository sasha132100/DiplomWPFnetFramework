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
    
    public partial class INN
    {
        public System.Guid Id { get; set; }
        public string Number { get; set; }
        public string FIO { get; set; }
        public string Gender { get; set; }
        public Nullable<System.DateTime> BirthDate { get; set; }
        public string BirthPlace { get; set; }
        public Nullable<System.DateTime> RegistrationDate { get; set; }
        public byte[] PhotoPage1 { get; set; }
        public Nullable<System.DateTime> UpdateTime { get; set; }
    
        public virtual Item Item { get; set; }
    }
}
