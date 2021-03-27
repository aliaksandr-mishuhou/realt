import { ComponentFixture, TestBed } from '@angular/core/testing';

import { StatsDayDetailsComponent } from './stats-day-details.component';

describe('StatsDayDetailsComponent', () => {
  let component: StatsDayDetailsComponent;
  let fixture: ComponentFixture<StatsDayDetailsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ StatsDayDetailsComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(StatsDayDetailsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
