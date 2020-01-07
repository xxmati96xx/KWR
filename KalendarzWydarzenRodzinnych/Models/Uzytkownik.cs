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
    using System.Web;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class Uzytkownik
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Uzytkownik()
        {
            this.Grupa = new HashSet<Grupa>();
            this.Odebrane_Wiadomosc = new HashSet<Odebrane_Wiadomosc>();
            this.Odebrane_Wiadomosc1 = new HashSet<Odebrane_Wiadomosc>();
            this.Powiadomienie = new HashSet<Powiadomienie>();
            this.Przypomnienie = new HashSet<Przypomnienie>();
            this.Przypomnienie1 = new HashSet<Przypomnienie>();
            this.Uczestnicy = new HashSet<Uczestnicy>();
            this.UzytkownicyWGrupie = new HashSet<UzytkownicyWGrupie>();
            this.Wpis = new HashSet<Wpis>();
            this.Wydarzenie = new HashSet<Wydarzenie>();
            this.Wyslane_Wiadomosc = new HashSet<Wyslane_Wiadomosc>();
            this.Wyslane_Wiadomosc1 = new HashSet<Wyslane_Wiadomosc>();
            this.ZadanieUczestnik = new HashSet<ZadanieUczestnik>();
            this.Zaproszenie = new HashSet<Zaproszenie>();
            this.Zaproszenie1 = new HashSet<Zaproszenie>();
        }
    
        public int id { get; set; }
        public string Imie { get; set; }
        public string Nazwisko { get; set; }
        [Display(Name = "Data urodzenia")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public System.DateTime DataUrodzenia { get; set; }
        [Display(Name = "Zdj�cie")]
        public string Zdjcie { get; set; }
        [Display(Name = "Numer telefonu")]
        public string NrTelefonu { get; set; }
        [Display(Name = "Adres e-mail")]
        public string AdresEmail { get; set; }
        [NotMapped]
        [Display(Name = "Wybierz zdj�cie:")]
        public HttpPostedFileBase[] files { get; set; }


        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Grupa> Grupa { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Odebrane_Wiadomosc> Odebrane_Wiadomosc { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Odebrane_Wiadomosc> Odebrane_Wiadomosc1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Powiadomienie> Powiadomienie { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Przypomnienie> Przypomnienie { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Przypomnienie> Przypomnienie1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Uczestnicy> Uczestnicy { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<UzytkownicyWGrupie> UzytkownicyWGrupie { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Wpis> Wpis { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Wydarzenie> Wydarzenie { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Wyslane_Wiadomosc> Wyslane_Wiadomosc { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Wyslane_Wiadomosc> Wyslane_Wiadomosc1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ZadanieUczestnik> ZadanieUczestnik { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Zaproszenie> Zaproszenie { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Zaproszenie> Zaproszenie1 { get; set; }
    }
}
