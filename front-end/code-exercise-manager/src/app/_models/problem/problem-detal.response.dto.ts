import { DifficultyEnum } from '../../_utils/difficulty.enum';

export interface ProblemDTO {
  id: string;
  title: string;
  description: string;
  difficulty: DifficultyEnum;
  timeLimit: number;
  memoryLimit: number;
  slug: string;
  createdAt: Date;
  updatedAt?: Date | null;
  isDeleted: boolean;

  createdById?: string | null;
  updatedById?: string | null;
  deletedById?: string | null;

  testCases: TestCaseDTO[];
  templateCodes: TemplateCodeDTO[];
}

export interface TestCaseDTO {
  id: string;
  input: string;
  expectedOutput: string;
  inputDisplay: string;
  expectedOutputDisplay: string;
  isHidden: boolean;
  problemId: string;
  createdAt?: Date | null;
  updatedAt?: Date | null;
  isDeleted: boolean;
  createdById?: string | null;
  updatedById?: string | null;
  deletedById?: string | null;
}

export interface TemplateCodeDTO {
  id: string;
  userTemplateCode: string;
  hiddenTemplateCode: string;

  problemId: string;
  programmingLanguageId: string;
  programmingLanguageName: string;
  programmingLanguageDisplayName: string;

  createdAt?: Date | null;
  updatedAt?: Date | null;
  isDeleted: boolean;
  createdById?: string | null;
  updatedById?: string | null;
  deletedById?: string | null;
}
