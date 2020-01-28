using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TomasosPizzeriaUppgift.Models;
using TomasosPizzeriaUppgift.ViewModels;
using TomasosPizzeriaUppgift.Services;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;



namespace TomasosPizzeriaUppgift.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult HomePage()
        {
            ViewBag.Layout = Services.Services.Instance.CheckIfInlogged(Request);
            return View();
        }

        public IActionResult MenuPage()
        {
            ViewBag.Layout = Services.Services.Instance.CheckIfInlogged(Request);
            var model = Services.Services.Instance.GetMenuInfo();
            var id = Services.Services.Instance.GetCustomerIDCache(Request);
            if (id != 0)
            {
                
                model = Services.Services.Instance.MenuPageData(id,Request,Response);
                return View(model);
            }
            else
            {
                return RedirectToAction("LoginPage");
            }
            
            
        }
        public IActionResult RegisterPage()
        {
            ViewBag.Layout = Services.Services.Instance.CheckIfInlogged(Request);
            ViewBag.Message = "Din personliga information";
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult RegisterUser(Kund user)
        {
            var kund = Services.Services.Instance.CheckUserName(user);
            if (ModelState.IsValid && kund == null)
            {
                Services.Services.Instance.SaveUser(user);
                ModelState.Clear();
                return RedirectToAction("LoginPage");
            }
            else
            {
                ViewBag.Message = "Användarnamn Upptaget";
                return View(nameof(RegisterPage));
            }
        }
        
       
        public ActionResult CustomerInfoPage()
        {
            ViewBag.Layout = Services.Services.Instance.CheckIfInlogged(Request);
            var id = Services.Services.Instance.GetCustomerIDCache(Request);
            if (id == 0)
            {
                return RedirectToAction("LoginPage");
            }
            var customer = Services.Services.Instance.GetById(id);
            ViewBag.Message = "Din personliga information";
            return View(customer);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateUser(Kund user)
        {
            ViewBag.Layout = Services.Services.Instance.CheckIfInlogged(Request);
            var id = Services.Services.Instance.GetCustomerIDCache(Request);
            var valid = Services.Services.Instance.CheckUserNameIsValid(user,Request,Response);
            if (ModelState.IsValid && valid == true)
            {               
                ModelState.Clear();
                Services.Services.Instance.UpdateUser(user, id);
                return RedirectToAction("CustomerInfoPage");
            }
            else if(ModelState.IsValid && valid == false)
            {
                ViewBag.Message = "Användarnamn Upptaget";
                var customer = Services.Services.Instance.GetById(id);
                return View(nameof(CustomerInfoPage),customer);
            }
            else
            {
                return View(nameof(CustomerInfoPage));
            }
        }
        public IActionResult LoginPage()
        {
            ViewBag.Layout = Services.Services.Instance.CheckIfInlogged(Request);
            ViewBag.Message = "Var vänlig logga in";
            return View();
        }
        public IActionResult LogOut()
        {
            Services.Services.Instance.ResetCookie(Request, Response);
            return RedirectToAction("LoginPage");
        }
        [HttpPost]
        public IActionResult UserLogginValidation(Kund customer)
        {

            var kund = Services.Services.Instance.GetUserId(customer);
            if (kund != null)
            {
                Services.Services.Instance.SetCustomerCache(kund, Request, Response);
                return RedirectToAction("HomePage");
            }
            else
            {
                ViewBag.Message = "Fel inlogg försök igen";
                return View(nameof(LoginPage));
            };
        }

        public ActionResult RemoveItemCustomerBasket(int id, int count)
        {
            ViewBag.Layout = Services.Services.Instance.CheckIfInlogged(Request);
            var model = Services.Services.Instance.RemoveItemCustomerBasket(id,count, Request, Response);
            return PartialView("MenuPage", model); 
        }

        public ActionResult CustomerBasket(int id)
        {
            ViewBag.Layout = Services.Services.Instance.CheckIfInlogged(Request);
            var model = Services.Services.Instance.CustomerBasket(id, Request, Response);
            return PartialView("MenuPage", model);
        }
        public ActionResult PaymentLoggin()
        {
            ViewBag.Message = "Logga in, för att betala";
            ViewBag.Layout = Services.Services.Instance.CheckIfInlogged(Request);
            return View();
        }
        [HttpPost]
        public ActionResult PaymentLogginValidation(Kund customer)
        {
            var cust = Services.Services.Instance.GetUserId(customer);
            var cacheid = Services.Services.Instance.GetCustomerIDCache(Request);
            ViewBag.Layout = Services.Services.Instance.CheckIfInlogged(Request);
            if (cacheid == cust.KundId)
            {
                return RedirectToAction("PayPage");
            }
            else
            {
                ViewBag.Message = "Fel inlogg försök igen";
                return View(nameof(PaymentLoggin));
            };
        }
        public ActionResult PayPage()
        {
            var model = Services.Services.Instance.PayPage(Request, Response);
            ViewBag.Layout = Services.Services.Instance.CheckIfInlogged(Request);
            return View(model);
        }
        public ActionResult PayUser()
        {
            ViewBag.Layout = Services.Services.Instance.CheckIfInlogged(Request);
            Services.Services.Instance.PayUser(Request, Response);
            return View();
        }
    }
}
