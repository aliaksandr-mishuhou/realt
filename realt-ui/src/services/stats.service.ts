import { Item } from './item';
import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { StatsResponse } from './stats.response';
import { StatsDayResponse } from './stats.day.response';
import { Search } from './search';
import { DateTimeUtils } from 'src/utils/datetime.utils';
import { environment } from 'src/environments/environment';

@Injectable({ providedIn: 'root' })
export class StatsService {

  dailyCountUrl = `${environment.STATS_URL}/stats/daily/count`;
  dailyCountWithYearUrl = `${environment.STATS_URL}/stats/daily/count-year`;
  dailyPriceUrl = `${environment.STATS_URL}/stats/daily/price`;
  dailyPriceWithYearUrl = `${environment.STATS_URL}/stats/daily/price-year`;
  dayDetailsUrlFn = (day : String) =>  `${environment.STATS_URL}/stats/daily/detailed/${day}`;

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

  public getDay(day : string, source: string = "1"): Observable<StatsDayResponse> {
    // const search = new Search();
    // search.source = source;
    // search.start = day;
    // search.end = day;
    return this.http.get<StatsDayResponse>(this.dayDetailsUrlFn(day));
  }

  private buildQuery(search: Search) : String {
    var searchParams = new URLSearchParams();
    searchParams.append("source", search.source.toString());
    searchParams.append("startDate", DateTimeUtils.toISOString(search.start));
    searchParams.append("endDate", DateTimeUtils.toISOString(search.end));
    return searchParams.toString();
  }
}
