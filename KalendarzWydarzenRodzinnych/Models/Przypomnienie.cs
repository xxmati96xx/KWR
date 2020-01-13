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
    public partial class Przypomnienie
    {
        public int id { get; set; }
        [Display(Name = "Data przypomnienia")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm}", ApplyFormatInEditMode = true)]
        public System.DateTime Data { get; set; }
        public int id_wydarzenie { get; set; }
        public int id_uzytkownik { get; set; }
        [Display(Name = "Tre��")]
        [DataType(DataType.MultilineText)]
        public string Tresc { get; set; }
        
        public string identyfier { get; set; }
        public Nullable<int> id_organizator { get; set; }
        [Display(Name = "Rodzaj przypomnienia")]
        public string rodzaj { get; set; }
    
        public virtual Uzytkownik Uzytkownik { get; set; }
        public virtual Uzytkownik Uzytkownik1 { get; set; }
        public virtual Wydarzenie Wydarzenie { get; set; }
    }
}
