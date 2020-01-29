using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TomasosPizzeriaUppgift.Models;
using TomasosPizzeriaUppgift.ViewModels;

namespace TomasosPizzeriaUppgift.Interface
{
    interface IRepository
    {
        Kund GetById(int id);
        MenuPage GetMenuInfo();
        void SaveUser(Kund user);
        Kund GetUserId(Kund customer);
        Matratt GetMatratterToCustomerbasket(int id);
        void SaveOrder(List<Matratt> matratter, int userid);
        int GetTotalPayment(List<Matratt> matratter);
        Kund GetCustomerByUsername(string username);
        void SaveBestallningMatratter(List<Matratt> matratter);
        void UpdateUser(Kund user, int userid);
        List<MatrattTyp> GetMatrattTyper();
        Kund CheckUserName(Kund customer);
        Kund CheckCustomerUsernamePassword(LoginViewModel model);
    }
}
