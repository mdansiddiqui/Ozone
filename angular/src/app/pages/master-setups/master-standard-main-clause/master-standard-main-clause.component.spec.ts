import { ComponentFixture, TestBed } from '@angular/core/testing';

import { MasterStandardMainClauseComponent } from './master-standard-main-clause.component';

describe('MasterStandardMainClauseComponent', () => {
  let component: MasterStandardMainClauseComponent;
  let fixture: ComponentFixture<MasterStandardMainClauseComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ MasterStandardMainClauseComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(MasterStandardMainClauseComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
