using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace FilmHub.Models
{

    [Table("favoriti")]
    public class Favoriti
    {
        [Column("id")]
        [Key]
        [Required]
        [Display(Name = "ID filma")]
        public int Id { get; set; }

        [Column("naslov")]
        [Required(ErrorMessage = "Ovo polje je obavezno")]
        [Display(Name = "Naslov filma")]
        public string Naslov { get; set; }

        [Column("kategorija")]
        [Display(Name = "Kategorija")]
        public Kategorija Kategorija { get; set; }

        [Column("godina")]
        [Required(ErrorMessage = "Ovo polje je obavezno")]
        [Display(Name = "Godina")]
        public string Godina { get; set; }

        
    }
    public class Kosarica
    {
        public List<Favoriti> Items { get; set; }

        public Kosarica()
        {
            Items = new List<Favoriti>();
        }

        public void AddItem(Favoriti item)
        {
            Items.Add(item);
        }

        public void RemoveItem(int itemId)
        {
            var itemToRemove = Items.FirstOrDefault(i => i.Id == itemId);
            if (itemToRemove != null)
            {
                Items.Remove(itemToRemove);
            }
        }
    }
    }