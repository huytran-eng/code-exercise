import { Routes } from '@angular/router';
import { LoginComponent } from './login/login.component';
import { NotLoggedInGuard } from './_guards/not-logged-in.guard';
import { AuthGuard } from './_guards/auth.guard';
import { HomeComponent } from './home/home.component';
import { TagListComponent } from './tag/tag-list/tag-list.component';
import { CreateTagComponent } from './tag/create-tag/create-tag.component';
import { TagDetailComponent } from './tag/tag-detail/tag-detail.component';
import { TagUpdateComponent } from './tag/tag-update/tag-update.component';
import { ProblemListComponent } from './problem/problem-list/problem-list.component';
import { CreateProblemComponent } from './problem/create-problem/create-problem.component';
import { ProblemDetailComponent } from './problem/problem-detail/problem-detail.component';
import { UpdateProblemComponent } from './problem/update-problem/update-problem.component';

export const routes: Routes = [
  { path: 'login', component: LoginComponent, canActivate: [NotLoggedInGuard] },
  {
    path: '',
    canActivate: [AuthGuard],
    children: [
      { path: '', component: HomeComponent },
      {
        path: 'tag/list',
        component: TagListComponent,
      },
      {
        path: 'tag/create-tag',
        component: CreateTagComponent,
      },
      {
        path: 'tag/detail/:id',
        component: TagDetailComponent,
      },
      {
        path: 'tag/update/:id',
        component: TagUpdateComponent,
      },
      {
        path: 'problem/list',
        component: ProblemListComponent,
      },
      {
        path: 'problem/create-problem',
        component: CreateProblemComponent,
      },
      {
        path: 'problem/list',
        component: ProblemListComponent,
      },
      {
        path: 'problem/detail/:id',
        component: ProblemDetailComponent,
      },
      {
        path: 'problem/update/:id',
        component: UpdateProblemComponent,
      },
    ],
  },
];
