using CodeExercise.Core.Common;
using CodeExercise.Core.Enums;
using CodeExercise.Core.Utils;

namespace CodeExercise.Core.Entities
{
    public class Problem : BaseModel
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public DifficultyEnum Difficulty { get; set; }
        public int TimeLimit { get; set; }
        public int MemoryLimit { get; set; }
        public string Slug { get; set; }

        public Problem(string title, string description, DifficultyEnum difficulty, int timeLimit, int memoryLimit, Guid createdById)
        {
            Validate(title, description, timeLimit, memoryLimit);

            Id = Guid.NewGuid();
            Title = title;
            Description = description;
            Difficulty = difficulty;
            TimeLimit = timeLimit;
            MemoryLimit = memoryLimit;
            Slug = CommonMethod.GenerateSlug(title);
            CreatedById = createdById;
            Description = description;
            CreatedAt = DateTime.UtcNow;
        }

        public void Update(string title, string description, DifficultyEnum difficulty, int timeLimit, int memoryLimit,Guid updatedById)
        {
            Validate(title, description, timeLimit, memoryLimit);

            Title = title;
            Description = description;
            Difficulty = difficulty;
            TimeLimit = timeLimit;
            MemoryLimit = memoryLimit;
            Slug = CommonMethod.GenerateSlug(title);
            UpdatedById = updatedById;
            UpdatedAt = DateTime.UtcNow;
        }

        public void Delete(Guid deletedById)
        {
            IsDeleted = true;
            DeletedAt = DateTime.UtcNow;
            DeletedById = deletedById;
        }
        private static void Validate(string title, string description, int timeLimit, int memoryLimit)
        {
            if (string.IsNullOrWhiteSpace(title))
                throw new ArgumentWithMessageCodeException(MessageCodeEnum.PROBLEM_TITLE_EMPTY_VALIDATE);
            if (title.Length > 100)
                throw new ArgumentWithMessageCodeException(MessageCodeEnum.PROBLEM_TITLE_MAX_LENGTH_VALIDATE);
            if (title.Length < 3)
                throw new ArgumentWithMessageCodeException(MessageCodeEnum.PROBLEM_TITLE_MIN_LENGTH_VALIDATE);
            if (string.IsNullOrEmpty(description))
                throw new ArgumentWithMessageCodeException(MessageCodeEnum.PROBLEM_DESCRIPTION_EMPTY_VALIDATE);
            if (timeLimit < 0)
                throw new ArgumentWithMessageCodeException(MessageCodeEnum.PROBLEM_TIME_LIMIT_INVALID);
            if (memoryLimit < 0)
                throw new ArgumentWithMessageCodeException(MessageCodeEnum.PROBLEM_MEMORY_LIMIT_INVALID);
          
        }

        public Problem() { }
    }
}
