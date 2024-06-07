using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DATN.Models;
using X.PagedList;
using Microsoft.CodeAnalysis.Elfie.Diagnostics;
using Newtonsoft.Json;

namespace DATN.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ClassesController : BaseController
    {
        private readonly QldiemSvContext _context;

        public ClassesController(QldiemSvContext context)
        {
            _context = context;
        }

        // GET: Admin/Classes
        public async Task<IActionResult> Index(string name, int page = 1)
        {
            int limit = 5;

            var major = await _context.Classes.OrderBy(c => c.Id).ToPagedListAsync(page, limit);
            if (!String.IsNullOrEmpty(name))
            {
                major = await _context.Classes.Where(c => c.Name.Contains(name)).OrderBy(c => c.Id).ToPagedListAsync(page, limit);
            }
            ViewBag.keyword = name;
            return View(major);
        }

        // GET: Admin/Classes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var @class = await _context.Classes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (@class == null)
            {
                return NotFound();
            }

            return View(@class);
        }

        // GET: Admin/Classes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/Classes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Code,Name,CreateBy,UpdateBy,CreateDate,UpdateDate,IsDelete,IsActive")] Class @class)
        {
            if (ModelState.IsValid)
            {
                var admin = JsonConvert.DeserializeObject<UserStaff>(HttpContext.Session.GetString("AdminLogin"));
                @class.CreateBy = admin.Username;
                @class.UpdateBy = admin.Username;
                @class.IsDelete = false;

                _context.Add(@class);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(@class);
        }

        // GET: Admin/Classes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var @class = await _context.Classes.FindAsync(id);
            if (@class == null)
            {
                return NotFound();
            }
            return View(@class);
        }

        // POST: Admin/Classes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Code,Name,CreateBy,UpdateBy,CreateDate,UpdateDate,IsDelete,IsActive")] Class @class)
        {
            if (id != @class.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var admin = JsonConvert.DeserializeObject<UserStaff>(HttpContext.Session.GetString("AdminLogin"));
                    @class.UpdateDate = DateTime.Now;
                    @class.UpdateBy = admin.Username;
                    _context.Update(@class);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ClassExists(@class.Id))
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
            return View(@class);
        }

        // GET: Admin/Classes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            var @class = await _context.Classes.FindAsync(id);
            if (@class != null)
            {
                _context.Classes.Remove(@class);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // POST: Admin/Classes/Delete/5
       /* [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var @class = await _context.Classes.FindAsync(id);
            if (@class != null)
            {
                _context.Classes.Remove(@class);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }*/

        private bool ClassExists(int id)
        {
            return _context.Classes.Any(e => e.Id == id);
        }
    }
}
