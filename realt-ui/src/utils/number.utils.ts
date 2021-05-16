export class NumberUtils {
  public static round(value: number, decimalPlaces: number): number {
    const factorOfTen = Math.pow(10, decimalPlaces)
    return Math.round(value * factorOfTen) / factorOfTen
  }
}
