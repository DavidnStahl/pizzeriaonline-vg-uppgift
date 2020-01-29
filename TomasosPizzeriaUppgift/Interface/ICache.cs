using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TomasosPizzeriaUppgift.Models;
using TomasosPizzeriaUppgift.ViewModels;

namespace TomasosPizzeriaUppgift.Interface
{
    interface ICache
    {
        int GetCustomerIDCache(HttpRequest Request);
        void SetCustomerCache(Kund kund, HttpRequest request, HttpResponse response);
        List<Matratt> GetMatratterCacheList(int id, string options, HttpRequest request, HttpResponse response);
        MenuPage SetMatratterCacheList(List<Matratt> matratteradded, HttpRequest request, HttpResponse response);
        void ResetCookie(HttpRequest request, HttpResponse response);
        List<Matratt> PayUser(HttpRequest request, HttpResponse response);
        List<Matratt> GetMatratterToPay(HttpRequest request, HttpResponse response);
        void DeleteFoodListCache(HttpRequest request, HttpResponse response);
    }
}
