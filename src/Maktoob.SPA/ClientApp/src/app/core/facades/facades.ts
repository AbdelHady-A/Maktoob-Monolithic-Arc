import { Provider } from '@angular/core';
import { ThemeFacade, IThemeFacade } from './theme.facade';

export const FacadeProviders: Provider[] = [
    { provide: IThemeFacade, useClass: ThemeFacade, multi: false }
]