using System;
using System.Collections.Generic;

namespace HRDS.Web.Models.Entities;

public partial class AttendanceLog
{
    public int LogId { get; set; }

    public int EmployeeId { get; set; }

    public DateTime LogDateTime { get; set; }

    public string? DeviceSerialNumber { get; set; }

    public short InOutType { get; set; }

    public virtual Employee Employee { get; set; } = null!;
}
