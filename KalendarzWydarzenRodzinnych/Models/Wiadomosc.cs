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

    public partial class Wiadomosc
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Wiadomosc()
        {
            this.Odebrane_Wiadomosc = new HashSet<Odebrane_Wiadomosc>();
            this.Wyslane_Wiadomosc = new HashSet<Wyslane_Wiadomosc>();
        }
    
        public int id { get; set; }
        public string Temat { get; set; }
        [Display(Name = "Tre��")]
        [DataType(DataType.MultilineText)]
        public string Tresc { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Odebrane_Wiadomosc> Odebrane_Wiadomosc { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Wyslane_Wiadomosc> Wyslane_Wiadomosc { get; set; }
    }
}
