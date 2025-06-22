import { inject, Injectable, signal } from '@angular/core';
import { environment } from '../../environments/environment.development';
import { HttpClient } from '@angular/common/http';
import { GetProblemsResponseDTO } from '../_models/problem/get-problems.reponse.dto';
import { BaseResponseDTO } from '../_models/common/base.response';
import { GetProblemsRequestDTO } from '../_models/problem/get-problems.request.dto';
import { ProblemDetailDTO } from '../_models/problem/problem-detail.response.dto';

@Injectable({
  providedIn: 'root',
})
export class ProblemService {
  private baseUrl = environment.apiUrl;
  private http = inject(HttpClient);
  paginatedResult = signal<BaseResponseDTO<GetProblemsResponseDTO[]> | null>(
    null
  );

  getProblems(filter: GetProblemsRequestDTO) {
    const url = `${this.baseUrl}/problem/list`;
    return this.http.post<BaseResponseDTO<GetProblemsResponseDTO[]>>(
      url,
      filter
    );
  }

  getProblemBySlug(slug: string) {
    const url = `${this.baseUrl}/problem/${slug}`;
    return this.http.get<BaseResponseDTO<ProblemDetailDTO>>(url);
  }
}
