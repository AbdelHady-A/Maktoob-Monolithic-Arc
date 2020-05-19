import { TestBed } from '@angular/core/testing';

import { SignInFacade } from './signin.facade';

describe('SigninFacade', () => {
  let service: SignInFacade;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(SignInFacade);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
