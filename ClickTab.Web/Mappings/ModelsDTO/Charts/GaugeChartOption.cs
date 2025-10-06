using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClickTab.Web.Mappings.ModelsDTO.Charts
{
    public class GaugeChartOption
    {
        /// <summary>
        /// Permette di configurare il tooltip per il gauge.
        /// Al momento permette la configurazione solo del formatter del tooltip(valore di default "{a} <br/>{b} : {c}")
        /// </summary>
        public GaugeTooltip tooltip { get; set; }

        /// <summary>
        /// Permette di configurare le diverse serie da mostrare nel chart gauge.
        /// Ogni serie coinciderà con un indicatore nel chart.
        /// </summary>
        public List<GaugeChartSeries> series { get; set; }

        /// <summary>
        /// A partire dai dati minimi per il chart, passati come parametri, restituisce un oggetto BarChartOption configurato e pronto 
        /// per essere mostrato con echarts.
        /// </summary>
        /// <param name="seriesData">Dizionario contenente la configurazione delle serie di valori da mostrare per il gauge chart</param>
        /// <returns>Restituice un oggetto di tipo BarChartOption pronto per essere visualizzato con echarts</returns>
        public static GaugeChartOption CreateGaugeChartModel(Dictionary<string, List<GaugeChartData>> seriesData)
        {
            if (seriesData == null)
                seriesData = new Dictionary<string, List<GaugeChartData>>();

            GaugeChartOption gaugeChart = new GaugeChartOption()
            {
                tooltip = new GaugeTooltip("{a} <br/>{b} : {c}"),
                series = new List<GaugeChartSeries>()
            };

            foreach (string seriesName in seriesData.Keys)
            {
                GaugeChartSeries gaugeSeries = new GaugeChartSeries()
                {
                    name = seriesName,
                    detail = new GaugeChartDetailData("{value}"),
                    data = seriesData[seriesName]
                };
                gaugeChart.series.Add(gaugeSeries);
            }

            return gaugeChart;
        }
    }

    /// <summary>
    /// Configura la serie da mostrare nel gauge
    /// </summary>
    public class GaugeChartSeries
    {
        /// <summary>
        /// Nome della serie
        /// </summary>
        public string name { get; set; }

        /// <summary>
        /// Type, definito di default nel costruttore di GaugeChartOption col valore 'gauge'
        /// </summary>
        public string type { get { return "gauge"; } }

        /// <summary>
        /// Permette di definire la configurazione dell'etichetta da mostrare nel gauge
        /// </summary>
        public GaugeChartDetailData detail { get; set; }

        /// <summary>
        /// Permette di definire i dati delle serie da mostare nel chart
        /// </summary>
        public List<GaugeChartData> data { get; set; }
    }

    /// <summary>
    /// Classe per la configurazione del valore del chart
    /// </summary>
    public class GaugeChartData
    {
        /// <summary>
        /// Valore da mostrare nel gauge
        /// </summary>
        public decimal value { get; set; }

        /// <summary>
        /// Etichetta da attribuire al valore
        /// </summary>
        public string name { get; set; }
    }

    /// <summary>
    /// Classe per configurare il formatter del grafico gauge
    /// </summary>
    public class GaugeChartDetailData
    {
        public GaugeChartDetailData(string formatter = null)
        {
            if (!string.IsNullOrEmpty(formatter))
                this.formatter = formatter;
        }

        /// <summary>
        ///  Permette di definire il tooltip da mostrare quando si passa sull'indicatore del gauge
        /// </summary>
        public string formatter { get; set; }
    }

    public class GaugeTooltip
    {
        public GaugeTooltip(string formatter = null)
        {
            if (!string.IsNullOrEmpty(formatter))
                this.formatter = formatter;
        }

        public string formatter { get; set; } = "{a} <br/>{b} : {c}";
    }
}
