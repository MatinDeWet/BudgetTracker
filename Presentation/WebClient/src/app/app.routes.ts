import { Routes } from '@angular/router';
import { NotFoundComponent } from './pages/not-found/not-found.component';
import { UserRegisterComponent } from './pages/auth/user-register/user-register.component';
import { UserLoginComponent } from './pages/auth/user-login/user-login.component';
import { DashboardComponent } from './pages/dashboard/dashboard.component';
import { authGuard } from './guards/auth.guard';
import { AccountListComponent } from './pages/account/account-list/account-list.component';
import { AccountDetailComponent } from './pages/account/account-detail/account-detail.component';
import { TransactionListComponent } from './pages/transaction/transaction-list/transaction-list.component';

export const routes: Routes = [
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
        path: 'accounts', 
        component: AccountListComponent 
    },
    { 
        path: 'accounts/:id', 
        component: AccountDetailComponent 
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
