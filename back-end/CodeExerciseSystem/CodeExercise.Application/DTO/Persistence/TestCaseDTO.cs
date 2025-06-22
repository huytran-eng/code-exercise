namespace CodeExercise.Application.DTO.Persistence
{
    public class TestCaseDTO
    {
        public Guid Id { get; set; } 
        public string Input { get; set; }
        public string ExpectedOutput { get; set; }
        public string InputDisplay { get; set; }
        public string ExpectedOutputDisplay { get; set; }
        public bool IsHidden { get; set; } = false;

        public Guid ProblemId { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public bool IsDeleted { get; set; }
        public Guid? CreatedById { get; set; }
        public Guid? UpdatedById { get; set; }
        public Guid? DeletedById { get; set; }
    }
}
