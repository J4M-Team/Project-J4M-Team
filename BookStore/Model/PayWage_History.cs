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
    
    public partial class PayWage_History
    {
        public int PayWage_Id { get; set; }
        public int Employee_Id { get; set; }
        public System.DateTime PayWage_Date { get; set; }
        public double Employee_Salary { get; set; }
    
        public virtual Employee Employee { get; set; }
    }
}
