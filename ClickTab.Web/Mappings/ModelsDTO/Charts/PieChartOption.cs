using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClickTab.Web.Mappings.ModelsDTO.Charts
{
    /// <summary>
    /// Classe per la configurazione di grafici a torta
    /// </summary>
    public class PieChartOption
    {
        /// <summary>
        /// Proprietà per la configurazione del tooltip del grafico (di default viene mostrato all'over sullo spicchio della torta)
        /// </summary>
        public PieTooltipConfig tooltip { get; set; }

        /// <summary>
        /// Proprietà per la configurazione della legenda del grafico (di default viene mostrata in alto a sinistra)
        /// </summary>
        public PieLegendData legend { get; set; }

        /// <summary>
        /// Proprietà per la definizione delle serie del grafico.
        /// Per ciascuna serie è possibile definire il nome e i suoi valori
        /// </summary>
        public List<PieSeries> series { get; set; }

        /// <summary>
        /// Se TRUE allora il grafico viene mostrato a ciambella
        /// </summary>
        public bool isDoughnut { get; set; }

        /// <summary>
        /// Genera un oggetto PieChartOption con la configurazione per il grafico a torta.
        /// Il parametro seriesData deve contenere tanti nodi quanti sono le serie diverse da dover mostrare nel chart (ogni serie 
        /// mostrerà una torta).
        /// Per ciascuna serie è necessario indicare una lista di elementi di tipo PieSeriesValue con indicati il nome e il valore
        /// di ogni elemento della serie (il singolo elemento della serie comporrà lo spicchio della torta)
        /// </summary>
        /// <param name="seriesData"></param>
        /// <returns></returns>
        public static PieChartOption CreatePieChartModel(Dictionary<string, List<PieSeriesValue>> seriesData, bool isDoughnut = false)
        {
            PieChartOption result = new PieChartOption()
            {
                legend = new PieLegendData(),
                tooltip = new PieTooltipConfig(),
                series = new List<PieSeries>(),
                isDoughnut = isDoughnut
            };

            if (seriesData == null)
                seriesData = new Dictionary<string, List<PieSeriesValue>>();

            foreach (string seriesName in seriesData.Keys)
            {
                PieSeries pieSeries = new PieSeries()
                {
                    name = seriesName,
                    data = seriesData[seriesName].ToArray()
                };
                result.series.Add(pieSeries);
            }

            return result;
        }
    }

    /// <summary>
    /// Classe per la configurazione delle diverse serie da mostrare nel grafico a torta
    /// </summary>
    public class PieSeries
    {
        /// <summary>
        /// Nome della singola serie
        /// </summary>
        public string name { get; set; }

        public string type { get { return "pie"; } }

        /// <summary>
        /// Permette di ridefinire la percentuale di ridimensionamento (default 50%)
        /// </summary>
        public string radius { get; set; } = "50%";

        /// <summary>
        /// Permette di definire il posizionamento del chart.
        /// Di default 50% significa che posiziona il chart al centro del canvas, in caso di più serie di grafici permette di posizionarli orizzontalmente
        /// </summary>
        public object center { get; set; } = "50%";

        public PieSeriesValue[] data { get; set; }
    }

    public class PieSeriesValue
    {
        public string name { get; set; }
        public decimal value { get; set; }
        public PieSeriesValueColor itemStyle { get; set; } = null;
    }

    public class PieSeriesValueColor
    {
        public string color { get; set; } = null;
    }

    /// <summary>
    /// Classe per la configurazione del trigger sul tooltip
    /// </summary>
    public class PieTooltipConfig
    {
        public string trigger { get; set; } = "item";
    }

    /// <summary>
    /// Permette di configurare posizione e orientamento della legenda del chart
    /// </summary>
    public class PieLegendData
    {
        /// <summary>
        /// Orientamento della legenda (default: vertical)
        /// </summary>
        public string orient { get; set; } = "vertical";

        /// <summary>
        /// Allineamento della legenda (default: left)
        /// </summary>
        public string left { get; set; } = "left";
    }
}
