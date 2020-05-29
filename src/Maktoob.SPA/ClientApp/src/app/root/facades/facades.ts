import { Provider } from '@angular/core';
import { IOverlayFacade, OverlayFacade } from './overlay.facade';

export const FacadeProviders: Provider[] = [
    { provide: IOverlayFacade, useClass: OverlayFacade }
]