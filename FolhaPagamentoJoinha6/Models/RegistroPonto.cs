using System;

namespace FolhaPagamentoJoinha6.Models
{
    public class RegistroPonto
    {
        private List<RegistroPontoDiario> listaRegistroDiario { get; set; }
        private int qtdHorasNecessariaSemana { get; set; }
        private TimeSpan qtdHorastrabalhadaSemana { get; set; }
        private int qtdDiasTrabalhados;
        private TimeSpan qtdHorasExtras;

        //metodo construtor
        public RegistroPonto(int qtdHorasNessSem, int qtdDiasTrab)
        {
            // qtdHNessSem = 40; //ex 40 horas
            qtdHorasNecessariaSemana = qtdHorasNessSem;

            // ex: dias uteis de seg a sex
            qtdDiasTrabalhados = qtdDiasTrab; //ex 20 dias;
            listaRegistroDiario = new List<RegistroPontoDiario>();
        }

        public static TimeSpan GerarRegistroPontoMesInteiroAutomatico(int qtdHorasNecesSem, int qtdDiasTrab)
        {
            qtdDiasTrab = 20;
            qtdHorasNecesSem = 40;

            RegistroPonto registroPonto = new RegistroPonto(qtdHorasNecesSem, qtdDiasTrab);

            //Loop que cria e calcula todas as entradas e saidas do mes todo
            registroPonto.listaRegistroDiario = registroPonto.CriaEntradaESaidaMesTodo();

            //loop que calcula as horas trabalhadas do metodo anterior e verifica as horas extras
            TimeSpan horasTrabalhadas = registroPonto.CalculaHorasTrabalhadas(registroPonto.listaRegistroDiario, qtdHorasNecesSem);

            return registroPonto.qtdHorasExtras;
        }

        public static List<RegistroPontoDiario> GerarListaRegistroPontoMesInteiroAutomatico(int qtdHorasNecesSem, int qtdDiasTrab, out string horasExtrasString)
        {
            RegistroPonto registroPonto = new RegistroPonto(qtdHorasNecesSem, qtdDiasTrab);
            registroPonto.listaRegistroDiario = registroPonto.CriaEntradaESaidaMesTodo();
            TimeSpan horasExtras = registroPonto.CalculaHorasTrabalhadas(registroPonto.listaRegistroDiario, qtdHorasNecesSem);
            horasExtrasString = "";

            if (horasExtras.TotalMinutes < 0)
            {
                horasExtrasString = horasExtras.ToString(@"hh\:mm");
                horasExtrasString = "-" + horasExtrasString;
            }

            else
            {
                horasExtrasString = horasExtras.ToString(@"hh\:mm");
            }

            return registroPonto.listaRegistroDiario;
        }

        private List<RegistroPontoDiario> CriaEntradaESaidaMesTodo()
        {
            List<RegistroPontoDiario> listaRegistroDiario = new List<RegistroPontoDiario>();
            

            for (int a = 0; a < qtdDiasTrabalhados; a++)
            {
                RegistroPontoDiario regPDiario = new RegistroPontoDiario();
                regPDiario.entrada = new TimeSpan(7, 0, 0);

                regPDiario.saidaAlmoco = new TimeSpan(12, 0, 0);
                regPDiario.retornoAlmoco = new TimeSpan(13, 0, 0);
                regPDiario.saida = new TimeSpan(16, 0, 0);

                regPDiario.entrada = regPDiario.entrada.Add(CalculaValorAleatorio());
                regPDiario.saida = regPDiario.saida.Add(CalculaValorAleatorio());

                listaRegistroDiario.Add(regPDiario);
            }

            return listaRegistroDiario;
        }

        private TimeSpan CalculaValorAleatorio()
        {
            Random random = new Random();
            int minutosAleatorio = random.Next(-5, 5);
            return TimeSpan.FromMinutes(minutosAleatorio);
        }

        private TimeSpan CalculaHorasTrabalhadas(List<RegistroPontoDiario> listaPontoDiario, int qtdHorasSemanNeces)
        {
            TimeSpan horasTrabalhadas = TimeSpan.Zero;

            foreach (RegistroPontoDiario PontoDiario in listaPontoDiario)
            {
                horasTrabalhadas += PontoDiario.saidaAlmoco - PontoDiario.entrada + (PontoDiario.saida - PontoDiario.retornoAlmoco);
            }
            qtdHorasExtras = horasTrabalhadas - new TimeSpan(qtdHorasSemanNeces * 4, 0, 0);
            return qtdHorasExtras;
        }
    }
}
