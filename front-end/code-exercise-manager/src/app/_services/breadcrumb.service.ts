import { ActivatedRouteSnapshot, NavigationEnd, Router } from '@angular/router';
import { Breadcrumb } from '../_models/layout/breadcrumb.layout';
import { Injectable } from '@angular/core';
import { BehaviorSubject, filter } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class BreadcrumbService {
  private readonly _breadcrumbs = new BehaviorSubject<Breadcrumb[]>([]);
  readonly breadcrumbs$ = this._breadcrumbs.asObservable();
  constructor(private router: Router) {
    this.router.events
      .pipe(filter((event) => event instanceof NavigationEnd))
      .subscribe(() => {
        const root = this.router.routerState.snapshot.root;
        const crumbs = this.buildBreadcrumbs(root);
        this._breadcrumbs.next(crumbs);
      });
  }

  private buildBreadcrumbs(
    route: ActivatedRouteSnapshot,
    url: string = '',
    breadcrumbs: Breadcrumb[] = []
  ): Breadcrumb[] {
    const routeConfig = route.routeConfig;

    if (routeConfig) {
      const routePath = this.replaceRouteParams(
        routeConfig.path ?? '',
        route.params
      );
      url += `/${routePath}`;
      if (route.data['breadcrumb']) {
        breadcrumbs.push({ label: route.data['breadcrumb'], url });
      }
    }

    if (route.firstChild) {
      return this.buildBreadcrumbs(route.firstChild, url, breadcrumbs);
    }

    return breadcrumbs;
  }

  private replaceRouteParams(path: string, params: any): string {
    return path.replace(/:([a-zA-Z]+)/g, (_, key) => params[key] ?? key);
  }
}
