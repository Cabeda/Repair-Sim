using RepairSim.Enums;
using RepairSim.Models.Distribuicao;
using RepairSim.Models.Entidades;
using RepairSim.Models.SimModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RepairSim.Models.Eventos
{
    public class ExecutaTeste : IEvento
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public float TempoEntradaEmFilaEspera { get; set; }
        public float TempoEntrada { get; set; }
        public string NomeEvento { get; set; } = "ExecutaTestes";

        //Entidades
        public Equipamento Equipamento { get; set; }
        public Tester Funcionario { get; set; }

        private DistribuicaoUniforme dist { get; set; }

        public float Duracao
        {
            get; set;
        }
        private float gerarValorAleatorio(int min, int max)
        {
            DistribuicaoUniforme dist = new DistribuicaoUniforme();
            return (dist.gerarValorAleatorio(min, max) * this.Funcionario.Experiencia);
        }

        public ExecutaTeste()
        {
            this.dist = new DistribuicaoUniforme();
        }

        public Simulador Run(Simulador Sim)
        {
            AtualizarContadorEvento(Sim.Contador,Sim.ListaEsperaExecutaTeste.Count());
            AtualizarEstadoFuncionario(Sim.Testers.ToList(), false);
            AvancarProximoEvento(Sim);
            if (Sim.ListaEsperaExecutaTeste.Count > 0)
                GerarEvento(Sim.Testers.ToList(), Sim.ListaEsperaExecutaTeste, Sim.ListaEventos, Sim.Contador);

            return Sim;
        }

        public ContadorEstatistico AtualizarContadorEvento(ContadorEstatistico Contador, int NumEquipamentosEspera)
        {
            //Determina o tempo de testes com base da experienca e do valor aleatorio entre 40 e 60 u tempo
            Duracao = (Funcionario.Experiencia) * (float)(Reparacao.Testes) + (gerarValorAleatorio(2, 4));
            Contador.IncrementaExecutaTestes(TempoEntrada, TempoEntradaEmFilaEspera, Duracao, NumEquipamentosEspera);

            return Contador;
        }

        //Atualiza estado do tester
        public List<Tester> AtualizarEstadoFuncionario(List<Tester> funcionarios, bool Ocupado)
        {
            Funcionario.Ocupado = Ocupado;
            foreach (var item in funcionarios.Where(x => x.Id == Funcionario.Id))
            {
                item.Ocupado = Ocupado;
            }
            return funcionarios;
        }

        private void GerarEvento(List<Tester> Funcionarios, List<ExecutaTeste> ListaEspera, List<IEvento> ListaEventos, ContadorEstatistico contador)
        {
            if (ListaEspera.Count() > 0 && Funcionarios.Any(x => x.Ocupado == false))
            {
                ListaEspera = ListaEspera.OrderBy(x => x.TempoEntradaEmFilaEspera).ToList();

                var novoEvento = ListaEspera.FirstOrDefault();
                ListaEspera.RemoveAt(0);

                novoEvento.Funcionario = Funcionarios.FirstOrDefault(x => x.Ocupado == false);
                novoEvento.TempoEntrada = contador.TempoAtual;
                ListaEventos.Add(novoEvento);
                novoEvento.AtualizarEstadoFuncionario(Funcionarios.ToList(), true);
            }
        }

        private void AvancarProximoEvento(Simulador Sim)
        {
            if (PassaTeste() == true)
            {
                AvancaEventoAposTeste(Sim.ColaboradoresLogistica.ToList(), Sim.ListaEsperaEmbalarEquipamentoParaEnvio, Sim.Contador, Sim.ListaEventos);

            }
            else
            {
                RetornaTesteParaDiagnostico(Sim.Tecnicos.ToList(), Sim.ListaEsperaProcederDiagnostico, Sim.Contador, Sim.ListaEventos);
            }
        }

        private void AvancaEventoAposTeste(List<ColaboradorLogistica> Funcionarios, List<EmbalarEquipamentoParaEnvio> ListaEspera, ContadorEstatistico Contador, List<IEvento> ListaEventos)
        {
            if (Funcionarios.Where(x => x.Ocupado == false).Count() > 0)
            {
                var evento = new EmbalarEquipamentoParaEnvio()
                {
                    Equipamento = this.Equipamento,
                    Funcionario = Funcionarios.Where(x => x.Ocupado == false).FirstOrDefault(),
                    TempoEntradaEmFilaEspera = Contador.TempoAtual,
                    TempoEntrada = Contador.TempoAtual
                };

                evento.AtualizarEstadoFuncionario(Funcionarios.ToList(), true);
                ListaEventos.Add(evento);

            }
            else
            {
                var evento = new EmbalarEquipamentoParaEnvio()
                {
                    Equipamento = Equipamento,
                    TempoEntradaEmFilaEspera = Contador.TempoAtual
                };
                ListaEspera.Add(evento);
            }
        }

        private void RetornaTesteParaDiagnostico(List<Tecnico> Funcionarios, List<ProcederDiagnostico> ListaEspera, ContadorEstatistico Contador, List<IEvento> ListaEventos)
        {
            if (Funcionarios.Where(x => x.Ocupado == false).Count() > 0)
            {
                var evento = new ProcederDiagnostico()
                {
                    Equipamento = this.Equipamento,
                    Funcionario = Funcionarios.Where(x => x.Ocupado == false).FirstOrDefault(),
                    TempoEntradaEmFilaEspera = Contador.TempoAtual,
                    TempoEntrada = Contador.TempoAtual
                };

                evento.AtualizarEstadoFuncionario(Funcionarios.ToList(), true);
                ListaEventos.Add(evento);

            }
            else
            {
                var evento = new ProcederDiagnostico()
                {
                    Equipamento = Equipamento,
                    TempoEntradaEmFilaEspera = Contador.TempoAtual
                };
                ListaEspera.Add(evento);
            }
        }

        //percentagem de testes com sucesso de 80%
        private bool PassaTeste()
        {
            var PassaTeste = dist.gerarValorAleatorio(0, 100);
            if (PassaTeste <= 80)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
