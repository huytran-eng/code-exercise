import { inject, Injectable, signal } from '@angular/core';
import { environment } from '../../environments/environment.development';
import { JwtHelperService } from '@auth0/angular-jwt';
import { HttpClient, HttpParams } from '@angular/common/http';
import { CreateTagDto } from '../_models/tag/create-tag.request.dto';
import { GetTagsResponseDTO } from '../_models/tag/get-tag.response.dto';
import { GetTagsRequestDTO } from '../_models/tag/get-tag.request.dto';
import { PaginationResult } from '../_models/common/pagination.repsonse';
import { BaseResponseDTO } from '../_models/common/base.response';
import { TagDetailDTO } from '../_models/tag/detail-tag.response.dto';
import { UpdateTagRequestDTO } from '../_models/tag/update-tag.request.dto';

@Injectable({
  providedIn: 'root',
})
export class TagService {
  private baseUrl = environment.apiUrl;
  private http = inject(HttpClient);

  paginatedResult = signal<BaseResponseDTO<GetTagsResponseDTO[]> | null>(null);

  createTag(createTagDTO: CreateTagDto) {
    const url = `${this.baseUrl}/tag/create`;
    return this.http.post<BaseResponseDTO<object>>(url, createTagDTO);
  }

  getTags(filter: GetTagsRequestDTO) {
    const url = `${this.baseUrl}/tag/get`; // match your [HttpPost("tag/search")] endpoint

    return this.http.post<BaseResponseDTO<GetTagsResponseDTO[]>>(url, filter);
  }

  getTagById(tagId: string) {
    const url = `${this.baseUrl}/tag/${tagId}`; // Replace with your actual API endpoint
    return this.http.get<BaseResponseDTO<TagDetailDTO>>(url);
  }

  updateTag(tag: UpdateTagRequestDTO) {
    const url = `${this.baseUrl}/tag/update`; // Replace with your actual API endpoint
    return this.http.post<BaseResponseDTO<object>>(url, tag);
  }
}
