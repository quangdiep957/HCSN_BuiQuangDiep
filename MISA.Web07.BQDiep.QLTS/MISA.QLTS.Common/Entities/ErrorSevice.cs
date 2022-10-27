using System.Diagnostics;

namespace MISA.QLSP.Common.Entities.Entities
{
    public class ErrorSevice:Exception
    {
        #region Property
        /// <summary>
        /// MÃ lỗi
        /// </summary>
        public int Handle { get; set; }
        /// <summary>
        /// lỗi thông báo cho dev
        /// </summary>
        public string DevMsg { get; set; }

        /// <summary>
        /// thông tin lỗi
        /// </summary>
        public string Error { get; set; }

        /// <summary>
        /// Thông báo lỗi cho người dùng
        /// </summary>
        public string UserMsg { get; set; }

        /// <summary>
        /// Danh sách lỗi
        /// </summary>
        public List<string> DataError { get; set; }

        #endregion

        #region Constructor
        /// <summary>
        /// HÀm khởi tạo
        /// </summary>
        public ErrorSevice()
        {

        }

        /// <summary>
        /// Hàm khởi tạo có tham số
        /// </summary>
        /// <param name="handle"></param>
        /// <param name="userMsg"></param>
        /// <param name="error"></param>
        /// <param name="devMsg"></param>
        /// <param name="dataError"></param>
        public ErrorSevice(int handle ,string? userMsg ,string? error, string? devMsg, List<string>? dataError)
        {
            Handle = handle;
            UserMsg = userMsg;
            Error = error;
            DevMsg = devMsg;
            DataError = dataError;
        }
        #endregion
    }

  
}
