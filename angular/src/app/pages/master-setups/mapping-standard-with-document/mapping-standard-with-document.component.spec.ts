import { ComponentFixture, TestBed } from '@angular/core/testing';

import { MappingStandardWithDocumentComponent } from './mapping-standard-with-document.component';

describe('MappingStandardWithDocumentComponent', () => {
  let component: MappingStandardWithDocumentComponent;
  let fixture: ComponentFixture<MappingStandardWithDocumentComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ MappingStandardWithDocumentComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(MappingStandardWithDocumentComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
