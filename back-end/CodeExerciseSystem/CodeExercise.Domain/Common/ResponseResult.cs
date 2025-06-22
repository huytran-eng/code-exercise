namespace CodeExercise.Core.Common
{
    public class ResponseResult<T>
    {
        public bool Success { get; set; }
        public int StatusCode { get; set; }
        public MessageCodeEnum MessageCode { get; set; }
        public string Message { get; set; }
        public int PageCount { get; set; }
        public int PageNumber { get; set; }
        public int TotalCount { get; set; }
        public T Data { get; set; }

        public ResponseResult() { }
        public ResponseResult(bool success, int statusCode, MessageCodeEnum messageCode, T data)
        {
            Success = success;
            StatusCode = statusCode;
            MessageCode = messageCode;
            Message = MessageMapper.GetMessage(messageCode);
            Data = data;
        }

        public static ResponseResult<T> SuccessResponse(T data, int statusCode = 200, MessageCodeEnum messageCode = MessageCodeEnum.SUCCESS)
        {
            return new ResponseResult<T>(true, statusCode, messageCode, data);
        }

        public static ResponseResult<T> FailureResponse(int statusCode, MessageCodeEnum messageCode)
        {
            return new ResponseResult<T>(false, statusCode, messageCode, default);
        }
    }
}
