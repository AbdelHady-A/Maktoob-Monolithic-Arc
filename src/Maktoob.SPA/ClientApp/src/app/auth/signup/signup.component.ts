import { Component, OnInit, ViewChild, ElementRef } from '@angular/core';
import { FormGroup } from '@angular/forms';
import { ISignUpFacade } from '../facades/signup.facade';

@Component({
  selector: 'app-register',
  templateUrl: './signup.component.html',
  styleUrls: ['./signup.component.scss']
})
export class SignUpComponent implements OnInit {
  signUpForm: FormGroup;
  hidePassword: boolean = true;
  @ViewChild('FShadow') FShadow: ElementRef;
  @ViewChild('ProgressBar') ProgressBar: ElementRef;

  constructor(private signUpFacade: ISignUpFacade) { }

  vm$ = this.signUpFacade.ViewModel$;

  ngOnInit() {
    this.signUpForm = this.signUpFacade.BuildForm();
  }

  public async SignUpAsync(): Promise<void> {
    this.FShadow?.nativeElement?.classList.add('active');
    this.ProgressBar?.nativeElement?.classList.add('active');
    await this.signUpFacade.SignUpAsync();
    this.ProgressBar?.nativeElement?.classList.remove('active');
    this.FShadow?.nativeElement?.classList.remove('active');
  }

}
