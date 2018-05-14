using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RepairSim.Models.Distribuicao
{
    public class DistribuicaoUniforme
    {
        public DistribuicaoUniforme()
        {
        }
        Random random = new Random();

        public float gerarValorAleatorio(float valorMinimo, float valorMaximo)
        {
            //Distribuiçao uniforme entre [0,1] com valores de máximos e minimos difinidos.
            var rand = random.NextDouble();
            var result = (valorMinimo + (valorMaximo - valorMinimo) * (float)rand); 
            return result;
        }

    
    }
}
