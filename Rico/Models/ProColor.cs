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
    
    public partial class ProColor
    {
        public int Id { get; set; }
        public int IdColor { get; set; }
        public Nullable<int> Inventory { get; set; }
    
        public virtual Color Color { get; set; }
    }
}
