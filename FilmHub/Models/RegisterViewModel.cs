using System.ComponentModel.DataAnnotations;

namespace FilmHub.Models
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Korisničko ime je obavezno")]
        [Display(Name = "Korisničko ime")]
        public string KorisnickoIme { get; set; }

        [Required(ErrorMessage = "Email je obavezan")]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Lozinka je obavezna")]
        [DataType(DataType.Password)]
        [Display(Name = "Lozinka")]
        public string Lozinka { get; set; }

        [Required(ErrorMessage = "Prezime je obavezno")]
        [Display(Name = "Prezime")]
        public string Prezime { get; set; }

        [Required(ErrorMessage = "Ime je obavezno")]
        [Display(Name = "Ime")]
        public string Ime { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Potvrdi lozinku")]
        [Compare("Lozinka", ErrorMessage = "Lozinke se ne poklapaju.")]
        public string PotvrdiLozinku { get; set; }
    }
}
