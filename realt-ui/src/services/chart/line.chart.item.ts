export class LineChartItem {
  label: string;
  data: number[];
  hidden?: boolean;

  constructor(label: string, data: number[], hidden?: boolean) {
    this.label = label;
    this.data = data;
    this.hidden = hidden;
  }
}
