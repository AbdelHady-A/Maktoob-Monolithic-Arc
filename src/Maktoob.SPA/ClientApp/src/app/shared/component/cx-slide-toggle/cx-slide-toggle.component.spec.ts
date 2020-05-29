import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { CxSlideToggleComponent } from './cx-slide-toggle.component';

describe('CxSlideToggleComponent', () => {
  let component: CxSlideToggleComponent;
  let fixture: ComponentFixture<CxSlideToggleComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ CxSlideToggleComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CxSlideToggleComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
