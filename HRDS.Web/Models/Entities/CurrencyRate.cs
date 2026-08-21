using System;
using System.Collections.Generic;

namespace HRDS.Web.Models.Entities;

public partial class CurrencyRate
{
    public int CurrencyRateId { get; set; }

    public int CurrencyId { get; set; }

    public int BaseCurrencyId { get; set; }

    public decimal ExchangeRate { get; set; }

    public DateTime RateDate { get; set; }

    public bool IsActive { get; set; }

    public int? CreatedBy { get; set; }

    public DateTime CreatedDate { get; set; }

    public int? ModifiedBy { get; set; }

    public DateTime? ModifiedDate { get; set; }

    public virtual Currency BaseCurrency { get; set; } = null!;

    public virtual Currency Currency { get; set; } = null!;
}
