import { TokenModel } from '../models/token.model';

export interface TokenState {
    TokenModel: TokenModel;
    IsSignedIn: boolean;
    Claims: { [index: string]: any }
}