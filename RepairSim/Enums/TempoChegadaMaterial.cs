using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RepairSim.Enums
{
    /// <summary>
    /// Tempos de chegada de material ao minuto
    /// </summary>
    public enum TempoChegadaMaterial
    {
        Motherboard =14400,
        Disco = 1440,
        Memória = 1445,
        Ecrâ = 4325,
        Transformador = 4320,
        Nenhum = 0
    }
}
