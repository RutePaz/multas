using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Multas.Models
{
    public class Multas
    {
        [Key]
        public int ID { get; set; } //primary key

        public string Infracao { get; set; }

        public string LocalDaMulta { get; set; }

        public decimal ValorMulta { get; set; }

        public DateTime DataDaMulta { get; set; }



        //representar as chaves forasteiras que relacionam esta classe com as restantes

        //FK para a tabela dos condutores
        //Em SQL - Constraint NomeDaTabela
        //         Foreign Key NomeDoAtributo references NomeDaTabela(NomeDoAtributoPK)
        [ForeignKey("Condutor")]
        public int CondutorFK { get; set; }
        public virtual Condutores Condutor { get; set; }

        //FK para a tabela das viaturas
        [ForeignKey("Viatura")]
        public int ViaturaFK { get; set; }
        public virtual Viaturas Viatura { get; set; }

        //FK para a tabela dos agentes
        [ForeignKey("Agente")] //Especifica o nome do atributo e não da classe
        public int AgenteFK { get; set; }
        public virtual Agentes Agente { get; set; }

        

    }
}