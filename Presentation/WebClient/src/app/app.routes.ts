import { Routes } from '@angular/router';
import { NotFoundComponent } from './pages/not-found/not-found.component';
import { DashboardComponent } from './pages/dashboard/dashboard.component';
import { UserLoginComponent } from './pages/auth/user-login/user-login.component';
import { UserRegisterComponent } from './pages/auth/user-register/user-register.component';
import { authGuard } from './guards/auth.guard';
import { AccountDetailComponent } from './pages/account/account-detail/account-detail.component';
import { TransactionFormComponent } from './components/transaction/transaction-form/transaction-form.component';

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
        path: 'accounts/:id', 
        component: AccountDetailComponent
    },
    { 
        path: 'accounts/:accountId/transactions/new', 
        component: TransactionFormComponent 
    },
    { 
        path: 'accounts/:accountId/transactions/:id', 
        component: TransactionFormComponent 
    },
    {
        path: '**',
        component: NotFoundComponent,
    }
];
