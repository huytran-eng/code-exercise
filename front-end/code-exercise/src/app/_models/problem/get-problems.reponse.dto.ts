import { DifficultyEnum } from '../../_utils/difficulty.enum';
import { SubmissionStatusEnum } from '../../_utils/submission-status.enum';

export interface GetProblemsResponseDTO {
  id: string;
  title: string;
  difficulty: DifficultyEnum;
  submissionStatus: SubmissionStatusEnum;
  slug: string;
}
