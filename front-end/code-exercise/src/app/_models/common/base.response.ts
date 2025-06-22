export interface BaseResponseDTO<T>{
    success: boolean;
    statusCode: number;
    messageCode: string;
    message: string;
    pageCount: number;
    totalCount: number;
    pageNumber: number;
    data: T;
}