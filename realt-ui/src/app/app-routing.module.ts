import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { StatsDayDetailsComponent } from './stats-day-details/stats-day-details.component';
import { StatsGraphComponent } from './stats-graph/stats-graph.component';
import { StatsListComponent } from './stats-list/stats-list.component';


const routes: Routes = [
  { path: 'stats/list', component: StatsListComponent },
  { path: 'stats/details/:day', component: StatsDayDetailsComponent },
  { path: 'stats/today', component: StatsDayDetailsComponent },
  { path: 'stats/graph',
    children: [
      { path: 'count/realt', component: StatsGraphComponent, data: { id: "count", source: "1" } },
      { path: 'count/onliner', component: StatsGraphComponent, data: { id: "count", source: "2" } },
      { path: 'price/realt', component: StatsGraphComponent, data: { id: "price", source: "1" } },
      { path: 'price/onliner', component: StatsGraphComponent, data: { id: "price", source: "2" } }
    ]
  },
  { path: '', redirectTo: '/stats/list', pathMatch: 'full' },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
