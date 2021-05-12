import { LineChartItem } from './line-chart-item';

export class LineChartData {
  xLabels: string[];
  lines: LineChartItem[];

  constructor(xLabels: string[], lines: LineChartItem[]) {
    this.xLabels = xLabels;
    this.lines = lines;
  }
}
