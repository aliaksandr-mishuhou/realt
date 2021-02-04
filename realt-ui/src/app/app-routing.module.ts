import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { StatsGraphComponent } from './stats-graph/stats-graph.component';
import { StatsListComponent } from './stats-list/stats-list.component';
import { StatsTodayComponent } from './stats-today/stats-today.component';


const routes: Routes = [
  { path: 'stats-today', component: StatsTodayComponent },
  { path: 'stats-daily-list', component: StatsListComponent },
  { path: 'stats-daily-graph', component: StatsGraphComponent },
  { path: '', redirectTo: '/stats-daily-list', pathMatch: 'full' },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
