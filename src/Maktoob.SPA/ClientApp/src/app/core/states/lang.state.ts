import { LangModel } from '../models/lang.model';

export interface LangState{
    AvailableLangs: LangModel[];
    ActiveLang: LangModel;
}