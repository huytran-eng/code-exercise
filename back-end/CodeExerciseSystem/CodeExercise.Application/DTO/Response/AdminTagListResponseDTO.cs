namespace CodeExercise.Application.DTO.Response
{
    public class AdminTagListResponseDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string DisplayName { get; set; } 
        public string CreatedByName { get;set; }
        public Guid CreatedById { get; set; }
        public DateTime CreatedAt { get; set; } 
        public string? UpdatedByName { get; set; }
        public Guid? UpdatedById { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}