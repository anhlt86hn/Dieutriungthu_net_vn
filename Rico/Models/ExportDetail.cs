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
    
    public partial class ExportDetail
    {
        public int Id { get; set; }
        public int IdExport { get; set; }
        public int IdPro { get; set; }
        public int Number { get; set; }
        public double Price { get; set; }
        public double Total { get; set; }
        public Nullable<int> Ord { get; set; }
    
        public virtual Export Export { get; set; }
        public virtual Product Product { get; set; }
    }
}
