import { Search } from 'src/services/search';
import { StatsResponse } from 'src/services/stats.response';
import { StatsService } from 'src/services/stats.service';
import { DataUtils } from 'src/utils/data.utils';
import { LineChartBuilder } from '../line.chart.builder';
import { LineChartData } from '../line.chart.data';
import { LineChartItem } from '../line.chart.item';
import { Item } from 'src/services/item';

export class CountYearBuilder implements LineChartBuilder {

  private mapCallback: (i: Item) => any = i => i.total;

  constructor(private statsService : StatsService) {
  }

  build(search: Search, onSuccess: (data: LineChartData) => void): void {

    const result = this.statsService.getDailyCountWithYear(search);

    result.subscribe((response: StatsResponse) => {
      let items = response.items;

      const visibleFromYear = 2000;

      const itemsPerYear = items.reduce(function (r, a ) {
        if (a.years == null || a.years == ""){
          return null;
        }
        r[a.years] = r[a.years] || [];
        r[a.years].push(a);
        return r;
      }, Object.create(null));

      let labels : string[] = [];
      const lineChartItems : LineChartItem[] = [];

      const keys = Object.keys(itemsPerYear);
      for (let key of keys) {
        const lineItems = DataUtils.fixGaps(itemsPerYear[key], search.start);
        if (labels.length == 0){
          labels = lineItems.map(i => i.day);
        }
        const year = Number(key.substr(0, 4));
        const lineChartItem = { data : lineItems.map(this.mapCallback), label: key, hidden: year < visibleFromYear };
        lineChartItems.push(lineChartItem);
      }

      const data = new LineChartData(labels, lineChartItems);
      onSuccess(data);
   });
  }
}
