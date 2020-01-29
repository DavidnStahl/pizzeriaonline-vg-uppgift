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

                if(result.Succeeded)
                {
                    Services.Services.Instance.SaveUser(model);
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
        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("index", "home");
        }
        [Authorize]
        public ActionResult Update()
        {
            var customer = Services.Services.Instance.GetInloggedCustomerInfo(Request);
            ViewBag.Message = "Din personliga information";
            return View(customer);
        }
        /*[HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> UpdateUser(Kund user)
        {

            var id = Services.Services.Instance.GetCustomerIDCache(Request);
            var valid = Services.Services.Instance.CheckUserNameIsValid(user, Request);
            
            if (ModelState.IsValid && valid == true)
            {
                var customerinlogged = Services.Services.Instance.GetInloggedCustomerInfo(Request);
                var inloggeduser = await userManager.FindByNameAsync(User.Identity.Name);

                var resultPasswordChange = await userManager.ChangePasswordAsync(inloggeduser, customerinlogged.Losenord, user.Losenord);
                
                await userManager.UpdateNormalizedUserNameAsync(inloggeduser);
                await signInManager.RefreshSignInAsync(inloggeduser);
                ModelState.Clear();
                Services.Services.Instance.UpdateUser(user, id, Request, Response);
                var customer = Services.Services.Instance.GetById(id);
                return View(nameof(Update), customer);
               

                
            }
            else if (ModelState.IsValid && valid == false)
            {
                ViewBag.Message = "Användarnamn Upptaget";
                var customer = Services.Services.Instance.GetById(id);
                return View(nameof(Update), customer);
            }
            else
            {
                return View(nameof(Update));
            }
        }*/
    [HttpPost]
    [Authorize]
    [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateUser(Kund user)
        {

        var id = Services.Services.Instance.GetCustomerIDCache(Request);
        var valid = Services.Services.Instance.CheckUserNameIsValid(user, Request);

        if (ModelState.IsValid && valid == true)
        {

            IdentityUser identityuser = await userManager.GetUserAsync(User);
            var result = await userManager.DeleteAsync(identityuser);
            if(result.Succeeded)
             {
                    var iuser = new IdentityUser { UserName = user.AnvandarNamn, NormalizedUserName = user.AnvandarNamn };
                    var r = await userManager.CreateAsync(iuser, user.Losenord);

                    if (r.Succeeded)
                    {
                        Services.Services.Instance.SaveUser(user);
                        await signInManager.SignInAsync(iuser, isPersistent: false);

                        var res = await signInManager.PasswordSignInAsync(user.AnvandarNamn, user.Losenord, false, false);

                        if (res.Succeeded)
                        {
                            var model = new LoginViewModel();
                            model.Username = user.AnvandarNamn;
                            model.Password = user.Losenord;

                            ModelState.Clear();
                            return RedirectToAction("ChangeInformationConfirmation");
                        }
                        var c = Services.Services.Instance.GetById(id);
                        return View(nameof(Update), c);
                    }
            }
            

            var customer = Services.Services.Instance.GetById(id);
            return View(nameof(Update), customer);

        }
        else if (ModelState.IsValid && valid == false)
        {
            ViewBag.Message = "Användarnamn Upptaget";
            var customer = Services.Services.Instance.GetById(id);
            return View(nameof(Update), customer);
        }
        else
        {
            return View(nameof(Update));
        }
        }
        public async Task<IActionResult> changeInformationConfirmation()
        {
            var customer = Services.Services.Instance.GetInloggedCustomerInfo(Request);
            if (ModelState.IsValid)
            {
                var result = await signInManager.PasswordSignInAsync(customer.AnvandarNamn, customer.Namn,
                                           false, false);

                if (result.Succeeded)
                {
                    var model = new LoginViewModel();
                    model.Username = customer.AnvandarNamn;
                    model.Password = customer.Namn;
                    Services.Services.Instance.SetCustomerCache(model, Request, Response);
                    return RedirectToAction("index", "home");
                }
            }
            ViewBag.Error = "Inloggning Misslyckades";
            return View("Login");

        }





    }
}
