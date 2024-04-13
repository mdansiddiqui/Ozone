import { ComponentFixture, TestBed } from '@angular/core/testing';

import { WageAnalysisComponent } from './wage-analysis.component';

describe('WageAnalysisComponent', () => {
  let component: WageAnalysisComponent;
  let fixture: ComponentFixture<WageAnalysisComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ WageAnalysisComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(WageAnalysisComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
