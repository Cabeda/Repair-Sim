using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RepairSim.Models.SimModels
{
    /// <summary>
    /// Este modelo armazena o estado da simulação e respetivas variáveis
    /// </summary>
    public class ContadorEstatistico
    {
#region Constructor
        public ContadorEstatistico()
        {
            //Time of simulation is set in the statistical counter
            TempoAtual = 0;
            TempoUltimoEvento = 0;
            DuracaoDesdeUltimoEvento = 0;
            NumEquipamentos = 0;
            TempoEsperaDiagnosticoTotal = 0;
            NumEsperaDiagnostico = 0;
            TempoAguardaColaboradorArmazemTotal = 0;
            NumAguardaColaboradorArmazem = 0;
            TempoAguardaTecnicoTotal = 0;
            NumAguardaTecnico = 0;
            TempoAguardaTestesTotal = 0;
            NumAguardaTestes = 0;
            TempoAguardaEmbalarTotal = 0;
            NumAguardaEmbalar = 0;
            TempoEsperaReceberEquipamentoTotal = 0;
            NumReceberEquipamento = 0;
        }
#endregion
        /// <summary>
        /// 
        /// </summary>
        /// 
        public void ActualizacaoEstatistica(float duracao)
        {
            DuracaoDesdeUltimoEvento = TempoAtual - TempoUltimoEvento;
            TempoUltimoEvento = TempoAtual;
            TempoAtual += duracao;
        }



        //Contadores da Simulação globais
        [Display(Name = "Tempo de Execução")]
        [DisplayFormat(DataFormatString = "{0:N}", ApplyFormatInEditMode = true)]
        public TimeSpan TimeSpan { get; set; }

        [Display(Name = "Tempo Simulado")]
        [DisplayFormat(DataFormatString = "{0:N}", ApplyFormatInEditMode = true)]
        public float TempoAtual { get; set; }

        [DisplayFormat(DataFormatString = "{0:N}", ApplyFormatInEditMode = true)]
        public float TempoUltimoEvento { get; set; }

        [DisplayFormat(DataFormatString = "{0:N}", ApplyFormatInEditMode = true)]
        public float DuracaoDesdeUltimoEvento { get; set; }

        [Display(Name = "Número de Equipamentos  Simulados")]
        public float NumEquipamentos { get; set; }


        [DisplayFormat(DataFormatString = "{0:N}", ApplyFormatInEditMode = true)]
        public float TempoTotalEspera

        {
            get
            {
                var tempoEspera = TempoEsperaReceberEquipamentoTotal + TempoEsperaDiagnosticoTotal +
                                  TempoAguardaColaboradorArmazemTotal + TempoAguardaTecnicoTotal +
                                  TempoAguardaTestesTotal + TempoAguardaEmbalarTotal;
                return tempoEspera;
            }
        }

        [Display(Name = "Tempo médio das filas")]
        [DisplayFormat(DataFormatString = "{0:N}", ApplyFormatInEditMode = true)]
        public float TempoMediaEspera
        {
            get
            {
                var tempomedio = (TempoMedioFilaAguardaColaboradorArmazem +
                        TempoMedioFilaAguardaEmbalar +
                        TempoMedioFilaAguardaTecnico +
                        TempoMedioFilaAguardaTestes +
                        TempoMedioFilaEsperarDiagnostico +
                        TempoMedioFilaReceberEquipamento) / 6;
                return tempomedio;
            }
        }



#region Taxas Ocupacao
        [DisplayFormat(DataFormatString = "{0:P2}", ApplyFormatInEditMode = true)]
        [Display(Name = "Taxa de Ocupação de técnicos")]
        public float TaxaOcupacaoTecnico
        {
            get
            {
                return (TempoProcederDiagnostico + TempoReparacao) / TempoAtual;
            }
        }


        [DisplayFormat(DataFormatString = "{0:P2}", ApplyFormatInEditMode = true)]
        [Display(Name = "Taxa de Ocupação de Colaboradores de Logística")]
        public float TaxaOcupacaoColaboradorLogistica
        {
            get
            {
                return (TempoEmbalarEquipamentoParaEnvio + TempoReceberEquipamento) / TempoAtual;
            }
        }


        [DisplayFormat(DataFormatString = "{0:P2}", ApplyFormatInEditMode = true)]
        [Display(Name = "Taxa de Ocupação de Colaboradores de Armazém")]
        public float TaxaOcupacaoColaboradorArmazem
        {
            get
            {
                return TempoFazPedidoRecebeMaterial/TempoAtual;
            }
        }


        [DisplayFormat(DataFormatString = "{0:P2}", ApplyFormatInEditMode = true)]
        [Display(Name = "Taxa de Ocupação de testers")]
        public float TaxaOcupacaoTester
        {
            get
            {
                return TempoExecutaTestes /TempoAtual;
            }
        }



        [DisplayFormat(DataFormatString = "{0:P2}", ApplyFormatInEditMode = true)]

        [Display(Name = "Taxa de Ocupação Total")]
        public float TaxaOcupacao
        {
            get
            {
                return (TaxaOcupacaoTecnico + TaxaOcupacaoColaboradorArmazem + TaxaOcupacaoColaboradorLogistica + TaxaOcupacaoTester) / 4;
            }
        }
        #endregion

#region Tempos Medios por Atividade
        [DisplayFormat(DataFormatString = "{0:N}", ApplyFormatInEditMode = true)]
        [Display(Name = "Tempo de Reparação médio")]
        public float TempoReparacaoMedio
        {
            get
            {
                
                return (TempoReparacao==0 || NumAguardaTecnico ==0) ? 0 : TempoReparacao / NumAguardaTecnico;
            }
        }

        [DisplayFormat(DataFormatString = "{0:N}", ApplyFormatInEditMode = true)]
        [Display(Name = "Tempo médio de receção de  equipamento")]
        public float TempoMedioReceberEquipamento
        {
            get
            {
                return TempoReceberEquipamento / NumReceberEquipamento;
            }
        }

        [DisplayFormat(DataFormatString = "{0:N}", ApplyFormatInEditMode = true)]
        [Display(Name = "Tempo médio de Testes")]
        public float TempoMedioTestes
        {
            get
            {
                return TempoExecutaTestes / NumAguardaTestes;
            }
        }

        [DisplayFormat(DataFormatString = "{0:N}", ApplyFormatInEditMode = true)]
        [Display(Name = "Tempo médio de Armazem")]
        public float TempoMedioArmazem
        {
            get
            {
                return TempoFazPedidoRecebeMaterial / NumAguardaColaboradorArmazem;
            }
        }
#endregion

        //Contadores da simulação por evento
        [DisplayFormat(DataFormatString = "{0:N}", ApplyFormatInEditMode = true)]
        public float TempoEsperaReceberEquipamentoTotal { get; set; }

        [DisplayFormat(DataFormatString = "{0:N}", ApplyFormatInEditMode = true)]
        public float NumReceberEquipamento { get; set; }

        [DisplayFormat(DataFormatString = "{0:N}", ApplyFormatInEditMode = true)]
        public float TempoReceberEquipamento { get; set; }

  

        [DisplayFormat(DataFormatString = "{0:N}", ApplyFormatInEditMode = true)]
        [Display(Name = "Tempo médio à espera de receber equipamento")]
        public float TempoMedioFilaReceberEquipamento
        {
            get
            {
                return TempoEsperaReceberEquipamentoTotal / NumReceberEquipamento;
            }
        }
        [DisplayFormat(DataFormatString = "{0:N}", ApplyFormatInEditMode = true)]
        public float TempoEsperaDiagnosticoTotal { get; set; }

        [DisplayFormat(DataFormatString = "{0:N}", ApplyFormatInEditMode = true)]
        public float NumEsperaDiagnostico { get; set; }

        [DisplayFormat(DataFormatString = "{0:N}", ApplyFormatInEditMode = true)]
        public float TempoProcederDiagnostico { get; set; }

        [DisplayFormat(DataFormatString = "{0:N}", ApplyFormatInEditMode = true)]
        [Display(Name = "Tempo médio à espera de diagnóstico")]
        public float TempoMedioFilaEsperarDiagnostico {
            get
            {
                return (TempoEsperaDiagnosticoTotal== 0 || NumEsperaDiagnostico ==0) ? 0 : TempoEsperaDiagnosticoTotal / NumEsperaDiagnostico;
            }
        }

        [DisplayFormat(DataFormatString = "{0:N}", ApplyFormatInEditMode = true)]
        public float TempoAguardaColaboradorArmazemTotal { get; set; }

        [DisplayFormat(DataFormatString = "{0:N}", ApplyFormatInEditMode = true)]
        public float NumAguardaColaboradorArmazem { get; set; }

        [DisplayFormat(DataFormatString = "{0:N}", ApplyFormatInEditMode = true)]
        public float TempoFazPedidoRecebeMaterial { get; set; }

        [DisplayFormat(DataFormatString = "{0:N}", ApplyFormatInEditMode = true)]
        [Display(Name = "Tempo médio à espera de Colaborador de Armazém")]
        public float TempoMedioFilaAguardaColaboradorArmazem {
            get
            {
                return (TempoAguardaColaboradorArmazemTotal == 0 || NumAguardaColaboradorArmazem == 0) ? 0 : TempoAguardaColaboradorArmazemTotal / NumAguardaColaboradorArmazem;
            }
        }

        [DisplayFormat(DataFormatString = "{0:N}", ApplyFormatInEditMode = true)]
        public float TempoAguardaTecnicoTotal { get; set; }

        [DisplayFormat(DataFormatString = "{0:N}", ApplyFormatInEditMode = true)]
        public float NumAguardaTecnico { get; set; }

        [DisplayFormat(DataFormatString = "{0:N}", ApplyFormatInEditMode = true)]
        public float TempoReparacao { get; set; }

        [DisplayFormat(DataFormatString = "{0:N}", ApplyFormatInEditMode = true)]
        [Display(Name = "Tempo médio à espera de técnico")]
        public float TempoMedioFilaAguardaTecnico {
            get
            {
                return (TempoAguardaTecnicoTotal == 0 || NumAguardaTecnico == 0) ? 0 : TempoAguardaTecnicoTotal / NumAguardaTecnico;
            }
        }

        [DisplayFormat(DataFormatString = "{0:N}", ApplyFormatInEditMode = true)]
        public float TempoAguardaTestesTotal { get; set; }

        public float NumAguardaTestes { get; set; }
        [DisplayFormat(DataFormatString = "{0:N}", ApplyFormatInEditMode = true)]
        public float TempoExecutaTestes { get; set; }

        [DisplayFormat(DataFormatString = "{0:N}", ApplyFormatInEditMode = true)]
        [Display(Name = "Tempo médio à espera de efetuar testes")]
        public float TempoMedioFilaAguardaTestes {
            get
            {
                return (TempoAguardaTestesTotal == 0 || NumAguardaTestes == 0) ? 0 : TempoAguardaTestesTotal / NumAguardaTestes;
            }
        }

        [DisplayFormat(DataFormatString = "{0:N}", ApplyFormatInEditMode = true)]
        public float TempoAguardaEmbalarTotal { get; set; }

        public float NumAguardaEmbalar { get; set; }

        [DisplayFormat(DataFormatString = "{0:N}", ApplyFormatInEditMode = true)]
        public float TempoEmbalarEquipamentoParaEnvio { get; set; }

        [DisplayFormat(DataFormatString = "{0:N}", ApplyFormatInEditMode = true)]
        [Display(Name = "Tempo médio fila Aguarda Embalar")]
        public float TempoMedioFilaAguardaEmbalar {
            get
            {
                return (TempoAguardaEmbalarTotal == 0 || NumAguardaEmbalar == 0) ? 0 : TempoAguardaEmbalarTotal / NumAguardaEmbalar;
            }
        }


#region Incrementadores

        public void IncrementaReceberEquipamento(float TempoEntrada, float TempoEntradaEmFilaEspera, float duracao, int NequipamentoFilaEspera)
        {
            //Incrementa número total de equipamentos na simulação
            NumEquipamentos++;
            NumReceberEquipamento++;
            TempoReceberEquipamento += duracao;
            //Total da duracao a receber + tempo na fila
            TempoEsperaReceberEquipamentoTotal +=  (TempoEntrada-TempoEntradaEmFilaEspera);
        }

        public void IncrementaProcederDiagnostico(float TempoEntrada, float TempoEntradaEmFilaEspera, float duracao, int NequipamentoFilaEspera)
        {
            NumEsperaDiagnostico++;
            TempoProcederDiagnostico += duracao;
            TempoEsperaDiagnosticoTotal += (TempoEntrada-TempoEntradaEmFilaEspera);
           
        }

        public void IncrementaFazPedidoRecebeMaterial(float TempoEntrada, float TempoEntradaEmFilaEspera, float duracao, int NequipamentoFilaEspera)
        {
            NumAguardaColaboradorArmazem++;
            TempoFazPedidoRecebeMaterial += duracao;
            TempoAguardaColaboradorArmazemTotal +=  (TempoEntrada-TempoEntradaEmFilaEspera);
        }

        public void IncrementaProcederReparacao(float TempoEntrada, float TempoEntradaEmFilaEspera, float Duracao, int NequipamentoFilaEspera)
        {
            NumAguardaTecnico++;
            TempoReparacao += Duracao;
            TempoAguardaTecnicoTotal +=  (TempoEntrada-TempoEntradaEmFilaEspera);

        }

        public void IncrementaExecutaTestes(float TempoEntrada, float TempoEntradaEmFilaEspera, float duracao, int NequipamentoFilaEspera)
        {
            NumAguardaTestes++;
            TempoExecutaTestes += duracao;
            TempoAguardaTestesTotal +=  (TempoEntrada - TempoEntradaEmFilaEspera);
        }

        public void IncrementaEmbalarEquipamentoParaEnvio(float TempoEntrada, float TempoEntradaEmFilaEspera, float duracao, int NequipamentoFilaEspera)
        {
            NumAguardaEmbalar++;
            TempoEmbalarEquipamentoParaEnvio += duracao;
            TempoAguardaEmbalarTotal +=  (TempoEntrada-TempoEntradaEmFilaEspera);
        }

#endregion


    }

}
