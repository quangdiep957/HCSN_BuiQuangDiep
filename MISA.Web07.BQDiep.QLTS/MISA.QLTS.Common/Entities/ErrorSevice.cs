using System.Diagnostics;

namespace MISA.QLSP.Common.Entities.Entities
{
    public class ErrorSevice
    {
        #region Property
        public string DevMsg { get; set; }
        public string UserMsg { get; set; }
        public List<string> data { get; set; }

        #endregion

        #region Constructor
        public ErrorSevice()
        {

        }

        public ErrorSevice(string? userMsg, string? devMsg, List<string>? data )
        {
            UserMsg = userMsg;
            DevMsg = devMsg;
            data = data;
        }
        #endregion
    }

  
}
