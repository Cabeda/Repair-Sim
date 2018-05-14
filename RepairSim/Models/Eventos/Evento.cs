using RepairSim.Models.Entidades;
using RepairSim.Models.SimModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RepairSim.Models.Eventos
{
    public interface IEvento
    {
        Guid Id { get; set; }
        float TempoEntradaEmFilaEspera { get; set; }
        float TempoEntrada { get; set; }
        float Duracao { get; }

        string NomeEvento { get; set; }

        Equipamento Equipamento { get; set; }
        Simulador Run(Simulador Sim);
    }
}
