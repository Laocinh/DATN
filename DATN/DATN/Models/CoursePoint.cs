using System;
using System.Collections.Generic;

namespace DATN.Models;

public partial class CoursePoint
{
    public long Id { get; set; }

    public long? Student { get; set; }

    public long? DetailTerm { get; set; }

    public long? RegistStudent { get; set; }

    public double? PointProcess { get; set; }

    public double? TestScore { get; set; }

    public double? OverallScore { get; set; }

    public int? NumberTest { get; set; }

    public bool? Status { get; set; }

    public long? Staff { get; set; }

    public string? CreateBy { get; set; }

    public string? UpdateBy { get; set; }

    public DateTime? CreateDate { get; set; }

    public DateTime? UpdateDate { get; set; }

    public bool? IsDelete { get; set; }

    public bool? IsActive { get; set; }

    public double? AttendancePoint { get; set; }

    public double? MidtermPoint { get; set; }

    public virtual DetailTerm? DetailTermNavigation { get; set; }

    public virtual RegistStudent? RegistStudentNavigation { get; set; }

    public virtual Staff? StaffNavigation { get; set; }

    public virtual Student? StudentNavigation { get; set; }
}
