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
    public class RegistStudentsController : BaseController
    {
        private readonly QldiemSvContext _context;

        public RegistStudentsController(QldiemSvContext context)
        {
            _context = context;
        }

        // GET: Admin/RegistStudents
        public async Task<IActionResult> Index(string name, int page = 1)
        {
            int limit = 5;

            var major = await _context.RegistStudents.Include(x => x.StudentNavigation).Include(y => y.DetailTermNavigation).OrderBy(c => c.Id).ToPagedListAsync(page, limit);
            
            ViewBag.keyword = name;
            return View(major);
        }

        // GET: Admin/RegistStudents/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var registStudent = await _context.RegistStudents
                .Include(r => r.DetailTermNavigation)
                .Include(r => r.StudentNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (registStudent == null)
            {
                return NotFound();
            }
            var sqlQuery = "SELECT DETAIL_TERM.ID,TERM.NAME, '' AS STAFFNAME,DETAIL_TERM.ROOM,'' as SEMESTER FROM [dbo].[DETAIL_TERM] JOIN TERM ON DETAIL_TERM.TERM =TERM.ID ";
            
            var model = _context.Database.SqlQueryRaw<HocPhan>(sqlQuery);
            ViewData["DetailTerm"] = new SelectList(model, "Id", "Name");
            ViewData["Student"] = new SelectList(_context.Students, "Id", "Name");
            return View(registStudent);
        }

        // GET: Admin/RegistStudents/Create
        public IActionResult Create()
        {
            var sqlQuery = "SELECT DETAIL_TERM.ID,TERM.NAME, '' AS STAFFNAME,DETAIL_TERM.ROOM,'' as SEMESTER FROM [dbo].[DETAIL_TERM] JOIN TERM ON DETAIL_TERM.TERM =TERM.ID  ";

            var model = _context.Database.SqlQueryRaw<HocPhan>(sqlQuery);
            ViewData["DetailTerm"] = new SelectList(model, "Id", "Name");
            ViewData["Student"] = new SelectList(_context.Students, "Id", "Name");
            return View();
        }

        // POST: Admin/RegistStudents/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Student,DetailTerm,Relearn,Status,CreateBy,UpdateBy,CreateDate,UpdateDate,IsDelete,IsActive")] RegistStudent registStudent)
        {
            if (ModelState.IsValid)
            {
                var admin = JsonConvert.DeserializeObject<UserStaff>(HttpContext.Session.GetString("AdminLogin"));
                registStudent.CreateBy = admin.Username;
                registStudent.UpdateBy = admin.Username;
                registStudent.IsDelete = false;

                _context.Add(registStudent);
                await _context.SaveChangesAsync();


                var datacoursePoint = _context.CoursePoints.Where(c => c.Id == registStudent.Id).FirstOrDefault();
                CoursePoint at = new CoursePoint();
                at.Student = registStudent.Student;
                at.DetailTerm = registStudent.DetailTerm;
                at.RegistStudent = registStudent.Id;
                at.PointProcess = null;
                at.TestScore = null;
                at.OverallScore = null;
                at.NumberTest = null;
                at.AttendancePoint = null;
                at.MidtermPoint = null;
                at.Status = null;
                at.Staff = null;
                at.CreateBy = admin.Username;
                at.UpdateBy = admin.Username;
                at.IsDelete = false;
                _context.Add(at);
                await _context.SaveChangesAsync();
                datacoursePoint = at;

                return RedirectToAction(nameof(Index));
            }
            var sqlQuery = "SELECT DETAIL_TERM.ID,TERM.NAME, '' AS STAFFNAME,DETAIL_TERM.ROOM,'' as SEMESTER FROM [dbo].[DETAIL_TERM] JOIN TERM ON DETAIL_TERM.TERM =TERM.ID ";

            var model = _context.Database.SqlQueryRaw<HocPhan>(sqlQuery);
            ViewData["DetailTerm"] = new SelectList(model, "Id", "Name", registStudent.DetailTerm);
            ViewData["Student"] = new SelectList(_context.Students, "Id", "Name", registStudent.Student);
            return View(registStudent);
        }

        // GET: Admin/RegistStudents/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var registStudent = await _context.RegistStudents.FindAsync(id);
            if (registStudent == null)
            {
                return NotFound();
            }
            var sqlQuery = "SELECT DETAIL_TERM.ID,TERM.NAME, '' AS STAFFNAME,DETAIL_TERM.ROOM,'' as SEMESTER FROM [dbo].[DETAIL_TERM] JOIN TERM ON DETAIL_TERM.TERM =TERM.ID ";

            var model = _context.Database.SqlQueryRaw<HocPhan>(sqlQuery);
            ViewData["DetailTerm"] = new SelectList(model, "Id", "Name", registStudent.DetailTerm);
            ViewData["Student"] = new SelectList(_context.Students, "Id", "Name", registStudent.Student);
            return View(registStudent);
        }

        // POST: Admin/RegistStudents/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("Id,Student,DetailTerm,Relearn,Status,CreateBy,UpdateBy,CreateDate,UpdateDate,IsDelete,IsActive")] RegistStudent registStudent)
        {
            if (id != registStudent.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var admin = JsonConvert.DeserializeObject<UserStaff>(HttpContext.Session.GetString("AdminLogin"));
                    registStudent.UpdateDate = DateTime.Now;
                    registStudent.UpdateBy = admin.Username;
                    _context.Update(registStudent);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RegistStudentExists(registStudent.Id))
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
            var sqlQuery = "SELECT DETAIL_TERM.ID,TERM.NAME, '' AS STAFFNAME,DETAIL_TERM.ROOM,'' as SEMESTER FROM [dbo].[DETAIL_TERM] JOIN TERM ON DETAIL_TERM.TERM =TERM.ID ";

            var model = _context.Database.SqlQueryRaw<HocPhan>(sqlQuery);
            ViewData["DetailTerm"] = new SelectList(model, "Id", "Name", registStudent.DetailTerm);
            ViewData["Student"] = new SelectList(_context.Students, "Id", "Name", registStudent.Student);
            return View(registStudent);
        }

        // GET: Admin/RegistStudents/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            var registStudent = await _context.RegistStudents.FindAsync(id);
            if (registStudent != null)
            {
                _context.RegistStudents.Remove(registStudent);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // POST: Admin/RegistStudents/Delete/5
       /* [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var registStudent = await _context.RegistStudents.FindAsync(id);
            if (registStudent != null)
            {
                _context.RegistStudents.Remove(registStudent);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }*/

        private bool RegistStudentExists(long id)
        {
            return _context.RegistStudents.Any(e => e.Id == id);
        }
    }
}
