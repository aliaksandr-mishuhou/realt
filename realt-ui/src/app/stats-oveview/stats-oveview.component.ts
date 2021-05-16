import { Component, OnInit } from '@angular/core';
import { CountBuilder } from 'src/services/chart/builders/count.builder';
import { PriceBuilder } from 'src/services/chart/builders/price.builder';
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

  private static DAYS: number = 30;

  public countRealt: LineChartData | undefined;
  public countRealtStart: number = 0;
  public countRealtEnd: number = 0;

  public countOnliner: LineChartData | undefined;
  public countOnlinerStart: number = 0;
  public countOnlinerEnd: number = 0;

  public priceRealt: LineChartData | undefined;
  public priceRealtStart: number = 0;
  public priceRealtEnd: number = 0;

  public priceOnliner: LineChartData | undefined;
  public priceOnlinerStart: number = 0;
  public priceOnlinerEnd: number = 0;

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
    const start = DateTimeUtils.addDays(end, -1 * StatsOveviewComponent.DAYS);
    searchRealt.start = searchOnliner.start = start;
    searchRealt.source = "1";
    searchOnliner.source = "2";

    this.countBuilder.build(searchRealt, data => {
      this.countRealt = data;
      [this.countRealtStart, this.countRealtEnd] = this.getStartAndEnd(data);
    });
    this.countBuilder.build(searchOnliner, data => {
      this.countOnliner = data;
      [this.countOnlinerStart, this.countOnlinerEnd] = this.getStartAndEnd(data);
    });

    this.priceBuilder.build(searchRealt, data => {
      this.priceRealt = data;
      [this.priceRealtStart, this.priceRealtEnd] = this.getStartAndEnd(data);
    });
    this.priceBuilder.build(searchOnliner, data => {
      this.priceOnliner = data;
      [this.priceOnlinerStart, this.priceOnlinerEnd] = this.getStartAndEnd(data);
    });
  }

  private getStartAndEnd(data: LineChartData) : number[] {
    const allLine = data.lines.filter(l => l.label == "All")[0];
    return [allLine.data[0], allLine.data[allLine.data.length - 1]];
  }
}
