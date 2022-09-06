using System.Diagnostics;

namespace MISA.QLSP.Common.Entities.Entities
{
    public class ErrorSevice
    {
        #region Property
        public string DevMsg { get; set; }

        public string Error { get; set; }
        public string UserMsg { get; set; }
        public List<string> data { get; set; }

        #endregion

        #region Constructor
        public ErrorSevice()
        {

        }

        public ErrorSevice(string? userMsg ,string? error, string? devMsg, List<string>? data )
        {
            UserMsg = userMsg;
            Error = error;
            DevMsg = devMsg;
            data = data;
        }
        #endregion
    }

  
}
