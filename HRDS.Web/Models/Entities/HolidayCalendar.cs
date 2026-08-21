using System;
using System.Collections.Generic;

namespace HRDS.Web.Models.Entities;

public partial class HolidayCalendar
{
    public int HolidayId { get; set; }

    public string HolidayNameAr { get; set; } = null!;

    public string? HolidayNameEn { get; set; }

    public DateOnly StartDate { get; set; }

    public DateOnly EndDate { get; set; }

    public short TotalDays { get; set; }

    public int Year { get; set; }

    public bool IsRecurring { get; set; }

    public bool IsActive { get; set; }

    public int? CompanyId { get; set; }
}
