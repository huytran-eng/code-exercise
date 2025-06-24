import { DifficultyEnum } from '../../_utils/difficulty.enum';

export interface ProblemDetailDTO {
  id: string;
  title: string;
  description: string;
  difficulty: DifficultyEnum;
  slug: string;

  testCases: TestCaseDTO[];
  templateCodes: TemplateCodeDTO[];
}

export interface TestCaseDTO {
  id: string;
  inputDisplay: string;
  expectedOutputDisplay: string;
}

export interface TemplateCodeDTO {
  id: string;
  userTemplateCode: string;
  programmingLanguageId: string;
  programmingLanguageDisplayName: string;
  programmingLanguageName: string;

}
