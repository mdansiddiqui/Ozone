import { ComponentFixture, TestBed } from '@angular/core/testing';

import { VisitLevelComponent } from './visit-level.component';

describe('VisitLevelComponent', () => {
  let component: VisitLevelComponent;
  let fixture: ComponentFixture<VisitLevelComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ VisitLevelComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(VisitLevelComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
