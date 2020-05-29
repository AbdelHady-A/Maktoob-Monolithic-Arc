import { TestBed } from '@angular/core/testing';

import { OverlayFacade } from './overlay.facade';

describe('OverlayFacade', () => {
  let service: OverlayFacade;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(OverlayFacade);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
