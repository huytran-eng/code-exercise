using CodeExercise.Core.Enums;

namespace CodeExercise.Core.Entities
{
    public class User : BaseModel
    {
        public string Name { get; set; }
        public string Username { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public DateTime? BirthDate { get; set; }
        public PositionEnum Position { get; set; }
        public string Note { get; set; }
    }
}
