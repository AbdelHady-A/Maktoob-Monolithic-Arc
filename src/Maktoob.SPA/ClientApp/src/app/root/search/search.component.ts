import { Component, OnInit, Output, EventEmitter, NgModule } from '@angular/core';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatIconModule } from '@angular/material/icon';

@Component({
  selector: 'app-search',
  templateUrl: './search.component.html',
  styleUrls: ['./search.component.scss']
})
export class SearchComponent implements OnInit {
  @Output()
  CancelSearch = new EventEmitter<void>();

  constructor() { }

  ngOnInit(): void { }

}


@NgModule({
  declarations: [SearchComponent],
  imports: [MatToolbarModule, MatIconModule]
})

export class SearchModule { }