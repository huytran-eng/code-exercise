import { DifficultyEnum } from "../../_utils/difficulty.enum";

export interface GetProblemsResponseDTO {
  id: string;
  title: string;
  difficulty: DifficultyEnum;
  timeLimit: number;
  memoryLimit: number;
  createdByName: string;
  createdById: string;
  createdAt: Date;
  updatedByName?: string;
  updatedById?: string;
  updatedAt?: Date;
}