import { TestBed } from '@angular/core/testing';

import { LangFacade } from './lang.facade';

describe('LangService', () => {
  let service: LangFacade;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(LangFacade);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
