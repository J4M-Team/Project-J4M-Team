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
    
    public partial class Bill_Info
    {
        public int Bill_Info_Id { get; set; }
        public int Bill_Id { get; set; }
        public int Book_Id { get; set; }
        public int Book_Count { get; set; }
    
        public virtual Bill Bill { get; set; }
    }
}
