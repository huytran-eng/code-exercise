namespace CodeExercise.Application.DTO.Persistence
{
    public class ProblemTagDTO
    {
        public Guid Id { get; set; }
        public Guid ProblemId { get; set; }
        public Guid TagId { get; set; } 
        public string TagName { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public bool IsDeleted { get; set; }

        public Guid? CreatedById { get; set; }
        public Guid? UpdatedById { get; set; }
        public Guid? DeletedById { get; set; }
    }
}
