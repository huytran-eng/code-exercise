import { Routes } from '@angular/router';
import { LoginComponent } from './login/login.component';
import { ProblemListComponent } from './problem/problem-list/problem-list.component';
import { ProblemDetailComponent } from './problem/problem-detail/problem-detail.component';

export const routes: Routes = [
  { path: 'login', component: LoginComponent },
  { path: 'problem', component: ProblemListComponent },
  { path: 'problem/:slug', component: ProblemDetailComponent },
];
