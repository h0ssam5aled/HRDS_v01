using System;
using System.Collections.Generic;

namespace HRDS.Web.Models.Entities;

public partial class PaymentMethod
{
    public int PaymentMethodId { get; set; }

    public string PaymentMethodCode { get; set; } = null!;

    public string PaymentMethodNameAr { get; set; } = null!;

    public string? PaymentMethodNameEn { get; set; }

    public string? Description { get; set; }

    public bool IsActive { get; set; }
}
