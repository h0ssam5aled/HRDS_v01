using System;
using System.Collections.Generic;

namespace HRDS.Web.Models.Entities;

public partial class Document
{
    public int DocumentId { get; set; }

    public int EmployeeId { get; set; }

    public int DocumentTypeId { get; set; }

    public string? DocumentNumber { get; set; }

    public DateOnly? IssueDate { get; set; }

    public DateOnly? ExpiryDate { get; set; }

    public string? FilePath { get; set; }

    public bool IsMandatory { get; set; }

    public string? Notes { get; set; }

    public bool IsActive { get; set; }

    public int? CreatedBy { get; set; }

    public DateTime CreatedAt { get; set; }

    public int? UpdatedBy { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public int? DeletedBy { get; set; }

    public DateTime? DeletedAt { get; set; }

    public bool IsDeleted { get; set; }

    public virtual DocumentType DocumentType { get; set; } = null!;

    public virtual Employee Employee { get; set; } = null!;
}
