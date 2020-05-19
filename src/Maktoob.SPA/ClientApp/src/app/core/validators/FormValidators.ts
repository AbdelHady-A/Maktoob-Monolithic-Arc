import { FormControl, ValidationErrors, ValidatorFn } from '@angular/forms';
const nonAlphNumeric = '\x00-\x1F\x21-\x2F\x3A-\x40\x5B-\x60\x7B-\x7E'
export class FormValidators {

    static PersonalName: ValidatorFn =
        (control: FormControl): ValidationErrors | null => {
            return (/(?!.*[\x00-\x1F\x21-\x26\x28-\x2C\x2F\x3A-\x40\x5B-\x60\x7B-\x7E])(?!.*(.)\1\1\1)^[^\s\'\.\-]+([\s\'\.\-]+[^\s\'\.\-]+)*$/.test(control.value))
                ? null
                : { NonPersonalName: true }
        }

    static StartsWithPeriod: ValidatorFn =
        (control: FormControl): ValidationErrors | null => {
            return /(?!\.)^.*$/.test(control.value)
                ? null
                : { StartsWithPeriod: true }
        }
    static PreventWhiteSpacesAtTheBeginningOrTheEnd: ValidatorFn =
        (control: FormControl): ValidationErrors | null => {
            return /^[^\s]+(\s+[^\s]+)*$/.test(control.value)
                ? null
                : { StartsOrEndsWithSpaces: true }
        }

    static PreventNonEnglishLetters: ValidatorFn =
        (control: FormControl): ValidationErrors | null => {
            return /^[\x00-\x7F]+$/.test(control.value)
                ? null
                : { NonEnglishLetters: true }
        }
}