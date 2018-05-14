using RepairSim.Models.Distribuicao;
using RepairSim.Models.Entidades;
using RepairSim.Models.SimModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RepairSim.Models.Eventos
{
    public class FazPedidoRecebeMaterial : IEvento
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        //Entidades
        public Equipamento Equipamento { get; set; }
        public ColaboradorArmazem Funcionario { get; set; }

        public float TempoEntradaEmFilaEspera { get; set; }
        public float TempoEntrada { get; set; }
        public string NomeEvento { get; set; } = "FazPedidoRecebeMaterial";

        private DistribuicaoUniforme dist { get; set; }

        //Valida a existencia de Stock ou não com 70% de probabilidades de existencia de stock
        //Com base disso calcula o tempo de entrega do material

        public float Duracao
        {
            get; set;
        }
        private float gerarValorAleatorio(int min, int max)
        {
            DistribuicaoUniforme dist = new DistribuicaoUniforme();
            return (dist.gerarValorAleatorio(min, max) * this.Funcionario.Experiencia);
        }


        //Arranca com o simulador gerando os eventos do armazem
        public Simulador Run(Simulador Sim)
        {
            AtualizarContadorEvento(Sim.Contador, Sim.ListaEsperaFazPedidoRecebeMaterial.Count());
            AtualizarEstadoFuncionario(Sim.ColaboradoresArmazem.ToList(), false);
            AvancarProximoEvento(Sim.Tecnicos.ToList(), Sim.ListaEsperaProcederReparacao, Sim.Contador, Sim.ListaEventos);
            if(Sim.ListaEsperaFazPedidoRecebeMaterial.Count > 0)
            GerarEvento(Sim.ColaboradoresArmazem.ToList(), Sim.ListaEsperaFazPedidoRecebeMaterial, Sim.ListaEventos, Sim.Contador);

            return Sim;
        }

        //vai atualizando o contador estatistico
        public ContadorEstatistico AtualizarContadorEvento(ContadorEstatistico Contador, int NumEquipamentosEspera)
        {
            var distribuicaoStock = gerarValorAleatorio(0, 100);

            if (distribuicaoStock >= 30)
                Duracao = gerarValorAleatorio(3, 5);

            else if (distribuicaoStock <= 30)
                Duracao = (float)(Funcionario.Experiencia) * (float)(gerarValorAleatorio(4, 9));

            Contador.IncrementaFazPedidoRecebeMaterial(TempoEntrada, TempoEntradaEmFilaEspera, Duracao, NumEquipamentosEspera);

            return Contador;
        }

        //Altera o estado do funcionario do colaborador do armazem
        public List<ColaboradorArmazem> AtualizarEstadoFuncionario(List<ColaboradorArmazem> funcionarios, bool Ocupado)
        {
            Funcionario.Ocupado = Ocupado;
            foreach (var item in funcionarios.Where(x => x.Id == Funcionario.Id))
            {
                item.Ocupado = Ocupado;
            }
            return funcionarios;
        }

        private void GerarEvento(List<ColaboradorArmazem> Funcionarios, List<FazPedidoRecebeMaterial> ListaEspera, List<IEvento> ListaEventos, ContadorEstatistico contador)
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

        private void AvancarProximoEvento(List<Tecnico> Funcionarios, List<ProcederReparacao> ListaEspera, ContadorEstatistico Contador, List<IEvento> ListaEventos)
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
    }
}
