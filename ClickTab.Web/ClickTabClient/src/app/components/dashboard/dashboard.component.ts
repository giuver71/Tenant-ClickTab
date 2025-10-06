import { FilterConfig, InputType, WherePartType, FilterSizeClass, FilterCvlConfig } from '@eqproject/eqp-common';
import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { TestService } from '../../services/test.service';
import { BarChartOption, EqpDashboardService, GaugeChartData, GaugeChartOption, LineChartOption, PieChartOption, PieSeriesValue, RadarChartOption, RadarIndicatorData, RadarSeriesData, WidgetConfig, WidgetSizeEnum, WidgetTypeEnum } from '@eqproject/eqp-dashboard';
import { TestComponent } from '../test-component/test-component';

@Component({
  templateUrl: 'dashboard.component.html',
  styleUrls: ['./dashboard.component.scss']
})
export class DashboardComponent implements OnInit {

  chartConfigs: Array<WidgetConfig> = new Array<WidgetConfig>();
  filters: Array<FilterConfig> = new Array<FilterConfig>();
  testComponent = TestComponent;

  constructor(private http: HttpClient, private testService: TestService, private eqpDashboardService: EqpDashboardService) {
  }

  ngOnInit() {
    this.configureFilters();
    this.configureDashboard();
  }

  //#region Funzioni demo per direttiva eqp-dashboard

  configureDashboard() {

    this.eqpDashboardService.setLocale("it-IT");

    //#region LineChart example

    let lineChartConfig: WidgetConfig = new WidgetConfig();
    lineChartConfig.WidgetID = "Dashboard_LineChart";
    lineChartConfig.WidgetTitle = "Line Chart"
    lineChartConfig.WidgetSizeClass = WidgetSizeEnum.XS;
    lineChartConfig.WidgetType = WidgetTypeEnum.LINE_CHART;
    let lineSeriesData: Map<string, number[]> = new Map<string, number[]>();
    lineSeriesData.set("Serie 1", [10, 25, 35]);
    lineChartConfig.ChartOptions = LineChartOption.CreateLineChartModel(["Valore 1", "Valore 2", "Valore 3"], lineSeriesData);

    //#endregion

    //#region BarChart example

    let barChartConfig: WidgetConfig = new WidgetConfig();
    barChartConfig.WidgetID = "Dashboard_BarChart";
    barChartConfig.WidgetTitle = "Bar Chart"
    barChartConfig.WidgetSizeClass = WidgetSizeEnum.XS;
    barChartConfig.WidgetType = WidgetTypeEnum.BARS_CHART;
    let barSeriesData: Map<string, number[]> = new Map<string, number[]>();
    barSeriesData.set("Serie 1", [10, 25, 35]);
    barChartConfig.ChartOptions = BarChartOption.CreateBarChartModel(["Valore 1", "Valore 2", "Valore 3"], barSeriesData);

    //#endregion

    //#region PieChart example

    let pieChartConfig: WidgetConfig = new WidgetConfig();
    pieChartConfig.WidgetID = "Dashboard_Pie";
    pieChartConfig.WidgetTitle = "Pie Chart"
    pieChartConfig.WidgetSizeClass = WidgetSizeEnum.XS;
    pieChartConfig.WidgetType = WidgetTypeEnum.PIE_CHART;
    let pieSeries: Map<string, PieSeriesValue[]> = new Map<string, PieSeriesValue[]>();
    pieSeries.set("Serie 1", [{ name: "A", value: 10 }, { name: "B", value: 25 }, { name: "C", value: 37 }, { name: "D", value: 42 }]);
    pieChartConfig.ChartOptions = PieChartOption.CreatePieChartModel(pieSeries);

    //#endregion

    //#region RadarChart example

    let radarChartConfig: WidgetConfig = new WidgetConfig();
    radarChartConfig.WidgetID = "Dashboard_Radar";
    radarChartConfig.WidgetTitle = "Radar chart"
    radarChartConfig.WidgetSizeClass = WidgetSizeEnum.XS;
    radarChartConfig.WidgetType = WidgetTypeEnum.RADAR_CHART;

    let radarSeriesName: string = "Serie 1";
    let indicators: Map<string, RadarIndicatorData[]> = new Map<string, RadarIndicatorData[]>();
    indicators.set(radarSeriesName, [{ text: "Valore 1", max: 100 }, { text: "Valore 2", max: 100 }, { text: "Valore 3", max: 100 }, { text: "Valore 4", max: 100 }, { text: "Valore 5", max: 100 }]);

    let seriesData: Map<string, RadarSeriesData[]> = new Map<string, RadarSeriesData[]>();
    seriesData.set(radarSeriesName, [{ name: "Serie 1", value: [10, 25, 35, 40, 80] }]);
    radarChartConfig.ChartOptions = RadarChartOption.CreateRadarChartModel([radarSeriesName], indicators, seriesData);

    //#endregion

    //#region GaugeChart example

    let gaugeChartConfig: WidgetConfig = new WidgetConfig();
    gaugeChartConfig.WidgetID = "Dashboard_Gauge";
    gaugeChartConfig.WidgetTitle = "Gauge chart"
    gaugeChartConfig.WidgetSizeClass = WidgetSizeEnum.XS;
    gaugeChartConfig.WidgetType = WidgetTypeEnum.GAUGE_CHART;

    let gaugeSeries: Map<string, GaugeChartData[]> = new Map<string, GaugeChartData[]>();
    gaugeSeries.set("Valore 1", [{ name: "Km/h", value: 50 }])
    gaugeChartConfig.ChartOptions = GaugeChartOption.CreateGaugeChartModel(gaugeSeries)

    //#endregion

    //#region StatisticWidget example

    let statisticConfig: WidgetConfig = new WidgetConfig();
    statisticConfig.WidgetTitle = "Statistic Widget";
    statisticConfig.WidgetSizeClass = WidgetSizeEnum.XS;
    statisticConfig.WidgetID = 'StatisticWidget_Dashboard';
    statisticConfig.WidgetType = WidgetTypeEnum.STATISTIC_CHART;
    statisticConfig.StatisticWidget = [
      { Icon: "check_circle", Label: "Completati", Value: 70, MaxValue: 100 }
    ]

    //#endregion

    this.chartConfigs = [
      lineChartConfig,
      barChartConfig,
      pieChartConfig,
      radarChartConfig,
      gaugeChartConfig,
      statisticConfig
    ];
  }

  //#endregion

  //#region Funzioni demo per direttiva eqp-filters

  configureFilters() {

    //#region Filtro tipo TEXT

    let textFilter: FilterConfig = FilterConfig.CreateStandardFilterConfig("PROVA_TEXT_ID", "Nome", "Name", InputType.Text, WherePartType.Equal);
    textFilter.PreventRemoval = true;
    this.filters.push(textFilter);

    //#endregion

    //#region Filtro tipo NUMBER

    let numberFilter: FilterConfig = FilterConfig.CreateStandardFilterConfig("PROVA_NUMBER_ID", "Counter", "Filtro Numero", InputType.Number, WherePartType.Equal);
    this.filters.push(numberFilter);

    //#endregion

    //#region Filtro di tipo booleano

    let booleanFilter: FilterConfig = FilterConfig.CreateStandardFilterConfig("PROVA_BOOLEAN", "Filtro Si/No", "IsValid", InputType.Boolean, WherePartType.Equal);
    this.filters.push(booleanFilter);

    //#endregion



    //#region Filtro range di date

    let filterDateStart: FilterConfig = FilterConfig.CreateStandardFilterConfig("PROVA_DATE_START", "Data inizio", "StartDate", InputType.Date, WherePartType.GreaterThanOrEqual, FilterSizeClass.SMALL);
    this.filters.push(filterDateStart);

    let filterDateEnd: FilterConfig = FilterConfig.CreateStandardFilterConfig("PROVA_DATE_END", "Data fine", "EndDate", InputType.Date, WherePartType.LessThanOrEqual, FilterSizeClass.SMALL);
    this.filters.push(filterDateEnd);

    //#endregion

    //#region Filtro DateTime

    let filterDateTime: FilterConfig = FilterConfig.CreateStandardFilterConfig("PROVA_DATETIME", "Data/Ora", "DateWithTime", InputType.Datetime, WherePartType.GreaterThanOrEqual);
    this.filters.push(filterDateTime);

    //#endregion

    //#region Filtro CVL

    let cvlConfig: FilterCvlConfig = FilterCvlConfig.CreateFilterCVLConfig(null, [{ key: 1, value: "Valore 1" }, { key: 2, value: "Valore 2" }, { key: 3, value: "Valore 3" }], "key", "value", true, true, true, undefined, false);
    let cvlFilter: FilterConfig = FilterConfig.CreateStandardFilterConfig("PROVA_CVL", "Cvl", "CvlValue", InputType.Cvl, WherePartType.Equal, undefined, cvlConfig);
    this.filters.push(cvlFilter);


    //#endregion

    //#region Filtro CVL per booleani (3 stati)

    let booleanCvlConfig: FilterCvlConfig = FilterCvlConfig.CreateFilterCVLConfig(null, [{ key: true, value: "Si" }, { key: false, value: "No" }], "key", "value", false, true, true, undefined, false);
    let booleanCvlFilter: FilterConfig = FilterConfig.CreateStandardFilterConfig("PROVA_CVL_BOOLEAN", "Boolean Cvl (Si/No/Tutti)", "BooleanCvlValue", InputType.BooleanCvl, WherePartType.Equal, undefined, booleanCvlConfig);
    this.filters.push(booleanCvlFilter);


    //#endregion




  }

  filtersSelected($event) {
    console.log($event);
  }

  //#endregion

}
