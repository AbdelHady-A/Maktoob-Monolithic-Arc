import { Component, OnInit } from '@angular/core';
import { ISignInFacade } from '../facades/signin.facade';
import { Observable } from 'rxjs';
import { SignInState } from '../states/signin.state';
import { FormGroup } from '@angular/forms';

@Component({
  selector: 'app-signin',
  templateUrl: './signin.component.html',
  styleUrls: ['./signin.component.scss']
})
export class SignInComponent implements OnInit {
  public vm$: Observable<SignInState>;

  public hidePassword: boolean = true;

  public signInForm: FormGroup;

  constructor(private signFacade: ISignInFacade) {
    this.vm$ = this.signFacade.ViewModel$;
  }

  ngOnInit(): void {
    this.signInForm = this.signFacade.BuildForm();
  }

  public async SignInAsync() {
    await this.signFacade.SignInAsync();
  }
}
