import { Provider } from '@angular/core';
import { AuthService, IAuthService } from './auth.service';
import { IStorageService, StorageService } from './storage.service';
import { ILinkeService, LinkService } from './link.service';

export const ServiceProviders: Provider[] = [
    { provide: IAuthService, useClass: AuthService, multi: false },
    { provide: IStorageService, useClass: StorageService, multi: false },
    { provide: ILinkeService, useClass: LinkService, multi: false }
];