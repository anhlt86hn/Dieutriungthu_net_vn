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
    
    public partial class SearchPrice
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Nullable<int> PriceFrom { get; set; }
        public Nullable<int> PriceTo { get; set; }
        public int Ord { get; set; }
        public bool Active { get; set; }
    }
}