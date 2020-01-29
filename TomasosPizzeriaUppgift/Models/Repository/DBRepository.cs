using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TomasosPizzeriaUppgift.Interface;
using TomasosPizzeriaUppgift.ViewModels;


namespace TomasosPizzeriaUppgift.Models.Repository
{
    public class DBRepository : IRepository
    {
        public Kund GetById(int id)
        {
            var model = new Kund();
            using (TomasosContext db = new TomasosContext())
            {
                model = db.Kund.FirstOrDefault(ratt => ratt.KundId == id);
            }
            return model;
                
        }
        public Kund CheckCustomerUsernamePassword(LoginViewModel model)
        {
            var customer = new Kund();
            using (TomasosContext db = new TomasosContext())
            {
                customer = db.Kund.FirstOrDefault(c => c.AnvandarNamn == model.Username && c.Losenord == model.Password);
            }
            return customer;
        }
        public MenuPage GetMenuInfo()
        {
            var model = new MenuPage();

            using (TomasosContext db = new TomasosContext())
            {
                model.Matratter = db.Matratt.ToList();
                model.Ingredins.MatrattProdukt = db.MatrattProdukt.ToList();
                model.Ingredienses = db.Produkt.ToList();
                model.mattratttyper = db.MatrattTyp.ToList();

            }
            return model;
        }
        public void SaveUser(Kund user)
        {
            using (TomasosContext db = new TomasosContext())
            {
                db.Kund.Add(user);
                db.SaveChanges();
            }
        }
        public Kund GetUserId(Kund customer)
        {
            var user = new Kund();
            using (TomasosContext db = new TomasosContext())
            {
                user = db.Kund.FirstOrDefault(r => r.AnvandarNamn == customer.AnvandarNamn && r.Losenord == customer.Losenord);
            }
            
            return user;
        }
        public Matratt GetMatratterToCustomerbasket(int id)
        {
            var model = new Matratt();
            using (TomasosContext db = new TomasosContext())
            {
                
                model = db.Matratt.FirstOrDefault(r => r.MatrattId == id);
            }
            return model;
        }

        public void SaveOrder(List<Matratt> matratter, int userid)
        {
            var customer = GetById(userid);
            var totalmoney = GetTotalPayment(matratter);
            var bestallning = new Bestallning()
            {
                BestallningDatum = DateTime.Now,
                KundId = customer.KundId,
                Totalbelopp = totalmoney,
                Levererad = false
            };


            using (TomasosContext db = new TomasosContext())
            {
                db.Add(bestallning);
                db.SaveChanges();
            }
            SaveBestallningMatratter(matratter);

        }

        public void SaveBestallningMatratter(List<Matratt> matratter)
        {
            
            var bestallningsmatrattlista = new List<BestallningMatratt>();
            var id = 0;
            var first = 0;
            var count = 0;
            var nymatratter = matratter.OrderBy(r => r.MatrattNamn).ToList();
            using (TomasosContext db = new TomasosContext())
            {
                var listbestallning = db.Bestallning.OrderByDescending(r => r.BestallningDatum).ToList();
                for (var i = 0; i < nymatratter.Count; i++)
                {
                    
                    if(id != nymatratter[i].MatrattId)
                    {
                        first++;
                        var best = new BestallningMatratt();
                        id = nymatratter[i].MatrattId;
                        best.BestallningId = listbestallning[0].BestallningId;
                        best.MatrattId = nymatratter[i].MatrattId;
                        best.Antal = 1;
                        bestallningsmatrattlista.Add(best);

                    }
                    else if(id == nymatratter[i].MatrattId)
                    {
                        count = first - 1;
                        bestallningsmatrattlista[count].Antal++;

                    }   
                }
                foreach (var item in bestallningsmatrattlista)
                {
                    db.Add(item);
                    db.SaveChanges();
                }
            }
        }

        public int GetTotalPayment(List<Matratt> matratter)
        {
            var totalmoney = 0;
            foreach (var matratt in matratter)
            {
                totalmoney += matratt.Pris;
            }
            return totalmoney;
        }

        public void UpdateUser(Kund user, int customerid)
        {
            using (TomasosContext db = new TomasosContext())
            {
                var customer = GetById(customerid);

                customer.Namn = user.Namn;
                customer.Gatuadress = user.Gatuadress;
                customer.Postnr = user.Postnr;
                customer.Postort = user.Postort;
                customer.Email = user.Email;
                customer.Telefon = user.Telefon;
                customer.AnvandarNamn = user.AnvandarNamn;
                customer.Losenord = user.Losenord;
                db.Kund.Update(customer);
                db.SaveChanges();
            }
        }

        public List<MatrattTyp> GetMatrattTyper()
        {
            var matratttyper = new List<MatrattTyp>();
            using (TomasosContext db = new TomasosContext())
            {
                matratttyper = db.MatrattTyp.ToList();
            }
            return matratttyper;
        }

        public Kund CheckUserName(Kund customer)
        {
            var kund = new Kund();
            using (TomasosContext db = new TomasosContext())
            {
                kund = db.Kund.FirstOrDefault(r => r.AnvandarNamn == customer.AnvandarNamn);
            }
            return kund;
        }

        public Kund GetCustomerByUsername(string username)
        {
            var kund = new Kund();
            using (TomasosContext db = new TomasosContext())
            {
                kund = db.Kund.FirstOrDefault(r => r.AnvandarNamn == username);
            }
            return kund;
        }
    }

}
