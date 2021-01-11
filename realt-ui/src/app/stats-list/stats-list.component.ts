import { Component, OnInit } from '@angular/core';
import { Item } from 'src/services/item';
import { StatsService } from 'src/services/stats.service';

@Component({
  selector: 'app-stats-list',
  templateUrl: './stats-list.component.html',
  styleUrls: ['./stats-list.component.scss']
})
export class StatsListComponent implements OnInit {

  public items : Item[] = [];

  constructor(private statsService : StatsService) { }

  ngOnInit(): void {
    this.items = this.statsService.getDaily();
  }

}
