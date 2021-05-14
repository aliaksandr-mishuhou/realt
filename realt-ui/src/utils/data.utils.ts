import { Item } from 'src/services/item';
import { DateTimeUtils } from './datetime.utils';

export class DataUtils {
  public static fixGaps(items : Item[], start : Date) : Item[] {

    if (items == null || items.length == 0) {
      return items;
    }

    let result : Item[] = new Array();

    let prevDate = start;
    let sorted = items.sort((a, b) => DateTimeUtils.getDiffDays(new Date(b.day), new Date(a.day)));
    for (let item of sorted) {
      let curDate = new Date(item.day);
      let diffDays = DateTimeUtils.getDiffDays(curDate, prevDate);
      if (diffDays > 1) {
        for (let i = 1; i < diffDays; i++) {
          let empty : Item = { day: DateTimeUtils.addDays(prevDate, i).toISOString().split('T')[0]};
          result.push(empty);
        }
      }

      result.push(item);
      prevDate = curDate;
    }

    return result;
  }

}
