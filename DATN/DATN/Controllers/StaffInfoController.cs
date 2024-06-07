using DATN.Models;
using DATN.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace DATN.Controllers
{
    public class StaffInfoController : BaseController
    {
        private readonly QldiemSvContext _context;

        public StaffInfoController(QldiemSvContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            var user_staff = JsonConvert.DeserializeObject<UserStaff>(HttpContext.Session.GetString("StaffLogin"));

            var data = await (from userstaff in _context.UserStaffs
                             join staff in _context.Staff on userstaff.Staff equals staff.Id
                             join teachingassignment in _context.TeachingAssignments on staff.Id equals teachingassignment.Staff
                             join detailterm in _context.DetailTerms on teachingassignment.DetailTerm equals detailterm.Id
                             join term in _context.Terms on detailterm.Term equals term.Id 
                              join staffsubject in _context.StaffSubjects on staff.Id equals staffsubject.Staff
                             join subject in _context.Subjects on staffsubject.Subject equals subject.Id
                             where userstaff.Id == user_staff.Id

                             select new StaffInfo
                             {
                                 TermId = term.Id,
                                 StaffCode = staff.Code,
                                 StaffName = staff.Name,
                                 SubjectName = subject.Name,
                                 TermName = term.Name,
                                 StartDate = detailterm.StartDate,
                                 EndDate = detailterm.EndDate,
                                 Room = detailterm.Room,
                                 CollegeCredit = term.CollegeCredit,
                             }).ToListAsync();

            return View(data);
        }

        public async Task<IActionResult> EnterScore(long? id)
        {
            var data = await (from term in _context.Terms
                              join detailterm in _context.DetailTerms on term.Id equals detailterm.Term
                              join registstudent in _context.RegistStudents on detailterm.Id equals registstudent.DetailTerm
                              join student in _context.Students on registstudent.Student equals student.Id
                              join CoursePoint in _context.CoursePoints on registstudent.Id equals CoursePoint.RegistStudent
                              where term.Id == id
                              group new { student, CoursePoint } by new
                              {
                                  student.Code,
                                  student.Name,
                                  coursepointId = CoursePoint.Id,
                                  CoursePoint.PointProcess,
                                  CoursePoint.AttendancePoint,
                                  CoursePoint.MidtermPoint,
                                  CoursePoint.TestScore,
                                  CoursePoint.OverallScore,
                                  CoursePoint.Student,
                                  CoursePoint.DetailTerm,
                                  CoursePoint.RegistStudent,
                                  CoursePoint.NumberTest,
                                  CoursePoint.Staff,
                                  CoursePoint.Status,
                                  CoursePoint.CreateBy,
                                  CoursePoint.UpdateBy,
                                  CoursePoint.CreateDate,
                                  CoursePoint.UpdateDate,
                                  CoursePoint.IsDelete,
                                  CoursePoint.IsActive
                              } into g
                              select new EnterScore
                              {
                                  StudentCode = g.Key.Code,
                                  StudentName = g.Key.Name,
                                  AttendancePoint = g.Key.AttendancePoint,
                                  PointProcess = g.Key.PointProcess,
                                  PointId = g.Key.coursepointId,
                                  MidtermPoint = g.Key.MidtermPoint,
                                  TestScore = g.Key.TestScore,
                                  Student = g.Key.Student,
                                  OverallScore = g.Key.OverallScore,
                                  Status = g.Key.Status,
                                  DetailTerm = g.Key.DetailTerm,
                                  RegistStudent = g.Key.RegistStudent,
                                  NumberTest = g.Key.NumberTest,
                                  Staff = g.Key.Staff,
                                  CreateBy = g.Key.CreateBy,
                                  UpdateBy = g.Key.UpdateBy,
                                  CreateDate = g.Key.CreateDate,
                                  UpdateDate = g.Key.UpdateDate,
                                  IsDelete = g.Key.IsDelete,
                                  IsActive = g.Key.IsActive,
                              }).ToListAsync();
            var termName = await _context.Terms.FindAsync(id);
            ViewBag.TermName = termName.Name;
            return View(data);
        }

        [HttpPost]
        public async Task<IActionResult> EnterScore(IFormCollection form)
        {
            var user_staff = JsonConvert.DeserializeObject<UserStaff>(HttpContext.Session.GetString("StaffLogin"));
            int itemCount = form["PointProcess"].Count;
            for (int i = 0; i < itemCount; i++)
            {
                CoursePoint coursePoint = new CoursePoint();
                coursePoint.Id = long.Parse(form["PointId"][i]); 
                coursePoint.DetailTerm = long.Parse(form["DetailTerm"][i]);
                coursePoint.Student = long.Parse(form["Student"][i]);
                coursePoint.RegistStudent = long.Parse(form["RegistStudent"][i]); 
                coursePoint.PointProcess = Double.Parse(form["PointProcess"][i]);
                coursePoint.MidtermPoint = Double.Parse(form["MidtermPoint"][i]);
                coursePoint.TestScore = Double.Parse(form["TestScore"][i]);
                Double valueToRound = (coursePoint.PointProcess ?? 0) * 0.1 + (coursePoint.MidtermPoint ?? 0) * 0.3 + (coursePoint.TestScore ?? 0) * 0.6;
                coursePoint.OverallScore = Math.Round(valueToRound, 2);
                if (coursePoint.OverallScore >= 4)
                {
                    coursePoint.Status = true;
                }
                else
                {
                    coursePoint.Status = false;
                }
                coursePoint.NumberTest = 1;
                coursePoint.Staff = user_staff.Staff;
                coursePoint.CreateBy = form["CreateBy"][i].ToString();
                coursePoint.UpdateBy = user_staff.Username;
                coursePoint.CreateDate = DateTime.Parse(form["CreateDate"][i]);
                coursePoint.UpdateDate = DateTime.Now;
                coursePoint.IsActive = bool.Parse(form["IsActive"][i]);
                coursePoint.IsDelete = bool.Parse(form["IsDelete"][i]);

                _context.Update(coursePoint);
               
            }
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(EnterScore));
        }
    }


}
