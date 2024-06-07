using System;
using System.Collections.Generic;

namespace DATN.Models;

public partial class DetailTerm
{
    public long Id { get; set; }

    public long? Term { get; set; }

    public int? Semester { get; set; }

    public DateTime? StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    public string? Room { get; set; }

    public int? MaxNumber { get; set; }

    public string? CreateBy { get; set; }

    public string? UpdateBy { get; set; }

    public DateTime? CreateDate { get; set; }

    public DateTime? UpdateDate { get; set; }

    public bool? IsDelete { get; set; }

    public bool? IsActive { get; set; }

    public int? Subject { get; set; }

    public virtual ICollection<CoursePoint> CoursePoints { get; set; } = new List<CoursePoint>();

    public virtual ICollection<RegistStudent> RegistStudents { get; set; } = new List<RegistStudent>();

    public virtual Semester? SemesterNavigation { get; set; }

    public virtual Subject? SubjectNavigation { get; set; }

    public virtual ICollection<TeachingAssignment> TeachingAssignments { get; set; } = new List<TeachingAssignment>();

    public virtual Term? TermNavigation { get; set; }
}
