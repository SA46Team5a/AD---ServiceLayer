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


    public partial class Requisition
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Requisition()
        {
            this.Disbursements = new HashSet<Disbursement>();
            this.RequisitionDetails = new HashSet<RequisitionDetail>();
        }
    
        public int RequisitionID { get; set; }
        public string EmployeeID { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd-MMM-yyyy}")]
        public Nullable<System.DateTime> RequestedDate { get; set; }
        public Nullable<int> AuthorityID { get; set; }
        public Nullable<System.DateTime> ApproveDate { get; set; }
        public Nullable<int> RetrievalStatusID { get; set; }
        public int ApprovalStatusID { get; set; }
    
        public virtual ApprovalStatus ApprovalStatus { get; set; }
        public virtual Authority Authority { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Disbursement> Disbursements { get; set; }
        public virtual Employee Requester { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<RequisitionDetail> RequisitionDetails { get; set; }
        public virtual RetrievalStatus RetrievalStatus { get; set; }
    }
}
