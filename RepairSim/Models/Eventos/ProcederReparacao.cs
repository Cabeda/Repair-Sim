using RepairSim.Models.Entidades;
using RepairSim.Models.SimModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RepairSim.Models.Eventos
{
    public class ProcederReparacao : IEvento
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public float TempoEntradaEmFilaEspera { get; set; }
        public float TempoEntrada { get; set; }
        public string NomeEvento { get; set; } = "ProcederReparacao";
        public float Duracao
        {
            get
            {
                return (float)(Funcionario.Experiencia * (float)Equipamento.Reparacao);
            }
        }
        public Equipamento Equipamento { get; set; }
        public Tecnico Funcionario { get; set; }

        public Simulador Run(Simulador Sim)
        {
            AtualizarContadorEvento(Sim.Contador, Sim.ListaEsperaProcederReparacao.Count());
            AtualizarEstadoFuncionario(Sim.Tecnicos.ToList(), false);
            AvancarProximoEvento(Sim.Testers.ToList(), Sim.ListaEsperaExecutaTeste.ToList(), Sim.Contador, Sim.ListaEventos);
            if (Sim.ListaEsperaProcederReparacao.Count > 0)
                GerarEvento(Sim.Tecnicos.ToList(), Sim.ListaEsperaProcederReparacao.ToList(), Sim.ListaEventos, Sim.Contador);

            return Sim;
        }

        public ContadorEstatistico AtualizarContadorEvento(ContadorEstatistico Contador,int NumEquipamentosEspera)
        {
            Contador.IncrementaProcederReparacao(TempoEntrada, TempoEntradaEmFilaEspera, Duracao, NumEquipamentosEspera);

            return Contador;
        }


        public List<Tecnico> AtualizarEstadoFuncionario(List<Tecnico> funcionarios, bool Ocupado)
        {
            Funcionario.Ocupado = Ocupado;
            foreach (var item in funcionarios.Where(x => x.Id == Funcionario.Id))
            {
                item.Ocupado = Ocupado;
            }
            return funcionarios;
        }

        private void GerarEvento(List<Tecnico> Funcionarios, List<ProcederReparacao> ListaEspera, List<IEvento> ListaEventos, ContadorEstatistico contador)
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

        private void AvancarProximoEvento(List<Tester> Funcionarios, List<ExecutaTeste> ListaEspera, ContadorEstatistico Contador, List<IEvento> ListaEventos)
        {
            if (Funcionarios.Where(x => x.Ocupado == false).Count() > 0)
            {
                var evento = new ExecutaTeste()
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
                var evento = new ExecutaTeste()
                {
                    Equipamento = Equipamento,
                    TempoEntradaEmFilaEspera = Contador.TempoAtual
                };
                ListaEspera.Add(evento);
            }
        }
    }
}
