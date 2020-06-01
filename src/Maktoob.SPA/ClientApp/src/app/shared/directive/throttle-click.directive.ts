import { Directive, Input, Output, EventEmitter, HostListener, NgModule } from '@angular/core';
import { Subject, Subscription } from 'rxjs';
import { throttleTime } from 'rxjs/operators';

@Directive({
  selector: '[gen-throttle-click]'
})

export class ThrottleClickDirective {
  @Input() ThrottleTime = 700;
  @Output() ThrottleClick = new EventEmitter();
  private clicks = new Subject();
  private subscription: Subscription;

  constructor() { }

  ngOnInit() {
    this.subscription = this.clicks
      .pipe(throttleTime(this.ThrottleTime))
      .subscribe(e => this.ThrottleClick.emit(e));
  }

  ngOnDestroy() {
    this.subscription.unsubscribe();
  }

  @HostListener('click', ['$event'])
  clickEvent(event) {
    event.preventDefault();
    event.stopPropagation();
    this.clicks.next(event);
  }
}


@NgModule({
  declarations: [ThrottleClickDirective],
  exports: [ThrottleClickDirective]
})
export class ThrottleClickModule { }