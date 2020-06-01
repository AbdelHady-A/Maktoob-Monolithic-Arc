import { Component, OnInit, NgModule, Output, EventEmitter, Input } from '@angular/core';

@Component({
  selector: 'gen-slide-toggle',
  templateUrl: './slide-toggle.component.html',
  styleUrls: ['./slide-toggle.component.scss']
})
export class SlideToggleComponent implements OnInit {

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
  declarations: [SlideToggleComponent],
  exports: [SlideToggleComponent]
})
export class GenSlideToggleModule { }