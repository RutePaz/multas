using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Multas.Models
{
    public class Agentes
    {
        public Agentes()
        {
            ListaDeMultas = new HashSet<Multas>();
        }
        [Key]
        public int ID { get; set; }

        public string NOME { get; set; }

        public string Esquadra { get; set; }

        public string Fotografia { get; set; }
        //lista de objetos que este atributo vai ter como chave forasteira
        //referência ás multas que um agente emite 
        public virtual ICollection<Multas> ListaDeMultas { get; set; }

    }
}