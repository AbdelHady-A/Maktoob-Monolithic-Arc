import { Component, OnInit, Output, EventEmitter, Input } from '@angular/core';

@Component({
  selector: 'app-nav-box',
  templateUrl: './nav-box.component.html',
  styleUrls: ['./nav-box.component.scss']
})
export class NavBoxComponent implements OnInit {
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
