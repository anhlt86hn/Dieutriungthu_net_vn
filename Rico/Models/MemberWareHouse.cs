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
    
    public partial class MemberWareHouse
    {
        public int Id { get; set; }
        public Nullable<int> IdMember { get; set; }
        public Nullable<int> IdWareHouse { get; set; }
        public Nullable<System.DateTime> SDate { get; set; }
        public Nullable<System.DateTime> EDate { get; set; }
        public Nullable<int> Ord { get; set; }
    
        public virtual Member Member { get; set; }
        public virtual WareHouse WareHouse { get; set; }
    }
}
