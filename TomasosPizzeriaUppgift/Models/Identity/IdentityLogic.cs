using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TomasosPizzeriaUppgift.Interface;
using TomasosPizzeriaUppgift.ViewModels;

namespace TomasosPizzeriaUppgift.Models.IdentityLogic
{
    public class IdentityLogic : IIdentity
    {
        public async Task<IdentityResult> CreateUserIdentity(Kund model, UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, HttpRequest request, HttpResponse response)
        {
            var user = new IdentityUser { UserName = model.AnvandarNamn, NormalizedUserName = model.AnvandarNamn };
            var result = await userManager.CreateAsync(user, model.Losenord);

            if (result.Succeeded)
            {
                var loginmodel = new LoginViewModel();
                loginmodel.Username = model.AnvandarNamn;
                loginmodel.Password = model.Namn;
                Services.Services.Instance.SaveUser(model);
                Services.Services.Instance.SetCustomerCache(loginmodel, request, response);

                await signInManager.SignInAsync(user, isPersistent: false);
                return result;
            }

            return result;
        }


        public async Task<SignInResult> SignInIdentity(LoginViewModel model, UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, HttpRequest request, HttpResponse response)
        {
            
            var result = await signInManager.PasswordSignInAsync(model.Username, model.Password,
                                           model.RememberMe, false);

            if (result.Succeeded)
            {
                Services.Services.Instance.SetCustomerCache(model, request, response);
                return result;
            }

            return result;
        }

        

        public async Task<IdentityResult> UpdateUserIdentity(Kund model, UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, HttpRequest request, HttpResponse response, System.Security.Claims.ClaimsPrincipal user)
        {
            var identityuser = await userManager.GetUserAsync(user);
            identityuser.UserName = model.AnvandarNamn;
            await userManager.UpdateNormalizedUserNameAsync(identityuser);
            var customer = Services.Services.Instance.GetInloggedCustomerInfo(request);
            var result = await userManager.ChangePasswordAsync(identityuser, customer.Losenord, model.Losenord);
            if (result.Succeeded)
            {
                await signInManager.RefreshSignInAsync(identityuser);
                var id = Services.Services.Instance.GetCustomerIDCache(request);
                Services.Services.Instance.UpdateUser(model, id, request, response);
                return result;
            }
            return result;
        }
    }
}
