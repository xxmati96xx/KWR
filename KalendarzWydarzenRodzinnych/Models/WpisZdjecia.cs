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
    
    public partial class WpisZdjecia
    {
        public int id { get; set; }
        public int id_wpis { get; set; }
        public string zdjecie { get; set; }
    
        public virtual Wpis Wpis { get; set; }
    }
}
