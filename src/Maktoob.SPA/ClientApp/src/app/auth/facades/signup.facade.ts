import { Injectable } from '@angular/core';
import { FormBuilder, Validators, FormGroup, FormControl } from '@angular/forms';
import { SignUpUserCommand } from 'src/app/core/commands/user.commnd';
import { BehaviorSubject, Observable, Subscription } from 'rxjs';
import { GError } from 'src/app/core/results/error';
import { IAuthService } from 'src/app/core/services/auth.service';
import { SignUpState } from '../states/signup.state';
import { finalize } from 'rxjs/operators';
import { FormValidators } from 'src/app/core/validators/FormValidators';
import { Router } from '@angular/router';



@Injectable()
export abstract class ISignUpFacade {
  /**
  * Viewmodel that resolves once all the data is ready (or updated)...
  */
  readonly ViewModel$: Observable<SignUpState>;
  /**
   * perform signup request and change state according to response
   */
  abstract SignUpAsync(): Promise<void>;
  /**
   * build signup form that used to catch signup data
   */
  abstract BuildForm(): FormGroup;
}


@Injectable()
export class SignUpFacade implements ISignUpFacade {

  private command: SignUpUserCommand;

  private state: SignUpState = {};
  private store = new BehaviorSubject<SignUpState>(this.state);

  private state$ = this.store.asObservable();

  public ViewModel$ = this.state$;


  private userName: FormControl;
  private firstName: FormControl;
  private lastName: FormControl;
  private password: FormControl;
  private email: FormControl;

  constructor(
    private formBuilder: FormBuilder,
    private authService: IAuthService,
    private router: Router
  ) { }



  // ------- Private Methods ------------------------

  /** Update internal state cache and emit from store... */
  private updateState(state: SignUpState) {
    this.store.next(this.state = state);
  }

  private extractErrors(field: FormControl): GError[] {
    let errors = Object.getOwnPropertyNames(field.errors).filter(key => key !== 'serverErrors').map(key => {
      return { Code: key, Description: undefined } as GError;
    });
    if (field.errors['serverErrors']) {
      errors = errors.concat(field.errors['serverErrors'] as GError[]);
    }
    return errors;
  }


  // ------- Public Methods ------------------------

  public async SignUpAsync() {
    try {
      await this.authService.SignUpAsync(this.command);
      this.subscriptions.forEach(sub => {
        sub.unsubscribe();
      })
      this.signUpForm.reset();
      this.router.navigate(['']);
    } catch (errors) {
      const emailErrors = errors?.filter((e: GError) => e.Code.endsWith('Email'));
      const userNameErrors = errors?.filter((e: GError) => e.Code.endsWith('UserName'));
      const firstNameErrors = errors?.filter((e: GError) => e.Code.endsWith('FirstName'));
      const lastNameErrors = errors?.filter((e: GError) => e.Code.endsWith('LastName'));
      const passwordErrors = errors?.filter((e: GError) => e.Code.endsWith('Password'));

      if (emailErrors?.length > 0) {
        this.email.setErrors({ serverErrors: emailErrors });
      }
      if (userNameErrors?.length > 0) {
        this.userName.setErrors({ serverErrors: userNameErrors });
      }
      if (firstNameErrors?.length > 0) {
        this.firstName.setErrors({ serverErrors: firstNameErrors });
      }
      if (lastNameErrors?.length > 0) {
        this.lastName.setErrors({ serverErrors: lastNameErrors });
      }
      if (passwordErrors?.length > 0) {
        this.password.setErrors({ serverErrors: passwordErrors })
      }
    }
  }


  private subscriptions: Subscription[] = [];
  private signUpForm: FormGroup;
  public BuildForm(): FormGroup {
    let sub;

    // start username field
    this.userName = this.formBuilder.control('',
      [
        Validators.required,
        FormValidators.StartsWithPeriod,
        Validators.pattern(/^[A-Za-z0-9\.]*$/),
        Validators.maxLength(30),
        Validators.minLength(6),
      ]
    );

    sub = this.userName.statusChanges.subscribe(status => {
      if (status === 'INVALID') {
        const errors = this.extractErrors(this.userName);
        this.updateState({ ...this.state, UserNameErrors: errors })
      }
    });
    this.subscriptions = [...this.subscriptions, sub];
    // end username field


    // start firstname field
    this.firstName = this.formBuilder.control('',
      [
        Validators.required,
        Validators.maxLength(40),
        FormValidators.PersonalName
      ],
    );

    sub = this.firstName.statusChanges.subscribe(status => {
      if (status === 'INVALID') {
        const errors = this.extractErrors(this.firstName);
        this.updateState({ ...this.state, FirstNameErrors: errors })
      }
    });
    this.subscriptions = [...this.subscriptions, sub];
    // end firstname field


    // start lastname field
    this.lastName = this.formBuilder.control('',
      [
        Validators.required,
        Validators.maxLength(40),
        FormValidators.PersonalName
      ]
    );
    sub = this.lastName.statusChanges.subscribe(status => {
      if (status === 'INVALID') {
        const errors = this.extractErrors(this.lastName);
        this.updateState({ ...this.state, LastNameErrors: errors })
      }
    });
    this.subscriptions = [...this.subscriptions, sub];
    // end lastname field


    // start email field
    this.email = this.formBuilder.control('',
      [
        Validators.required,
        Validators.email
      ]
    );
    sub = this.email.statusChanges.subscribe(status => {
      if (status === 'INVALID') {
        const errors = this.extractErrors(this.email);
        this.updateState({ ...this.state, EmailErrors: errors })
      }
    });
    this.subscriptions = [...this.subscriptions, sub];
    // end email field


    // start password field
    this.password = this.formBuilder.control('',
      [
        Validators.required,
        FormValidators.PreventWhiteSpacesAtTheBeginningOrTheEnd,
        FormValidators.PreventNonEnglishLetters,
        Validators.minLength(8),
        Validators.maxLength(100),
        Validators.pattern(/^(?=.*[a-zA-Z])(?=.*[0-9])[a-zA-Z0-9 !"#$%&'()*+,\-.\/:;<=>?@[\\\]^_`{|}~]+$/),
      ]
    );
    sub = this.password.statusChanges.subscribe(status => {
      if (status === 'INVALID') {
        const errors = this.extractErrors(this.password);
        this.updateState({ ...this.state, PasswordErrors: errors })
      }
    });
    this.subscriptions = [...this.subscriptions, sub];
    // end password field


    // build form group
    this.signUpForm = this.formBuilder.group({
      UserName: this.userName,
      Email: this.email,
      Password: this.password,
      FirstName: this.firstName,
      LastName: this.lastName
    });

    sub = this.signUpForm.valueChanges.pipe(
      finalize(() => {
        this.command = null; // delete data
      })
    ).subscribe((value: SignUpUserCommand) => {
      this.command = value;
    });
    this.subscriptions = [...this.subscriptions, sub];

    return this.signUpForm;
  }
}

