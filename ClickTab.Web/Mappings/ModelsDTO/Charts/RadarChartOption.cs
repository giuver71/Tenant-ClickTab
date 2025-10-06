using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClickTab.Web.Mappings.ModelsDTO.Charts
{
    /// <summary>
    /// Modello per la configurazione di grafici di tipo RADAR
    /// </summary>
    public class RadarChartOption
    {
        /// <summary>
        /// Proprietà che permette la configurazione del tooltip.
        /// </summary>
        public RadarTooltip tooltip { get; set; }

        /// <summary>
        /// Permette la configurazione della legenda del chart, definendo le etichette e la sua posizione
        /// </summary>
        public RadarLegendData legend { get; set; }

        /// <summary>
        /// Permette la configurazione di diversi radar chart, 
        /// per ciascun chart vanno indicati: elenco di indicatori (marker) del chart ed eventuale
        /// dimensione percentuale (di default = 75%)
        /// </summary>
        public List<RadarIndicator> radar { get; set; }

        /// <summary>
        /// Permette la configurazione delle serie da mostrare nei diversi chart definiti nella proprietà
        /// radar.
        /// Per ciascuna serie verranno richiesti: nome, tipo (default = radar), configurazione del tooltip, dati della serie
        /// e lo stile da applicare all'area del chart (di default trasparente)
        /// </summary>
        public List<RadarSeries> series { get; set; }

        /// <summary>
        /// A partire dai dati minimi per il chart, passati come parametri, restituisce un oggetto RadarChartOption configurato e pronto
        /// per essere mostrato con echarts.
        /// </summary>
        /// <param name="legendLabels">Elenco delle etichette da usare come legenda del chart. Se il char contiene un solo radar allora l'array dovrà contenere un solo elemento altrimenti ne conterrà uno per ogni radar diverso</param>
        /// <param name="radars">Dizionario contenente le configurazioni di tutti i radar da mostrare nel chart. Ogni nodo del dizionario dovrà contenere come chiave il nome da attribuire al radar e la configurazione degli indicatori del radar</param>
        /// <param name="seriesData">Dizionario contenente la configurazione delle serie di valori da mostrare per ogni radar configurato</param>
        /// <param name="radiusChartPercentage">Indica la percentuale di ridimensionamento dei radar (valore di default = 75%)</param>
        /// <param name="showTooltip">Se TRUE allora configura i tooltip per il chart (nel tooltip verranno mostrati i valori della serie del radar su cui si passa con il mouse)</param>
        /// <param name="fillChartArea">Se TRUE allora per ogni radar applica uno stile che permetterà di colorare l'intera area disegnata nel radar</param>
        /// <returns>Restituice un oggetto di tipo RadarChartOption pronto per essere visualizzato con echarts</returns>
        public static RadarChartOption CreateRadarChartModel(string[] legendLabels, Dictionary<string, List<RadarIndicatorData>> radars, Dictionary<string, List<RadarSeriesData>> seriesData, string radiusChartPercentage = "75%", bool showTooltip = true, bool fillChartArea = true)
        {
            if (legendLabels == null)
                legendLabels = new string[] { };

            if (radars == null)
                radars = new Dictionary<string, List<RadarIndicatorData>>();

            if (seriesData == null)
                seriesData = new Dictionary<string, List<RadarSeriesData>>();

            RadarChartOption result = new RadarChartOption()
            {
                tooltip = showTooltip == true ? new RadarTooltip() { trigger = "axis" } : null,
                legend = new RadarLegendData() { data = legendLabels },
                series = new List<RadarSeries>(),
                radar = new List<RadarIndicator>()
            };

            foreach (string radarName in radars.Keys)
            {
                result.radar.Add(new RadarIndicator()
                {
                    indicator = radars[radarName],
                    radius = radiusChartPercentage
                });
            }

            foreach (string seriesName in seriesData.Keys)
            {
                result.series.Add(new RadarSeries()
                {
                    tooltip = showTooltip == true ? new RadarTooltip() { trigger = "item" } : null,
                    name = seriesName,
                    data = seriesData[seriesName],
                    areaStyle = fillChartArea == true ? new { } : null
                });
            }

            return result;
        }
    }

    /// <summary>
    /// Classe per la configurazione della legenda del chart
    /// </summary>
    public class RadarLegendData
    {
        /// <summary>
        /// Posizione legenda (default = center)
        /// </summary>
        public string left { get; set; } = "center";

        /// <summary>
        /// Etichette della legenda
        /// </summary>
        public string[] data { get; set; }
    }

    /// <summary>
    /// Classe per la configurazione degli indicatori (marker) per un singolo radar
    /// </summary>
    public class RadarIndicator
    {
        /// <summary>
        /// Elenco degli indicatori (cioè i vertici del radar). PEr ciascun indicatore andranno valorizzati il nome e il valore massimo
        /// </summary>
        public List<RadarIndicatorData> indicator { get; set; }

        /// <summary>
        /// Percentuale di ridimensionamento del radar (default = 75%)
        /// </summary>
        public string radius { get; set; } = "75%";
    }

    /// <summary>
    /// Classe per la configurazione del singolo indicatore del chart
    /// </summary>
    public class RadarIndicatorData
    {
        /// <summary>
        /// Nome da mostrare per l'indicatore (verrà visualizzato nel vertice del radar)
        /// </summary>
        public string text { get; set; }

        /// <summary>
        /// Valore massimo da usare come riferimento per l'indicatore
        /// </summary>
        public decimal max { get; set; }
    }

    /// <summary>
    /// Classe per la configurazione dei valori di una singola serie del chart
    /// </summary>
    public class RadarSeriesData
    {
        /// <summary>
        /// Valori da mostrare per la serie.
        /// Il numero di valori indicati deve essere lo stesso degli indicatori configurati, in modo
        /// la posizione di ogni valore della serie venga rappresentata nell'indicatore avente la stessa posizione dentro
        /// la proprietà indicator del radar
        /// </summary>
        public decimal[] value { get; set; }

        /// <summary>
        /// Nome da attribuire alla serie del radar
        /// </summary>
        public string name { get; set; }
    }

    /// <summary>
    /// Classe per la configurazione di tutte le serie del chart
    /// </summary>
    public class RadarSeries
    {
        public string name { get; set; }
        public object areaStyle { get; set; }
        public string type { get { return "radar"; } }
        public RadarTooltip tooltip { get; set; }
        public List<RadarSeriesData> data { get; set; }
    }

    public class RadarTooltip
    {
        public string trigger { get; set; }
    }
}
