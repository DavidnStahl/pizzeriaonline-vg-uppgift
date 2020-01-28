﻿using System;
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
using TomasosPizzeriaUppgift.Interface;
using TomasosPizzeriaUppgift.Models.Repository;

namespace TomasosPizzeriaUppgift.Services
{
    public class Services
    {
        private static Services instance = null;
        private static readonly Object padlock = new Object();
        private IRepository _repository;
        private ICache _cache;


        public static Services Instance
        {
            get
            {
                lock (padlock)
                {
                    if (instance == null)
                    {
                        instance = new Services();
                        instance._repository = new DBRepository();
                        instance._cache = new CacheHandler();

                    }
                    return instance;

                }
            }
        }

        public Services()
        {
        }
        public Kund GetUserId(Kund customer)
        {
            return _repository.GetUserId(customer);


        }

        public void SetInloggedCustomer(Kund customer)
        {

        }
        public Kund GetById(int id)
        {
            return _repository.GetById(id);
        }

        public MenuPage GetMenuInfo()
        {
            return _repository.GetMenuInfo(); ;
        }
        public void SaveUser(Kund user)
        {
            _repository.SaveUser(user);
        }

        public Matratt GetMatratterById(int id)
        {
            return _repository.GetMatratterToCustomerbasket(id);
        }
        public void UserPay(List<Matratt> matratter, int userid)
        {
            _repository.SaveOrder(matratter, userid);

        }
        public void UpdateUser(Kund user, int userid)
        {
            _repository.UpdateUser(user, userid);
        }
        public List<MatrattTyp> GetMatratttyper()
        {
            return _repository.GetMatrattTyper();
        }
        public Kund CheckUserName(Kund customer)
        {
            return _repository.CheckUserName(customer);
        }

        public int GetCustomerIDCache(HttpRequest Request)
        {
            return _cache.GetCustomerIDCache(Request);
        }

        public void SetCustomerCache(Kund kund, HttpRequest request, HttpResponse response)
        {
            _cache.SetCustomerCache(kund,request,response);
        }
        public List<Matratt> GetMatratterCacheList(int id, string options, HttpRequest request, HttpResponse response)
        {
            return _cache.GetMatratterCacheList(id,options,request,response);
        }
        public MenuPage SetMatratterCacheList(List<Matratt> matratteradded, HttpRequest request, HttpResponse response)
        {
            return _cache.SetMatratterCacheList(matratteradded, request, response);
        }
        public void ResetCookie(HttpRequest request, HttpResponse response)
        {
            _cache.ResetCookie(request,response);
        }
        public void PayUser(HttpRequest request, HttpResponse response)
        {
            var userid = GetCustomerIDCache(request);
            var matratteradded = _cache.PayUser(request, response);
            UserPay(matratteradded, userid);
            _cache.ResetCookie(request, response);
        }
        public MenuPage PayPage(HttpRequest request, HttpResponse response)
        {
            
            var matratteradded = _cache.GetMatratterToPay(request,response);
            var model = new MenuPage();
            model.Matratteradded = matratteradded;
            model.mattratttyper = GetMatratttyper();
            return model;
        }
        public MenuPage CustomerBasket(int id, HttpRequest request, HttpResponse response)
        {
            var matratteradded = GetMatratterCacheList(id, "1", request, response);
            var menumodel = SetMatratterCacheList(matratteradded, request, response);
            menumodel.mattratttyper = GetMatratttyper();
            return menumodel;
        }
        public MenuPage RemoveItemCustomerBasket(int id,int count ,HttpRequest request, HttpResponse response)
        {
            var matratteradded = GetMatratterCacheList(id, "2", request,response);
            matratteradded.RemoveAt(count);
            var menumodel = SetMatratterCacheList(matratteradded, request, response);
            return menumodel;
        }
        public bool CheckUserNameIsValid(Kund user, HttpRequest request, HttpResponse response)
        {
            var customer = CheckUserName(user);
            var customerid = GetCustomerIDCache(request);
            var cachecustomer = GetById(customerid);
            if (customer == null)
            {
                return true;
            }
            else if(user.AnvandarNamn == cachecustomer.AnvandarNamn)
            {
                return true;
            }
            else
            {
                return false;
            }

        }
        public MenuPage MenuPageData(int id, HttpRequest request, HttpResponse response)
        {
            var model = GetMenuInfo();
            var matratteradded = GetMatratterCacheList(id, "2", request, response);
            matratteradded.Add(model.matratt);
            model.Matratteradded = matratteradded;
            model.mattratttyper = GetMatratttyper();
            return model;
        }
        public string CheckIfInlogged(HttpRequest request)
        {
            var id = GetCustomerIDCache(request);
            if (id != 0)
            {
                return "Inloggad";
            }
            else
            {
                return  "inte inloggad";
            }
        }
    }
}
