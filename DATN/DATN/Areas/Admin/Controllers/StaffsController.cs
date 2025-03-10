﻿using System;
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
    public class StaffsController : BaseController
    {
        private readonly QldiemSvContext _context;

        public StaffsController(QldiemSvContext context)
        {
            _context = context;
        }

        // GET: Admin/Staffs
        public async Task<IActionResult> Index(string name, int page = 1)
        {
            int limit = 5;

            var major = await _context.Staff.Include(s => s.MajorNavigation).Include(x => x.PositionNavigation).OrderBy(c => c.Id).ToPagedListAsync(page, limit);
            if (!String.IsNullOrEmpty(name))
            {
                major = await _context.Staff.Where(c => c.Name.Contains(name)).OrderBy(c => c.Id).ToPagedListAsync(page, limit);
            }
            ViewBag.keyword = name;
            return View(major);
        }

        // GET: Admin/Staffs/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var staff = await _context.Staff
                .Include(s => s.MajorNavigation)
                .Include(s => s.PositionNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (staff == null)
            {
                return NotFound();
            }
            ViewData["Major"] = new SelectList(_context.Majors, "Id", "Name");
            ViewData["Position"] = new SelectList(_context.Positions, "Id", "Name");
            return View(staff);
        }

        // GET: Admin/Staffs/Create
        public IActionResult Create()
        {
           
            ViewData["Major"] = new SelectList(_context.Majors, "Id", "Name");
            ViewData["Position"] = new SelectList(_context.Positions, "Id", "Name");
            return View();
        }

        // POST: Admin/Staffs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,BirthDate,NumberPhone,Gender,Email,Degree,Major,Yearofwork,Position,CreateBy,UpdateBy,CreateDate,UpdateDate,IsDelete,IsActive")] Staff staff)
        {
            if (ModelState.IsValid)
            {
                var admin = JsonConvert.DeserializeObject<UserStaff>(HttpContext.Session.GetString("AdminLogin"));
                staff.CreateBy = admin.Username;
                staff.UpdateBy = admin.Username; 

                _context.Add(staff);
                await _context.SaveChangesAsync();
               /* Khi tạo 1 thông tin giáo viên sẽ tự tạo ra 1 tài khoản bằng email và sdt*/
                var dataStaff = _context.Staff.Where(c => c.Id == staff.Id).FirstOrDefault();
                UserStaff us = new UserStaff();
                us.Staff = dataStaff.Id;
                us.Username = dataStaff.Email;
                us.Password = dataStaff.NumberPhone;
                us.CreateBy = admin.Username;
                us.UpdateBy = admin.Username;
                us.IsDelete = false;
                _context.Add(us);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));

            }
            List<Gender> items = new List<Gender>
            {
               new Gender { Id=true, Name="Nam" },
               new Gender { Id=false, Name="Nữ" }
            };
            ViewBag.Gender = new SelectList(items, "Id", "Name", staff.Gender);
            ViewData["Major"] = new SelectList(_context.Majors, "Id", "Name", staff.Major);
            ViewData["Position"] = new SelectList(_context.Positions, "Id", "Name", staff.Position);
            return View(staff);
        }

        // GET: Admin/Staffs/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var staff = await _context.Staff.FindAsync(id);
            if (staff == null)
            {
                return NotFound();
            }
            List<Gender> items = new List<Gender>
            {
               new Gender { Id=true, Name="Nam" },
               new Gender { Id=false, Name="Nữ" }
            };
            ViewBag.Gender =  new SelectList(items, "Id", "Name", staff.Gender);
            ViewData["Major"] = new SelectList(_context.Majors, "Id", "Name", staff.Major);
            ViewData["Position"] = new SelectList(_context.Positions, "Id", "Name", staff.Position);
            return View(staff);
        }

        // POST: Admin/Staffs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("Id,Name,BirthDate,NumberPhone,Gender,Email,Degree,Major,Yearofwork,Position,CreateBy,UpdateBy,CreateDate,UpdateDate,IsDelete,IsActive")] Staff staff)
        {
            if (id != staff.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var admin = JsonConvert.DeserializeObject<UserStaff>(HttpContext.Session.GetString("AdminLogin"));
                    staff.UpdateDate = DateTime.Now;
                    staff.UpdateBy = admin.Username;

                    _context.Update(staff);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StaffExists(staff.Id))
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
            ViewData["Major"] = new SelectList(_context.Majors, "Id", "Name", staff.Major);
            ViewData["Position"] = new SelectList(_context.Positions, "Id", "Name", staff.Position);
            return View(staff);
        }

        // GET: Admin/Staffs/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            var staff = await _context.Staff.FindAsync(id);
            if (staff != null)
            {
                _context.Staff.Remove(staff);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // POST: Admin/Staffs/Delete/5
       /* [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var staff = await _context.Staff.FindAsync(id);
            if (staff != null)
            {
                _context.Staff.Remove(staff);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }*/

        private bool StaffExists(long id)
        {
            return _context.Staff.Any(e => e.Id == id);
        }
    }
}
