import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { CountBuilder } from 'src/services/chart/builders/count.builder';
import { CountYearBuilder } from 'src/services/chart/builders/count.year.builder';
import { PriceBuilder } from 'src/services/chart/builders/price.builder';
import { PriceYearBuilder } from 'src/services/chart/builders/price.year.builder';
import { LineChartBuilder } from 'src/services/chart/line.chart.builder';
import { LineChartData } from 'src/services/chart/line.chart.data';
import { Search } from 'src/services/search';
import { StatsService } from 'src/services/stats.service';
import { DateTimeUtils } from 'src/utils/datetime.utils';

@Component({
  selector: 'app-stats-oveview',
  templateUrl: './stats-oveview.component.html',
  styleUrls: ['./stats-oveview.component.scss']
})
export class StatsOveviewComponent implements OnInit {

  public countRealt: LineChartData | undefined;
  public countOnliner: LineChartData | undefined;
  public priceRealt: LineChartData | undefined;
  public priceOnliner: LineChartData | undefined;

  private countBuilder: CountBuilder;
  private priceBuilder: PriceBuilder;

  constructor(private statsService: StatsService) {
    this.countBuilder = new CountBuilder(this.statsService);
    this.priceBuilder = new PriceBuilder(this.statsService);
  }

  ngOnInit(): void {
    const searchRealt: Search = new Search();
    const searchOnliner: Search = new Search();
    const end = new Date();
    const start = DateTimeUtils.addDays(end, -14);
    searchRealt.start = searchOnliner.start = start;
    searchRealt.source = "1";
    searchOnliner.source = "2";


    this.countBuilder.build(searchRealt, data => this.countRealt = data);
    this.priceBuilder.build(searchRealt, data => this.priceRealt = data);
    this.countBuilder.build(searchOnliner, data => this.countOnliner = data);
    this.priceBuilder.build(searchOnliner, data => this.priceOnliner = data);
  }

}
