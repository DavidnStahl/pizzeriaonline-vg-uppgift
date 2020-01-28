using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TomasosPizzeriaUppgift.Models;

namespace TomasosPizzeriaUppgift.ViewModels
{
    public class MenuPage
    {
        public List<Matratt> Matratter { get; set; }
        public Produkt Ingredins { get; set; }

        public Matratt matratt{ get; set; }
        public List<MatrattTyp> mattratttyper { get; set; }

        public MatrattTyp mattratttyp { get; set; }
        public List<Produkt> Ingredienses { get; set; }

        public List<Matratt> Matratteradded { get; set; }
        public List<MatrattProdukt> MattrattProdukt { get; set; }

        public MenuPage()
        {

            matratt = new Matratt();
            mattratttyp = new MatrattTyp();
            mattratttyper = new List<MatrattTyp>();
            MattrattProdukt = new List<MatrattProdukt>();
            Matratter = new List<Matratt>();
            Matratteradded = new List<Matratt>();
            Ingredins = new Produkt();
            Ingredienses = new List< Produkt>();
        }
    }
}
