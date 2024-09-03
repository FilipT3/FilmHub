using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using MySql.Data.EntityFramework;
using System.Web;
using FilmHub.Models;

namespace FilmHub.Data
{
    [DbConfigurationType(typeof(MySqlEFConfiguration))]
    public class BazaDbContext : DbContext
    {
        public DbSet<Film> PopisFilmova { get; set; }
        public DbSet<Korisnik> PopisKorisnika { get; set; }
        public DbSet<Ovlast> PopisOvlasti { get; set; }
        public DbSet<Favoriti> PopisFavorita { get; set; }

        public DbSet<Novost> PopisNovosti { get; set; }

        public void ObrisiFavorite()
        {
            this.PopisFavorita.RemoveRange(this.PopisFavorita);
            this.SaveChanges();
        }

    }
}