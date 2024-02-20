import { ComponentFixture, TestBed } from '@angular/core/testing';

import { MasterStandardSubClauseComponent } from './master-standard-sub-clause.component';

describe('MasterStandardSubClauseComponent', () => {
  let component: MasterStandardSubClauseComponent;
  let fixture: ComponentFixture<MasterStandardSubClauseComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ MasterStandardSubClauseComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(MasterStandardSubClauseComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
