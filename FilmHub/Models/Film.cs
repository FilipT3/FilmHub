using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace FilmHub.Models
{

    [Table("filmovi")]
    public class Film
    {
        [Key]
        [Column("id")]
        [Display(Name = "ID filma")]
        public int Id { get; set; }

        [Column("naslov")]
        [Required(ErrorMessage = "Ovo polje je obavezno")]
        [Display(Name = "Naslov filma")]
        public string Naslov { get; set; }

        [Column("produkcija")]
        [Required(ErrorMessage = "Ovo polje je obavezno")]
        [Display(Name = "Produkcija")]
        public string Produkcija { get; set; }

        [Column("kategorija")]
        [Display(Name = "Kategorija")]
        public  Kategorija Kategorija { get; set; }

        [Column("glumci")]
        [Required(ErrorMessage = "Ovo polje je obavezno")]
        [Display(Name = "Glumci")]
        public string Glumci { get; set; }

        [Column("trajanje")]
        [Required(ErrorMessage = "Ovo polje je obavezno")]
        [Display(Name = "Trajanje:")]
        public string Trajanje { get; set; }

        [Column("godina")]
        [Required(ErrorMessage = "Ovo polje je obavezno")]
        [Display(Name = "Godina:")]
        public string Godina { get; set; }

        [Column("opis")]
        [Required(ErrorMessage = "Ovo polje je obavezno")]
        [Display(Name = "Opis")]
        public string Opis { get; set; }

    }
}