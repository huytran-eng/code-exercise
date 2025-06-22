using System.Text.Json;

namespace CodeExercise.Core.Common
{
    public static class MessageMapper
    {
        private static readonly Dictionary<string, string> _messages;

        static MessageMapper()
        {
            var json = File.ReadAllText("C:\\Users\\h\\Desktop\\CodeExercise\\back-end\\CodeExerciseSystem\\CodeExercise.Domain\\Common\\message-mapping.json");
            _messages = JsonSerializer.Deserialize<Dictionary<string, string>>(json) ?? new();
        }

        public static string GetMessage(MessageCodeEnum code)
        {
            var key = code.ToString();
            return _messages.TryGetValue(key, out var message) ? message : "Unknown message code.";
        }
    }
}
