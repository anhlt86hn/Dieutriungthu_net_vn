//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Rico.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Price
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Tag { get; set; }
        public string Quote { get; set; }
        public string Promotion1 { get; set; }
        public string Promotion2 { get; set; }
        public string Album { get; set; }
        public string BigPhoto { get; set; }
        public string Dress { get; set; }
        public bool Active { get; set; }
        public System.DateTime SDate { get; set; }
        public string Detail { get; set; }
        public Nullable<int> Ord { get; set; }
    }
}
