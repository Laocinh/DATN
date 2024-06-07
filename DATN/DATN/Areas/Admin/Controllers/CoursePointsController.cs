using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DATN.Models;
using X.PagedList;
using Newtonsoft.Json;
using DATN.ViewModels;

namespace DATN.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CoursePointsController : BaseController
    {
        private readonly QldiemSvContext _context;

        public CoursePointsController(QldiemSvContext context)
        {
            _context = context;
        }

        // GET: Admin/CoursePoints
        public async Task<IActionResult> Index(string name, int page = 1)
        {
            int limit = 5;

            var model = await _context.CoursePoints.Include(s => s.DetailTermNavigation).Include(x => x.RegistStudentNavigation).Include(y => y.StaffNavigation).Include(t => t.StudentNavigation).OrderBy(c => c.Id).ToPagedListAsync(page, limit);

            ViewBag.keyword = name;
            return View(model);
        }

        // GET: Admin/CoursePoints/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var coursePoint = await _context.CoursePoints
                .Include(c => c.DetailTermNavigation)
                 
                .Include(c => c.RegistStudentNavigation)
                .Include(c => c.StaffNavigation)
                .Include(c => c.StudentNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (coursePoint == null)
            {
                return NotFound();
            }
            var detailTerm = from DetailTerm in _context.DetailTerms
                             join term in _context.Terms on DetailTerm.Term equals term.Id
                             select new { Id = DetailTerm.Id, Name = term.Name };
            ViewData["DetailTerm"] = new SelectList(detailTerm, "Id", "Name");

            //var registStudent = _context.RegistStudents.Where(x=>x.Student)
            ViewData["RegistStudent"] = new SelectList(_context.RegistStudents, "Id", "Id");
            ViewData["Staff"] = new SelectList(_context.Staff, "Id", "Name");
            ViewData["Student"] = new SelectList(_context.Students, "Id", "Name");
            return View(coursePoint);
        }

        // GET: Admin/CoursePoints/Create
        public IActionResult Create()
        {
            ViewData["DetailTerm"] = new SelectList(_context.DetailTerms, "Id", "Id");
           
            ViewData["RegistStudent"] = new SelectList(_context.RegistStudents, "Id", "Id");
            ViewData["Staff"] = new SelectList(_context.Staff, "Id", "Name");
            ViewData["Student"] = new SelectList(_context.Students, "Id", "Name");
            return View();
        }

        // POST: Admin/CoursePoints/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Student,DetailTerm,RegistStudent,PointProcess,TestScore,OverallScore,NumberTest,Status,Staff,CreateBy,UpdateBy,CreateDate,UpdateDate,IsDelete,IsActive")] CoursePoint coursePoint)
        {
            if (ModelState.IsValid)
            {
                var admin = JsonConvert.DeserializeObject<UserStaff>(HttpContext.Session.GetString("AdminLogin"));
                coursePoint.CreateBy = admin.Username;
                coursePoint.UpdateBy = admin.Username;
                coursePoint.IsDelete = false;
                _context.Add(coursePoint);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["DetailTerm"] = new SelectList(_context.DetailTerms, "Id", "Id", coursePoint.DetailTerm);
             
            ViewData["RegistStudent"] = new SelectList(_context.RegistStudents, "Id", "Id", coursePoint.RegistStudent);
            ViewData["Staff"] = new SelectList(_context.Staff, "Id", "Name", coursePoint.Staff);
            ViewData["Student"] = new SelectList(_context.Students, "Id", "Name", coursePoint.Student);
            return View(coursePoint);
        }

        // GET: Admin/CoursePoints/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var coursePoint = await _context.CoursePoints.FindAsync(id);
            if (coursePoint == null)
            {
                return NotFound();
            }
            ViewData["DetailTerm"] = new SelectList(_context.DetailTerms, "Id", "Id", coursePoint.DetailTerm);
            
            ViewData["RegistStudent"] = new SelectList(_context.RegistStudents, "Id", "Id", coursePoint.RegistStudent);
            ViewData["Staff"] = new SelectList(_context.Staff, "Id", "Name", coursePoint.Staff);
            ViewData["Student"] = new SelectList(_context.Students, "Id", "Name", coursePoint.Student);
            return View(coursePoint);
        }

        // POST: Admin/CoursePoints/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("Id,Student,DetailTerm,RegistStudent,PointProcess,TestScore,OverallScore,NumberTest,Status,Staff,CreateBy,UpdateBy,CreateDate,UpdateDate,IsDelete,IsActive")] CoursePoint coursePoint)
        {
            if (id != coursePoint.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var admin = JsonConvert.DeserializeObject<UserStaff>(HttpContext.Session.GetString("AdminLogin"));
                    coursePoint.UpdateDate = DateTime.Now;
                    coursePoint.UpdateBy = admin.Username;
                    _context.Update(coursePoint);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CoursePointExists(coursePoint.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["DetailTerm"] = new SelectList(_context.DetailTerms, "Id", "Id", coursePoint.DetailTerm);
 
            ViewData["RegistStudent"] = new SelectList(_context.RegistStudents, "Id", "Id", coursePoint.RegistStudent);
            ViewData["Staff"] = new SelectList(_context.Staff, "Id", "Name", coursePoint.Staff);
            ViewData["Student"] = new SelectList(_context.Students, "Id", "Name", coursePoint.Student);
            return View(coursePoint);
        }

        // GET: Admin/CoursePoints/Delete/Diem
        public async Task<IActionResult> Delete(long? id, long? termId)
        {
            var coursePoint = await _context.CoursePoints.FindAsync(id);
            if (coursePoint != null)
            {
                coursePoint.PointProcess = null;
                coursePoint.MidtermPoint = null;
                coursePoint.TestScore = null;
                coursePoint.OverallScore = null;   
                _context.Update(coursePoint);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(DetailCoursePoint), new { id = termId });
        }


        // GET: Admin/CoursePoints/Delete/5
        public async Task<IActionResult> Delete1(long? id)
        {
            var @coursePoint = await _context.CoursePoints.FindAsync(id);
            if (@coursePoint != null)
            {
                _context.CoursePoints.Remove(@coursePoint);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        // POST: Admin/CoursePoints/Delete/5
        /*  [HttpPost, ActionName("Delete")]
          [ValidateAntiForgeryToken]
          public async Task<IActionResult> DeleteConfirmed(long id)
          {
              var coursePoint = await _context.CoursePoints.FindAsync(id);
              if (coursePoint != null)
              {
                  _context.CoursePoints.Remove(coursePoint);
              }

              await _context.SaveChangesAsync();
              return RedirectToAction(nameof(Index));
          }
  */
        private bool CoursePointExists(long id)
        {
            return _context.CoursePoints.Any(e => e.Id == id);
        }


        // GET: Admin/CoursePoints
        public async Task<IActionResult> TongKetTheoHocPhan(string name, int page = 1)
        {
            int limit = 5;

            //var model = await (from detailTerms in _context.DetailTerms
            //                   select
            //                   //join teachingAssignments in _context.TeachingAssignments on detailTerms.TeachingAssignments equals teachingAssignments.Id
            //                   //join staff in _context.Staff on teachingAssignments.staff equals staff.Id
            //                   //join semesters in _context.Semesters on detailTerms.Semester equals semesters.Id
            //                   //select new DetailTerm { }
            //                  );

            var sqlQuery = "SELECT TERM.ID,TERM.NAME, STAFF.NAME AS STAFFNAME,DETAIL_TERM.ROOM,SEMESTER.NAME as SEMESTER FROM [dbo].[DETAIL_TERM] JOIN TEACHING_ASSIGNMENT ON DETAIL_TERM.ID = TEACHING_ASSIGNMENT.DETAIL_TERM JOIN STAFF ON TEACHING_ASSIGNMENT.STAFF = STAFF.ID JOIN SEMESTER ON DETAIL_TERM.SEMESTER =SEMESTER.ID JOIN TERM ON DETAIL_TERM.TERM =TERM.ID ";
            if (!string.IsNullOrEmpty(name))
            {
                sqlQuery += " WHERE TERM.NAME = N'" + name + "'";
            }
            var model = _context.Database.SqlQueryRaw<HocPhan>(sqlQuery);
            ViewBag.keyword = name;
            return View(model);
        }

        public async Task<IActionResult> DetailCoursePoint(long? id, string name, int page = 1)
        {
            int limit = 5;
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
            ViewBag.TermId = id;
            //ViewBag.ClassName = 
            return View(data);
        }
    }
}
