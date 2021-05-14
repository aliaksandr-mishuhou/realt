import { Component, Input, OnChanges, OnInit, SimpleChanges } from '@angular/core';
import { ChartDataSets, ChartOptions } from 'chart.js';
import { Label } from 'ng2-charts';
// import { interpolateRgb } from 'd3-interpolate';
import { LineChartData } from 'src/services/chart/line.chart.data';

@Component({
  selector: 'app-line-chart',
  templateUrl: './line-chart.component.html',
  styleUrls: ['./line-chart.component.scss']
})
export class LineChartComponent implements OnInit, OnChanges {

  @Input()
  public data: LineChartData | undefined;

  public datasets: ChartDataSets[] = [];
  public labels: Label[] = [];

  public options: ChartOptions = {
    responsive: true,
    maintainAspectRatio: false,
    scales: {
      xAxes: [{}],
      yAxes: [{ position: 'right' }]
    }
  };

  constructor() {
  }

  ngOnChanges(changes: SimpleChanges): void {
    if (this.data == null) {
      return;
    }

    console.log(this.data);

    this.labels = this.data.xLabels;
    this.datasets = this.data.lines;
  }

  ngOnInit(): void {
    // DO NOTHING
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
