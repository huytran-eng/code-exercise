using CodeExercise.Core.Common;
using CodeExercise.Core.Utils;

namespace CodeExercise.Core.Entities
{
    public class Tag : BaseModel
    {
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public string Description { get; set; }

        public Tag(string name, string displayName, string description, Guid createdById)
        {
            Validate(name, displayName);

            Id = Guid.NewGuid();
            Name = name;
            DisplayName = displayName;
            Description = description;
            CreatedById = createdById;
            CreatedAt = DateTime.UtcNow;
        }

        public void Update(string name, string displayName, string description, Guid updatedById)
        {
            Validate(name, displayName);

            Name = name;
            DisplayName = displayName;
            Description = description;
            UpdatedById = updatedById;
            UpdatedAt = DateTime.UtcNow;
        }

        private static void Validate(string name, string displayName)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentWithMessageCodeException(MessageCodeEnum.TAG_NAME_EMPTY_VALIDATE);

            if (name.Length > 20)
                throw new ArgumentWithMessageCodeException(MessageCodeEnum.TAG_NAME_LENGTH_VALIDATE);

            if (string.IsNullOrWhiteSpace(displayName))
                throw new ArgumentWithMessageCodeException(MessageCodeEnum.TAG_DISPLAYNAME_EMPTY_VALIDATE);

            if (displayName.Length > 20)
                throw new ArgumentWithMessageCodeException(MessageCodeEnum.TAG_DISPLAYNAME_LENGTH_VALIDATE);
        }

        public Tag() { }
    }
}
