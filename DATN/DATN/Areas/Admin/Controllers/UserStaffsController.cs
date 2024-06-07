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
    public class UserStaffsController : BaseController
    {
        private readonly QldiemSvContext _context;

        public UserStaffsController(QldiemSvContext context)
        {
            _context = context;
        }

        // GET: Admin/UserStaffs
        public async Task<IActionResult> Index(string name, int page = 1)
        {
            int limit = 5;

            var major = await _context.UserStaffs.Include(s => s.StaffNavigation).OrderBy(c => c.Id).ToPagedListAsync(page, limit);

            ViewBag.keyword = name;
            return View(major);
        }

        // GET: Admin/UserStaffs/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userStaff = await _context.UserStaffs
                .Include(u => u.StaffNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (userStaff == null)
            {
                return NotFound();
            }
            ViewData["Staff"] = new SelectList(_context.Staff, "Id", "Name");
            return View(userStaff);
        }

        // GET: Admin/UserStaffs/Create
        public IActionResult Create()
        {
            ViewData["Staff"] = new SelectList(_context.Staff, "Id", "Name");

            return View();
        }

        // POST: Admin/UserStaffs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Staff,Username,Password,CreateBy,UpdateBy,CreateDate,UpdateDate,IsDelete,IsActive")] UserStaff userStaff)
        {
            if (ModelState.IsValid)
            {
                var admin = JsonConvert.DeserializeObject<UserStaff>(HttpContext.Session.GetString("AdminLogin"));
                userStaff.CreateBy = admin.Username;
                userStaff.UpdateBy = admin.Username;
                userStaff.IsDelete = false;
                _context.Add(userStaff);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["Staff"] = new SelectList(_context.Staff, "Id", "Name", userStaff.Staff);
            return View(userStaff);
        }

        // GET: Admin/UserStaffs/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userStaff = await _context.UserStaffs.FindAsync(id);
            if (userStaff == null)
            {
                return NotFound();
            }
            ViewData["Staff"] = new SelectList(_context.Staff, "Id", "Name", userStaff.Staff);
            return View(userStaff);
        }

        // POST: Admin/UserStaffs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("Id,Staff,Username,Password,CreateBy,UpdateBy,CreateDate,UpdateDate,IsDelete,IsActive")] UserStaff userStaff)
        {
            if (id != userStaff.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var admin = JsonConvert.DeserializeObject<UserStaff>(HttpContext.Session.GetString("AdminLogin"));
                    userStaff.UpdateDate = DateTime.Now;
                    userStaff.UpdateBy = admin.Username;

                    _context.Update(userStaff);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserStaffExists(userStaff.Id))
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
            ViewData["Staff"] = new SelectList(_context.Staff, "Id", "Name", userStaff.Staff);
            return View(userStaff);
        }

        // GET: Admin/UserStaffs/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            var userStaff = await _context.UserStaffs.FindAsync(id);
            if (userStaff != null)
            {
                _context.UserStaffs.Remove(userStaff);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // POST: Admin/UserStaffs/Delete/5
       /* [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var userStaff = await _context.UserStaffs.FindAsync(id);
            if (userStaff != null)
            {
                _context.UserStaffs.Remove(userStaff);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
*/
        private bool UserStaffExists(long id)
        {
            return _context.UserStaffs.Any(e => e.Id == id);
        }
    }
}
