import { Component, inject, OnInit, signal } from '@angular/core';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { MatInputModule } from '@angular/material/input';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatMenuModule } from '@angular/material/menu';
import { MatListModule } from '@angular/material/list';
import { AuthService } from '../../_services/auth.service';
import { HttpClient } from '@angular/common/http';
import { NavigationEnd, Router } from '@angular/router';
import { BsDropdownModule } from 'ngx-bootstrap/dropdown';

@Component({
  selector: 'app-navbar',
  standalone: true,
  imports: [
    MatListModule,
    MatToolbarModule,
    MatIconModule,
    MatButtonModule,
    MatInputModule,
    MatFormFieldModule,
    MatMenuModule,
    BsDropdownModule,
  ],
  templateUrl: './navbar.component.html',
  styleUrl: './navbar.component.css',
})
export class NavbarComponent implements OnInit {
  private authService = inject(AuthService);
  private http = inject(HttpClient);
  private router = inject(Router);

  isLoggedIn: boolean = false;
  currentUserName: string = '';

  ngOnInit(): void {
    const user = this.authService.currentUser();
    this.isLoggedIn = !!user;

    if (user) {
      this.currentUserName = user.unique_name ?? ''; // Optional chaining
    }

    // Listen to route changes to update login status dynamically
    this.router.events.subscribe((event) => {
      if (event instanceof NavigationEnd) {
        const user = this.authService.currentUser();
        this.isLoggedIn = !!user;
        this.currentUserName = user?.unique_name ?? '';
      }
    });
  }
  logout() {
    this.authService.logout();
    this.router.navigate(['/login']);
  }
}
