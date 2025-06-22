export interface GetProgrammingLanguageResponseDTO {
  id: string;
  name: string;
  displayName: string;
  createdByName: string;
  createdById: string;
  createdAt: Date;
  updatedByName?: string;
  updatedById?: string;
  updatedAt?: Date; // or Date
}
