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
    using System.ComponentModel.DataAnnotations;
    public partial class Powiadomienie_Wyswietl_Result
    {
        public Nullable<int> id { get; set; }
        [Display(Name = "Data przypomnienia")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm}", ApplyFormatInEditMode = true)]
        public System.DateTime DataPowiadomienia { get; set; }
        [Display(Name = "Tre��")]
        public string Tresc { get; set; }
        [Display(Name = "Nazwa wydarzenia")]
        public string Tytul { get; set; }
        public string identyfier { get; set; }
        [Display(Name = "Rodzaj przypomnienia")]
        public string rodzaj { get; set; }
    }
}
