import { Component, OnInit } from '@angular/core';
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
  
  constructor(private signUpFacade: ISignUpFacade) { }

  vm$ = this.signUpFacade.ViewModel$;

  ngOnInit() {
    this.signUpForm = this.signUpFacade.BuildForm();
  }

  public async SignUpAsync() : Promise<void>{
    await this.signUpFacade.SignUpAsync();
  }
}
