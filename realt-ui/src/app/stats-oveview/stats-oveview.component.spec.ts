import { ComponentFixture, TestBed } from '@angular/core/testing';

import { StatsOveviewComponent } from './stats-oveview.component';

describe('StatsOveviewComponent', () => {
  let component: StatsOveviewComponent;
  let fixture: ComponentFixture<StatsOveviewComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ StatsOveviewComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(StatsOveviewComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
