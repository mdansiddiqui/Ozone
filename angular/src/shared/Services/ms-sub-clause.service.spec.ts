import { TestBed } from '@angular/core/testing';

import { MsSubClauseService } from './ms-sub-clause.service';

describe('MsSubClauseService', () => {
  let service: MsSubClauseService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(MsSubClauseService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
