using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Multas.Models
{
    public class Utilizadores
    {
        /// <summary>
        /// os atributos que aqui vao ser adicionados serao adicionados a  tabela dos utilizadores
        /// </summary>
        [Key]
        public int ID { get; set; }
        public string NomeProprio { get; set; }
        public string Apelido { get; set; }
        // ? o valor colocado em DataNascimento é facultativo
        //Na string isto não é necessário pois esta aceita valores nulos. Uma vez que a string é um array de caracteres o valor "nulo" é aceite 
        public DateTime? DataNascimento { get; set; }
        public string NIF { get; set; }
        //******* o atirbuto sequinte vai criar uma chave forasteira para a tabela da 'autenticação'**********
        public string NomeRegistoDoUtilizador { get; set; }

    }
}