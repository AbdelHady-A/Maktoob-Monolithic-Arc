import { Component, OnInit, EventEmitter, Output, Input } from '@angular/core';

@Component({
  selector: 'app-theme-mode',
  templateUrl: './theme-mode.component.html',
  styleUrls: ['./theme-mode.component.scss']
})

export class ThemeModeComponent implements OnInit {

  @Input()
  Theme: string;

  Checked: boolean = false;

  @Output()
  Change = new EventEmitter<string>();

  constructor() { }

  ngOnInit(): void {
    this.Checked = this.Theme === 'dark' ? true : false;
  }
  ChangeTheme() {
    this.Checked = !this.Checked;
    if (this.Checked) {
      this.Change.emit('dark')
    } else {
      this.Change.emit('light');
    }
  }
}
