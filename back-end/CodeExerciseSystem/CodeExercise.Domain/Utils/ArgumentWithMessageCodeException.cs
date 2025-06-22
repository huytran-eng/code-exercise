using CodeExercise.Core.Common;

namespace CodeExercise.Core.Utils
{
    public class ArgumentWithMessageCodeException : ArgumentException
    {
        public MessageCodeEnum MessageCode { get; }

        public ArgumentWithMessageCodeException(MessageCodeEnum messageCode)
            : base(messageCode.ToString()) // Store the code as the message temporarily
        {
            MessageCode = messageCode;
        }
    }

}
