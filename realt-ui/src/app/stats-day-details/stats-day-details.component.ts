import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { ChartOptions, ChartType } from 'chart.js';
import { Label } from 'ng2-charts/lib/base-chart.directive';
//import * as pluginDataLabels from 'chartjs-plugin-datalabels';
import { RoomItem } from 'src/services/room.item';
import { StatsDayResponse } from 'src/services/stats.day.response';
import { StatsService } from 'src/services/stats.service';

@Component({
  selector: 'app-stats-day-details',
  templateUrl: './stats-day-details.component.html',
  styleUrls: ['./stats-day-details.component.scss']
})
export class StatsDayDetailsComponent implements OnInit {
  day: String;
  public items : RoomItem[] = [];

  // pie chart
  public pieChartOptions: ChartOptions = {
    responsive: true,
    legend: {
      position: 'top',
    },
  };
  public pieChartLabels: Label[] = [];
  public pieChartData: number[] = [];
  public pieChartType: ChartType = 'pie';
  public pieChartLegend = true;
  public pieChartColors = [
    {
      backgroundColor: [
        'rgba(255,0,0,0.3)',
        'rgba(0,255,0,0.3)',
        'rgba(0,0,255,0.3)',
        'rgba(0,255,255,0.3)',
        'rgba(255,0,255,0.3)'
      ],
    },
  ];

  constructor(private statsService : StatsService, private route: ActivatedRoute) {
    this.day = route.snapshot.params['day'];
    console.log(this.day);
    if (this.day == null){
      var today = new Date();
      this.day = today.toISOString().split('T')[0];
    }
  }

  ngOnInit(): void {
    this.statsService.getDay(this.day).subscribe((response: StatsDayResponse) => {

      // items for table
      this.items = response.items;

      // pie chart
      const split = 5;
      // rooms: 1-4
      var pieItems = response.items.slice(0, split - 1);
      // rooms: 5+
      var otherItems = response.items.slice(split - 1);
      const otherItem : RoomItem = {
        total: otherItems.map(i => i.total).reduce((a, b) => a + b, 0),
        rooms: 0,
        priceMean: 0,
        squareMean: 0
      };
      pieItems.push(otherItem);

      // values
      this.pieChartData = pieItems.map(i => i.total);
      // labels
      this.pieChartLabels = pieItems.map(i => {
        return (i.rooms > 0) ? i.rooms.toString() : split + "+";
      });
    });
  }
}
