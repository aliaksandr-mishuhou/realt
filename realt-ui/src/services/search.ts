import { DateTimeUtils } from 'src/utils/datetime.utils';

export class Search {
  start: Date;
  end: Date;
  source: string;

  constructor(){
    const today = new Date();
    this.start = DateTimeUtils.addMonths(today, -2);
    this.end = today;
    this.source = "1";
  }

  public toString = () : string => {
    return `(source: ${this.source}, start: ${this.start}, end: ${this.end})`;
  }
}
