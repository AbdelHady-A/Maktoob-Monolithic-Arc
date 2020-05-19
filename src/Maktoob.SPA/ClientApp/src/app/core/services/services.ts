import { Provider } from '@angular/core';
import { AuthService, IAuthService } from './auth.service';
import { IStorageService, StorageService } from './storage.service';

export const ServiceProviders: Provider[] = [
    { provide: IAuthService, useClass: AuthService },
    { provide: IStorageService, useClass: StorageService }
];