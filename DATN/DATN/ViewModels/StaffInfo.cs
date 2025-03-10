﻿namespace DATN.ViewModels
{
    public class StaffInfo
    {
        public long TermId { get; set; }
        public string? StaffCode { get; set; }
        public string? StaffName { get; set; }
        public string? SubjectName { get; set; }
        public string? TermName { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string? Room { get; set; }
        public int? CollegeCredit { get; set; }
    }
}
