import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { NavBoxComponent } from './nav-box.component';

describe('NavBoxComponent', () => {
  let component: NavBoxComponent;
  let fixture: ComponentFixture<NavBoxComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ NavBoxComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(NavBoxComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
