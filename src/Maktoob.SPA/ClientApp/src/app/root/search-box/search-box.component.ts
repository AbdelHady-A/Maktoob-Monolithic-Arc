import { Component, OnInit, EventEmitter, Output, ViewChild, ElementRef, Input } from '@angular/core';

@Component({
  selector: 'app-search-box',
  templateUrl: './search-box.component.html',
  styleUrls: ['./search-box.component.scss']
})
export class SearchBoxComponent implements OnInit {
  @ViewChild('SearchInput')
  SearchInput: ElementRef;

  @Output()
  OpenSearch = new EventEmitter<void>();
  @Output()
  CloseSearch = new EventEmitter<void>();

  @Input()
  Focus = false;

  @Input()
  Active = false;

  _OpenSearch() {
    this.OpenSearch.emit();
  }

  _CloseSearch(){
    this.CloseSearch.emit();
  }
  constructor() { }

  ngOnInit(): void {
    if (this.Focus) {
      setTimeout(() => { // this will make the execution after the above boolean has changed
        this.SearchInput?.nativeElement.focus();
      }, 0);
    }
  }

}