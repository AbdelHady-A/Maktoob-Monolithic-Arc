import { Component, OnInit, ViewChild, ElementRef } from '@angular/core';
import { ISignInFacade } from '../facades/signin.facade';
import { FormGroup } from '@angular/forms';

@Component({
  selector: 'app-signin',
  templateUrl: './signin.component.html',
  styleUrls: ['./signin.component.scss']
})
export class SignInComponent implements OnInit {
  public vm$ = this.signFacade.ViewModel$;

  public hidePassword: boolean = true;

  @ViewChild('FShadow') FShadow: ElementRef;
  @ViewChild('ProgressBar') ProgressBar: ElementRef;

  public signInForm: FormGroup;

  constructor(private signFacade: ISignInFacade) { }

  ngOnInit(): void {
    this.signInForm = this.signFacade.BuildForm();
  }

  public async SignInAsync() {
    this.FShadow?.nativeElement?.classList.add('active');
    this.ProgressBar?.nativeElement?.classList.add('active');
    await this.signFacade.SignInAsync();
    this.ProgressBar?.nativeElement?.classList.remove('active');
    this.FShadow?.nativeElement?.classList.remove('active');
  }

}
