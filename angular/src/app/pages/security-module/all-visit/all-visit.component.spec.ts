import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AllVisitComponent } from './all-visit.component';

describe('AllVisitComponent', () => {
  let component: AllVisitComponent;
  let fixture: ComponentFixture<AllVisitComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ AllVisitComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(AllVisitComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
