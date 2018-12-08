﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class BookStoreEntities : DbContext
    {
        public BookStoreEntities()
            : base("name=BookStoreEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Bill> Bills { get; set; }
        public virtual DbSet<Bill_Info> Bill_Info { get; set; }
        public virtual DbSet<Bill_Remove> Bill_Remove { get; set; }
        public virtual DbSet<Book> Books { get; set; }
        public virtual DbSet<Book_Input> Book_Input { get; set; }
        public virtual DbSet<Book_Input_Price> Book_Input_Price { get; set; }
        public virtual DbSet<Book_Output> Book_Output { get; set; }
        public virtual DbSet<Book_Output_Price> Book_Output_Price { get; set; }
        public virtual DbSet<Book_Output_PromotionPrice> Book_Output_PromotionPrice { get; set; }
        public virtual DbSet<Book_Query> Book_Query { get; set; }
        public virtual DbSet<Customer> Customers { get; set; }
        public virtual DbSet<Customer_Types> Customer_Types { get; set; }
        public virtual DbSet<Customer_Vip> Customer_Vip { get; set; }
        public virtual DbSet<Decentralization> Decentralizations { get; set; }
        public virtual DbSet<Employee> Employees { get; set; }
        public virtual DbSet<Employee_Account> Employee_Account { get; set; }
        public virtual DbSet<Employee_Info> Employee_Info { get; set; }
        public virtual DbSet<Employee_Role> Employee_Role { get; set; }
        public virtual DbSet<Evaluate> Evaluates { get; set; }
        public virtual DbSet<Evaluate_Store> Evaluate_Store { get; set; }
        public virtual DbSet<Output_Info> Output_Info { get; set; }
        public virtual DbSet<PayWage_History> PayWage_History { get; set; }
        public virtual DbSet<Salary_Bonus> Salary_Bonus { get; set; }
    }
}
