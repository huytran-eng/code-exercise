using CodeExercise.Core.Enums;

namespace CodeExercise.Core.Entities
{
    public class UserSubscription : BaseModel
    {
        public Guid UserId { get; set; }
        public SubscriptionEnum SubscriptionType { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsActive { get; set; }
    }
}
