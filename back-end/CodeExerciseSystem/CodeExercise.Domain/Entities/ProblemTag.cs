namespace CodeExercise.Core.Entities
{
    public class ProblemTag : BaseModel
    {
        public Guid ProblemId { get; set; }
        public Guid TagId { get; set; }

        public ProblemTag(Guid problemId, Guid tagId, Guid createdById)
        {
            Id = Guid.NewGuid();
            ProblemId = problemId;
            TagId = tagId;
            CreatedById = createdById;
            CreatedAt = DateTime.UtcNow;
        }

        public ProblemTag() { }
    }
}
