import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AuditorReportsComponent } from './auditor-reports.component';

describe('AuditorReportsComponent', () => {
  let component: AuditorReportsComponent;
  let fixture: ComponentFixture<AuditorReportsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ AuditorReportsComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(AuditorReportsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
