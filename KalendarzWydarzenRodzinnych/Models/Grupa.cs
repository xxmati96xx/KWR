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
    
    public partial class Grupa
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Grupa()
        {
            this.UzytkownicyWGrupie = new HashSet<UzytkownicyWGrupie>();
        }
    
        public int id { get; set; }
        public string Nazwa { get; set; }
        public string Opis { get; set; }
        public int id_uzytkownik { get; set; }
    
        public virtual Uzytkownik Uzytkownik { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<UzytkownicyWGrupie> UzytkownicyWGrupie { get; set; }
    }
}