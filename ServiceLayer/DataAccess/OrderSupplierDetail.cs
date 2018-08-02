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
    using System.ComponentModel.DataAnnotations;

    public partial class OrderSupplierDetail
    {
        public int OrderSupplierDetailsID { get; set; }
        public int OrderSupplierID { get; set; }
        public string ItemID { get; set; }
        public int Quantity { get; set; }
        [DisplayFormat(DataFormatString = "{0:C2}")]
        public decimal UnitCost { get; set; }
        public Nullable<int> ActualQuantityReceived { get; set; }
    
        public virtual Item Item { get; set; }
        public virtual OrderSupplier OrderSupplier { get; set; }
    }
}
