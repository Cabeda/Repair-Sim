using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RepairSim.Enums
{
    /// <summary>
    /// Tempos de reparação ao minuto
    /// </summary>
    ///
    public enum Reparacao
    {
        
        SubstituiçãoMotherboard = 14,
        SubstituicaoDisco = 12,
        SubstituicaoMemoria = 10,
        SubstituicaoEcra = 9,
        SubstituicaoTransformador = 8,
        Diagnostico = 4,
        Testes = 9,
        OutrasReparações = 5,
        TestesFinais = 8
    }
}
