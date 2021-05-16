import { Component, Input, OnChanges, OnInit, SimpleChanges } from '@angular/core';
import { NumberUtils } from 'src/utils/number.utils';

@Component({
  selector: 'app-diff',
  templateUrl: './diff.component.html',
  styleUrls: ['./diff.component.scss']
})
export class DiffComponent implements OnInit, OnChanges {

  @Input()
  public start: number = 0;

  @Input()
  public end: number = 0;

  @Input()
  public sku: string = "";

  @Input()
  public fullInfo: boolean = false;

  public value: number | undefined;
  public percent: number | undefined;
  public asc: boolean = false;

  constructor() { }

  ngOnInit(): void {
  }

  ngOnChanges(changes: SimpleChanges): void {
    if (this.start == null || this.end == null) {
      return;
    }

    const diff = this.end - this.start;
    this.asc = diff >= 0;
    this.value = Math.abs(diff);
    this.percent = NumberUtils.round(this.value / this.start * 100, 1);
  }
}
