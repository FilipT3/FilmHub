using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FilmHub.Models
{
    public class NovostiDB
    {
        private static List<Novost> lista = new List<Novost>();
        private static bool listaInicijalizirana = false; 

        public NovostiDB()
        {
            if (listaInicijalizirana == false)
            {
                lista.Add(new Novost()
                {
                    Id = 1,
                    Naslov = "Otvorenje",
                    Opis = "Otvorenje stranice bit će krajem ljeta.",
                    DatumUnosa = new DateTime(2024, 7, 2)
                });

                lista.Add(new Novost()
                {
                    Id = 2,
                    Naslov = "Svecano otvorenje",
                    Opis = "Svecano otvorenje stranice bit će 4.9.",
                    DatumUnosa = new DateTime(2024, 8, 20)
                });
            }
        }

        public List<Novost> VratiListu()
        {
            return lista;
        }

        public void AzurirajNovost(Novost novost)
        {
            int novostIndex = lista.FindIndex(x => x.Id == novost.Id);
            lista[novostIndex] = novost;
        }
        
    }
}