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
    
    public partial class RequisitionDetail
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public RequisitionDetail()
        {
            this.DisbursementDetails = new HashSet<DisbursementDetail>();
        }
    
        public int RequisitionDetailsID { get; set; }
        public int RequisitionID { get; set; }
        public string ItemID { get; set; }
        public int Quantity { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DisbursementDetail> DisbursementDetails { get; set; }
        public virtual Item Item { get; set; }
        public virtual Requisition Requisition { get; set; }
        public virtual OutstandingRequisitionView OutstandingRequisitionView { get; set; }
    }
}
