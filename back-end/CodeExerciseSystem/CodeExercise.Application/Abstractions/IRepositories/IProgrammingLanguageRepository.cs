using CodeExercise.Application.DTO.Response;

namespace CodeExercise.Application.Abstractions.IRepositories
{
    public interface IProgrammingLanguageRepository
    {
        Task<List<AdminProgrammingLanguageListResponseDTO>> AdminGetAllProgrammingLanguagesAsync();

        /// <summary>
        /// Check if all element of a given list of programming language ids exist in the database
        /// </summary>
        /// <param name="programmingLanguageIds">given list of programming language id</param>
        /// <returns></returns>
        Task<bool> CheckListProgrammingLanguageIdsExistAsync(List<Guid> programmingLanguageIds);
    }
}
