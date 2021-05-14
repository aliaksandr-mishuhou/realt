import { LineChartData } from './line.chart.data';
import { Search } from '../search';

export interface LineChartBuilder {
  build(search: Search, onSuccess: (data: LineChartData) => void): void;
}
