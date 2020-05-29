import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { CxButtonComponent } from './cx-button.component';

describe('CxButtonComponent', () => {
  let component: CxButtonComponent;
  let fixture: ComponentFixture<CxButtonComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ CxButtonComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CxButtonComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
