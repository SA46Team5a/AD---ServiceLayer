//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ServiceLayer.DataAccess
{
    using System;
    using System.Collections.Generic;
    
    public partial class SupplierItem
    {
        public int SupplierItemID { get; set; }
        public string SupplierID { get; set; }
        public string ItemID { get; set; }
        public Nullable<int> Rank { get; set; }
        public Nullable<decimal> Cost { get; set; }
    
        public virtual Item Item { get; set; }
        public virtual Supplier Supplier { get; set; }
    }
}
