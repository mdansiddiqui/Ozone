import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AuditorAuditDetailsComponent } from './auditor-audit-details.component';

describe('AuditorAuditDetailsComponent', () => {
  let component: AuditorAuditDetailsComponent;
  let fixture: ComponentFixture<AuditorAuditDetailsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ AuditorAuditDetailsComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(AuditorAuditDetailsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
