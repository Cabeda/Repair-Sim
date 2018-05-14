using RepairSim.Enums;
using RepairSim.Models.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RepairSim.Models.SimModels;

namespace RepairSim.Models.Eventos
{
    public class ProcederDiagnostico : IEvento
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public float TempoEntradaEmFilaEspera { get; set; }
        public float TempoEntrada { get; set; }
        public string NomeEvento { get; set; } = "ProcederDiagnostico";
        public float Duracao
        {
            get
            {
                //Diagnostico é sempre 15 e varia a taxa de experiencia
                return (Funcionario.Experiencia * (float)Reparacao.Diagnostico);
            }
        }
        public Equipamento Equipamento { get; set; }
        public Tecnico Funcionario { get; set; }

        public Simulador Run(Simulador Sim)
        {
            AtualizarContadorEvento(Sim.Contador, Sim.ListaEsperaProcederDiagnostico.Count());
            AtualizarEstadoFuncionario(Sim.Tecnicos.ToList(), false);
            AvancarProximoEvento(Sim);
            if (Sim.ListaEsperaProcederDiagnostico.Count() > 0)
                GerarEvento(Sim.Tecnicos.ToList(), Sim.ListaEsperaProcederDiagnostico, Sim.ListaEventos, Sim.Contador);

            return Sim;
        }

        private ContadorEstatistico AtualizarContadorEvento(ContadorEstatistico Contador, int NumEquipamentosEspera)
        {
            Contador.IncrementaProcederDiagnostico(TempoEntrada, TempoEntradaEmFilaEspera, Duracao, NumEquipamentosEspera);

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


        private void GerarEvento(List<Tecnico> Funcionarios, List<ProcederDiagnostico> ListaEspera, List<IEvento> ListaEventos, ContadorEstatistico contador)
        {

            if (ListaEspera.Count() > 0 && Funcionarios.Any(x => x.Ocupado == false))
            {
                ListaEspera = ListaEspera.OrderBy(x => x.TempoEntradaEmFilaEspera )
                                        .OrderBy(x=> x.Equipamento.EquipamentoUrgencia || x.Equipamento.EquipamentoReclamacao)
                                        .ToList();

                var novoEvento = ListaEspera.FirstOrDefault();
                ListaEspera.RemoveAt(0);

                novoEvento.Funcionario = Funcionarios.FirstOrDefault(x => x.Ocupado == false);
                novoEvento.TempoEntrada = contador.TempoAtual;
                ListaEventos.Add(novoEvento);
                novoEvento.AtualizarEstadoFuncionario(Funcionarios.ToList(), true);
            }

        }

        private Simulador AvancarProximoEvento(Simulador Sim)
        {

            if (Equipamento.EquipamentoAceitacaoOrcamento == false)
            {
                GerarEmbalarEquipamentoParaEnvio(Sim.ColaboradoresLogistica.ToList(), Sim.ListaEsperaEmbalarEquipamentoParaEnvio, Sim.Contador, Sim.ListaEventos);
            }
            else if(Equipamento.TempoChegadaMaterial == TempoChegadaMaterial.Nenhum)
            {
                GerarProcederReparacao(Sim.Tecnicos.ToList(), Sim.ListaEsperaProcederReparacao, Sim.Contador, Sim.ListaEventos);
            }
            else {
                GerarFazPedidoRecebeMaterial(Sim.ColaboradoresArmazem.ToList(), Sim.ListaEsperaFazPedidoRecebeMaterial, Sim.Contador, Sim.ListaEventos);
            }

            return Sim;
        }

        private void GerarEmbalarEquipamentoParaEnvio(List<ColaboradorLogistica> Funcionarios, List<EmbalarEquipamentoParaEnvio> ListaEspera, ContadorEstatistico Contador, List<IEvento> ListaEventos)
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

        private void GerarProcederReparacao(List<Tecnico> Funcionarios, List<ProcederReparacao> ListaEspera, ContadorEstatistico Contador, List<IEvento> ListaEventos)
        {
            if (Funcionarios.Where(x => x.Ocupado == false).Count() > 0)
            {
                var evento = new ProcederReparacao()
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
                var evento = new ProcederReparacao()
                {
                    Equipamento = Equipamento,
                    TempoEntradaEmFilaEspera = Contador.TempoAtual
                };
                ListaEspera.Add(evento);
            }
        }

        private void GerarFazPedidoRecebeMaterial(List<ColaboradorArmazem> Funcionarios, List<FazPedidoRecebeMaterial> ListaEspera, ContadorEstatistico Contador, List<IEvento> ListaEventos)
        {
            if (Funcionarios.Where(x => x.Ocupado == false).Count() > 0)
            {
                var evento = new FazPedidoRecebeMaterial()
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
                var evento = new FazPedidoRecebeMaterial()
                {
                    Equipamento = Equipamento,
                    TempoEntradaEmFilaEspera = Contador.TempoAtual
                };
                ListaEspera.Add(evento);
            }
        }
    }
}
