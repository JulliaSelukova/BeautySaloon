using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BeautyService.Models;

namespace BeautyService.Controllers
{
    public class RegistrationsController : Controller
    {
        private readonly BeautyServiceContext _context;

        public RegistrationsController(BeautyServiceContext context)
        {
            _context = context;
        }

        // GET: Registrations
        public async Task<IActionResult> IndexForAdministrator(int? serviceCategoryId)
        {
            if (serviceCategoryId == null)
                NotFound();
            var defaultDate = DateTime.Now.AddDays(1).Date;
            ViewBag.ServiceCategory = _context.ServiceCategories.Where(t => t.Id == serviceCategoryId).FirstOrDefault();
            var registrations = _context.Registrations.Where(x => x.ServiceCategoryId == serviceCategoryId
                                        && x.Date.Date == defaultDate).ToList();
            return View(registrations);
        }

        // GET: Registrations
        public async Task<IActionResult> Index(int? serviceTypeId, int? serviceCategoryId)
        {
            if (serviceTypeId == null)
                NotFound();
            var defaultDate = DateTime.Now.AddDays(1).Date;
            ViewBag.ServiceType = _context.ServiceTypes.Where(t => t.Id == serviceTypeId).FirstOrDefault();
            var registrations = _context.Registrations.Where(x => x.ServiceCategoryId == serviceCategoryId
                                        && x.Date.Date == defaultDate && !x.IsEngaged).ToList();
            return View(registrations);
        }

        [HttpGet]
        public async Task<IActionResult> DateSearchForAdmin(DateTime dateSearch, int? serviceCategoryId)
        {
            var registrations = _context.Registrations.Where(x => x.ServiceCategoryId == serviceCategoryId
                                        && x.Date.Date == dateSearch.Date).ToList();
            ViewBag.ServiceCategory = _context.ServiceCategories.Where(t => t.Id == serviceCategoryId).FirstOrDefault();
            return View("IndexForAdministrator", registrations);
        }

        [HttpGet]
        public async Task<IActionResult> DateSearch(DateTime dateSearch, int? serviceTypeId, int? serviceCategoryId)
        {    
            if(dateSearch.Date < DateTime.Now.Date)
            {
                var registrations = new List<Registration>();
                ViewBag.ServiceType = _context.ServiceTypes.Where(t => t.Id == serviceTypeId).FirstOrDefault();
                return View("Index", registrations);
            }
            var registrations = _context.Registrations.Where(x => x.ServiceCategoryId == serviceCategoryId
                                        && x.Date.Date == dateSearch.Date && !x.IsEngaged).ToList();
            ViewBag.ServiceType = _context.ServiceTypes.Where(t => t.Id == serviceTypeId).FirstOrDefault();            
            return View("Index", registrations);
        }        

        // GET: Registrations/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var registration = await _context.Registrations
                .Include(r => r.ServiceType)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (registration == null)
            {
                return NotFound();
            }

            return View(registration);
        }

        // GET: Registrations/Create
        public IActionResult Create(int? serviceCategoryId)
        {
            if (serviceCategoryId == null)
                return NotFound();
            ViewData["ServiceCategoryId"] = new SelectList(_context.ServiceCategories, "Id", "Id");
            var registration = new Registration(Convert.ToInt32(serviceCategoryId));
            return View(registration);
        }

        // POST: Registrations/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Date,Time,ClientName,ClientPhone,ServiceTypeId,ServiceCategoryId,IsEngaged")] Registration registration)
        {
            if (ModelState.IsValid)
            {
                _context.Add(registration);
                await _context.SaveChangesAsync();
                ViewBag.ServiceCategory = _context.ServiceCategories.Where(t => t.Id == registration.ServiceCategoryId).FirstOrDefault();
                return RedirectToAction(nameof(IndexForAdministrator), new { serviceCategoryId = registration.ServiceCategoryId });
            }
            ViewData["ServiceCategoryId"] = new SelectList(_context.ServiceCategories, "Id", "Id", registration.ServiceCategoryId);
            ViewData["ServiceTypeId"] = new SelectList(_context.ServiceTypes, "Id", "Id", registration.ServiceTypeId);
            return View(registration);
        }

        // GET: Registrations/Edit/5
        public async Task<IActionResult> Edit(int? id, int? serviceTypeId)
        {
            if (id == null)
            {
                return NotFound();
            }

            var registration = await _context.Registrations.FindAsync(id);
            if (registration == null)
            {
                return NotFound();
            }
            ViewData["ServiceTypeId"] = new SelectList(_context.ServiceTypes, "Id", "Id", registration.ServiceTypeId);
            ViewData["ServiceCategoryId"] = new SelectList(_context.ServiceCategories, "Id", "Id", registration.ServiceCategoryId);
            registration.ServiceTypeId = Convert.ToInt32(serviceTypeId);
            registration.IsEngaged = true;
            return View(registration);
        }

        // POST: Registrations/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Date,Time,ClientName,ClientPhone,ServiceTypeId,ServiceCategoryId,IsEngaged")] Registration registration)
        {
            if (id != registration.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(registration);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RegistrationExists(registration.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Details), new { id = registration.Id });
            }
            ViewData["ServiceTypeId"] = new SelectList(_context.ServiceTypes, "Id", "Id", registration.ServiceTypeId);
            return View(registration);
        }

        // GET: Registrations/Edit/5
        public async Task<IActionResult> EditForAdministrator(int? id, int? serviceTypeId)
        {
            if (id == null)
            {
                return NotFound();
            }

            var registration = await _context.Registrations.FindAsync(id);
            if (registration == null)
            {
                return NotFound();
            }
            ViewData["ServiceTypeId"] = new SelectList(_context.ServiceTypes, "Id", "Id", registration.ServiceTypeId);
            ViewData["ServiceCategoryId"] = new SelectList(_context.ServiceCategories, "Id", "Id", registration.ServiceCategoryId);
            //ViewBag.ServiceType = _context.ServiceTypes.Where(t => t.Id == serviceTypeId).FirstOrDefault();
            registration.ServiceTypeId = Convert.ToInt32(serviceTypeId);
            return View(registration);
        }

        // POST: Registrations/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditForAdministrator(int id, [Bind("Id,Date,Time,ClientName,ClientPhone,ServiceTypeId,ServiceCategoryId,IsEngaged")] Registration registration)
        {
            if (id != registration.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(registration);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RegistrationExists(registration.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                ViewBag.ServiceType = _context.ServiceTypes.Where(t => t.Id == registration.ServiceTypeId).FirstOrDefault();
                return RedirectToAction(nameof(IndexForAdministrator), new { serviceTypeId = registration.ServiceTypeId });
            }
            ViewData["ServiceTypeId"] = new SelectList(_context.ServiceTypes, "Id", "Id", registration.ServiceTypeId);
            return View(registration);
        }

        // GET: Registrations/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var registration = await _context.Registrations
                .Include(r => r.ServiceType)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (registration == null)
            {
                return NotFound();
            }

            ViewBag.ServiceCategory = registration.ServiceCategory;
            return View(registration);
        }

        // POST: Registrations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var registration = await _context.Registrations.FindAsync(id);
            _context.Registrations.Remove(registration);
            await _context.SaveChangesAsync();
            //ViewBag.ServiceType = _context.ServiceTypes.Where(t => t.Id == registration.ServiceTypeId).FirstOrDefault();
            ViewBag.ServiceCategory = registration.ServiceCategory;
            return RedirectToAction(nameof(IndexForAdministrator), new { serviceCategoryId = registration.ServiceCategoryId });
        }

        private bool RegistrationExists(int id)
        {
            return _context.Registrations.Any(e => e.Id == id);
        }
    }
}
