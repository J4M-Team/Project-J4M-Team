//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace BookStore.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class Bill_Remove
    {
        public int Bill_Remove_Id { get; set; }
        public Nullable<int> Bill_Id { get; set; }
        public string Bill_Status { get; set; }
    
        public virtual Bill Bill { get; set; }
    }
}
