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
      { path: 'count', component: StatsGraphComponent, data: { id: "count" } },
      { path: 'price', component: StatsGraphComponent, data: { id: "price" } }
    ]
  },
  { path: '', redirectTo: '/stats/list', pathMatch: 'full' },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
