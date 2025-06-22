import { PagingRequest } from '../common/pagination.request';

export interface GetTagsRequestDTO extends PagingRequest {
  name?: string;
  displayName?: string;
  fromDate?: Date;
  toDate?: Date;
}
