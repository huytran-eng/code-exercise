export interface TagDetailDTO{
    id: string;
    name: string;
    displayName: string;
    description: string;
    createdAt: Date;
    updatedAt: Date;
    createdById: string;
    updatedById: string;
    createdByName: string;
    updatedByName: string;
    isDeleted: boolean;
}