using RepairSim.Models.Entidades;
using RepairSim.Models.Eventos;
using RepairSim.Models.SimModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace RepairSim.Models.SimModels
{
    /// <summary>
    /// Classe responsável por armazenar os eventos da simulação
    /// </summary>
    public class Simulador
    {
        public float? DuracaoSim { get; set; }
        public int NumEquipamentosSim { get; set; }
        public float AceitacaoOrcamentos { get; set; }
       
        public float TaxaReclamacao { get; set; }
        public float TaxaUrgencia { get; set; }

        public ContadorEstatistico Contador { get; set; }
        public IList<Tecnico> Tecnicos { get; set; }
        public IList<ColaboradorArmazem> ColaboradoresArmazem { get; set; }
        public IList<ColaboradorLogistica> ColaboradoresLogistica { get; set; }
        public IList<Equipamento> Equipamentos { get; set; }
        public IList<Tester> Testers { get; set; }

        public List<IEvento> ListaEventos { get; set; }
        public List<ProcederDiagnostico> ListaEsperaProcederDiagnostico { get; set; }
        public List<ReceberEquipamento> ListaEsperaReceberEquipamento { get; set; }
        public List<FazPedidoRecebeMaterial> ListaEsperaFazPedidoRecebeMaterial { get; set; }
        public List<ProcederReparacao> ListaEsperaProcederReparacao { get; set; }
        public List<ExecutaTeste> ListaEsperaExecutaTeste { get; set; }
        public List<EmbalarEquipamentoParaEnvio> ListaEsperaEmbalarEquipamentoParaEnvio { get; set; }

        //2. Se(server_status == BUSY)
        ////Se o servidor está ocupado cliente fica em fila de espera 2.1 Colocar cliente na Fila de Espera
        //2.2 Registar o tempo de chegada desse cliente
        //2.3 Incrementar o número de clientes em fila de espera
        //3. Senão
        ////Se o servidor está livre começa o atendimento ao cliente
        //3.1 Atraso do cliente = 0
        //3.2 Incrementar número de clientes com atraso contabilizado
        //3.3 Escalonar Evento de Fim de Atendimento(com base tempo de atendimento)

        public Simulador(float? duracaoSim,
                        int numEquipamentosSim,
                        float aceitacaoOrcamentos,
                        float taxaReclamacao,
                        float taxaUrgencia,
                        IList<Tecnico> tecnicos,
                        IList<ColaboradorArmazem> colaboradoresArmazem,
                        IList<ColaboradorLogistica> colaboradoresLogistica,
                        IList<Equipamento> equipamentos,
                        IList<Tester> testers)
        {
            DuracaoSim = duracaoSim;
            NumEquipamentosSim = numEquipamentosSim;
            AceitacaoOrcamentos = aceitacaoOrcamentos;
            TaxaReclamacao = taxaReclamacao;
            TaxaUrgencia = taxaUrgencia;
            Contador = new ContadorEstatistico();
            Tecnicos = tecnicos;
            ColaboradoresArmazem = colaboradoresArmazem;
            ColaboradoresLogistica = colaboradoresLogistica;
            Equipamentos = equipamentos;
            Testers = testers;
            ListaEventos = new List<IEvento>();

            ListaEsperaReceberEquipamento = new List<ReceberEquipamento>();
            ListaEsperaProcederDiagnostico = new List<ProcederDiagnostico>();
            ListaEsperaEmbalarEquipamentoParaEnvio = new List<EmbalarEquipamentoParaEnvio>();
            ListaEsperaExecutaTeste = new List<ExecutaTeste>();
            ListaEsperaFazPedidoRecebeMaterial = new List<FazPedidoRecebeMaterial>();
            ListaEsperaProcederReparacao = new List<ProcederReparacao>();


        }


    }
}
