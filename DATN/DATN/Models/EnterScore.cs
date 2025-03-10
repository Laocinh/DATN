﻿namespace DATN.ViewModels
{
    public class EnterScore
    {
        public string? StudentCode { get; set; }
        public string? StudentName { get; set; }
        public long PointId { get; set; }
        public long? Student { get; set; }

        public long? DetailTerm { get; set; }

        public long? RegistStudent { get; set; } 

        public double? PointProcess { get; set; }
        public double? AttendancePoint { get; set; }
        public double? OverallScore { get; set; }
        public double? MidtermPoint { get; set; }

        public double? TestScore { get; set; }

        public int? NumberTest { get; set; }

        public long? Staff { get; set; }
        public bool? Status { get; set; }
        public string? CreateBy { get; set; }

        public string? UpdateBy { get; set; }

        public DateTime? CreateDate { get; set; }

        public DateTime? UpdateDate { get; set; }

        public bool? IsDelete { get; set; }

        public bool? IsActive { get; set; }
    }
}
