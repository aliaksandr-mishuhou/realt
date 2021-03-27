import { Item } from './item';
import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { StatsResponse } from './stats.response';
import { StatsDayResponse } from './stats.day.response';

@Injectable({ providedIn: 'root' })
export class StatsService {

  dailyCountUrl = "http://localhost:8080/stats/daily/count";
  dailyPriceUrl = "http://localhost:8080/stats/daily/price";
  dayDetailsUrlFn = (day : String) => `http://localhost:8080/stats/daily/detailed/${day}`;

  constructor(private http: HttpClient) { }

  public getDailyCount(): Observable<StatsResponse> {
    return this.http.get<StatsResponse>(this.dailyCountUrl);
  }

  public getDailyPrice(): Observable<StatsResponse> {
    return this.http.get<StatsResponse>(this.dailyPriceUrl);
  }

  public getDay(day : String): Observable<StatsDayResponse> {
    return this.http.get<StatsDayResponse>(this.dayDetailsUrlFn(day));
  }
}
