using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.SqlTypes;
using System.Linq;
using System.Web;

namespace FilmHub.Models
{
    [Table("novosti")]
    public class Novost
    {
        [Key]
        [Display(Name = "ID novosti")]
        public int Id { get; set; }

        [Display(Name = "Naslov")]
        [Required(ErrorMessage = "{0} je obavezan")]
        public string Naslov { get; set; }

        [Display(Name = "Opis")]
        [Required(ErrorMessage = "{0} je obavezan")]
        public string Opis { get; set; }

        [Column("datum_rodenja")]
        [Display(Name = "Datum Unosa")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        [Required(ErrorMessage = "{0} je obavezan")]
        [DataType(DataType.Date)]
        public DateTime? DatumUnosa { get; set; }

    }
}