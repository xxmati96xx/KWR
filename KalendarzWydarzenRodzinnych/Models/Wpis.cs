//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace KalendarzWydarzenRodzinnych.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public partial class Wpis
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Wpis()
        {
            this.Powiadomienie = new HashSet<Powiadomienie>();
            this.WpisZdjecia = new HashSet<WpisZdjecia>();
        }
    
        public int id { get; set; }
        [DataType(DataType.MultilineText)]
        public string Relacja { get; set; }
        public Nullable<int> id_wydarzenie { get; set; }
        public Nullable<int> id_przebieg { get; set; }
        public int id_uzytkownik { get; set; }
        public Nullable<System.DateTime> data_dodania { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Powiadomienie> Powiadomienie { get; set; }
        public virtual Przebieg Przebieg { get; set; }
        public virtual Uzytkownik Uzytkownik { get; set; }
        public virtual Wydarzenie Wydarzenie { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<WpisZdjecia> WpisZdjecia { get; set; }
    }
}
