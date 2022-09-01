using Microsoft.AspNetCore.Mvc.ModelBinding;
using MISA.QLSP.Common.Entities.Entities;
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
                var errorsData = new List<string>();
              foreach (var error in errors)
                {
                    // Gán thông báo cho người dùng

                    if (error == "e006")
                    {
                        errorsData.Add(Resource.ResourceValidate.AssetCode);
                    }
                    if (error == "e007")
                    {
                        errorsData.Add(Resource.ResourceValidate.AssetName);
                    }
                    if (error == "e008")
                    {
                        errorsData.Add(Resource.ResourceValidate.DepartmentID);
                    }
                    if (error == "e009")
                    {
                        errorsData.Add(Resource.ResourceValidate.CategoryID);
                    }
                    if(error == "e010")
                    {
                        errorsData.Add(Resource.ResourceValidate.MaxLength);
                    }    
                    if(error == "e011")
                    {
                        errorsData.Add(Resource.ResourceValidate.Number);
                    }    
                   
                }    
                var errorResult = new ErrorSevice();
                errorResult.DevMsg = Resource.ResourceVN.Error_ValidateData;
                errorResult.UserMsg = "Nhập dữ liệu đầy đủ";
                errorResult.data = errorsData;

                return errorResult;
            }
            return null;
        }

    }
}
