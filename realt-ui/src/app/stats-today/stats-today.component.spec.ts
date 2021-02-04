import { ComponentFixture, TestBed } from '@angular/core/testing';

import { StatsTodayComponent } from './stats-today.component';

describe('StatsTodayComponent', () => {
  let component: StatsTodayComponent;
  let fixture: ComponentFixture<StatsTodayComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ StatsTodayComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(StatsTodayComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
