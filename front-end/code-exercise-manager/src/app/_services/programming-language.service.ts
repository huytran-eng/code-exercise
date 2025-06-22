import { inject, Injectable } from '@angular/core';
import { environment } from '../../environments/environment.development';
import { HttpClient } from '@angular/common/http';
import { GetProgrammingLanguageResponseDTO } from '../_models/programming-language/programming-language.response.dto';
import { BaseResponseDTO } from '../_models/common/base.response';

@Injectable({
  providedIn: 'root',
})
export class ProgrammingLanguageService {
  private baseUrl = environment.apiUrl;
  private http = inject(HttpClient);

  getAllProgrammingLanguage() {
    const url = `${this.baseUrl}/programminglanguage/get`;
    return this.http.get<BaseResponseDTO<GetProgrammingLanguageResponseDTO[]>>(
      url
    );
  }
}
