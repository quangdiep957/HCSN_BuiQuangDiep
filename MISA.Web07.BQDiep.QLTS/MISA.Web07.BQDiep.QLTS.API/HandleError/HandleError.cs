using Microsoft.AspNetCore.Mvc.ModelBinding;
using MISA.QLSP.Common.Entities.Entities;
using MySqlConnector;
using System.Diagnostics;


namespace MISA.Web07.BQDiep.QLTS.API.HandleError
{
    public class HandleError
    {
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
                        errorsdata.Add(Resource.ResourceValidate.AssetCode);
                    }
                    if (error == "e007")
                    {
                        errorsdata.Add(Resource.ResourceValidate.AssetName);
                    }
                    if (error == "e008")
                    {
                        errorsdata.Add(Resource.ResourceValidate.DepartmentID);
                    }
                    if (error == "e009")
                    {
                        errorsdata.Add(Resource.ResourceValidate.CategoryID);
                    }
                    if (error == "e010")
                    {
                        errorsdata.Add(Resource.ResourceValidate.MaxLength);
                    }
                    if (error == "e011")
                    {
                        errorsdata.Add(Resource.ResourceValidate.Number);
                    }

                }
                var errorResult = new ErrorSevice();
                errorResult.DevMsg = Resource.ResourceVN.Error_ValidateData;
                errorResult.UserMsg = Resource.ResourceValidate.Required;
                errorResult.data = errorsdata;

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
                errorResult.DevMsg = Resource.ResourceVN.Error_ValidateData;
                errorResult.Error = Resource.Error.e1002;
                errorResult.UserMsg = Resource.ResourceValidate.DoubleKey + $" {arrListStr[1]}" +" cột " + $" {arrListStr[3]} ";
                errorResult.data= arrListStr.ToList();
                return errorResult;
            }


            
            errorResult.DevMsg = Resource.ResourceVN.Error_Exception;
            errorResult.UserMsg = Resource.ResourceVN.Error_Exception;
            errorResult.data.Add("demo");
            return errorResult;

        }

        public static ErrorSevice? checkProfessional(FixedAsset record)
        {

            var errorResult = new ErrorSevice();
            var dataError = new List<string>();
            if (record != null)
            {
                if (record.depreciationRate != 1/record.lifeTime)
                {
                    dataError.Add(Resource.ResourceValidate.professional1);
                }

                if ((decimal)record.depreciationRate / record.depreciationYear > record.cost)
                {
                    dataError.Add(Resource.ResourceValidate.professional2);
                }    
            }


            return null;
        }
    }


}
