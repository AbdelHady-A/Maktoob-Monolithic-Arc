import { TestBed } from '@angular/core/testing';

import { SignUpFacade } from './signup.facade';

describe('Register.FacadeService', () => {
  let service: SignUpFacade;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(SignUpFacade);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
