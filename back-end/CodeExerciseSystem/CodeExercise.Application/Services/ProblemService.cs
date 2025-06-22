using CodeExercise.Application.Abstractions.IRepositories;
using CodeExercise.Application.Abstractions.IServices;
using CodeExercise.Application.DTO.Persistence;
using CodeExercise.Application.DTO.Request;
using CodeExercise.Application.DTO.Response;
using CodeExercise.Application.EntityMapper;
using CodeExercise.Core.Common;
using CodeExercise.Core.Entities;
using CodeExercise.Core.Enums;
using CodeExercise.Core.Utils;
using Microsoft.Extensions.Logging;

namespace CodeExercise.Application.Services
{
    public class ProblemService : IProblemService
    {
        private readonly ILogger<ProblemService> _logger;
        private readonly IProblemRepository _problemRepository;
        private readonly ITagRepository _tagRepository;
        private readonly IProgrammingLanguageRepository _programmingLanguageRepository;
        private readonly ITestCaseRepository _testCaseRepository;
        public ProblemService(ILogger<ProblemService> logger, IProblemRepository problemRepository, ITagRepository tagRepository, IProgrammingLanguageRepository programmingLanguageRepository, ITestCaseRepository testCaseRepository)
        {
            _logger = logger;
            _problemRepository = problemRepository;
            _tagRepository = tagRepository;
            _programmingLanguageRepository = programmingLanguageRepository;
            _testCaseRepository = testCaseRepository;
        }

        public async Task<ResponseResult<List<AdminProblemListResponseDTO>>> AdminGetProblemsAsync(AdminProblemListInDTO adminProblemListInDTO)
        {
            try
            {
                if (adminProblemListInDTO.PageSize > 1000)
                {
                    return ResponseResult<List<AdminProblemListResponseDTO>>.FailureResponse(400, MessageCodeEnum.RECORDS_MAX_ROW_COUNT);

                }
                var res = new ResponseResult<List<AdminProblemListResponseDTO>>();
                var (problems, totalCount) = await _problemRepository.AdminGetProblemsAsync(adminProblemListInDTO);
                if (problems == null || !problems.Any())
                {
                    return ResponseResult<List<AdminProblemListResponseDTO>>.FailureResponse(404, MessageCodeEnum.RECORDS_NOT_FOUND);
                }
                else
                {
                    res.Data = problems;
                    res.Success = true;
                    res.TotalCount = totalCount;
                    res.StatusCode = 200;
                    res.PageNumber = adminProblemListInDTO.PageNumber;
                    res.PageCount = (int)Math.Ceiling((double)totalCount / adminProblemListInDTO.PageSize);
                    return res;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred when getting tags {ex.Message}");
                return ResponseResult<List<AdminProblemListResponseDTO>>.FailureResponse(500, MessageCodeEnum.SYSTEM_FAILURE);
            }
        }

        public async Task<ResponseResult<ProblemDTO>> AdminGetProblemDetailAsync(Guid problemId)
        {
            try
            {
                var problem = await _problemRepository.AdminGetProblemDetailByIdAsync(problemId);
                if (problem == null)
                {
                    return ResponseResult<ProblemDTO>.FailureResponse(404, MessageCodeEnum.NOT_FOUND);
                }
                else
                {
                    return ResponseResult<ProblemDTO>.SuccessResponse(problem, 200, MessageCodeEnum.SUCCESS);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred when getting tag detail {ex.Message}");
                return ResponseResult<ProblemDTO>.FailureResponse(500, MessageCodeEnum.SYSTEM_FAILURE);
            }
        }

        /// <summary>
        /// Create new problem
        /// </summary>
        /// <param name="problemDTO">information of the problem will be created</param>
        /// <param name="userId">logged in user id</param>
        /// <returns></returns>
        public async Task<ResponseResult<Guid>> AdminCreateProblemAsync(AdminCreateProblemInDTO problemDTO, Guid userId)
        {
            try
            {
                // Create the Problem entity with constructor validation
                var problem = new Problem(problemDTO.Title, problemDTO.Description, problemDTO.Difficulty, problemDTO.TimeLimit, problemDTO.MemoryLimit, userId);

                // validate the test cases
                var testCases = problemDTO.TestCases.Select(tc =>
                       new TestCase(tc.Input, tc.ExpectedOutput, tc.InputDisplay, tc.ExpectedOutputDisplay, userId, problem.Id, tc.IsHidden)
                   ).ToList();
                if (testCases == null || !testCases.Any())
                {
                    return ResponseResult<Guid>.FailureResponse(400, MessageCodeEnum.PROBLEM_TESTCASES_EMPTY_VALIDATE);
                }

                //// Validate tag list
                //var tagIds = problemDTO.TagIds;
                //if (tagIds == null || !tagIds.Any())
                //{
                //    return ResponseResult<Guid>.FailureResponse(400, MessageCodeEnum.PROBLEM_TAGS_EMPTY_VALIDATE);
                //}

                //bool tagHasDuplicates = tagIds.GroupBy(x => x)
                //                    .Any(g => g.Count() > 1);
                //if (tagHasDuplicates)
                //{
                //    return ResponseResult<Guid>.FailureResponse(400, MessageCodeEnum.PROBLEM_TAGS_DUPLICATE_VALIDATE);
                //}

                //var checkAllTagsExist = await _tagRepository.CheckListTagsExistAsync(tagIds);
                //if (!checkAllTagsExist)
                //{
                //    return ResponseResult<Guid>.FailureResponse(404, MessageCodeEnum.TAG_ID_NOT_EXIST_VALIDATE);
                //}

                //var problemTags = tagIds.Select(ti => new ProblemTag(problem.Id, ti, userId)).ToList();

                // Validate template codes
                var templateCodes = problemDTO.TemplateCodes.Select(tplc => new TemplateCode(tplc.UserTemplateCode,tplc.HiddenTemplateCode ,tplc.ProgrammingLanguageId, problem.Id, userId)).ToList();
                if (templateCodes == null || !templateCodes.Any())
                {
                    return ResponseResult<Guid>.FailureResponse(400, MessageCodeEnum.PROBLEM_TEMPLATE_CODE_EMPTY_VALIDATE);
                }

                var templateCodeProgrammingLanguageIds = problemDTO.TemplateCodes.Select(tplc => tplc.ProgrammingLanguageId).ToList();
                bool templateCodeProgrammingLanguageHasDuplicates = templateCodeProgrammingLanguageIds.GroupBy(x => x)
                                   .Any(g => g.Count() > 1);
                if (templateCodeProgrammingLanguageHasDuplicates)
                {
                    return ResponseResult<Guid>.FailureResponse(400, MessageCodeEnum.PROBLEM_TEMPLATE_CODE_LANGUAGE_DUPLICATE_VALIDATE);
                }

                var checkAllProgrammingLanguageIdsExist = await _programmingLanguageRepository.CheckListProgrammingLanguageIdsExistAsync(templateCodeProgrammingLanguageIds);
                if (!checkAllProgrammingLanguageIdsExist)
                {
                    return ResponseResult<Guid>.FailureResponse(404, MessageCodeEnum.PROGRAMMING_LANGUAGE_ID_NOT_EXIST_VALIDATE);
                }

                // Insert the problem into the database
                var createdProblemId = await _problemRepository.AdminCreateProblemAsync(problem, testCases, templateCodes);

                // If problem creation is successful, return the DTO
                return ResponseResult<Guid>.SuccessResponse(createdProblemId, 201, MessageCodeEnum.CREATE_PROBLEM_SUCCESSFUL);
            }
            catch (ArgumentWithMessageCodeException ex)
            {
                var message = MessageMapper.GetMessage(ex.MessageCode);
                _logger.LogError(ex, $"Validation failed when creating problem: {message}");
                return ResponseResult<Guid>.FailureResponse(400, ex.MessageCode);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while creating the problem: {ex.Message}");
                return ResponseResult<Guid>.FailureResponse(500, MessageCodeEnum.SYSTEM_FAILURE);
            }
        }

        /// <summary>
        /// updating a problem with its tags, template codes, test cases
        /// </summary>
        /// <param name="updateProblemDTO">problem updated information from payload</param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<ResponseResult<bool>> AdminUpdateProblemAsync(AdminUpdateProblemInDTO updateProblemDTO, Guid userId)
        {
            try
            {
                // check if problem exist
                var problemDTO = await _problemRepository.AdminGetProblemDetailByIdAsync(updateProblemDTO.Id);

                if (problemDTO == null)
                {
                    return ResponseResult<bool>.FailureResponse(404, MessageCodeEnum.RECORDS_NOT_FOUND);
                }

                var problemEntity = problemDTO.ToEntity();

                //update the problem entity
                problemEntity.Update(updateProblemDTO.Title, updateProblemDTO.Description, updateProblemDTO.Difficulty, updateProblemDTO.TimeLimit, updateProblemDTO.MemoryLimit, userId);


                // validate the given list of test case and set the test case list for updating
                var testCaseValidate = ValidateAndUpdateTestCasesForProblem(updateProblemDTO, problemDTO, userId);

                if (!testCaseValidate.Success)
                {
                    return ResponseResult<bool>.FailureResponse(testCaseValidate.StatusCode, testCaseValidate.MessageCode);
                }

                var updateTestCase = testCaseValidate.Data;

                // get all the new test case
                var insertTestCases = updateProblemDTO.TestCases
                    .Where(tc => tc.ActionFlag == ActionFlagEnum.Insert)
                    .Select(tc =>
                       new TestCase(tc.Input, tc.ExpectedOutput, tc.InputDisplay, tc.ExpectedOutputDisplay, userId, updateProblemDTO.Id, tc.IsHidden)
                   ).ToList();

                var existingTemplateCodes = problemDTO.TemplateCodes;

                var insertTemplateCodes = new List<TemplateCode>();
                var updateTemplateCodes = new List<TemplateCode>();

                foreach (var tplc in updateProblemDTO.TemplateCodes)
                {
                    if (tplc.ActionFlag == ActionFlagEnum.Insert)
                    {
                        // Check if already exists for the same language
                        var exists = existingTemplateCodes.Any(e => e.ProgrammingLanguageId == tplc.ProgrammingLanguageId);
                        if (exists)
                        {
                            return ResponseResult<bool>.FailureResponse(400, MessageCodeEnum.TEMPLATE_CODE_ALREADY_EXISTS);
                        }

                        insertTemplateCodes.Add(new TemplateCode(
                            tplc.UserTemplateCode,
                            tplc.HiddenTemplateCode,
                            tplc.ProgrammingLanguageId,
                            problemDTO.Id,
                            userId
                        ));
                    }
                    else if (tplc.ActionFlag == ActionFlagEnum.Update && tplc.Id.HasValue)
                    {
                        var existingTpl = existingTemplateCodes.FirstOrDefault(e => e.Id == tplc.Id.Value);
                        if (existingTpl == null)
                        {
                            return ResponseResult<bool>.FailureResponse(404, MessageCodeEnum.TEMPLATE_CODE_NOT_FOUND);
                        }

                        if (existingTpl.ProgrammingLanguageId != tplc.ProgrammingLanguageId)
                        {
                            return ResponseResult<bool>.FailureResponse(400, MessageCodeEnum.LANGUAGE_ID_MISMATCH);
                        }

                        var templateCode = existingTpl.ToEntity();
                        templateCode.Update(tplc.UserTemplateCode, tplc.HiddenTemplateCode, userId);
                        updateTemplateCodes.Add(templateCode); 
                    }else if(tplc.ActionFlag == ActionFlagEnum.NoAction)
                    {
                        continue;
                    }
                    else
                    {
                        return ResponseResult<bool>.FailureResponse(400, MessageCodeEnum.INVALID_ACTION_FLAG);
                    }
                }


                var result = await _problemRepository.AdminUpdateProblemAsync(problemEntity, updateTestCase, insertTestCases, updateTemplateCodes, insertTemplateCodes);
                if (result == false)
                {
                    return ResponseResult<bool>.FailureResponse(400, MessageCodeEnum.SYSTEM_FAILURE);
                }

                return ResponseResult<bool>.SuccessResponse(result, 201, MessageCodeEnum.UPDATE_PROBLEM_SUCCESSFUL);
            }
            catch (ArgumentWithMessageCodeException ex)
            {
                var message = MessageMapper.GetMessage(ex.MessageCode);
                _logger.LogError(ex, $"Validation failed when creating problem: {message}");
                return ResponseResult<bool>.FailureResponse(400, ex.MessageCode);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while creating the problem: {ex.Message}");
                return ResponseResult<bool>.FailureResponse(500, MessageCodeEnum.SYSTEM_FAILURE);
            }
        }

        public async Task<ResponseResult<bool>> AdminDeleteProblemAsync(Guid problemId, Guid userId)
        {
            try
            {
                var problemDTO = await _problemRepository.AdminGetProblemDetailByIdAsync(problemId);

                if (problemDTO == null)
                {
                    return ResponseResult<bool>.FailureResponse(404, MessageCodeEnum.RECORDS_NOT_FOUND);
                }

                var problemEntity = problemDTO.ToEntity();
                problemEntity.Delete(userId);
                var result = await _problemRepository.AdminDeleteProblemAsync(problemEntity);
                if (!result)
                {
                    return ResponseResult<bool>.FailureResponse(400, MessageCodeEnum.SYSTEM_FAILURE);
                }
                return ResponseResult<bool>.SuccessResponse(result, 201, MessageCodeEnum.UPDATE_PROBLEM_SUCCESSFUL);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while creating the problem: {ex.Message}");
                return ResponseResult<bool>.FailureResponse(500, MessageCodeEnum.SYSTEM_FAILURE);
            }
        }

        public async Task<ResponseResult<List<UserProblemListResponseDTO>>> UserGetProblemsAsync(UserProblemListInDTO filter, Guid? userId)
        {
            try
            {
                if (filter.PageSize > 1000)
                {
                    return ResponseResult<List<UserProblemListResponseDTO>>.FailureResponse(400, MessageCodeEnum.RECORDS_MAX_ROW_COUNT);

                }
                var res = new ResponseResult<List<UserProblemListResponseDTO>>();
                var (problems, totalCount) = await _problemRepository.UserGetProblemsAsync(filter, userId);
                if (problems == null || !problems.Any())
                {
                    return ResponseResult<List<UserProblemListResponseDTO>>.FailureResponse(404, MessageCodeEnum.RECORDS_NOT_FOUND);
                }
                else
                {
                    res.Data = problems;
                    res.Success = true;
                    res.TotalCount = totalCount;
                    res.StatusCode = 200;
                    res.PageNumber = filter.PageNumber;
                    res.PageCount = (int)Math.Ceiling((double)totalCount / filter.PageSize);
                    return res;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred when getting tags {ex.Message}");
                return ResponseResult<List<UserProblemListResponseDTO>>.FailureResponse(500, MessageCodeEnum.SYSTEM_FAILURE);
            }
        }

        public async Task<ResponseResult<UserProblemDetailResponseDTO>> UserGetProblemDetailAsync(string slug)
        {
            try
            {
                var problem = await _problemRepository.UserGetProblemDetailBySlugAsync(slug);
                if (problem == null)
                {
                    return ResponseResult<UserProblemDetailResponseDTO>.FailureResponse(404, MessageCodeEnum.NOT_FOUND);
                }
                else
                {
                    return ResponseResult<UserProblemDetailResponseDTO>.SuccessResponse(problem, 200, MessageCodeEnum.SUCCESS);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred when getting tag detail {ex.Message}");
                return ResponseResult<UserProblemDetailResponseDTO>.FailureResponse(500, MessageCodeEnum.SYSTEM_FAILURE);
            }
        }


        /// <summary>
        /// validate and return list of changed (update, delete) test cases when update a problem
        /// </summary>
        /// <param name="updateProblemDTO">payload of the problem sent from front end</param>
        /// <param name="problemDTO">the current problem stored in the database</param>
        /// <param name="userId">logged in user</param>
        /// <returns></returns>
        private ResponseResult<List<TestCase>> ValidateAndUpdateTestCasesForProblem(AdminUpdateProblemInDTO updateProblemDTO, ProblemDTO problemDTO, Guid userId)
        {
            try
            {
                var updateTestCaseIds = updateProblemDTO.TestCases
                    .Where(tc => tc.ActionFlag == ActionFlagEnum.Update && tc.Id.HasValue)
                    .Select(tc => tc.Id.Value).ToList();

                var deleteTestCaseIds = updateProblemDTO.TestCases
                    .Where(tc => tc.ActionFlag == ActionFlagEnum.Delete && tc.Id.HasValue)
                    .Select(tc => tc.Id.Value).ToList();

                var existingTestCaseIds = problemDTO.TestCases.Select(tc => tc.Id).ToHashSet();

                bool hasInvalidUpdateIds = updateTestCaseIds.Any(id => !existingTestCaseIds.Contains(id));
                bool hasInvalidDeleteIds = deleteTestCaseIds.Any(id => !existingTestCaseIds.Contains(id));


                if (hasInvalidUpdateIds || hasInvalidDeleteIds)
                {
                    return ResponseResult<List<TestCase>>.FailureResponse(400, MessageCodeEnum.TEST_CASE_INVALID_REFERENCE);
                }

                var testCaseToBeChanged = new List<TestCase>();

                var updateTestCaseDTOs = updateProblemDTO.TestCases.Where(tc => tc.ActionFlag == ActionFlagEnum.Update && tc.Id.HasValue).ToList();
                foreach (var updateTestCaseDTO in updateTestCaseDTOs)
                {
                    var updatingTestCaseEntity = problemDTO.TestCases.FirstOrDefault(tc => tc.Id == updateTestCaseDTO.Id).ToEntity();
                    updatingTestCaseEntity.Update(updateTestCaseDTO.Input, updateTestCaseDTO.ExpectedOutput, updateTestCaseDTO.InputDisplay, updateTestCaseDTO.ExpectedOutputDisplay, updateTestCaseDTO.IsHidden, userId);
                    testCaseToBeChanged.Add(updatingTestCaseEntity);
                }

                var deleteTestCaseDTOs = updateProblemDTO.TestCases.Where(tc => tc.ActionFlag == ActionFlagEnum.Delete && tc.Id.HasValue).ToList();
                foreach (var deleteTestCaseDTO in deleteTestCaseDTOs)
                {
                    var deletingTestCaseEntity = problemDTO.TestCases.FirstOrDefault(tc => tc.Id == deleteTestCaseDTO.Id).ToEntity();
                    deletingTestCaseEntity.Delete(userId);
                    testCaseToBeChanged.Add(deletingTestCaseEntity);
                }
                return ResponseResult<List<TestCase>>.SuccessResponse(testCaseToBeChanged, 200, MessageCodeEnum.SUCCESS);
            }
            catch (ArgumentWithMessageCodeException ex)
            {
                var message = MessageMapper.GetMessage(ex.MessageCode);
                _logger.LogError(ex, $"Validation failed when creating problem: {message}");
                return ResponseResult<List<TestCase>>.FailureResponse(400, ex.MessageCode);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while creating the problem: {ex.Message}");
                return ResponseResult<List<TestCase>>.FailureResponse(500, MessageCodeEnum.SYSTEM_FAILURE);
            }
        }



        //private ResponseResult<List<TemplateCode>> ValidateAndUpdateTemplateCodesForProblem(AdminUpdateProblemInDTO updateProblemDTO, ProblemDTO problemDTO, Guid userId)
        //{
        //    try
        //    {
        //        var updateTemplateCodeIds = updateProblemDTO.TemplateCodes
        //            .Where(tplc => tplc.ActionFlag == ActionFlagEnum.Update && tplc.Id.HasValue)
        //            .Select(tplc => tplc.Id.Value).ToList();

        //        var deleteTemplateCodeIds = updateProblemDTO.TemplateCodes
        //           .Where(tplc => tplc.ActionFlag == ActionFlagEnum.Delete && tplc.Id.HasValue)
        //           .Select(tplc => tplc.Id.Value).ToList();

        //        var existingTemplateCodeIds = problemDTO.ProblemTemplateCodes.Select(tplc => tplc.Id).ToHashSet();

        //        bool hasInvalidUpdateIds = updateTemplateCodeIds.Any(id => !existingTemplateCodeIds.Contains(id));
        //        bool hasInvalidDeleteIds = deleteTemplateCodeIds.Any(id => !deleteTemplateCodeIds.Contains(id));

        //        if (hasInvalidUpdateIds || hasInvalidDeleteIds)
        //        {
        //            return ResponseResult<List<TemplateCode>>.FailureResponse(400, MessageCodeEnum.TEST_CASE_INVALID_REFERENCE);
        //        }

        //        var templateCodesToBeChanged = new List<TemplateCode>();

        //        var updateTemplateCodeDTOs = updateProblemDTO.TemplateCodes.Where(tc => tc.ActionFlag == ActionFlagEnum.Update && tc.Id.HasValue).ToList();
        //        foreach (var updateTemplateCodeDTO in updateTemplateCodeDTOs)
        //        {
        //            var updatingTemplateCodeEntity = problemDTO.ProblemTemplateCodes.FirstOrDefault(tc => tc.Id == updateTemplateCodeDTO.Id).ToEntity();
        //            updatingTemplateCodeEntity.Update(updateTemplateCodeDTO.Code, userId);
        //            templateCodesToBeChanged.Add(updatingTemplateCodeEntity);
        //        }

        //        var deleteTemplateCodeDTOs = updateProblemDTO.TemplateCodes.Where(tc => tc.ActionFlag == ActionFlagEnum.Delete && tc.Id.HasValue).ToList();
        //        foreach (var deleteTemplateCodeDTO in deleteTemplateCodeDTOs)
        //        {
        //            var deletingTestCaseEntity = problemDTO.ProblemTemplateCodes.FirstOrDefault(tc => tc.Id == deleteTemplateCodeDTO.Id).ToEntity();
        //            deletingTestCaseEntity.Delete(userId);
        //            templateCodesToBeChanged.Add(deletingTestCaseEntity);
        //        }

        //        return ResponseResult<List<TemplateCode>>.SuccessResponse(templateCodesToBeChanged, 200, MessageCodeEnum.SUCCESS);
        //    }
        //    catch (ArgumentWithMessageCodeException ex)
        //    {
        //        var message = MessageMapper.GetMessage(ex.MessageCode);
        //        _logger.LogError(ex, $"Validation failed when creating problem: {message}");
        //        return ResponseResult<List<TemplateCode>>.FailureResponse(400, ex.MessageCode);
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, $"An error occurred while creating the problem: {ex.Message}");
        //        return ResponseResult<List<TemplateCode>>.FailureResponse(500, MessageCodeEnum.SYSTEM_FAILURE);
        //    }
        //}


    }
}
