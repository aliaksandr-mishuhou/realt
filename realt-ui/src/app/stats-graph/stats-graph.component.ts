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

  public loading : boolean = false;

  public data: LineChartData | undefined;

  public searchParams : Search = new Search();

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
    console.log(this.searchParams);
    // TODO: use factory (id, years) => init(...)
    this.loading = true;
    if (this.years) {
      if (this.id == "count"){
        this.countYearBuilder.build(this.searchParams, (data) => this.onSearchCompleted(data));
      }
      if (this.id == "price"){
        this.priceYearBuilder.build(this.searchParams, (data) => this.onSearchCompleted(data));
      }
    } else {
      if (this.id == "count"){
        this.countBuilder.build(this.searchParams, (data) => this.onSearchCompleted(data));
      }
      if (this.id == "price"){
        this.priceBuilder.build(this.searchParams, (data) => this.onSearchCompleted(data));
      }
    }
  }

  private onSearchCompleted(data: LineChartData) : void {
    this.data = data;
    this.loading = false;
  }
}
