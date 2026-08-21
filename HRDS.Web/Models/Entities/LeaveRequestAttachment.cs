using System;
using System.Collections.Generic;

namespace HRDS.Web.Models.Entities;

public partial class LeaveRequestAttachment
{
    public int AttachmentId { get; set; }

    public int LeaveRequestId { get; set; }

    public string FileName { get; set; } = null!;

    public string FilePath { get; set; } = null!;

    public string? FileType { get; set; }

    public long? FileSizeInBytes { get; set; }

    public string? Notes { get; set; }

    public int? UploadedBy { get; set; }

    public DateTime UploadedAt { get; set; }

    public bool IsActive { get; set; }

    public virtual LeaveRequest LeaveRequest { get; set; } = null!;
}
