using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TomasosPizzeriaUppgift.Models
{
    public partial class Kund
    {
        public Kund()
        {
            Bestallning = new HashSet<Bestallning>();
        }

        public int KundId { get; set; }
        [StringLength(100, ErrorMessage = "Namn får vara max 100 tecken")]
        [Required(ErrorMessage = "Namn är obligatoriskt")]
        public string Namn { get; set; }
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Gatuadress måste vara minst 2 och max 50 tecken")]
        [Required(ErrorMessage = "Garuadress är obligatoriskt")]
        public string Gatuadress { get; set; }
        [StringLength(20, MinimumLength = 3, ErrorMessage = "Postnummer måste vara minst 3 och max 20 tecken")]
        [Required(ErrorMessage = "Postnummer är obligatoriskt")]
        public string Postnr { get; set; }
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Postort måste vara minst 2 och max 100 tecken")]
        [Required(ErrorMessage = "Postort är obligatoriskt")]
        public string Postort { get; set; }
        public string Email { get; set; }
        public string Telefon { get; set; }
        [StringLength(20, MinimumLength = 3, ErrorMessage = "Användarnamn måste vara minst 3 och max 20 tecken")]
        [Required(ErrorMessage = "Användarnamn är obligatoriskt")]
        public string AnvandarNamn { get; set; }
        [StringLength(20, MinimumLength = 5, ErrorMessage = "Lösenord måste vara minst 5 och max 20 tecken")]
        [Required(ErrorMessage = "Lösenord är obligatoriskt")]
        public string Losenord { get; set; }

        public virtual ICollection<Bestallning> Bestallning { get; set; }
    }
}
