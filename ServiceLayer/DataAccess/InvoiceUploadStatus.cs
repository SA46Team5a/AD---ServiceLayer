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
    
    public partial class InvoiceUploadStatus
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public InvoiceUploadStatus()
        {
            this.OrderSuppliers = new HashSet<OrderSupplier>();
        }
    
        public int InvoiceUploadStatusID { get; set; }
        public string InvoiceUploadStatusName { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<OrderSupplier> OrderSuppliers { get; set; }
    }
}
