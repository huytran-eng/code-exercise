namespace CodeExercise.Core.Entities
{
    public class MenuItem
    {
        public Guid Id { get; set; }
        public string MenuName { get; set; }
        public string MenuDisplayName { get; set; }
        public string MenuUrl { get; set; }
        public int IsActive { get; set; }
        public int OrderNo { get; set; }

        public Guid ParentItemId { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public bool IsDeleted { get; set; }
        public Guid? CreatedById { get; set; }
        public Guid? UpdatedById { get; set; }
        public Guid? DeletedById { get; set; }
    }
}
