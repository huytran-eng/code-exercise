export interface GetTagsResponseDTO{
    id: string;
    name: string;
    displayName: string;
    createdAt: Date;
    updatedAt: Date;
    createdById: string;
    updatedById: string;
    createdByName: string;
    updatedByName: string;
    isDeleted: boolean;
}