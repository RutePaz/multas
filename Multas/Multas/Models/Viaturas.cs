using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.Web;

namespace Multas.Models
{
    public class Viaturas
    {
        public Viaturas() {
            ListaDeMultas = new HashSet<Multas>();
        }
        [Key]
        public int  ID { get; set; } //primary key

        //dados da viatura
        public string Matricula { get; set; }
        public string Marca { get; set; }
        public string Modelo { get; set; }

        public string Cor { get; set; }
        //dados do dono
        public string NomeDono { get; set; }
        public string MoradaDono { get; set; }
        public string CodPostalDono { get; set; }
        //lista de objetos que este atributo vai ter como chave forasteira
        //referência ás multas que um condutor recebe numa viatura
        public virtual ICollection <Multas> ListaDeMultas {get;set;}
    }
}