import { ActionFlagEnum } from '../../_utils/action-flag.enum';
import { DifficultyEnum } from '../../_utils/difficulty.enum';

export interface UpdateTestCaseRequestDTO {
  id?: string;
  input: string;
  expectedOutput: string;
  inputDisplay: string;
  expectedOutputDisplay: string;
  isHidden: boolean;
  actionFlag: ActionFlagEnum;
}

export interface UpdateProblemRequestDTO {
  id: string;
  title: string;
  description: string;
  requirements: string;
  difficulty: DifficultyEnum;
  timeLimit: number;
  memoryLimit: number;
  templateCode: string;
  testCases: UpdateTestCaseRequestDTO[];
}
