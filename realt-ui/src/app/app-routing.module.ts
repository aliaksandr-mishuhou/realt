import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { StatsDayDetailsComponent } from './stats-day-details/stats-day-details.component';
import { StatsGraphComponent } from './stats-graph/stats-graph.component';
import { StatsListComponent } from './stats-list/stats-list.component';
import { StatsOveviewComponent } from './stats-oveview/stats-oveview.component';
import { TestComponent } from './test/test.component';

const routes: Routes = [
  { path: 'test', component: TestComponent },
  { path: 'stats/overview', component: StatsOveviewComponent },
  { path: 'stats/list', component: StatsListComponent },
  { path: 'stats/details/:day', component: StatsDayDetailsComponent },
  { path: 'stats/today', component: StatsDayDetailsComponent },
  { path: 'stats/graph',
    children: [
      // new
      { path: 'count', component: StatsGraphComponent, data: { id: "count", source: "1" } },
      { path: 'count-years', component: StatsGraphComponent, data: { id: "count", years: true, source: "1" } },
      { path: 'price', component: StatsGraphComponent, data: { id: "price", source: "1" } },
      { path: 'price-year', component: StatsGraphComponent, data: { id: "price", years: true, source: "1" } }
    ]
  },
  { path: '', redirectTo: '/stats/graph/count', pathMatch: 'full' },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
