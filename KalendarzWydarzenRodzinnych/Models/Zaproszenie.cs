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

    public partial class Zaproszenie
    {
        public int id { get; set; }
        public int Od { get; set; }
        public int Do { get; set; }
        public int id_wydarzenie { get; set; }
    
        public virtual Uzytkownik Uzytkownik { get; set; }
        public virtual Uzytkownik Uzytkownik1 { get; set; }
        public virtual Wydarzenie Wydarzenie { get; set; }
    }
}
