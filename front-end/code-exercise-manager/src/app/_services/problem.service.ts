import { inject, Injectable, signal } from '@angular/core';
import { environment } from '../../environments/environment.development';
import { HttpClient } from '@angular/common/http';
import { BaseResponseDTO } from '../_models/common/base.response';
import { CreateProblemRequestDTO } from '../_models/problem/create-problem.request.dto';
import { GetProblemsRequestDTO } from '../_models/problem/get-problems.request.dto';
import { GetProblemsResponseDTO } from '../_models/problem/get-problems.response.dto';
import { ProblemDTO } from '../_models/problem/problem-detal.response.dto';
import { UpdateProblemRequestDTO } from '../_models/problem/update-problem.request.dto';

@Injectable({
  providedIn: 'root',
})
export class ProblemService {
  private baseUrl = environment.apiUrl;
  private http = inject(HttpClient);
  paginatedResult = signal<BaseResponseDTO<GetProblemsResponseDTO[]> | null>(
    null
  );

  createProblem(createProblemDTO: CreateProblemRequestDTO) {
    const url = `${this.baseUrl}/problem/create`;
    return this.http.post<BaseResponseDTO<object>>(url, createProblemDTO);
  }

  updateProblem(updateProblemDTO: UpdateProblemRequestDTO) {
    const url = `${this.baseUrl}/problem/update`;
    return this.http.post<BaseResponseDTO<object>>(url, updateProblemDTO);
  }

  getProblems(filter: GetProblemsRequestDTO) {
    const url = `${this.baseUrl}/problem/get`;
    return this.http.post<BaseResponseDTO<GetProblemsResponseDTO[]>>(
      url,
      filter
    );
  }

  getProblemById(problemId: string) {
    const url = `${this.baseUrl}/problem/${problemId}`;
    return this.http.get<BaseResponseDTO<ProblemDTO>>(url);
  }

  deleteProblem(problemId: string) {
    const url = `${this.baseUrl}/problem/delete`;
    return this.http.post<BaseResponseDTO<boolean>>(url, { id: problemId });
  }
}
