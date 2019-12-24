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
    
    public partial class GroupPicture
    {
        public GroupPicture()
        {
            this.Pictures = new HashSet<Picture>();
        }
    
        public int Id { get; set; }
        public string Name { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Keyword { get; set; }
        public string Place { get; set; }
        public string Tag { get; set; }
        public string Image { get; set; }
        public string Level { get; set; }
        public System.DateTime SDate { get; set; }
        public bool Index { get; set; }
        public bool Priority { get; set; }
        public bool Active { get; set; }
        public Nullable<int> Ord { get; set; }
        public int ParentId { get; set; }
    
        public virtual ICollection<Picture> Pictures { get; set; }
    }
}
