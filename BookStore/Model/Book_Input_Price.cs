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
    
    public partial class Book_Input_Price
    {
        public int Input_Price_Id { get; set; }
        public int Book_Id { get; set; }
        public System.DateTime Date_Set { get; set; }
        public double Input_Price { get; set; }
    
        public virtual Book Book { get; set; }
    }
}
