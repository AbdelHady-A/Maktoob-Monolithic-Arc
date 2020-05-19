import { TestBed } from '@angular/core/testing';

import { IAuthService } from './auth.service';

describe('AuthService', () => {
  let service: IAuthService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(IAuthService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
