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
    
    public partial class Advertise
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Detail { get; set; }
        public string Target { get; set; }
        public string Link { get; set; }
        public string Image { get; set; }
        public bool Active { get; set; }
        public Nullable<int> Width { get; set; }
        public Nullable<int> Height { get; set; }
        public Nullable<int> Position { get; set; }
        public Nullable<int> Ord { get; set; }
    }
}
