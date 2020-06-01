import { Injectable, OnInit, OnDestroy } from '@angular/core';
import { BehaviorSubject, Observable, combineLatest, Subscription } from 'rxjs';
import { map, distinctUntilChanged, distinctUntilKeyChanged } from 'rxjs/operators';
import { TranslateService } from '@ngx-translate/core';
import { IStorageService } from '../services/storage.service';
import { LangState } from '../states/lang.state';

let _state: LangState = {
    AvailableLangs: [
        { key: "ar", name: "العربية", dir: "rtl" },
        { key: "en", name: "English", dir: "ltr" },
    ],
    ActiveLang: { key: "en", name: "English", dir: "ltr" },
}


@Injectable()
export abstract class ILangFacade {
    /**
     * Viewmodel that resolves once all the data is ready (or updated)...
     */
    readonly ViewModel$: Observable<LangState>;
    /**
     * Allows quick snapshot access to data for ngOnInit() purposes
     */
    abstract GetStateSanpshot(): LangState;
    /**
     * Allows change language of the application
     * @param langKey an key of the language
     */
    abstract ChangeLang(langKey: string): void
}


@Injectable()
export class LangFacade implements ILangFacade, OnDestroy {

    private store = new BehaviorSubject<LangState>(_state);
    private state$ = this.store.asObservable();
    private subscriptions: Subscription[] = [];

    ViewModel$: Observable<LangState> = combineLatest(
        this.state$.pipe(map(state => state.AvailableLangs), distinctUntilChanged()),
        this.state$.pipe(map(state => state.ActiveLang), distinctUntilKeyChanged('key'))
    ).pipe(map(([langs, lang]) => {
        return { AvailableLangs: langs, ActiveLang: lang } as LangState;
    }));

    // ------- Public Methods ------------------------

    public GetStateSanpshot(): LangState {
        return { ..._state, ActiveLang: { ..._state.ActiveLang } };
    }

    public ChangeLang(langKey: string): void {
        const lang = _state.AvailableLangs.find(l => l.key == langKey);
        this.translateService.use(lang.key);
        document.body.dir = lang.dir;
        this.updateState({ ..._state, ActiveLang: lang });
    }

    // ------- Private Methods ------------------------

    /** Update internal state cache and emit from store... */
    private updateState(state: LangState) {
        // persist selected language to storage
        this.storageService.SetState('lang', state.ActiveLang.key);
        this.store.next(_state = state);
    }

    private unSubscribeAll() {
        this.subscriptions.forEach(sub => {
            sub.unsubscribe();
        })
        this.subscriptions = []
    }

    constructor(private translateService: TranslateService, private storageService: IStorageService) {
        // initialize active language with the previously stored language
        let lang = this.storageService.GetItem('lang');
        if (lang) {
            this.ChangeLang(lang);
        } else {
            lang = this.GetStateSanpshot().ActiveLang.key;
            this.storageService.SetState('lang', lang);
        }
        const sub = this.storageService.GetState<string>('lang').subscribe(langKey => {
            this.ChangeLang(langKey);
        })
        this.subscriptions = [...this.subscriptions, sub];
    }
    ngOnDestroy(): void {
        this.unSubscribeAll();
    }
}



