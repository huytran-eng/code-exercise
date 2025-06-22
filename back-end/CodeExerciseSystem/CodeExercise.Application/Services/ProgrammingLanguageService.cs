using CodeExercise.Application.Abstractions.IRepositories;
using CodeExercise.Application.Abstractions.IServices;
using CodeExercise.Application.DTO.Response;
using CodeExercise.Core.Common;
using Microsoft.Extensions.Logging;

namespace CodeExercise.Application.Services
{
    public class ProgrammingLanguageService : IProgrammingLanguageService
    {
        private readonly ILogger<ProgrammingLanguageService> _logger;
        private readonly IProgrammingLanguageRepository _programmingLanguageRepository;
        public ProgrammingLanguageService(ILogger<ProgrammingLanguageService> logger, IProgrammingLanguageRepository programmingLanguageRepository)
        {
            _logger = logger;
            _programmingLanguageRepository = programmingLanguageRepository;
        }

        public async Task<ResponseResult<List<AdminProgrammingLanguageListResponseDTO>>> AdminGetAllProgrammingLanguagesAsync()
        {
            try
            {
                var programmingLanguages = await _programmingLanguageRepository.AdminGetAllProgrammingLanguagesAsync();

                if (programmingLanguages == null || !programmingLanguages.Any())
                {
                    return ResponseResult<List<AdminProgrammingLanguageListResponseDTO>>.FailureResponse(404, MessageCodeEnum.RECORDS_NOT_FOUND);
                }

                return ResponseResult<List<AdminProgrammingLanguageListResponseDTO>>.SuccessResponse(programmingLanguages, 200, MessageCodeEnum.SUCCESS);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating the problem");
                return ResponseResult<List<AdminProgrammingLanguageListResponseDTO>>.FailureResponse(500, MessageCodeEnum.SYSTEM_FAILURE);
            }

        }
    }
}
