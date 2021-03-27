import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { Item } from 'src/services/item';
import { StatsResponse } from 'src/services/stats.response';
import { StatsService } from 'src/services/stats.service';

@Component({
  selector: 'app-stats-list',
  templateUrl: './stats-list.component.html',
  styleUrls: ['./stats-list.component.scss']
})
export class StatsListComponent implements OnInit {

  public items : Item[] = [];
  public page : number = 1;
  public total: number = 0;

  constructor(private statsService : StatsService, private router : Router) { }

  ngOnInit(): void {
    this.statsService.getDailyCount().subscribe((response: StatsResponse) => {
      // change order
      this.items = response.items.reverse();
      this.page = 1;
      this.total = this.items.length;
    });
  }

  onPageChanged(event : number){
    this.page = event;
  }
}
