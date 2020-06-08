using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BeautyService.Models;
using Microsoft.AspNetCore.Mvc;

namespace BeautyService.Controllers
{
    public class ServiceTypesController : Controller
    {
        BeautyServiceContext db;

        public ServiceTypesController(BeautyServiceContext context)
        {
            db = context;
        }

        public IActionResult Index(int? serviceCategoryId)
        {
            var serviceTypes = db.ServiceTypes.Where(t => t.ServiceCategoryId == serviceCategoryId).ToList();
            return View(serviceTypes);
        }
    }
}
