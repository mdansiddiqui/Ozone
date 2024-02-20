import { TestBed } from '@angular/core/testing';

import { MsMainClauseService } from './ms-main-clause.service';

describe('MsMainClauseService', () => {
  let service: MsMainClauseService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(MsMainClauseService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
