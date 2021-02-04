import { Component, OnInit } from '@angular/core';
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

  constructor(private statsService : StatsService) { }

  ngOnInit(): void {
    this.statsService.getDaily().subscribe((response: StatsResponse) => {
      this.items = response.items;
      this.page = 1;
      this.total = this.items.length;
    });
  }

  onPageChanged(event : number){
    this.page = event;
  }
}
