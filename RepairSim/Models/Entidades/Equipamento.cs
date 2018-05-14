using RepairSim.Enums;
using RepairSim.Models.Distribuicao;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RepairSim.Models.Entidades
{
    public class Equipamento
    {
        Guid Id { get; set; }
        public bool EquipamentoUrgencia { get; set; }
        public bool EquipamentoAceitacaoOrcamento { get; set; }
        public bool EquipamentoReclamacao { get; set; }
        public Reparacao Reparacao
        {
            get
            {
                //TODO
                Array values = Enum.GetValues(typeof(Reparacao));
                DistribuicaoUniforme d = new DistribuicaoUniforme();
                var aleatorio = d.gerarValorAleatorio(0,18);
                if(aleatorio <= 6)
                {
                    var randomReparacao = Reparacao.SubstituicaoDisco;
                    return randomReparacao;
                }
                else if (aleatorio <= 6)
                {
                    var randomReparacao = Reparacao.SubstituicaoEcra;
                    return randomReparacao;
                }
                else if (aleatorio <= 9)
                {
                    var randomReparacao = Reparacao.SubstituicaoTransformador;
                    return randomReparacao;
                }
                else if (aleatorio <= 12)
                {
                    var randomReparacao = Reparacao.SubstituicaoDisco;
                    return randomReparacao;
                }
                else if (aleatorio <= 15)
                {
                    var randomReparacao = Reparacao.SubstituicaoMemoria;
                    return randomReparacao;
                }
               
                else if (aleatorio <= 18)
                {
                    var randomReparacao = Reparacao.OutrasReparações;
                    return randomReparacao;
                }

                else
                {
                    return 0;
                }


            }
        }
        public TempoChegadaMaterial TempoChegadaMaterial { get
            {
                switch(Reparacao)
                {
                    case Reparacao.SubstituicaoDisco:
                        return TempoChegadaMaterial.Disco;
                    case Reparacao.SubstituicaoEcra:
                        return TempoChegadaMaterial.Ecrâ;
                    case Reparacao.SubstituicaoMemoria:
                        return TempoChegadaMaterial.Memória;
                    case Reparacao.SubstituicaoTransformador:
                        return TempoChegadaMaterial.Transformador;
                    case Reparacao.SubstituiçãoMotherboard:
                        return TempoChegadaMaterial.Motherboard;
                    default:
                        return TempoChegadaMaterial.Nenhum;

                }
            }
            }

        //public Material MaterialReparacao = new Material();
    }
}
