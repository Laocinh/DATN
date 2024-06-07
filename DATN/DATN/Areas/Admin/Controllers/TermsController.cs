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
    public class TermsController : BaseController
    {
        private readonly QldiemSvContext _context;

        public TermsController(QldiemSvContext context)
        {
            _context = context;
        }

        // GET: Admin/Terms
        public async Task<IActionResult> Index(string name, int page = 1)
        {
            int limit = 5;

            var major = await _context.Terms.ToPagedListAsync(page, limit);
            if (!String.IsNullOrEmpty(name))
            {
                major = await _context.Terms.Where(c => c.Name.Contains(name)).OrderBy(c => c.Id).ToPagedListAsync(page, limit);
            }
            ViewBag.keyword = name;
            return View(major);
        }

        // GET: Admin/Terms/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var term = await _context.Terms
                .FirstOrDefaultAsync(m => m.Id == id);
            if (term == null)
            {
                return NotFound();
            }

            return View(term);
        }

        // GET: Admin/Terms/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/Terms/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,CollegeCredit,CreateBy,UpdateBy,CreateDate,UpdateDate,IsDelete,IsActive")] Term term)
        {
            if (ModelState.IsValid)
            {
                var admin = JsonConvert.DeserializeObject<UserStaff>(HttpContext.Session.GetString("AdminLogin"));
                term.CreateBy = admin.Username;
                term.UpdateBy = admin.Username;
                term.IsDelete = false;
                _context.Add(term);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(term);
        }

        // GET: Admin/Terms/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var term = await _context.Terms.FindAsync(id);
            if (term == null)
            {
                return NotFound();
            }
            return View(term);
        }

        // POST: Admin/Terms/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("Id,Name,CollegeCredit,CreateBy,UpdateBy,CreateDate,UpdateDate,IsDelete,IsActive")] Term term)
        {
            if (id != term.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var admin = JsonConvert.DeserializeObject<UserStaff>(HttpContext.Session.GetString("AdminLogin"));
                    term.UpdateDate = DateTime.Now;
                    term.UpdateBy = admin.Username;
                    _context.Update(term);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TermExists(term.Id))
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
            return View(term);
        }

        // GET: Admin/Terms/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            var term = await _context.Terms.FindAsync(id);
            if (term != null)
            {
                _context.Terms.Remove(term);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // POST: Admin/Terms/Delete/5
        /*[HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var term = await _context.Terms.FindAsync(id);
            if (term != null)
            {
                _context.Terms.Remove(term);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }*/

        private bool TermExists(long id)
        {
            return _context.Terms.Any(e => e.Id == id);
        }
    }
}
