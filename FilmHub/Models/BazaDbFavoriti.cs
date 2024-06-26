using MySql.Data.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace FilmHub.Models
{
    [DbConfigurationType(typeof(MySqlEFConfiguration))]
    public class BazaDbFavoriti : DbContext
    {
        public DbSet<Favoriti> PopisFavorita { get; set; }
    }
}