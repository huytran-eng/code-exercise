namespace CodeExercise.Application.DTO.Response
{
    public class AdminMenuItemListResponseDTO
    {
        public Guid Id { get; set; }
        public string? MenuName { get; set; }
        public string? MenuDisplayName { get; set; }
        public string? MenuUrl { get; set; }
        public int IsActive { get; set; }
        public int OrderNo { get; set; }
        public List<AdminMenuItemListResponseDTO> ChildItems { get; set; }
        public Guid? ParentItemId { get; set; }
    }
} 
