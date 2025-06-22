import { inject, Injectable } from '@angular/core';
import { environment } from '../../environments/environment.development';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { MenuItem } from '../_models/layout/menu-item.layout';

@Injectable({
  providedIn: 'root',
})
export class MenuService {
  private baseUrl = environment.apiUrl;
  private http = inject(HttpClient);
  getMenuItems(): Observable<MenuItem[]> {
    return this.http.get<MenuItem[]>(this.baseUrl + '/menuitem');
  }
}
