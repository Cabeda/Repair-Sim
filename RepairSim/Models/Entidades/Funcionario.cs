using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RepairSim.Models.Entidades
{
    public interface IFuncionario
    {
        Guid Id { get; set; }
        float Experiencia { get; set; }
        bool Ocupado { get; set; }
    }
}
