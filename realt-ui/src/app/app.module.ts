import { BrowserModule } from '@angular/platform-browser';
import { HttpClientModule } from '@angular/common/http';
import { NgModule } from '@angular/core';

import { AppComponent } from './app.component';
import { StatsListComponent } from './stats-list/stats-list.component';
import { StatsGraphComponent } from './stats-graph/stats-graph.component';
import { AppRoutingModule } from './app-routing.module';
import { RouterModule } from '@angular/router';
import { NgxPaginationModule } from 'ngx-pagination';
import { StatsDayDetailsComponent } from './stats-day-details/stats-day-details.component';
import { ChartsModule } from 'ng2-charts';
import { HeaderComponent } from './header/header.component';
import { FormsModule } from '@angular/forms';
import { MaterialModule } from './material.module';
import { StatsOveviewComponent } from './stats-oveview/stats-oveview.component';
import { LineChartComponent } from './line-chart/line-chart.component';
import { DiffComponent } from './diff/diff.component';

@NgModule({
  declarations: [
    AppComponent,
    StatsListComponent,
    StatsGraphComponent,
    StatsDayDetailsComponent,
    HeaderComponent,
    StatsOveviewComponent,
    LineChartComponent,
    DiffComponent
  ],
  imports: [
    BrowserModule,
    HttpClientModule,
    RouterModule,
    AppRoutingModule,
    NgxPaginationModule,
    ChartsModule,
    FormsModule,
    MaterialModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
