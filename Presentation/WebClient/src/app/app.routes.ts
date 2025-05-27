import { Routes } from '@angular/router';
import { NotFoundComponent } from './pages/not-found/not-found.component';
import { DashboardComponent } from './pages/dashboard/dashboard.component';
import { UserLoginComponent } from './pages/auth/user-login/user-login.component';
import { UserRegisterComponent } from './pages/auth/user-register/user-register.component';
import { authGuard } from './guards/auth.guard';

export const routes: Routes = [
    {
        path: 'auth/login',
        component: UserLoginComponent
    },
    {
        path: 'auth/register',
        component: UserRegisterComponent
    },
    {
        path: '',
        redirectTo: '/dashboard',
        pathMatch: 'full'
    },
    {
        path: 'dashboard',
        component: DashboardComponent,
        canActivate: [authGuard]
    },
    {
        path: '**',
        component: NotFoundComponent,
    }
];
