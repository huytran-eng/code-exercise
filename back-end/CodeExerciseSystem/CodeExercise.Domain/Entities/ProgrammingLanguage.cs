namespace CodeExercise.Core.Entities
{
    public class ProgrammingLanguage:BaseModel
    {
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public string Description { get; set; }
    }
}
