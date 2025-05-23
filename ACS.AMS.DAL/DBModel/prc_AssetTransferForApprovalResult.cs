﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

#nullable enable

namespace ACS.AMS.DAL.DBModel
{
    public partial class prc_AssetTransferForApprovalResult
    {
        public int TransactionID { get; set; }
        public string TransactionNo { get; set; } = default!;
        public int TransactionTypeID { get; set; }
        public int? TransactionSubTypeID { get; set; }
        public string? ReferenceNo { get; set; }
        public string? CreatedFrom { get; set; }
        public int? SourceTransactionID { get; set; }
        public string? SourceDocumentNo { get; set; }
        public string? Remarks { get; set; }
        public DateTime? TransactionDate { get; set; }
        [Column("TransactionValue", TypeName = "decimal(18,5)")]
        public decimal? TransactionValue { get; set; }
        public int StatusID { get; set; }
        public int PostingStatusID { get; set; }
        public int? VerifiedBy { get; set; }
        public DateTime? VerifiedDateTime { get; set; }
        public int? PostedBy { get; set; }
        public DateTime? PostedDateTime { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public int? VendorID { get; set; }
        public string? ServiceDoneBy { get; set; }
        public DateTime? TransactionStartDate { get; set; }
        public DateTime? TransactionEndDate { get; set; }
        public int? SourceTransactionScheduleID { get; set; }
        public int lvl { get; set; }
        public string? InitorDoc { get; set; }
        public string? ApprovalDoc { get; set; }
    }
}
