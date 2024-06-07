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
    public class SubjectsController : BaseController
    {
        private readonly QldiemSvContext _context;

        public SubjectsController(QldiemSvContext context)
        {
            _context = context;
        }

        // GET: Admin/Subjects
        public async Task<IActionResult> Index(string name, int page = 1)
        {
            int limit = 5;

            var major = await _context.Subjects.Include(s => s.MajorNavigation).Include(y => y.TermNavigation).OrderBy(c => c.Id).ToPagedListAsync(page, limit);
            if (!String.IsNullOrEmpty(name))
            {
                major = await _context.Subjects.Where(c => c.Name.Contains(name)).OrderBy(c => c.Id).ToPagedListAsync(page, limit);
            }
            ViewBag.keyword = name;
            return View(major);
        }

        // GET: Admin/Subjects/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var subject = await _context.Subjects.Include(s => s.MajorNavigation).Include(y => y.TermNavigation)
                
                

                .FirstOrDefaultAsync(m => m.Id == id);
            if (subject == null)
            {
                return NotFound();
            }
            ViewData["Major"] = new SelectList(_context.Majors, "Id", "Name", subject.Major);
            ViewData["Term"] = new SelectList(_context.Terms, "Id", "Name", subject.Term);
            return View(subject);
        }

        // GET: Admin/Subjects/Create
        public IActionResult Create()
        {
            ViewData["Major"] = new SelectList(_context.Majors, "Id", "Name");
            ViewData["Term"] = new SelectList(_context.Terms, "Id", "Name");
            return View();
        }

        // POST: Admin/Subjects/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Major,Term,Status,CreateBy,UpdateBy,CreateDate,UpdateDate,IsDelete,IsActive")] Subject subject)
        {
            if (ModelState.IsValid)
            {
                var admin = JsonConvert.DeserializeObject<UserStaff>(HttpContext.Session.GetString("AdminLogin"));
                subject.CreateBy = admin.Username;
                subject.UpdateBy = admin.Username;
                subject.IsDelete = false;

                _context.Add(subject);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["Major"] = new SelectList(_context.Majors, "Id", "Name", subject.Major);
            ViewData["Term"] = new SelectList(_context.Terms, "Id", "Name", subject.Term);

            return View(subject);
        }

        // GET: Admin/Subjects/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var subject = await _context.Subjects.FindAsync(id);
            if (subject == null)
            {
                return NotFound();
            }
            ViewData["Major"] = new SelectList(_context.Majors, "Id", "Name", subject.Major);
            ViewData["Term"] = new SelectList(_context.Terms, "Id", "Name", subject.Term);

            return View(subject);
        }

        // POST: Admin/Subjects/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Major,Term,Status,CreateBy,UpdateBy,CreateDate,UpdateDate,IsDelete,IsActive")] Subject subject)
        {
            if (id != subject.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var admin = JsonConvert.DeserializeObject<UserStaff>(HttpContext.Session.GetString("AdminLogin"));
                    subject.UpdateDate = DateTime.Now;
                    subject.UpdateBy = admin.Username;
                    _context.Update(subject);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SubjectExists(subject.Id))
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
            ViewData["Major"] = new SelectList(_context.Majors, "Id", "Name", subject.Major);
            ViewData["Term"] = new SelectList(_context.Terms, "Id", "Name", subject.Term);

            return View(subject);
        }

        // GET: Admin/Subjects/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            var subject = await _context.Subjects.FindAsync(id);
            if (subject != null)
            {
                _context.Subjects.Remove(subject);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // POST: Admin/Subjects/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var subject = await _context.Subjects.FindAsync(id);
            if (subject != null)
            {
                _context.Subjects.Remove(subject);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SubjectExists(int id)
        {
            return _context.Subjects.Any(e => e.Id == id);
        }
    }
}
