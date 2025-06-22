namespace CodeExercise.Application.DTO.Request
{
    public class AdminUpdateTagInDTO
    {
        public required Guid Id { get; set; }
        public required string Name { get; set; }
        public required string DisplayName { get; set; }
        public string? Description { get; set; }
    }
}
