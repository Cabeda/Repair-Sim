using RepairSim.Models.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RepairSim.Models.SimModels;
using RepairSim.Models.Distribuicao;

namespace RepairSim.Models.Eventos
{
    public class EmbalarEquipamentoParaEnvio : IEvento
    {

        public Guid Id { get; set; } = Guid.NewGuid();
        public float TempoEntradaEmFilaEspera { get; set; }
        public float TempoEntrada { get; set; }
        public Equipamento Equipamento { get; set; }
        public ColaboradorLogistica Funcionario { get; set; }
        public string NomeEvento { get; set; } = "EmbalarEquipamentoParaEnvio";
        public float Duracao
        {
            get; set;
        }
        private float gerarValorAleatorio(int min, int max)
        {
            DistribuicaoUniforme dist = new DistribuicaoUniforme();
            return (dist.gerarValorAleatorio(min, max) * this.Funcionario.Experiencia);
        }
        public Simulador Run(Simulador Sim)
        {
            AtualizarContadorEvento(Sim.Contador, Sim.ListaEsperaEmbalarEquipamentoParaEnvio.Count());
            AtualizarEstadoFuncionario(Sim.ColaboradoresLogistica.ToList(), false);
            if (Sim.ListaEsperaEmbalarEquipamentoParaEnvio.Count > 0)
                GerarEvento(Sim.ColaboradoresLogistica.ToList(), Sim.ListaEsperaEmbalarEquipamentoParaEnvio, Sim.ListaEventos, Sim.Contador);

            return Sim;
        }

        private ContadorEstatistico AtualizarContadorEvento(ContadorEstatistico Contador, int NumEquipamentosEspera)
        {
            this.Duracao = (Funcionario.Experiencia * gerarValorAleatorio(1, 10));
            Contador.IncrementaEmbalarEquipamentoParaEnvio(TempoEntrada, TempoEntradaEmFilaEspera, Duracao, NumEquipamentosEspera);

            return Contador;
        }

        public List<ColaboradorLogistica> AtualizarEstadoFuncionario(List<ColaboradorLogistica> funcionarios, bool Ocupado)
        {
            Funcionario.Ocupado = Ocupado;
            foreach (var item in funcionarios.Where(x => x.Id == Funcionario.Id))
            {
                item.Ocupado = Ocupado;
            }
            return funcionarios;
        }

        private void GerarEvento(List<ColaboradorLogistica> Funcionarios, List<EmbalarEquipamentoParaEnvio> ListaEspera, List<IEvento> ListaEventos, ContadorEstatistico contador)
        {
            if (ListaEspera.Count() > 0 && Funcionarios.Any(x => x.Ocupado == false))
            {
                ListaEspera = ListaEspera.OrderBy(x => x.TempoEntradaEmFilaEspera).ToList();

                var novoEvento = ListaEspera.FirstOrDefault();
                ListaEspera.RemoveAt(0);

                novoEvento.Funcionario = Funcionarios.FirstOrDefault(x => x.Ocupado == false);
                novoEvento.TempoEntrada = contador.TempoAtual;
                novoEvento.AtualizarEstadoFuncionario(Funcionarios.ToList(), true);
                ListaEventos.Add(novoEvento);
            }
        }
    }
}
