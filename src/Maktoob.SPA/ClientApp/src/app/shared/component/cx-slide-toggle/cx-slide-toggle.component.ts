import { Component, OnInit, NgModule, Output, EventEmitter, Input } from '@angular/core';

@Component({
  selector: 'app-cx-slide-toggle',
  templateUrl: './cx-slide-toggle.component.html',
  styleUrls: ['./cx-slide-toggle.component.scss']
})
export class CxSlideToggleComponent implements OnInit {

  @Output()
  Change = new EventEmitter<boolean>();

  @Input() @Output()
  Checked: boolean = false;

  constructor() { }

  _Change() {
    this.Checked = !this.Checked;
    this.Change.emit(this.Checked);
  }
  ngOnInit(): void {
  }


}

@NgModule({
  declarations: [CxSlideToggleComponent],
  exports: [CxSlideToggleComponent]
})
export class CxSlideToggleModule { }