using System;
using System.Collections.Generic;

namespace HRDS.Web.Models.Entities;

public partial class Currency
{
    public int CurrencyId { get; set; }

    public string CurrencyCode { get; set; } = null!;

    public string CurrencyNameAr { get; set; } = null!;

    public string? CurrencyNameEn { get; set; }

    public string? Symbol { get; set; }

    public string? Description { get; set; }

    public bool IsBaseCurrency { get; set; }

    public bool IsActive { get; set; }

    public int? CreatedBy { get; set; }

    public DateTime CreatedDate { get; set; }

    public int? ModifiedBy { get; set; }

    public DateTime? ModifiedDate { get; set; }

    public virtual ICollection<Account> Accounts { get; set; } = new List<Account>();

    public virtual ICollection<CurrencyRate> CurrencyRateBaseCurrencies { get; set; } = new List<CurrencyRate>();

    public virtual ICollection<CurrencyRate> CurrencyRateCurrencies { get; set; } = new List<CurrencyRate>();
}
