using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TomasosPizzeriaUppgift.Models;
using TomasosPizzeriaUppgift.Services;

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
            if (ModelState.IsValid)
            {
                var user = new IdentityUser { UserName = model.AnvandarNamn };
                var result = await userManager.CreateAsync(user, model.Losenord);

                if(result.Succeeded)
                {
                    Services.Services.Instance.SaveUser(model);
                    await signInManager.SignInAsync(user, isPersistent: false);
                    return RedirectToAction("HomePage", "Home");
                }
            }
            return View(model);
        }
    }
}
