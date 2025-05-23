import { Routes } from '@angular/router';
import { NotFoundComponent } from './pages/not-found/not-found.component';
import { UserContainerComponent } from './pages/user-container/user-container.component';
import { UserRegisterComponent } from './pages/auth/user-register/user-register.component';
import { UserLoginComponent } from './pages/auth/user-login/user-login.component';

export const routes: Routes = [
    {
        path: '',
        component: UserContainerComponent
    },
    {
        path: 'auth/register',
        component: UserRegisterComponent
    },
    {
        path: 'auth/login',
        component: UserLoginComponent
    },
    {
        path: '**',
        component: NotFoundComponent,
    }
];
