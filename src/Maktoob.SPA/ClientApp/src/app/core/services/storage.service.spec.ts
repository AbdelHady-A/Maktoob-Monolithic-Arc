import { TestBed } from '@angular/core/testing';

import { IStorageService } from './storage.service';

describe('StorageService', () => {
  let service: IStorageService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(IStorageService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
