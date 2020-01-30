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
        public async Task<IActionResult> Register(Kund model)
        {
            var validUsername = Services.Services.Instance.CheckUserNameIsValid(model, Request);
            if (ModelState.IsValid && validUsername == true)
            {
                var user = new IdentityUser { UserName = model.AnvandarNamn, NormalizedUserName = model.AnvandarNamn };
                var result = await userManager.CreateAsync(user, model.Losenord);

                if (result.Succeeded)
                {
                    var loginmodel = new LoginViewModel();
                    loginmodel.Username = model.AnvandarNamn;
                    loginmodel.Password = model.Namn;
                    Services.Services.Instance.SaveUser(model);
                    Services.Services.Instance.SetCustomerCache(loginmodel, Request, Response);
                    
                    await signInManager.SignInAsync(user, isPersistent: false);
                    return RedirectToAction("Index", "Home");
                }
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
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await signInManager.PasswordSignInAsync(model.Username, model.Password,
                                           model.RememberMe, false);

                if (result.Succeeded)
                {
                    Services.Services.Instance.SetCustomerCache(model, Request, Response);
                    return RedirectToAction("index","home");
                }  
            }
            ViewBag.Error = "Inloggning Misslyckades";
            return View(model);
        }
        public async Task<IActionResult> KeepCustomerLoggedIn(LoginViewModel model)
        {
            
                var result = await signInManager.PasswordSignInAsync(model.Username, model.Password,
                                           model.RememberMe, false);

                if (result.Succeeded)
                {
                    Services.Services.Instance.SetCustomerCache(model, Request, Response);
                    return RedirectToAction("index", "home");
                }
            
            ViewBag.Error = "Inloggning Misslyckades";
            return View("Login");
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
        public async Task<IActionResult> UpdateUser(Kund model)
        {

        var id = Services.Services.Instance.GetCustomerIDCache(Request);
        var valid = Services.Services.Instance.CheckUserNameIsValid(model, Request);
        var customer = Services.Services.Instance.GetInloggedCustomerInfo(Request);

        if (ModelState.IsValid && valid == true)
        {

            var identityuser = await userManager.GetUserAsync(User);
            if(customer.Losenord != model.Losenord || customer.AnvandarNamn != model.AnvandarNamn)
            {
                    identityuser.UserName = model.AnvandarNamn;
                    await userManager.UpdateNormalizedUserNameAsync(identityuser);
                    var result = await userManager.ChangePasswordAsync(identityuser, customer.Losenord, model.Losenord);
                    if(!result.Succeeded)
                    {
                        return View(nameof(Update), customer);
                    }

                    await signInManager.RefreshSignInAsync(identityuser);
                    Services.Services.Instance.UpdateUser(model, id, Request, Response);
                    return RedirectToAction("index", "home");


            }
            else
            {
                    Services.Services.Instance.UpdateUser(model, id, Request, Response);

                    return RedirectToAction("index", "home");

            }
        }
        else if (ModelState.IsValid && valid == false)
        {
            ViewBag.Message = "Användarnamn Upptaget";
            customer = Services.Services.Instance.GetInloggedCustomerInfo(Request);
            return View(nameof(Update), customer);
        }
        
            return View(nameof(Update));
        
        }
    }
}
