using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace DATN.Models;

public partial class QldiemSvContext : DbContext
{
    public QldiemSvContext()
    {
    }

    public QldiemSvContext(DbContextOptions<QldiemSvContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Class> Classes { get; set; }

    public virtual DbSet<CoursePoint> CoursePoints { get; set; }

    public virtual DbSet<DetailTerm> DetailTerms { get; set; }

    public virtual DbSet<Major> Majors { get; set; }

    public virtual DbSet<Position> Positions { get; set; }

    public virtual DbSet<RegistStudent> RegistStudents { get; set; }

    public virtual DbSet<Semester> Semesters { get; set; }

    public virtual DbSet<Session> Sessions { get; set; }

    public virtual DbSet<Staff> Staff { get; set; }

    public virtual DbSet<StaffSubject> StaffSubjects { get; set; }

    public virtual DbSet<Student> Students { get; set; }

    public virtual DbSet<Subject> Subjects { get; set; }

    public virtual DbSet<TeachingAssignment> TeachingAssignments { get; set; }

    public virtual DbSet<Term> Terms { get; set; }

    public virtual DbSet<UserAdmin> UserAdmins { get; set; }

    public virtual DbSet<UserStaff> UserStaffs { get; set; }

    public virtual DbSet<UserStudent> UserStudents { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=.\\SQLEXPRESS; Database=QLDiemSV; Trusted_Connection=True; MultipleActiveResultSets=True; TrustServercertificate=true;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Class>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__CLASSES__3214EC27ECD18EBD");

            entity.ToTable("CLASSES");

            entity.HasIndex(e => e.Code, "UQ__CLASSES__AA1D43790B9F3598").IsUnique();

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Code)
                .HasMaxLength(25)
                .HasColumnName("CODE");
            entity.Property(e => e.CreateBy)
                .HasMaxLength(450)
                .HasColumnName("CREATE_BY");
            entity.Property(e => e.CreateDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("CREATE_DATE");
            entity.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasColumnName("IS_ACTIVE");
            entity.Property(e => e.IsDelete).HasColumnName("IS_DELETE");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("NAME");
            entity.Property(e => e.UpdateBy)
                .HasMaxLength(450)
                .HasColumnName("UPDATE_BY");
            entity.Property(e => e.UpdateDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("UPDATE_DATE");
        });

        modelBuilder.Entity<CoursePoint>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__COURSE_P__3214EC27F7CA47FB");

            entity.ToTable("COURSE_POINT");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.AttendancePoint).HasColumnName("ATTENDANCE_POINT");
            entity.Property(e => e.CreateBy)
                .HasMaxLength(450)
                .HasColumnName("CREATE_BY");
            entity.Property(e => e.CreateDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("CREATE_DATE");
            entity.Property(e => e.DetailTerm).HasColumnName("DETAIL_TERM");
            entity.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasColumnName("IS_ACTIVE");
            entity.Property(e => e.IsDelete).HasColumnName("IS_DELETE");
            entity.Property(e => e.MidtermPoint).HasColumnName("MIDTERM_POINT");
            entity.Property(e => e.NumberTest).HasColumnName("NUMBER_TEST");
            entity.Property(e => e.OverallScore).HasColumnName("OVERALL_SCORE");
            entity.Property(e => e.PointProcess).HasColumnName("POINT_PROCESS");
            entity.Property(e => e.RegistStudent).HasColumnName("REGIST_STUDENT");
            entity.Property(e => e.Staff).HasColumnName("STAFF");
            entity.Property(e => e.Status).HasColumnName("STATUS");
            entity.Property(e => e.Student).HasColumnName("STUDENT");
            entity.Property(e => e.TestScore).HasColumnName("TEST_SCORE");
            entity.Property(e => e.UpdateBy)
                .HasMaxLength(450)
                .HasColumnName("UPDATE_BY");
            entity.Property(e => e.UpdateDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("UPDATE_DATE");

            entity.HasOne(d => d.DetailTermNavigation).WithMany(p => p.CoursePoints)
                .HasForeignKey(d => d.DetailTerm)
                .HasConstraintName("FK__COURSE_PO__IDDET__503BEA1C");

            entity.HasOne(d => d.RegistStudentNavigation).WithMany(p => p.CoursePoints)
                .HasForeignKey(d => d.RegistStudent)
                .HasConstraintName("FK__COURSE_PO__IDREG__51300E55");

            entity.HasOne(d => d.StaffNavigation).WithMany(p => p.CoursePoints)
                .HasForeignKey(d => d.Staff)
                .HasConstraintName("FK__COURSE_PO__ID_ST__531856C7");

            entity.HasOne(d => d.StudentNavigation).WithMany(p => p.CoursePoints)
                .HasForeignKey(d => d.Student)
                .HasConstraintName("FK__COURSE_PO__IDSTU__4F47C5E3");
        });

        modelBuilder.Entity<DetailTerm>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__DETAIL_T__3214EC27AD33658E");

            entity.ToTable("DETAIL_TERM");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.CreateBy)
                .HasMaxLength(450)
                .HasColumnName("CREATE_BY");
            entity.Property(e => e.CreateDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("CREATE_DATE");
            entity.Property(e => e.EndDate)
                .HasColumnType("datetime")
                .HasColumnName("END_DATE");
            entity.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasColumnName("IS_ACTIVE");
            entity.Property(e => e.IsDelete).HasColumnName("IS_DELETE");
            entity.Property(e => e.MaxNumber).HasColumnName("MAX_NUMBER");
            entity.Property(e => e.Room)
                .HasMaxLength(255)
                .HasColumnName("ROOM");
            entity.Property(e => e.Semester).HasColumnName("SEMESTER");
            entity.Property(e => e.StartDate)
                .HasColumnType("datetime")
                .HasColumnName("START_DATE");
            entity.Property(e => e.Subject).HasColumnName("SUBJECT");
            entity.Property(e => e.Term).HasColumnName("TERM");
            entity.Property(e => e.UpdateBy)
                .HasMaxLength(450)
                .HasColumnName("UPDATE_BY");
            entity.Property(e => e.UpdateDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("UPDATE_DATE");

            entity.HasOne(d => d.SemesterNavigation).WithMany(p => p.DetailTerms)
                .HasForeignKey(d => d.Semester)
                .HasConstraintName("FK__DETAIL_TE__SEMES__6754599E");

            entity.HasOne(d => d.SubjectNavigation).WithMany(p => p.DetailTerms)
                .HasForeignKey(d => d.Subject)
                .HasConstraintName("FK_DETAIL_TERM_SUBJECT");

            entity.HasOne(d => d.TermNavigation).WithMany(p => p.DetailTerms)
                .HasForeignKey(d => d.Term)
                .HasConstraintName("FK__DETAIL_TER__TERM__66603565");
        });

        modelBuilder.Entity<Major>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__MAJOR__3214EC2709ADB72F");

            entity.ToTable("MAJOR");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.CreateBy)
                .HasMaxLength(450)
                .HasColumnName("CREATE_BY");
            entity.Property(e => e.CreateDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("CREATE_DATE");
            entity.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasColumnName("IS_ACTIVE");
            entity.Property(e => e.IsDelete).HasColumnName("IS_DELETE");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("NAME");
            entity.Property(e => e.UpdateBy)
                .HasMaxLength(450)
                .HasColumnName("UPDATE_BY");
            entity.Property(e => e.UpdateDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("UPDATE_DATE");
        });

        modelBuilder.Entity<Position>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__POSITION__3214EC27F55A44F5");

            entity.ToTable("POSITION");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.CreateBy)
                .HasMaxLength(450)
                .HasColumnName("CREATE_BY");
            entity.Property(e => e.CreateDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("CREATE_DATE");
            entity.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasColumnName("IS_ACTIVE");
            entity.Property(e => e.IsDelete).HasColumnName("IS_DELETE");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("NAME");
            entity.Property(e => e.UpdateBy)
                .HasMaxLength(450)
                .HasColumnName("UPDATE_BY");
            entity.Property(e => e.UpdateDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("UPDATE_DATE");
        });

        modelBuilder.Entity<RegistStudent>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__REGIST_S__3214EC27ECA27662");

            entity.ToTable("REGIST_STUDENT");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.CreateBy)
                .HasMaxLength(450)
                .HasColumnName("CREATE_BY");
            entity.Property(e => e.CreateDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("CREATE_DATE");
            entity.Property(e => e.DetailTerm).HasColumnName("DETAIL_TERM");
            entity.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasColumnName("IS_ACTIVE");
            entity.Property(e => e.IsDelete).HasColumnName("IS_DELETE");
            entity.Property(e => e.Relearn).HasColumnName("RELEARN");
            entity.Property(e => e.Status).HasColumnName("STATUS");
            entity.Property(e => e.Student).HasColumnName("STUDENT");
            entity.Property(e => e.UpdateBy)
                .HasMaxLength(450)
                .HasColumnName("UPDATE_BY");
            entity.Property(e => e.UpdateDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("UPDATE_DATE");

            entity.HasOne(d => d.DetailTermNavigation).WithMany(p => p.RegistStudents)
                .HasForeignKey(d => d.DetailTerm)
                .HasConstraintName("FK__REGIST_ST__DETAI__245D67DE");

            entity.HasOne(d => d.StudentNavigation).WithMany(p => p.RegistStudents)
                .HasForeignKey(d => d.Student)
                .HasConstraintName("FK__REGIST_ST__STUDE__236943A5");
        });

        modelBuilder.Entity<Semester>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__SEMESTER__3214EC27D93E0375");

            entity.ToTable("SEMESTER");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.CreateBy)
                .HasMaxLength(450)
                .HasColumnName("CREATE_BY");
            entity.Property(e => e.CreateDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("CREATE_DATE");
            entity.Property(e => e.EndDate)
                .HasColumnType("datetime")
                .HasColumnName("END_DATE");
            entity.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasColumnName("IS_ACTIVE");
            entity.Property(e => e.IsDelete).HasColumnName("IS_DELETE");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("NAME");
            entity.Property(e => e.StartDate)
                .HasColumnType("datetime")
                .HasColumnName("START_DATE");
            entity.Property(e => e.UpdateBy)
                .HasMaxLength(450)
                .HasColumnName("UPDATE_BY");
            entity.Property(e => e.UpdateDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("UPDATE_DATE");
        });

        modelBuilder.Entity<Session>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__SESSION__3214EC27CB650C02");

            entity.ToTable("SESSION");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.CreateBy)
                .HasMaxLength(450)
                .HasColumnName("CREATE_BY");
            entity.Property(e => e.CreateDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("CREATE_DATE");
            entity.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasColumnName("IS_ACTIVE");
            entity.Property(e => e.IsDelete).HasColumnName("IS_DELETE");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("NAME");
            entity.Property(e => e.StartDate).HasColumnName("START_DATE");
            entity.Property(e => e.UpdateBy)
                .HasMaxLength(450)
                .HasColumnName("UPDATE_BY");
            entity.Property(e => e.UpdateDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("UPDATE_DATE");
        });

        modelBuilder.Entity<Staff>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__STAFF__3214EC2766C6F717");

            entity.ToTable("STAFF");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.BirthDate)
                .HasColumnType("datetime")
                .HasColumnName("BIRTH_DATE");
            entity.Property(e => e.Code)
                .HasMaxLength(25)
                .IsUnicode(false)
                .HasColumnName("CODE");
            entity.Property(e => e.CreateBy)
                .HasMaxLength(450)
                .HasColumnName("CREATE_BY");
            entity.Property(e => e.CreateDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("CREATE_DATE");
            entity.Property(e => e.Degree)
                .HasMaxLength(255)
                .HasColumnName("DEGREE");
            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("EMAIL");
            entity.Property(e => e.Gender).HasColumnName("GENDER");
            entity.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasColumnName("IS_ACTIVE");
            entity.Property(e => e.IsDelete).HasColumnName("IS_DELETE");
            entity.Property(e => e.Major).HasColumnName("MAJOR");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("NAME");
            entity.Property(e => e.NumberPhone)
                .HasMaxLength(15)
                .IsUnicode(false)
                .HasColumnName("NUMBER_PHONE");
            entity.Property(e => e.Position).HasColumnName("POSITION");
            entity.Property(e => e.UpdateBy)
                .HasMaxLength(450)
                .HasColumnName("UPDATE_BY");
            entity.Property(e => e.UpdateDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("UPDATE_DATE");
            entity.Property(e => e.Yearofwork).HasColumnName("YEAROFWORK");

            entity.HasOne(d => d.MajorNavigation).WithMany(p => p.Staff)
                .HasForeignKey(d => d.Major)
                .HasConstraintName("FK__STAFF__MAJOR__571DF1D5");

            entity.HasOne(d => d.PositionNavigation).WithMany(p => p.Staff)
                .HasForeignKey(d => d.Position)
                .HasConstraintName("FK__STAFF__POSITION__5812160E");
        });

        modelBuilder.Entity<StaffSubject>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__STAFF_SU__3214EC27195D64A4");

            entity.ToTable("STAFF_SUBJECT");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.CreateBy)
                .HasMaxLength(450)
                .HasColumnName("CREATE_BY");
            entity.Property(e => e.CreateDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("CREATE_DATE");
            entity.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasColumnName("IS_ACTIVE");
            entity.Property(e => e.IsDelete).HasColumnName("IS_DELETE");
            entity.Property(e => e.Staff).HasColumnName("STAFF");
            entity.Property(e => e.Status).HasColumnName("STATUS");
            entity.Property(e => e.Subject).HasColumnName("SUBJECT");
            entity.Property(e => e.UpdateBy)
                .HasMaxLength(450)
                .HasColumnName("UPDATE_BY");
            entity.Property(e => e.UpdateDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("UPDATE_DATE");

            entity.HasOne(d => d.StaffNavigation).WithMany(p => p.StaffSubjects)
                .HasForeignKey(d => d.Staff)
                .HasConstraintName("FK__STAFF_SUB__IDSTA__151B244E");

            entity.HasOne(d => d.SubjectNavigation).WithMany(p => p.StaffSubjects)
                .HasForeignKey(d => d.Subject)
                .HasConstraintName("FK__STAFF_SUB__IDSUB__160F4887");
        });

        modelBuilder.Entity<Student>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__STUDENT__3214EC271F4D2506");

            entity.ToTable("STUDENT");

            entity.HasIndex(e => e.Code, "UQ__STUDENT__AA1D4379AD71E204").IsUnique();

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.AccountNumber)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("ACCOUNT_NUMBER");
            entity.Property(e => e.Address)
                .HasMaxLength(500)
                .HasColumnName("ADDRESS");
            entity.Property(e => e.BirthDate).HasColumnName("BIRTH_DATE");
            entity.Property(e => e.City)
                .HasMaxLength(50)
                .HasColumnName("CITY");
            entity.Property(e => e.Classes).HasColumnName("CLASSES");
            entity.Property(e => e.Code)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("CODE");
            entity.Property(e => e.CreateBy)
                .HasMaxLength(450)
                .HasColumnName("CREATE_BY");
            entity.Property(e => e.CreateDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("CREATE_DATE");
            entity.Property(e => e.CreateDateIdentityCard)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("CREATE_DATE_IDENTITY_CARD");
            entity.Property(e => e.District)
                .HasMaxLength(50)
                .HasColumnName("DISTRICT");
            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("EMAIL");
            entity.Property(e => e.Gender).HasColumnName("GENDER");
            entity.Property(e => e.IdentityCard)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("IDENTITY_CARD");
            entity.Property(e => e.Image)
                .HasMaxLength(2000)
                .HasColumnName("IMAGE");
            entity.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasColumnName("IS_ACTIVE");
            entity.Property(e => e.IsDelete).HasColumnName("IS_DELETE");
            entity.Property(e => e.Major).HasColumnName("MAJOR");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("NAME");
            entity.Property(e => e.NameBank)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("NAME_BANK");
            entity.Property(e => e.Nation)
                .HasMaxLength(50)
                .HasColumnName("NATION");
            entity.Property(e => e.Nationality)
                .HasMaxLength(50)
                .HasColumnName("NATIONALITY");
            entity.Property(e => e.Nationals)
                .HasMaxLength(50)
                .HasColumnName("NATIONALS");
            entity.Property(e => e.NumberPhone)
                .HasMaxLength(15)
                .IsUnicode(false)
                .HasColumnName("NUMBER_PHONE");
            entity.Property(e => e.PhoneFamily)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("PHONE_FAMILY");
            entity.Property(e => e.PlaceIdentityCard)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("PLACE_IDENTITY_CARD");
            entity.Property(e => e.Session).HasColumnName("SESSION");
            entity.Property(e => e.Status)
                .HasDefaultValue(true)
                .HasColumnName("STATUS");
            entity.Property(e => e.UpdateBy)
                .HasMaxLength(450)
                .HasColumnName("UPDATE_BY");
            entity.Property(e => e.UpdateDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("UPDATE_DATE");
            entity.Property(e => e.Ward)
                .HasMaxLength(50)
                .HasColumnName("WARD");

            entity.HasOne(d => d.ClassesNavigation).WithMany(p => p.Students)
                .HasForeignKey(d => d.Classes)
                .HasConstraintName("FK__STUDENT__CLASSES__787EE5A0");

            entity.HasOne(d => d.MajorNavigation).WithMany(p => p.Students)
                .HasForeignKey(d => d.Major)
                .HasConstraintName("FK__STUDENT__MAJOR__797309D9");

            entity.HasOne(d => d.SessionNavigation).WithMany(p => p.Students)
                .HasForeignKey(d => d.Session)
                .HasConstraintName("FK__STUDENT__SESSION__778AC167");
        });

        modelBuilder.Entity<Subject>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__SUBJECT__3214EC2732895102");

            entity.ToTable("SUBJECT");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.CreateBy)
                .HasMaxLength(450)
                .HasColumnName("CREATE_BY");
            entity.Property(e => e.CreateDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("CREATE_DATE");
            entity.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasColumnName("IS_ACTIVE");
            entity.Property(e => e.IsDelete).HasColumnName("IS_DELETE");
            entity.Property(e => e.Major).HasColumnName("MAJOR");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("NAME");
            entity.Property(e => e.Status).HasColumnName("STATUS");
            entity.Property(e => e.Term).HasColumnName("TERM");
            entity.Property(e => e.UpdateBy)
                .HasMaxLength(450)
                .HasColumnName("UPDATE_BY");
            entity.Property(e => e.UpdateDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("UPDATE_DATE");

            entity.HasOne(d => d.MajorNavigation).WithMany(p => p.Subjects)
                .HasForeignKey(d => d.Major)
                .HasConstraintName("FK__SUBJECT__MAJOR__70DDC3D8");

            entity.HasOne(d => d.TermNavigation).WithMany(p => p.Subjects)
                .HasForeignKey(d => d.Term)
                .HasConstraintName("FK_SUBJECT_TERM");
        });

        modelBuilder.Entity<TeachingAssignment>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__TEACHING__3214EC27A36A9F64");

            entity.ToTable("TEACHING_ASSIGNMENT");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.CreateBy)
                .HasMaxLength(450)
                .HasColumnName("CREATE_BY");
            entity.Property(e => e.CreateDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("CREATE_DATE");
            entity.Property(e => e.DetailTerm).HasColumnName("DETAIL_TERM");
            entity.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasColumnName("IS_ACTIVE");
            entity.Property(e => e.IsDelete).HasColumnName("IS_DELETE");
            entity.Property(e => e.Staff).HasColumnName("STAFF");
            entity.Property(e => e.Status).HasColumnName("STATUS");
            entity.Property(e => e.UpdateBy)
                .HasMaxLength(450)
                .HasColumnName("UPDATE_BY");
            entity.Property(e => e.UpdateDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("UPDATE_DATE");

            entity.HasOne(d => d.DetailTermNavigation).WithMany(p => p.TeachingAssignments)
                .HasForeignKey(d => d.DetailTerm)
                .HasConstraintName("FK__TEACHING___DETAI__0D7A0286");

            entity.HasOne(d => d.StaffNavigation).WithMany(p => p.TeachingAssignments)
                .HasForeignKey(d => d.Staff)
                .HasConstraintName("FK__TEACHING___STAFF__0E6E26BF");
        });

        modelBuilder.Entity<Term>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__TERM__3214EC2792CDAF22");

            entity.ToTable("TERM");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.CollegeCredit).HasColumnName("COLLEGE_CREDIT");
            entity.Property(e => e.CreateBy)
                .HasMaxLength(450)
                .HasColumnName("CREATE_BY");
            entity.Property(e => e.CreateDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("CREATE_DATE");
            entity.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasColumnName("IS_ACTIVE");
            entity.Property(e => e.IsDelete).HasColumnName("IS_DELETE");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("NAME");
            entity.Property(e => e.UpdateBy)
                .HasMaxLength(450)
                .HasColumnName("UPDATE_BY");
            entity.Property(e => e.UpdateDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("UPDATE_DATE");
        });

        modelBuilder.Entity<UserAdmin>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__USER_ADM__3214EC274CAA8B1F");

            entity.ToTable("USER_ADMIN");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.CreateBy)
                .HasMaxLength(450)
                .HasColumnName("CREATE_BY");
            entity.Property(e => e.CreateDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("CREATE_DATE");
            entity.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasColumnName("IS_ACTIVE");
            entity.Property(e => e.IsDelete).HasColumnName("IS_DELETE");
            entity.Property(e => e.Password)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("PASSWORD");
            entity.Property(e => e.UpdateBy)
                .HasMaxLength(450)
                .HasColumnName("UPDATE_BY");
            entity.Property(e => e.UpdateDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("UPDATE_DATE");
            entity.Property(e => e.Username)
                .HasMaxLength(50)
                .HasColumnName("USERNAME");
        });

        modelBuilder.Entity<UserStaff>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__USER_STA__3214EC27E9C98F9D");

            entity.ToTable("USER_STAFF");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.CreateBy)
                .HasMaxLength(450)
                .HasColumnName("CREATE_BY");
            entity.Property(e => e.CreateDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("CREATE_DATE");
            entity.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasColumnName("IS_ACTIVE");
            entity.Property(e => e.IsDelete).HasColumnName("IS_DELETE");
            entity.Property(e => e.Password)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("PASSWORD");
            entity.Property(e => e.Staff).HasColumnName("STAFF");
            entity.Property(e => e.UpdateBy)
                .HasMaxLength(450)
                .HasColumnName("UPDATE_BY");
            entity.Property(e => e.UpdateDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("UPDATE_DATE");
            entity.Property(e => e.Username)
                .HasMaxLength(50)
                .HasColumnName("USERNAME");

            entity.HasOne(d => d.StaffNavigation).WithMany(p => p.UserStaffs)
                .HasForeignKey(d => d.Staff)
                .HasConstraintName("FK__USER_STAF__STAFF__04E4BC85");
        });

        modelBuilder.Entity<UserStudent>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__USER_STU__3214EC273AAAE785");

            entity.ToTable("USER_STUDENT");

            entity.HasIndex(e => e.Username, "UQ__USER_STU__B15BE12EB50F3BEC").IsUnique();

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.CreateBy)
                .HasMaxLength(450)
                .HasColumnName("CREATE_BY");
            entity.Property(e => e.CreateDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("CREATE_DATE");
            entity.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasColumnName("IS_ACTIVE");
            entity.Property(e => e.IsDelete).HasColumnName("IS_DELETE");
            entity.Property(e => e.Password)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("PASSWORD");
            entity.Property(e => e.Student).HasColumnName("STUDENT");
            entity.Property(e => e.UpdateBy)
                .HasMaxLength(450)
                .HasColumnName("UPDATE_BY");
            entity.Property(e => e.UpdateDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("UPDATE_DATE");
            entity.Property(e => e.Username)
                .HasMaxLength(50)
                .HasColumnName("USERNAME");

            entity.HasOne(d => d.StudentNavigation).WithMany(p => p.UserStudents)
                .HasForeignKey(d => d.Student)
                .HasConstraintName("FK__USER_STUD__STUDE__42E1EEFE");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
