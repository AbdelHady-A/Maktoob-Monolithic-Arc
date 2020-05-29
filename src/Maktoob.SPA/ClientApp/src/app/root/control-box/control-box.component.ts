import { Component, OnInit, Output, EventEmitter, Input } from '@angular/core';

@Component({
  selector: 'app-control-box',
  templateUrl: './control-box.component.html',
  styleUrls: ['./control-box.component.scss']
})
export class ControlBoxComponent implements OnInit {
  @Output()
  OpenControl = new EventEmitter<string>();

  @Input()
  ActivePath: string;

  constructor() { }

  ngOnInit(): void {
  }

  _OpenControl(path: string) {
    this.ActivePath = path;
    this.OpenControl.emit(path);
  }
}
