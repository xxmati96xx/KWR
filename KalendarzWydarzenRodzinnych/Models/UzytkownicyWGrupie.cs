//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
using System.ComponentModel.DataAnnotations;

namespace KalendarzWydarzenRodzinnych.Models

{
    using System;
    using System.Collections.Generic;
    
    public partial class UzytkownicyWGrupie
    {
        public int id { get; set; }

        public int id_grupa { get; set; }
        [Display(Name = "Uzytkownik")]
        public int id_uzytkownik { get; set; }
    
        public virtual Grupa Grupa { get; set; }
        public virtual Uzytkownik Uzytkownik { get; set; }
    }
}
