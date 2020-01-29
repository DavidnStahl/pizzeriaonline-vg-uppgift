using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TomasosPizzeriaUppgift.Controllers
{
    public class MenuController : Controller
    {
        // GET: /<controller>/
        [Authorize]
        [HttpGet] 
        public IActionResult Menu()
        {
             var model = Services.Services.Instance.MenuPageData(Request, Response);
             return View(model);          
        }
        [Authorize]
        public IActionResult AddItemCustomerBasket(int id)
        {
            var model = Services.Services.Instance.CustomerBasket(id, Request, Response);            
            return PartialView("Menu", model);
        }
        [Authorize]
        public ActionResult RemoveItemCustomerBasket(int id, int count)
        {
            ViewBag.Layout = Services.Services.Instance.CheckIfInlogged(Request);
            var model = Services.Services.Instance.RemoveItemCustomerBasket(id, count, Request, Response);
            return PartialView("Menu", model);
        }
    }
    
}
