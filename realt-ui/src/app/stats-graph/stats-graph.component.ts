import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { ChartDataSets, ChartOptions, ChartType } from 'chart.js';
import { Color, Label } from 'ng2-charts';

import { Observable } from 'rxjs';
import { Item } from 'src/services/item';
import { Search } from 'src/services/search';
import { StatsResponse } from 'src/services/stats.response';
import { StatsService } from 'src/services/stats.service';
import { Line } from './line';
// import { interpolateRgb } from 'd3-interpolate';
import { DataUtils } from 'src/utils/data.utils';
import { LineChartData } from '../line-chart/line-chart-data';
import { LineChartItem } from '../line-chart/line-chart-item';


@Component({
  selector: 'app-stats-graph',
  templateUrl: './stats-graph.component.html',
  styleUrls: ['./stats-graph.component.scss']
})
export class StatsGraphComponent implements OnInit {

  public lineChartData: ChartDataSets[] = [];
  public lineChartLabels: Label[] = [];

  // TODO: 1. fill data gaps
  // TODO: 2. fixed colors

  public lineChartOptions: ChartOptions = {
    responsive: true,
    maintainAspectRatio: false,
    scales: {
      xAxes: [{}],
      yAxes: [{ position: 'right' }]
    }
  };

  public lineChartColors: Color[] = [];
  // public lineChartLegend = true;
  // public lineChartType: ChartType = 'line';

  private id : String;
  private years : boolean;

  public data: LineChartData | undefined;

  public search : Search = new Search();

  constructor(private statsService : StatsService, private route : ActivatedRoute) {
    // view type
    this.id = route.snapshot.data["id"];
    this.years = route.snapshot.data["years"] != null;
  }

  ngOnInit(): void {
    this.onSearch();
  }

  public onSearch(){
    console.log(this.search);
    // TODO: use factory (id, years) => init(...)
    if (this.years) {
      if (this.id == "count"){
        this.initDailyWithYear(this.statsService.getDailyCountWithYear(this.search), i => i.total);
      }
      if (this.id == "price"){
        this.initDailyWithYear(this.statsService.getDailyPriceWithYear(this.search), i => i.price);
      }
    } else {
      if (this.id == "count"){
        this.initDaily(this.statsService.getDailyCount(this.search), [
          { label: "All", mapFunc: i => i.total },
          { label: "1", mapFunc: i => i.total1 },
          { label: "2", mapFunc: i => i.total2 },
          { label: "3", mapFunc: i => i.total3 },
          { label: "4", mapFunc: i => i.total4 },
          { label: "5+", mapFunc: i => i.total5plus, hidden: true }
        ]);
        }
      if (this.id == "price"){
        this.initDaily(this.statsService.getDailyPrice(this.search), [
          { label: "All", mapFunc: i => i.price },
          { label: "1", mapFunc: i => i.price1 },
          { label: "2", mapFunc: i => i.price2 },
          { label: "3", mapFunc: i => i.price3 },
          { label: "4", mapFunc: i => i.price4 },
          { label: "5+", mapFunc: i => i.price5plus, hidden: true }
        ]);
        }
    }
  }

  private initDaily(result: Observable<StatsResponse>, lines: Line[]) {
    result.subscribe((response: StatsResponse) => {
      // prepare
      let items = DataUtils.fixGaps(response.items, this.search.start);
      // data
      const lineChartItems : LineChartItem[] = [];
      for (let line of lines) {
        lineChartItems.push({
          label : line.label,
          data : items.map(line.mapFunc),
          hidden: line.hidden
        });
      }

      this.data = new LineChartData(items.map(i => i.day), lineChartItems);
      this.updateGraph();
   });
  }

  private initDailyWithYear(result: Observable<StatsResponse>, mapCallback: (i: Item) => any) {
    result.subscribe((response: StatsResponse) => {
      let items = response.items;

      const visibleFromYear = 2000;

      const itemsPerYear = items.reduce(function (r, a ) {
        if (a.years == null || a.years == ""){
          return null;
        }
        r[a.years] = r[a.years] || [];
        r[a.years].push(a);
        return r;
      }, Object.create(null));

      let labels : string[] = [];
      const lineChartItems : LineChartItem[] = [];

      const keys = Object.keys(itemsPerYear);
      for (let key of keys) {
        const lineItems = DataUtils.fixGaps(itemsPerYear[key], this.search.start);
        if (labels.length == 0){
          labels = lineItems.map(i => i.day);
        }
        const year = Number(key.substr(0, 4));
        const lineChartItem = { data : lineItems.map(mapCallback), label: key, hidden: year < visibleFromYear };
        lineChartItems.push(lineChartItem);
      }

      this.data = new LineChartData(labels, lineChartItems);
      this.updateGraph();
   });
  }

  private updateGraph() {
    if (this.data == null) {
      return;
    }

    this.lineChartLabels = this.data.xLabels;
    this.lineChartData = this.data.lines;
  }

  // private setLabelColors() {

  //   const total = this.lineChartLabels.length;
  //   const interpolator = interpolateRgb("red", "yellow");
  //   const colors = new Array<Color>();

  //   for (let i = 0; i < total; i++) {
  //     const color = interpolator(i / total);

  //     colors.push({ borderColor: color });
  //   }

  //   console.log(colors);
  // }
}
