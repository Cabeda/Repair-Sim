using RepairSim.Models.Distribuicao;
using RepairSim.Models.Entidades;
using RepairSim.Models.SimModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RepairSim.Models.Eventos
{
    public class ReceberEquipamento : IEvento
    {
       

        public Guid Id { get; set; } = Guid.NewGuid();
        public float TempoEntradaEmFilaEspera { get; set; }
        public float TempoEntrada { get; set; }
        public string NomeEvento { get; set; } = "RecebeEquipamento";

        public Equipamento Equipamento { get; set; }
        public ColaboradorLogistica Funcionario { get; set; }
        public float Duracao
        {
            get; set;
        }
        private float gerarValorAleatorio(int min,int  max, float experiencia)
        {
            DistribuicaoUniforme dist = new DistribuicaoUniforme();
            return (dist.gerarValorAleatorio(min,max) * experiencia);
        }

        public Simulador Run(Simulador Sim)
        {
            AtualizarContadorEvento(Sim.Contador, Sim.ListaEsperaReceberEquipamento.Count());
            //Coloca o funcionario como desocupado
            AtualizarEstadoFuncionario(Sim.ColaboradoresLogistica.ToList(), false);
            AvancarProximoEvento(Sim);
            GerarEvento(Sim.ColaboradoresLogistica.ToList(), Sim.ListaEsperaReceberEquipamento, Sim.Contador, Sim.ListaEventos, Sim.NumEquipamentosSim);

            return Sim;
        }

        private ContadorEstatistico AtualizarContadorEvento(ContadorEstatistico Contador,int NEquipamentoFilaEspera)
        {
            this.Duracao =gerarValorAleatorio(1,7, this.Funcionario.Experiencia);
            Contador.IncrementaReceberEquipamento(TempoEntrada, TempoEntradaEmFilaEspera, Duracao, NEquipamentoFilaEspera);

            return Contador;
        }

        private List<ColaboradorLogistica> AtualizarEstadoFuncionario(List<ColaboradorLogistica> funcionarios, bool Ocupado)
        {
            Funcionario.Ocupado = Ocupado;
            foreach (var item in funcionarios.Where(x => x.Id == Funcionario.Id))
            {
                item.Ocupado = Ocupado;
            }
            return funcionarios;
        }

        public void GerarEvento(List<ColaboradorLogistica> Funcionarios, List<ReceberEquipamento> ListaEspera, ContadorEstatistico Contador, List<IEvento> ListaEventos, int NEquipamentos)
        {

            if (Funcionarios.Where(x => x.Ocupado == false).Count() > 0 && Contador.NumEquipamentos < NEquipamentos)
            {
                ReceberEquipamento novoEvento;
                if (ListaEspera.Any())
                {
                    novoEvento = ListaEspera.First();
                    novoEvento.Funcionario = Funcionarios.FirstOrDefault(x => x.Ocupado == false);

                    ListaEspera.RemoveAt(0);
                }
                else
                {

                    novoEvento = new ReceberEquipamento()
                    {
                        TempoEntradaEmFilaEspera = Contador.TempoAtual,
                        TempoEntrada = Contador.TempoAtual + gerarValorAleatorio(0,2, 1),
                        Funcionario = Funcionarios.FirstOrDefault(x => x.Ocupado == false)
                    };

                }

                ListaEventos.Add(novoEvento);
                novoEvento.AtualizarEstadoFuncionario(Funcionarios.ToList(), true);
            }
            else
            {
                var novoRecebeEquipamento = new ReceberEquipamento()
                {
                    TempoEntradaEmFilaEspera = Contador.TempoAtual,

                };

                ListaEspera.Add(novoRecebeEquipamento);
            }
        }

        private void AvancarProximoEvento(Simulador Sim)
        {
            Equipamento eq = GerarEquipamento(Sim);
            Sim.Equipamentos.Add(eq);

            if (Sim.Tecnicos.Where(x => x.Ocupado == false).Count() > 0)
            {
                var evento = new ProcederDiagnostico()
                {
                    Equipamento = eq,
                    Funcionario = Sim.Tecnicos.Where(x => x.Ocupado == false).FirstOrDefault(),
                    TempoEntradaEmFilaEspera = Sim.Contador.TempoAtual,
                    TempoEntrada = Sim.Contador.TempoAtual
                };

                evento.AtualizarEstadoFuncionario(Sim.Tecnicos.ToList(), true);
                Sim.ListaEventos.Add(evento);

            }
            else
            {
                var evento = new ProcederDiagnostico()
                {
                    Equipamento = eq,
                    TempoEntradaEmFilaEspera = Sim.Contador.TempoAtual
                };
                Sim.ListaEsperaProcederDiagnostico.Add(evento);
            }
        }

        private Equipamento GerarEquipamento(Simulador Sim)
        {
            //TODO - É necessário entender este bocado de código de forma a perceber como aplicar a distribuicao
            //Acertar os valores aleatórios
        

            Equipamento eq = new Equipamento()
            {
                EquipamentoUrgencia = gerarValorAleatorio(0,100, this.Funcionario.Experiencia) < Sim.TaxaUrgencia ? true: false,
                EquipamentoReclamacao =  gerarValorAleatorio(0,100, this.Funcionario.Experiencia) < Sim.TaxaReclamacao ? true : false,
                EquipamentoAceitacaoOrcamento = gerarValorAleatorio(0,100, this.Funcionario.Experiencia) < Sim.AceitacaoOrcamentos ? true : false
            };
            return eq;
        }
    }
}
