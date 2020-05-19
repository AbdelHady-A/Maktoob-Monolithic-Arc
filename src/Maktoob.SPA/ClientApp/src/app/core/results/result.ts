import { GError } from './error';


export class GResult<T = void> {
    Succeeded: boolean;
    Outcome: T;
    Errors: GError[];
}