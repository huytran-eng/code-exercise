import { DifficultyEnum } from '../../_utils/difficulty.enum';

export interface CreateTestCaseRequestDTO {
  input: string;
  expectedOutput: string;
  inputDisplay: string;
  expectedOutputDisplay: string;
  isHidden: boolean;
}

export interface CreateTemplateCodeRequestDTO {
  programmingLanguageId: string;
  userTemplateCode: string;
  hiddenTemplateCode: string;
}

export interface CreateProblemRequestDTO {
  title: string;
  description: string;
  requirements: string;
  difficulty: DifficultyEnum;
  timeLimit: number;
  memoryLimit: number;
  templateCode: string;
  testCases: CreateTestCaseRequestDTO[];
}
