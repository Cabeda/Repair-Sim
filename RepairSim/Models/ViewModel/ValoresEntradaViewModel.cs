using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using RepairSim.Models.Entidades;


namespace RepairSim.Models.ViewModel
{
    public class ValoresEntradaViewModel
    {

        public ValoresEntradaViewModel()
        {
            NumEquipamentosSim = 5000;
            Duracao = 20000;
            TaxaReclamacao =5;
            TaxaUrgencia = 7;
            AceitacaoOrcamentos = 95;

            Tecnicos = new List<Tecnico>();
            Equipamentos = new List<Equipamento>();
            ColaboradoresArmazem = new List<ColaboradorArmazem>();
            ColaboradoresLogistica = new List<ColaboradorLogistica>();
            Testers = new List<Tester>();

        }

        [Required]
        [Display(Name = "Taxa de Aceitação de Orçamentos")]
        public float AceitacaoOrcamentos { get; set; }

        [Required]
        [Display(Name = "Taxa de Reclamação")]
        public float TaxaReclamacao { get; set; }

        [Required]
        [Display(Name = "Taxa de Urgência")]
        public float TaxaUrgencia { get; set; }

        [Required]
        [Display(Name = "Duração da Simulação")]
        public float Duracao { get; set; } 

        [Required]
        [Display(Name = "Nº Equipamentos a Simular")]
        public int NumEquipamentosSim { get; set; }

        //Entities Values
        [Display(Name = "Tecnicos")]
        public IList<Tecnico> Tecnicos { get; set;}

        [Display(Name = "Equipamentos")]
        public IList<Equipamento> Equipamentos { get; set;}

        [Display(Name = "Colaboradores de Armazem")]
        public IList<ColaboradorArmazem> ColaboradoresArmazem { get; set;}

        [Display(Name = "Colaboradores de Logistica")]
        public IList<ColaboradorLogistica> ColaboradoresLogistica { get; set;}

        [Display(Name = "Lista de Testers")]
        public IList<Tester> Testers { get; set;}


        

    }
}
