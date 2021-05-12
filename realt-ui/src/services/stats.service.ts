import { Item } from './item';
import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { StatsResponse } from './stats.response';
import { StatsDayResponse } from './stats.day.response';
import { Search } from './search';

@Injectable({ providedIn: 'root' })
export class StatsService {

  dailyCountUrl = "http://localhost:8080/stats/daily/count";
  dailyCountWithYearUrl = "http://localhost:8080/stats/daily/count-year";
  dailyPriceUrl = "http://localhost:8080/stats/daily/price";
  dailyPriceWithYearUrl = "http://localhost:8080/stats/daily/price-year";
  dayDetailsUrlFn = (day : String) => `http://localhost:8080/stats/daily/detailed/${day}`;

  constructor(private http: HttpClient) { }

  public getDailyCount(search: Search = new Search()) : Observable<StatsResponse> {
    return this.http.get<StatsResponse>(this.dailyCountUrl + "?" + this.buildQuery(search));
  }

  public getDailyCountWithYear(search: Search = new Search()) : Observable<StatsResponse> {
    return this.http.get<StatsResponse>(this.dailyCountWithYearUrl + "?" + this.buildQuery(search));
  }

  public getDailyPrice(search: Search = new Search()): Observable<StatsResponse> {
    return this.http.get<StatsResponse>(this.dailyPriceUrl + "?" + this.buildQuery(search));
  }

  public getDailyPriceWithYear(search: Search = new Search()): Observable<StatsResponse> {
    return this.http.get<StatsResponse>(this.dailyPriceWithYearUrl + "?" + this.buildQuery(search));
  }

  public getDay(day : String): Observable<StatsDayResponse> {
    return this.http.get<StatsDayResponse>(this.dayDetailsUrlFn(day));
  }

  private buildQuery(search: Search) : String {
    var searchParams = new URLSearchParams();
    searchParams.append("source", search.source.toString());
    searchParams.append("start_date", search.start.toString());
    searchParams.append("end_date", search.end.toString());
    return searchParams.toString();
  }
}
