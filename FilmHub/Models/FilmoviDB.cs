using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;

namespace FilmHub.Models
{
    public class FilmoviDB
    {
        private static List<Film> lista = new List<Film>();
        private static bool listaInicijalizirana = false;

        public FilmoviDB()
        {
            if (listaInicijalizirana == false)
            {
                listaInicijalizirana |= true;
                lista.Add(new Film());
                lista.Add(new Film());
                lista.Add(new Film());
                lista.Add(new Film());
            }

           

            





        }
        public List<Film> VratiListu()
        {
            return lista;
        }

        public void AzurirajFilm(Film film)
        {
            int filmIndex = lista.FindIndex(x=>x.Id == film.Id);
            lista[filmIndex] = film;
        }

    }
}