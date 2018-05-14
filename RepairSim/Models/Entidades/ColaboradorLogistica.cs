using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RepairSim.Models.Entidades
{
    public class ColaboradorLogistica : IFuncionario
    {
        public ColaboradorLogistica()
        {
            Id = Guid.NewGuid();
            Ocupado = false; 
        }
        public Guid Id { get; set; }
        public float Experiencia { get; set; }
        public bool Ocupado { get; set; }

    
    }
}
