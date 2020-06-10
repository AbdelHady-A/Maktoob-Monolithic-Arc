import { Component, OnInit, EventEmitter, Output, Input } from '@angular/core';

@Component({
  selector: 'app-theme-mode',
  templateUrl: './theme-mode.component.html',
  styleUrls: ['./theme-mode.component.scss']
})

export class ThemeModeComponent implements OnInit {

  @Input()
  On: boolean = false;

  @Output()
  Change = new EventEmitter<boolean>();

  constructor() { }

  ngOnInit(): void {
  }
  ChangeTheme() {
    this.On = !this.On;
    this.Change.emit(this.On);
  }
}
