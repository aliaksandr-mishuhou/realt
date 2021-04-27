export class Search {
  start: Date;
  end: Date;
  source: string;

  constructor(){
    this.end = new Date();
    this.start = new Date(this.end.getDate() - 60);
    this.source = "1";
  }

  public toString = () : string => {
    return `(source: ${this.source}, start: ${this.start}, end: ${this.end})`;
  }
}
