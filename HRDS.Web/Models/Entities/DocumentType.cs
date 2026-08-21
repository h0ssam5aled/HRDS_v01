using System;
using System.Collections.Generic;

namespace HRDS.Web.Models.Entities;

public partial class DocumentType
{
    public int DocumentTypeId { get; set; }

    public string TypeCode { get; set; } = null!;

    public string TypeNameAr { get; set; } = null!;

    public string? TypeNameEn { get; set; }

    public bool IsExpiryRequired { get; set; }

    public int? ExpiryAlertDays { get; set; }

    public bool IsMandatory { get; set; }

    public virtual ICollection<Document> Documents { get; set; } = new List<Document>();
}
