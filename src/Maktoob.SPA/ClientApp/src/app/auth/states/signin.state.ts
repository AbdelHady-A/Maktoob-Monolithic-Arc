import { GError } from 'src/app/core/results/error';

export interface SignInState {
    CredentialsErrors?: GError[];
    PasswordErrors?: GError[];
    Errors?: GError[];
}