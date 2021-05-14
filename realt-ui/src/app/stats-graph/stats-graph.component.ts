import { Component, Input, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Search } from 'src/services/search';
import { StatsService } from 'src/services/stats.service';
import { LineChartData } from 'src/services/chart/line.chart.data';
import { LineChartBuilder } from 'src/services/chart/line.chart.builder';
import { CountBuilder } from 'src/services/chart/builders/count.builder';
import { PriceBuilder } from 'src/services/chart/builders/price.builder';
import { CountYearBuilder } from 'src/services/chart/builders/count.year.builder';
import { PriceYearBuilder } from 'src/services/chart/builders/price.year.builder';

@Component({
  selector: 'app-stats-graph',
  templateUrl: './stats-graph.component.html',
  styleUrls: ['./stats-graph.component.scss']
})
export class StatsGraphComponent implements OnInit {

  private id : String;
  private years : boolean;

  private countBuilder: LineChartBuilder;
  private priceBuilder: LineChartBuilder;
  private countYearBuilder: LineChartBuilder;
  private priceYearBuilder: LineChartBuilder;

  public data: LineChartData | undefined;

  public search : Search = new Search();

  constructor(private statsService : StatsService, private route : ActivatedRoute) {
    // view type
    this.id = route.snapshot.data["id"];
    this.years = route.snapshot.data["years"] != null;

    this.countBuilder = new CountBuilder(statsService);
    this.priceBuilder = new PriceBuilder(statsService);

    this.countYearBuilder = new CountYearBuilder(statsService);
    this.priceYearBuilder = new PriceYearBuilder(statsService);
  }

  ngOnInit(): void {
    this.onSearch();
  }

  public onSearch(){
    console.log(this.search);
    // TODO: use factory (id, years) => init(...)
    if (this.years) {
      if (this.id == "count"){
        this.countYearBuilder.build(this.search, (data) => this.data = data);
      }
      if (this.id == "price"){
        this.priceYearBuilder.build(this.search, (data) => this.data = data);
      }
    } else {
      if (this.id == "count"){
        this.countBuilder.build(this.search, (data) => this.data = data);
      }
      if (this.id == "price"){
        this.priceBuilder.build(this.search, (data) => this.data = data);
      }
    }
  }
}
