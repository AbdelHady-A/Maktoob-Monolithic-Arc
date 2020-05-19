import { Injectable } from '@angular/core';
import { SignInState } from '../states/signin.state';
import { Observable, BehaviorSubject, Subscription } from 'rxjs';
import { FormGroup, FormControl, FormBuilder, Validators } from '@angular/forms';
import { IAuthService } from 'src/app/core/services/auth.service';
import { SignInUserCommand } from 'src/app/core/commands/user.commnd';
import { GError } from 'src/app/core/results/error';
import { finalize, map } from 'rxjs/operators';

@Injectable()
export abstract class ISignInFacade {
  /**
   * Viewmodel that resolves once all the data is ready (or updated)...
   */
  readonly ViewModel$: Observable<SignInState>;
  /**
   * perform signup request and change state according to response
   */
  abstract SignInAsync(): Promise<void>;
  /**
   * build signup form that used to catch signup data
   */
  abstract BuildForm(): FormGroup;
}

@Injectable()
export class SignInFacade implements ISignInFacade {
  private command: SignInUserCommand;

  private state: SignInState = {};
  private store = new BehaviorSubject<SignInState>(this.state);

  private state$ = this.store.asObservable();

  constructor(
    private authService: IAuthService,
    private formBuilder: FormBuilder
  ) { }

  public ViewModel$ = this.state$;

  // ------- Private Methods ------------------------
  private updateState(state: SignInState) {
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

  // 
  public async SignInAsync(): Promise<void> {
    try {
      await this.authService.SignInAsync(this.command);
      this.subscriptions.forEach(sub => {
        sub.unsubscribe();
      });
    } catch (errors) {
      const credentialsErrors = errors?.filter((e: GError) =>
        e.Code.endsWith('Credentials')
      );
      const passwordErrors = errors?.filter((e: GError) =>
        e.Code.endsWith('Password')
      );

      if (credentialsErrors?.length > 0) {
        this.Credentials.setErrors({ serverErrors: credentialsErrors });
      }
      if (passwordErrors?.length > 0) {
        this.password.setErrors({ serverErrors: passwordErrors })
      }
    }
  }

  private Credentials: FormControl;
  private password: FormControl;
  private subscriptions: Subscription[] = [];

  public BuildForm(): FormGroup {
    this.Credentials = this.formBuilder.control('',
      [
        Validators.required
      ]
    );

    let sub;

    sub = this.Credentials.statusChanges.subscribe(status => {
      if (status == 'INVALID') {
        const errors = this.extractErrors(this.Credentials);
        this.updateState({ ...this.state, CredentialsErrors: errors })
      }
    })
    this.subscriptions = [...this.subscriptions, sub];

    this.password = this.formBuilder.control('',
      [
        Validators.required
      ]
    );

    sub = this.password.statusChanges.subscribe(status => {
      if (status == 'INVALID') {
        const errors = this.extractErrors(this.password);
        this.updateState({ ...this.state, PasswordErrors: errors })
      }
    })
    this.subscriptions = [...this.subscriptions, sub];

    const signInForm = this.formBuilder.group({
      Credentials: this.Credentials,
      Password: this.password
    });

    sub = signInForm.valueChanges.pipe(
      map((value: { Credentials: string, Password: string }) => {
        if (!Validators.email(this.Credentials)) { // no validation errors 
          return { Credentials: value.Credentials, Password: value.Password, EmailCredentials: true } as SignInUserCommand
        } else {
          return { Credentials: value.Credentials, Password: value.Password, EmailCredentials: false } as SignInUserCommand
        }
      }),
      finalize(() => { this.command = null }) // delete data
    ).subscribe(value => {
      this.command = value;
    })
    this.subscriptions = [...this.subscriptions, sub];

    return signInForm;
  }

}
