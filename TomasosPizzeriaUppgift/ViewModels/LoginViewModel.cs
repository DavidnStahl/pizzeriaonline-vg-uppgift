using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TomasosPizzeriaUppgift.ViewModels
{
    public class LoginViewModel
    {

        [Required(ErrorMessage = "Användarnamn är obligatoriskt")]
        public string Username { get; set; }


        [Required(ErrorMessage = "Lösenord är obligatoriskt")]
        public string Password { get; set; }

        [Display(Name = "Kom ihåg mig")]
        public bool RememberMe { get; set; }
    }
}
