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
    
    public partial class Przebieg
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Przebieg()
        {
            this.Wpis = new HashSet<Wpis>();
        }
    
        public int id { get; set; }
        public string Opis { get; set; }
        public int id_wydarzenie { get; set; }
        public string Tytul { get; set; }
    
        public virtual Wydarzenie Wydarzenie { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Wpis> Wpis { get; set; }
    }
}
