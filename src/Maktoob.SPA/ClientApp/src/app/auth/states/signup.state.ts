import { GError } from 'src/app/core/results/error';

export interface SignUpState {
    EmailErrors?: GError[];
    UserNameErrors?: GError[]
    FirstNameErrors?: GError[]
    LastNameErrors?: GError[]
    PasswordErrors?: GError[]
}