using RepairSim.Models.Entidades;
using RepairSim.Models.Eventos;
using RepairSim.Models.SimModels;
using RepairSim.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;


namespace RepairSim.Models.BlocoPrincipal
{
    public class ProgramaPrincipal
    {
        public List<Simulador> SnapshotsSim { get; set; }

        public Simulador Sim { get; set; }

        /// <summary>
        /// Initializes the simulator variables and events
        /// </summary>
        public void Inicializador(ValoresEntradaViewModel viewModel)
        {

            if (viewModel.ColaboradoresArmazem.Any() &&
                                    viewModel.ColaboradoresLogistica.Any() &&
                                    viewModel.Testers.Any())
            {

                DateTime startTime = DateTime.Now;

                Sim = new Simulador(viewModel.Duracao,
                                        viewModel.NumEquipamentosSim,
                                        viewModel.AceitacaoOrcamentos,
                                        viewModel.TaxaReclamacao,
                                        viewModel.TaxaUrgencia,
                                        viewModel.Tecnicos,
                                        viewModel.ColaboradoresArmazem,
                                        viewModel.ColaboradoresLogistica,
                                        viewModel.Equipamentos,
                                        viewModel.Testers
                                        );

                //Adiciona primeiro evento à lista
                ReceberEquipamento evento = new ReceberEquipamento();
                evento.GerarEvento(Sim.ColaboradoresLogistica.ToList(), Sim.ListaEsperaReceberEquipamento, Sim.Contador, Sim.ListaEventos, Sim.NumEquipamentosSim);

                SnapshotsSim = new List<Simulador>
                {
                    Sim
                };

                RotinaPrincipal();

                Sim.Contador.TimeSpan = (DateTime.Now - startTime);
            }
        }

        //A remover quando funcionar o formulário
    


        public void RotinaPrincipal()
        {
            //Bloco Executivo
            //1. Determinar tipo do próximo evento a ocorrer
            //2. Se Lista de Eventos vazia, terminar a simulação
            while(Sim.Contador.NumEquipamentos < Sim.NumEquipamentosSim && Sim.Contador.TempoAtual < Sim.DuracaoSim)
            {
                //3. Avançar relógio para o tempo de ocorrência do evento
                Temporizador();
                SnapshotsSim.Add(Sim);
            }
        }


        public void Temporizador()
        {

                var evento = Sim.ListaEventos.FirstOrDefault();
         
                Sim.ListaEventos.RemoveAt(0);

                //Correr próximo evento
                evento.Run(Sim);

                //Atualizar Contador estatístico
                Sim.Contador.ActualizacaoEstatistica(evento.Duracao);

        }
    }
}
