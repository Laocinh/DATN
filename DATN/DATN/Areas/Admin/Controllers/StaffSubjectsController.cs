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
    public class StaffSubjectsController : BaseController
    {
        private readonly QldiemSvContext _context;

        public StaffSubjectsController(QldiemSvContext context)
        {
            _context = context;
        }

        // GET: Admin/StaffSubjects
        public async Task<IActionResult> Index(string name, int page = 1)
        {
            int limit = 5;

            var major = await _context.StaffSubjects.Include(s => s.StaffNavigation).OrderBy(c => c.Id).Include(x => x.SubjectNavigation).ToPagedListAsync(page, limit);
         
            ViewBag.keyword = name;
            return View(major);
        }

        // GET: Admin/StaffSubjects/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var staffSubject = await _context.StaffSubjects
                .Include(s => s.StaffNavigation)
                .Include(s => s.SubjectNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (staffSubject == null)
            {
                return NotFound();
            }
            ViewData["Staff"] = new SelectList(_context.Staff, "Id", "Name");
            ViewData["Subject"] = new SelectList(_context.Subjects, "Id", "Name");

            return View(staffSubject);
        }

        // GET: Admin/StaffSubjects/Create
        public IActionResult Create()
        {
            ViewData["Staff"] = new SelectList(_context.Staff, "Id", "Name");
            ViewData["Subject"] = new SelectList(_context.Subjects, "Id", "Name"); ;
            return View();
        }

        // POST: Admin/StaffSubjects/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Staff,Subject,Status,CreateBy,UpdateBy,CreateDate,UpdateDate,IsDelete,IsActive")] StaffSubject staffSubject)
        {
            if (ModelState.IsValid)
            {
                var admin = JsonConvert.DeserializeObject<UserStaff>(HttpContext.Session.GetString("AdminLogin"));
                staffSubject.CreateBy = admin.Username;
                staffSubject.UpdateBy = admin.Username;
                staffSubject.IsDelete = false;
                _context.Add(staffSubject);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["Staff"] = new SelectList(_context.Staff, "Id", "Name", staffSubject.Staff);
            ViewData["Subject"] = new SelectList(_context.Subjects, "Id", "Name", staffSubject.Subject);
            return View(staffSubject);
        }

        // GET: Admin/StaffSubjects/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var staffSubject = await _context.StaffSubjects.FindAsync(id);
            if (staffSubject == null)
            {
                return NotFound();
            }
            ViewData["Staff"] = new SelectList(_context.Staff, "Id", "Name", staffSubject.Staff);
            ViewData["Subject"] = new SelectList(_context.Subjects, "Id", "Name", staffSubject.Subject);
            return View(staffSubject);
        }

        // POST: Admin/StaffSubjects/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("Id,Idstaff,Idsubject,Status,CreateBy,UpdateBy,CreateDate,UpdateDate,IsDelete,IsActive")] StaffSubject staffSubject)
        {
            if (id != staffSubject.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var admin = JsonConvert.DeserializeObject<UserStaff>(HttpContext.Session.GetString("AdminLogin"));
                    staffSubject.UpdateDate = DateTime.Now;
                    staffSubject.UpdateBy = admin.Username;
                    _context.Update(staffSubject);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StaffSubjectExists(staffSubject.Id))
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
            ViewData["Staff"] = new SelectList(_context.Staff, "Id", "Name", staffSubject.Staff);
            ViewData["Subject"] = new SelectList(_context.Subjects, "Id", "Name", staffSubject.Subject); 
            return View(staffSubject);
        }

        // GET: Admin/StaffSubjects/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            var staffSubject = await _context.StaffSubjects.FindAsync(id);
            if (staffSubject != null)
            {
                _context.StaffSubjects.Remove(staffSubject);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // POST: Admin/StaffSubjects/Delete/5
       /* [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var staffSubject = await _context.StaffSubjects.FindAsync(id);
            if (staffSubject != null)
            {
                _context.StaffSubjects.Remove(staffSubject);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }*/

        private bool StaffSubjectExists(long id)
        {
            return _context.StaffSubjects.Any(e => e.Id == id);
        }
    }
}
