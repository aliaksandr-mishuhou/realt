export class DateTimeUtils {
    // move to DateUtils
    public static getDiffDays(date1: Date, date2: Date) : number {
      let diffTime = Math.abs(date1.getTime() - date2.getTime());
      let diffDays = Math.ceil(diffTime / (1000 * 3600 * 24));
      return diffDays;
    }
    // move to DateUtils
    public static addDays(date: Date, days: number): Date {
      return new Date(date.getTime() + (days * 1000 * 60 * 60 * 24));
    }

    public static addMonths(date: Date, months: number): Date  {
      const newDate = new Date();
      newDate.setMonth(date.getMonth() + months);
      return newDate;
    }
}
