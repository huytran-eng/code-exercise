namespace CodeExercise.Core.Common
{
    public static class ResponseMessages
    {
        /// <summary>
        /// Commonly used messages
        /// </summary>
        public const string RECORDS_NOT_FOUND = "Không tìm thấy bản ghi nào từ điều kiện tìm kiếm";
        public const string RECORDS_MAX_ROW_COUNT = "Số lượng bản ghi vượt quá 1000, hãy thu hẹp phạm vi tìm kiếm";
        public const string SYSTEM_FAILURE = "Lỗi hệ thống";
        public const string SUCCESS = "Thành công";
        public const string NOT_FOUND = "Không có kết quả";

        /// <summary>
        /// Tag related messages
        /// </summary>
        public const string CREATE_TAG_SUCCESSFUL = "Tạo tag thành công";
        public const string TAG_NAME_EXIST = "Tag với tên tương tự đã tồn tại";
        public const string TAG_NAME_EMPTY_VALIDATE = "Tên tag không được để trống";
        public const string TAG_NAME_LENGTH_VALIDATE = "Tên tag không được vượt quá 20 ký tự";
        public const string TAG_DISPLAYNAME_EMPTY_VALIDATE = "Tên hiển thị của tag không được để trống";
        public const string TAG_DISPLAYNAME_LENGTH_VALIDATE = "Tên hiển thị không được để quá 20 ký tự";
        public const string UPDATE_TAG_SUCCESSFUL = "Cập nhật thông tin tag thành công";

    }
}
