import { Line } from '../line';
import { Search } from 'src/services/search';
import { StatsResponse } from 'src/services/stats.response';
import { StatsService } from 'src/services/stats.service';
import { DataUtils } from 'src/utils/data.utils';
import { LineChartBuilder } from '../line.chart.builder';
import { LineChartData } from '../line.chart.data';
import { LineChartItem } from '../line.chart.item';

export class CountBuilder implements LineChartBuilder {

  private lines: Line[] = [
    { label: "All", mapFunc: i => i.total },
    { label: "1", mapFunc: i => i.total1 },
    { label: "2", mapFunc: i => i.total2 },
    { label: "3", mapFunc: i => i.total3 },
    { label: "4", mapFunc: i => i.total4 },
    { label: "5+", mapFunc: i => i.total5plus, hidden: true }
  ]

  constructor(private statsService : StatsService) {
  }

  build(search: Search, onSuccess: (data: LineChartData) => void): void {

    const result = this.statsService.getDailyCount(search);

    result.subscribe((response: StatsResponse) => {
      // prepare
      let items = DataUtils.fixGaps(response.items, search.start);
      // data
      const lineChartItems : LineChartItem[] = [];
      for (let line of this.lines) {
        lineChartItems.push({
          label : line.label,
          data : items.map(line.mapFunc),
          hidden: line.hidden
        });
      }

      const data = new LineChartData(items.map(i => i.day), lineChartItems);
      onSuccess(data);
   });
  }
}
