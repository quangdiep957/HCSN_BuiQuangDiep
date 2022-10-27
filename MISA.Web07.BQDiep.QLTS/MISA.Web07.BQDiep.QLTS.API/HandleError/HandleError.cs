using Microsoft.AspNetCore.Mvc.ModelBinding;
using MISA.QLSP.Common.Entities.Entities;
using MISA.QLTS.Common.Resource;
using MySqlConnector;
using System.Diagnostics;


namespace MISA.Web07.BQDiep.QLTS.API.HandleError
{
    public class HandleError
    {
        /// <summary>
        /// Sinh ra đối tượng lỗi trả về chưa nhập dữ liệu
        /// </summary>
        /// <param name="modelState">Trạng thái của dữ liệu</param>
        /// <returns>Đối tượng chứa thông tin lỗi trả về cho client</returns>
        /// Created by: Bùi Quang Điệp (25/08/2022) 
        public static ErrorSevice? IsValidate(ModelStateDictionary modelState)
        {

             if(!modelState.IsValid)
            {
                // Khởi tạo 1 danh sách lưu lỗi
                var errors = new List<string>();
                foreach(var item in modelState)
                {
                    foreach (var error in item.Value.Errors)
                    {
                        errors.Add(error.ErrorMessage);
                     }
                }
                var errorsdata = new List<string>();
                foreach (var error in errors)
                {
                    // gán thông báo cho người dùng

                    if (error == "e006")
                    {
                        errorsdata.Add(ResourceValidate.AssetCode);
                    }
                    if (error == "e007")
                    {
                        errorsdata.Add(ResourceValidate.AssetName);
                    }
                    if (error == "e008")
                    {
                        errorsdata.Add(ResourceValidate.DepartmentID);
                    }
                    if (error == "e009")
                    {
                        errorsdata.Add(ResourceValidate.CategoryID);
                    }
                    if (error == "e010")
                    {
                        errorsdata.Add(ResourceValidate.MaxLength);
                    }
                    if (error == "e011")
                    {
                        errorsdata.Add(ResourceValidate.Number);
                    }

                }
                var errorResult = new ErrorSevice();
                errorResult.DevMsg = ResourceVN.Error_ValidateData;
                errorResult.UserMsg = ResourceValidate.Required;
                errorResult.DataError = errorsdata;

                return errorResult;
            }
            return null;
        }

        /// <summary>
        /// Sinh ra đối tượng lỗi trả về khi gặp lỗi trùng mã
        /// </summary>
        /// <param name="exception">Đối tượng exception gặp phải</param>
        /// <param name="httpContext">Context khi gọi API sử dụng để lấy được traceId</param>
        /// <returns>Đối tượng chứa thông tin lỗi trả về cho client</returns>
        /// Created by: Bùi Quang Điệp (25/08/2022) 
        public static ErrorSevice? GenerateDuplicateCodeErrorResult(MySqlException mySqlException)
        {
            var errorResult = new ErrorSevice();
            if (mySqlException.ErrorCode == MySqlErrorCode.DuplicateKeyEntry)
            {
                var messeage = new List<string>();
                string[] arrListStr = mySqlException.Message.Split("'");
                errorResult.DevMsg = ResourceVN.Error_ValidateData;
                errorResult.Error = Error.e002;
                errorResult.UserMsg = ResourceValidate.DoubleKey + $" {arrListStr[1]}" +" cột " + $" {arrListStr[3]} ";
                errorResult.DataError = arrListStr.ToList();
                return errorResult;
            }


            
            errorResult.DevMsg = ResourceVN.Error_Exception;
            errorResult.UserMsg = ResourceVN.Error_Exception;
            errorResult.DataError.Add("");
            return errorResult;

        }

        /// <summary>
        /// Sinh ra đối tượng lỗi trả về lỗi validate nghiệp vụ
        /// </summary>
        /// <param name="record">Dữ liệu người dùng nhập vào</param>
        /// <returns>Đối tượng chứa thông tin lỗi trả về cho client</returns>
        /// Created by: Bùi Quang Điệp (25/08/2022) 
        public static ErrorSevice? checkProfessional(FixedAsset record)
        {

            var errorResult = new ErrorSevice();
            var dataError = new List<string>();
            if (record != null)
            {
                if (record.DepreciationRate != 1/record.LifeTime)
                {
                    dataError.Add(ResourceValidate.professional1);
                }

                if ((decimal)record.DepreciationRate / record.DepreciationYear > record.Cost)
                {
                    dataError.Add(ResourceValidate.professional2);
                }    
            }


            return null;
        }
    }


}
