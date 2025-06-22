export interface PaginationResponse{
    pageNumber: number;
    pageSize: number;
    totalItgems: number;
    totalPages: number;
}

export class PaginationResult<T> {
    items?: T;
    pagination?: PaginationResponse;
}