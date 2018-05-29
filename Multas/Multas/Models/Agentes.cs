using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
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
        [DatabaseGenerated()]
        public int ID { get; set; }
        [Required(ErrorMessage ="O {0} é de preenchimento obrigatório")]
        [RegularExpression("[A-ZÁÉÍÓÚÀÈÌÒÙÂÊîÔÛ][a-záéíóúàèìòùâêîôûãõç]+ (( |’|-| dos | da | de| e | d’)[A-ZÁÉÍÓÚÀÈÌÒÙÂÊîÔÛ][a-záéíóúàèìòùâêîôûãõç]+){1,3})", ErrorMessage = "Não válido")]
        public string Nome { get; set; }
        [Required(ErrorMessage = "A {0} é de preenchimento obrigatório")]
        [RegularExpression("[A-ZÁÉÍÓÚÀÈÌÒÙÂÊîÔÛ]*[a-záéíóúàèìòùâêîôûãõç]*", ErrorMessage = "Não válido")]
        public string Esquadra { get; set; }

        public string Fotografia { get; set; }
        //lista de objetos que este atributo vai ter como chave forasteira
        //referência ás multas que um agente emite 
        public virtual ICollection<Multas> ListaDeMultas { get; set; }

        //nome do login (UserName) utilizado pelo agente
        //na prática, ´´e uma FK para a tabela dos utilizadores 
       // [Required]
        public string UserName { get; set; }
    }
}