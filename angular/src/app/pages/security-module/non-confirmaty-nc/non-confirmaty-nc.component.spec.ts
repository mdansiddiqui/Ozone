import { ComponentFixture, TestBed } from '@angular/core/testing';

import { NonConfirmatyNCComponent } from './non-confirmaty-nc.component';

describe('NonConfirmatyNCComponent', () => {
  let component: NonConfirmatyNCComponent;
  let fixture: ComponentFixture<NonConfirmatyNCComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ NonConfirmatyNCComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(NonConfirmatyNCComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
