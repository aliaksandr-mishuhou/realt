import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { ChartDataSets, ChartOptions, ChartType } from 'chart.js';
import { Color, Label } from 'ng2-charts';
import { Item } from 'src/services/item';
import { Search } from 'src/services/search';
import { StatsResponse } from 'src/services/stats.response';
import { StatsService } from 'src/services/stats.service';

@Component({
  selector: 'app-stats-graph',
  templateUrl: './stats-graph.component.html',
  styleUrls: ['./stats-graph.component.scss']
})
export class StatsGraphComponent implements OnInit {

  private readonly DEFAULT_DAYS = 60;

  public lineChartData: ChartDataSets[] = [];
  public lineChartLabels: Label[] = [];

  public lineChartOptions: ChartOptions = {
    responsive: true,
    scales: {
      xAxes: [{}],
      yAxes: [{ position: 'right' }]
    }
  };
  public lineChartColors: Color[] = [];

  public lineChartLegend = true;
  public lineChartType: ChartType = 'line';

  private id : String;

  private search : Search = new Search();

  constructor(private statsService : StatsService, private route : ActivatedRoute) {
    this.id = route.snapshot.data["id"];
    this.search.source = route.snapshot.data["source"];
    let today = new Date();
    this.search.start = this.addDays(today, (-1) * this.DEFAULT_DAYS);
    this.search.end = today;
  }

  ngOnInit(): void {
    console.log("init: " + this.id + "/" + this.search);

    if (this.id == "count"){
      this.initDailyCount();
    }
    if (this.id == "price"){
      this.initDailyPrice();
    }
  }

  private initDailyCount() {
    this.statsService.getDailyCount(this.search).subscribe((response: StatsResponse) => {
      let items = this.getItemsFromResponse(response);
      //this.items = items;
     this.lineChartLabels = items.map(i => i.day);
     this.lineChartData = [
       { data : items.map(i => i.total), label : "All" },
       { data : items.map(i => i.total1), label : "1" },
       { data : items.map(i => i.total2), label : "2" },
       { data : items.map(i => i.total3), label : "3" },
       { data : items.map(i => i.total4), label : "4" },
       { data : items.map(i => i.total5plus), label : "5+", hidden : true },
     ];
   });
  }

  private initDailyPrice() {
    this.statsService.getDailyPrice(this.search).subscribe((response: StatsResponse) => {
     let items = this.getItemsFromResponse(response);
     //this.items = items;
     this.lineChartLabels = items.map(i => i.day);
     this.lineChartData = [
       { data : items.map(i => i.price), label : "All" },
       { data : items.map(i => i.price1), label : "1" },
       { data : items.map(i => i.price2), label : "2" },
       { data : items.map(i => i.price3), label : "3" },
       { data : items.map(i => i.price4), label : "4" },
       { data : items.map(i => i.price5plus), label : "5+", hidden : true },
     ];
   });
  }

  private getItemsFromResponse(response : StatsResponse) : Item[] {
    let result = this.fixGaps(response.items, this.search.start);
    // if (result.length > this.MAX_DAYS) {
    //   result = result.slice(result.length - 1 - this.MAX_DAYS);
    // }
    return result;
  }

  private fixGaps(items : Item[], start : Date) : Item[] {

    if (items == null || items.length == 0) {
      return items;
    }

    let result : Item[] = new Array();

    let prevDate = start;
    for (let item of items) {
      let curDate = new Date(item.day);
      let diffDays = this.getDiffDays(curDate, prevDate);
      if (diffDays > 1) {
        for (let i = 1; i < diffDays; i++) {
          let empty : Item = { day: this.addDays(prevDate, i).toISOString().split('T')[0]};
          //console.log("adding empty: " + empty.day);
          result.push(empty);
        }
      }

      //console.log("adding regular: " + item.day);
      result.push(item);
      prevDate = curDate;
    }

    return result;
  }

  private getDiffDays(date1: Date, date2: Date) : number {
    let diffTime = Math.abs(date1.getTime() - date2.getTime());
    let diffDays = Math.ceil(diffTime / (1000 * 3600 * 24));
    return diffDays;
  }

  private addDays(date: Date, days: number): Date {
    return new Date(date.getTime() + (days * 1000 * 60 * 60 * 24));
  }
}
