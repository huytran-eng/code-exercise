import { HttpClient } from '@angular/common/http';
import { Component, computed, inject, OnInit, signal } from '@angular/core';
import { Router, RouterOutlet } from '@angular/router';
import { AuthService } from './_services/auth.service';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { MatSidenavModule } from '@angular/material/sidenav';
import { SideMenuComponent } from './layout/side-menu/side-menu.component';
import { BsDropdownModule } from 'ngx-bootstrap/dropdown';
import { BreadcrumbComponent } from "./layout/breadcrumb/breadcrumb.component";
@Component({
  selector: 'app-root',
  standalone: true,
  imports: [
    RouterOutlet,
    MatToolbarModule,
    MatIconModule,
    MatButtonModule,
    MatSidenavModule,
    SideMenuComponent,
    BsDropdownModule,
    BreadcrumbComponent
],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css',
})
export class AppComponent implements OnInit {
  authService = inject(AuthService);
  http = inject(HttpClient);
  isLoggedIn = false;
  private router = inject(Router);
  title = 'code-exercise-manager';
  collapsed = signal(true);
  currentUser = '';
  ngOnInit(): void {
    this.isLoggedIn = this.authService.currentUser();
    this.currentUser = this.authService.currentUser().unique_name;
    this.router.events.subscribe(() => {
      this.isLoggedIn = this.authService.currentUser();
    });
  }

  logout() {
    this.authService.logout();
    this.router.navigate(['/login']);
  }
  sidenavWidth = computed(() => (this.collapsed() ? '0px' : '250px'));
  collapseSideNav() {
    this.collapsed.set(true);
  }
}
