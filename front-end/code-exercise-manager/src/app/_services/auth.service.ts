import { inject, Injectable } from '@angular/core';
import { environment } from '../../environments/environment.development';
import { JwtHelperService } from '@auth0/angular-jwt';
import { HttpClient } from '@angular/common/http';
import { tap } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  private baseUrl = environment.apiUrl;
  private jwtHelper = new JwtHelperService();
  private http = inject(HttpClient )
  
  login(userDTO: any) {
    const url = `${this.baseUrl}/Admin/login`;
    return this.http.post(url, userDTO).pipe(
      tap((res: any) => {
        if (res && res.token) {
          localStorage.setItem('token', res.token);
        }
      })
    );
  }

  logout() {
    localStorage.removeItem('token');
  }

  getToken(): string | null {
    return localStorage.getItem('token');
  }

  currentUser(): any {
    const token = localStorage.getItem('token');
    if (token) {
      const decodedUser = this.jwtHelper.decodeToken(token);
      return decodedUser || null;
    }
    return null;
  }
}
