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

namespace DATN.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class SemestersController : BaseController
    {
        private readonly QldiemSvContext _context;

        public SemestersController(QldiemSvContext context)
        {
            _context = context;
        }

        // GET: Admin/Semesters
        public async Task<IActionResult> Index(string name, int page = 1)
        {
            int limit = 5;

            var major = await _context.Semesters.OrderBy(c => c.Id).ToPagedListAsync(page, limit);
            if (!String.IsNullOrEmpty(name))
            {
                major = await _context.Semesters.Where(c => c.Name.Contains(name)).OrderBy(c => c.Id).ToPagedListAsync(page, limit);
            }
            ViewBag.keyword = name;
            return View(major);
        }

        // GET: Admin/Semesters/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var semester = await _context.Semesters
                .FirstOrDefaultAsync(m => m.Id == id);
            if (semester == null)
            {
                return NotFound();
            }

            return View(semester);
        }

        // GET: Admin/Semesters/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/Semesters/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,StartDate,EndDate,CreateBy,UpdateBy,CreateDate,UpdateDate,IsDelete,IsActive")] Semester semester)
        {
            if (ModelState.IsValid)
            {
                var admin = JsonConvert.DeserializeObject<UserStaff>(HttpContext.Session.GetString("AdminLogin"));
                semester.CreateBy = admin.Username;
                semester.UpdateBy = admin.Username;
                semester.IsDelete = false;
                _context.Add(semester);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(semester);
        }

        // GET: Admin/Semesters/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var semester = await _context.Semesters.FindAsync(id);
            if (semester == null)
            {
                return NotFound();
            }
            return View(semester);
        }

        // POST: Admin/Semesters/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,StartDate,EndDate,CreateBy,UpdateBy,CreateDate,UpdateDate,IsDelete,IsActive")] Semester semester)
        {
            if (id != semester.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var admin = JsonConvert.DeserializeObject<UserStaff>(HttpContext.Session.GetString("AdminLogin"));
                    semester.UpdateDate = DateTime.Now;
                    semester.UpdateBy = admin.Username;
                    _context.Update(semester);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SemesterExists(semester.Id))
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
            return View(semester);
        }

        // GET: Admin/Semesters/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            var semester = await _context.Semesters.FindAsync(id);
            if (semester != null)
            {
                _context.Semesters.Remove(semester);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // POST: Admin/Semesters/Delete/5
       /* [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var semester = await _context.Semesters.FindAsync(id);
            if (semester != null)
            {
                _context.Semesters.Remove(semester);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
*/
        private bool SemesterExists(int id)
        {
            return _context.Semesters.Any(e => e.Id == id);
        }
    }
}
