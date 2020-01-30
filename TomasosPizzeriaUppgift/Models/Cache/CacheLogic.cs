using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TomasosPizzeriaUppgift.Interface;
using TomasosPizzeriaUppgift.ViewModels;

namespace TomasosPizzeriaUppgift.Models
{
    public class CacheLogic : ICache
    {
        public int GetCustomerIDCache(HttpRequest request)
        {
            var id = Convert.ToInt32(request.Cookies["cookie_customer"]);
            return id;
        }

        public List<Matratt> GetMatratterCacheList(int id, string options, HttpRequest request, HttpResponse response)
        {
            var model = Services.Services.Instance.GetMatratterById(id);
            var jsonget = request.Cookies["cookie_matratter"];
            var matratteradded = new List<Matratt>();
            if (jsonget != null)
            {
                matratteradded = JsonConvert.DeserializeObject<List<Matratt>>(jsonget);
            }
            if (options == "1")
            {
                matratteradded.Add(model);
            }
            return matratteradded;
        }
        public void DeleteFoodListCache(HttpRequest request, HttpResponse response)
        {
            
            response.Cookies.Delete("cookie_matratter");         
        }
        

        public void SetCustomerCache(Kund kund, HttpRequest request, HttpResponse response)
        {
            if(request.Cookies != null)
            {
                foreach (var cookieKey in request.Cookies.Keys)
                {
                    response.Cookies.Delete(cookieKey);
                }
            }
            
            CookieOptions options = new CookieOptions();
            options.Expires = DateTime.Now.AddMinutes(20);
            options.HttpOnly = true;
            response.Cookies.Append("cookie_customer", kund.KundId.ToString(), options);
        }
        public MenuPage SetMatratterCacheList(List<Matratt> matratteradded, HttpRequest request, HttpResponse response)
        {

            string json = JsonConvert.SerializeObject(matratteradded, Formatting.Indented);
            CookieOptions options = new CookieOptions();
            options.Expires = DateTime.Now.AddMinutes(20);
            options.HttpOnly = true;
            response.Cookies.Append("cookie_matratter", json, options);

            var menumodel = Services.Services.Instance.GetMenuInfo();
            menumodel.Matratteradded = matratteradded;

            return menumodel;
        }
        public void ResetCookie(HttpRequest request, HttpResponse response)
        {
            foreach (var cookieKey in request.Cookies.Keys)
            {
                response.Cookies.Delete(cookieKey);
            }
        }
        public List<Matratt> PayUser(HttpRequest request, HttpResponse response)
        {
            
            var jsonget = request.Cookies["cookie_matratter"];
            var matratteradded = new List<Matratt>();
            if (jsonget != null)
            {
                matratteradded = JsonConvert.DeserializeObject<List<Matratt>>(jsonget);
            }
            return matratteradded;
        }
        public List<Matratt> GetMatratterToPay(HttpRequest request, HttpResponse response)
        {
            var jsonget = request.Cookies["cookie_matratter"];
            var matratteradded = new List<Matratt>();
            if (jsonget != null)
            {
                matratteradded = JsonConvert.DeserializeObject<List<Matratt>>(jsonget);
            }
            return matratteradded;
        }
            
    }
}
