import { PagingRequest } from '../common/pagination.request';

export interface GetProblemsRequestDTO extends PagingRequest {
  title?: string;
}
