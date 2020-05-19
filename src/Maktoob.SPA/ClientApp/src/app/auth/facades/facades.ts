import { Provider } from "@angular/core";
import { SignUpFacade, ISignUpFacade } from './signup.facade';
import { ISignInFacade, SignInFacade } from './signin.facade';

export const FacadeProviders: Provider[] = [
    { provide: ISignUpFacade, useClass: SignUpFacade },
    { provide: ISignInFacade, useClass: SignInFacade }
];