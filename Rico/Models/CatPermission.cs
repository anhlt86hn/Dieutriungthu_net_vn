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
    
    public partial class CatPermission
    {
        public int ID { get; set; }
        public int UserID { get; set; }
        public int CatID { get; set; }
        public Nullable<bool> View { get; set; }
        public Nullable<bool> Full { get; set; }
    }
}