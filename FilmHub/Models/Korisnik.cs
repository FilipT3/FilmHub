using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace FilmHub.Models
{
    [Table("korisnici")]
    public class Korisnik
    {
        [Key]
        [Column("korisnicko_ime")]
        [Display(Name ="Korisničko ime")]
        [Required(ErrorMessage = "Korisničko ime je obavezno")]
        public string KorisnickoIme { get; set; }

        [Column("email")]
        [Display(Name = "Email")]
        [Required(ErrorMessage = "Ovo polje je obavezno")]
        [EmailAddress]
        public string Email { get; set; }

        [Column("lozinka")]
        [Display(Name = "Lozinka")]
        [Required(ErrorMessage = "Lozinka je obavezna")]
        public string Lozinka { get; set; }

        [Column("prezime")]
        [Display(Name = "Prezime")]
        [Required(ErrorMessage = "Ovo polje je obavezno")]
        public string Prezime { get; set; }

        [Column("ime")]
        [Display(Name = "Ime")]
        [Required(ErrorMessage = "Ovo polje je obavezno")]
        public string Ime { get; set; }

        public string PrezimeIme
        {
            get
            {
                return Prezime + " " + Ime;
            }
        }

        [Column("ovlast")]
        [Display(Name = "Ovlast")]
        [ForeignKey("Ovlast")]
        public string SifraOvlasti { get; set; }
        public virtual Ovlast Ovlast { get; set; }

        [Display(Name = "Lozinka")]
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Ovo polje je obavezno")]
        [NotMapped]
        public string LozinkaUnos { get; set; }

        [Display(Name = "Potvrdi lozinku")]
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Ovo polje je obavezno")]
        [NotMapped]
        [Compare("LozinkaUnos" , ErrorMessage ="Lozinke moraju biti jednake")]
        public string LozinkaUnos2 { get; set; }

    }
}


