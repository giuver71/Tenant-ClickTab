using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClickTab.Web.Mappings.ModelsDTO.Charts
{
    /// <summary>
    /// Classe per la configurazione dei Chart di tipo Bar
    /// </summary>
    public class BarChartOption
    {
        /// <summary>
        /// Proprietà per definire la configurazione dell'asse X del grafico
        /// </summary>
        public XAxisConfig xAxis { get; set; }

        /// <summary>
        /// Proprietà per definire la configurazione dell'asse Y del grafico
        /// </summary>
        public YAxisConfig yAxis { get; set; }

        /// <summary>
        /// Proprietà per definire le diverse serie di dati da mostrare nel grafico
        /// </summary>
        public List<LineSerie> series { get; set; } = new List<LineSerie>();

        /// <summary>
        /// Proprietà per la configurazione della legenda del chart
        /// </summary>
        public LineLegend legend { get; set; }

        /// <summary>
        /// Permette di configurare il tooltip del chart.
        /// Di default è istanziato con la proprietà trigger = "axis"
        /// in modo da mostrare sempre i tooltip quando si passa col mouse sul chart
        /// </summary>
        public LineTooltip tooltip { get; set; }

        /// <summary>
        /// Permette di definire i colori da usare per ciascuna serie del chart.
        /// L'array è posizionale quindi il primo colore verrà attribuito alla prima serie, il secondo colore alla seconda serie etc...
        /// </summary>
        public string[] seriesColors { get; set; }

        /// <summary>
        /// A partire dai dati minimi per il chart, passati come parametri, restituisce un oggetto BarChartOption configurato e pronto
        /// per essere mostrato con echarts.
        /// </summary>
        /// <param name="labels">Array di stringhe da utilizzare come etichette per l'asse X e come legenda del chart</param>
        /// <param name="seriesData">Dizionario contenente la configurazione delle serie di valori da mostrare per il bar chart</param>
        /// <param name="showLegend">Se TRUE allora imposta la visibilità della legenda sull'asse x altrimenti la nasconde</param>
        /// <param name="seriesColors">Permette di definire l'array di colori da usare per ciascuna serie del chart. L'array è posizionale quindi il primo colore verrà attribuito alla prima serie, il secondo colore alla seconda serie etc...</param>
        /// <returns>Restituice un oggetto di tipo BarChartOption pronto per essere visualizzato con echarts</returns>
        public static BarChartOption CreateBarChartModel(string[] labels, Dictionary<string, decimal[]> seriesData, bool showLegend = true, string[] seriesColors = null)
        {
            BarChartOption result = new BarChartOption()
            {
                xAxis = new XAxisConfig(),
                yAxis = new YAxisConfig(),
                series = new List<LineSerie>(),
                tooltip = new LineTooltip() { trigger = "axis" },
                seriesColors = seriesColors
            };

            if (labels != null && showLegend == true)
                result.legend = new LineLegend() { data = labels };

            result.xAxis.data = labels;

            if (seriesData == null)
            {
                seriesData = new Dictionary<string, decimal[]>();
                seriesData.Add("", new decimal[] { });
            }

            foreach (string seriesName in seriesData.Keys)
            {
                result.series.Add(new LineSerie()
                {
                    name = seriesName,
                    data = seriesData[seriesName],
                    type = "bar",
                    backgroundStyle = new LineSerieBackground() { color = "rgba(180, 180, 180, 0.2)" },
                    showBackground = true
                });
            }

            return result;
        }
    }
}
