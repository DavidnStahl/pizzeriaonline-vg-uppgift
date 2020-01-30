using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TomasosPizzeriaUppgift.Models;
using TomasosPizzeriaUppgift.Services;
using TomasosPizzeriaUppgift.ViewModels;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TomasosPizzeriaUppgift.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly SignInManager<IdentityUser> signInManager;

        public AccountController(UserManager<IdentityUser> userManager,
                                 SignInManager<IdentityUser> signInManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
        }
        // GET: /<controller>/
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(Kund model)
        {
            var validUsername = Services.Services.Instance.CheckUserNameIsValid(model, Request);
            if (ModelState.IsValid && validUsername == true)
            {
                var result = Services.Services.Instance.Identity("create",new LoginViewModel(),model,userManager,signInManager,Request,Response,User);
                if (result == true) { return RedirectToAction("Index", "Home"); }

                return View("Register", model);
              
            }
            return View(model);
        }
        
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginViewModel model)
        {
            
            if (ModelState.IsValid)
            {
                var result = Services.Services.Instance.Identity("signin", model, new Kund(), userManager, signInManager, Request, Response,User);
                if (result == true) { return RedirectToAction("Index", "Home"); }

                ViewBag.Error = "Inloggning Misslyckades";
                return View(model);
            }
            ViewBag.Error = "Inloggning Misslyckades";
            return View(model);
        }        

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("index", "home");
        }
        [Authorize]
        [HttpGet]
        public ActionResult Update()
        {
            var customer = Services.Services.Instance.GetInloggedCustomerInfo(Request);
            ViewBag.Message = "Din personliga information";
            return View(customer);
        }
    
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateUser(Kund model)
        {
        var valid = Services.Services.Instance.CheckUserNameIsValid(model, Request);

        if (ModelState.IsValid && valid == true)
        {
                var customer = Services.Services.Instance.GetInloggedCustomerInfo(Request);
                var id = Services.Services.Instance.GetCustomerIDCache(Request);
                
                var result = Services.Services.Instance.Identity("update", new LoginViewModel(), model, userManager, signInManager, Request, Response,User);
                if (result == true) { return RedirectToAction("Index", "Home"); }

                return View(nameof(Update), customer);
            }
        else if (ModelState.IsValid && valid == false)
        {
            ViewBag.Message = "Användarnamn Upptaget";
            var customer = Services.Services.Instance.GetInloggedCustomerInfo(Request);
            return View(nameof(Update), customer);
        }
        
            return View(nameof(Update));
        
        }
    }
}
