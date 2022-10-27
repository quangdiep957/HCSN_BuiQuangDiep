using Dapper;
using MISA.QLSP.Common.Entities.Entities;
using MISA.QLTS.Common.Entities;
using MISA.QLTS.DL;
using MISA.QLTS.Common.Enum;
using MISA.QLTS.Common.Resource;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Microsoft.IdentityModel.Tokens;
using System.Text.RegularExpressions;

namespace MISA.QLTS.BL.BaseBL
{
    public class BaseBL<T> : IBaseBL<T>
    {
        #region Field
        private IBaseDL<T> _baseDL;
        const int dozens = 10;
        const int hundreds = 100;
        #endregion

        #region constructor
        public BaseBL(IBaseDL<T> baseDL)
        {
            _baseDL = baseDL;

        }


        #endregion

        #region method
        /// <summary>
        /// Xóa một bản ghi trong 1 trang
        /// </summary>
        /// <returns>
        /// 200 kết quả thành công
        /// </returns>
        /// Createed By :Bùi Quang Điệp (23/08/2022)
        public int Delete(List<Guid> id)
        {
            return _baseDL.Delete(id);
        }

        /// <summary>
        /// Lất tất cả bảng ghi trong 1 bảng
        /// </summary>
        /// <returns>Tất cả bản ghi trong một bảng</returns>
        /// Createed By :Bùi Quang Điệp (23/08/2022)
        public IEnumerable<dynamic> GetAll()
        {
            return _baseDL.GetAll();
        }


        /// <summary>
        /// Danh sách một bản ghi
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Danh sách một bản ghi</returns>
        /// /// Createed By :Bùi Quang Điệp (23/08/2022)
        //public IEnumerable<dynamic> GetOneRecord(Guid id)
        //{
        //    return _baseDL.GetOneRecord(id);
        //}


        /// <summary>
        /// Thêm mới 1 bản ghi vào 1 Bảng
        /// </summary>
        /// <param name="record"></param>
        /// <returns>Giá trị cảu bản ghi được thêm mới</returns>
        ///  /// createdBy : Bùi Quang Điệp (24/08/2022)
        public Guid InsertOneRecord(T record)
        {
            // Validate dữ liệu
            Validate(record);

            // Validate riêng

            ValidateRecord(record);
            ValidateCheckCreate(record);
            return _baseDL.InsertOneRecord(record);
        }


        /// <summary>
        /// Tìm kiếm theo mã và tên của từng table
        /// </summary>
        /// <param name="keyword"></param>
        /// <returns> Danh sách bản ghi</returns>
        /// Created By:Bùi Quang Điệp (14/08/2022)
        public IEnumerable<dynamic> SearchRecord(string keyword)
        {

            return _baseDL.SearchRecord(keyword);
        }

        /// <summary>
        /// Sửa thông tin tài sản
        /// </summary>
        /// <param name="id"> ID tài sản cần chỉnh sửa</param>
        /// <param name="record"></param>
        /// <returns></returns>
        /// Create By:Bùi Quang Điệp( 18/08/2022)
        public Guid UpdateRecord(T record, Guid id)
        {
            // Validate dữ liệu
            Validate(record);

            // Validate riêng

            ValidateRecord(record);
            ValidateCheckUpdate(record, id);
            return _baseDL.UpdateRecord(record, id);
        }

        /// <summary>
        ///  Validate dữ liệu
        /// </summary>
        /// <param name="record"></param>
        /// <return>nếu có lỗi thì trả về</return>
        /// Created By:Bùi Quang Điệp (14/08/2022)
        public void Validate(T record)
        {
            // Check các trường không được bỏ trống
            var props = record.GetType().GetProperties();
            var lableName = string.Empty;
            // Lấy ra các trường được định nghĩa là không dược phép để trống

            var propertisNoEmpties = record.GetType().GetProperties().Where(p => Attribute.IsDefined(p, typeof(NoEmpty)));
            var errorsdata = new List<string>();
            foreach (var prop in propertisNoEmpties)
            {
                var propValue = prop.GetValue(record);
                var propName = prop.Name;

                // Lấy tên của properties
                var Nameproperty = prop.GetCustomAttributes(typeof(NameProperty), true);


                // gán lablename bằng NameProperty nếu NameProperty có
                if (Nameproperty.Length > 0)
                {
                    lableName = (Nameproperty[0] as NameProperty).Name;
                }

                // Kiểm tra có trống

                if (string.IsNullOrEmpty(propValue.ToString()))
                {
                    lableName = (lableName == string.Empty ? propName : lableName);



                    errorsdata.Add($"{lableName}" + ResourceValidate.Required1);


                }
            }
            // Lấy ra các trường cần kiểm tra độ dài
            var propertisLength = record.GetType().GetProperties().Where(prop => Attribute.IsDefined(prop, typeof(MaxLengthText)));
            foreach (var prop in propertisLength)
            {
                var propValueText = prop.GetValue(record);
                var propNameText = prop.Name;

                // Lấy độ dài cần kiểm tra
                var lengths = prop.GetCustomAttributes(typeof(MaxLengthText), true);
                var length = (lengths[0] as MaxLengthText).Length;

                if (propValueText.ToString().Length > length)
                {
                    errorsdata.Add($" {propNameText} " + ResourceValidate.MaxLength);
                }
            }

            if (errorsdata.Count > 0)
            {
                var errorResult = new ErrorSevice();
                errorResult.Handle = (int)Handler.Required;
                errorResult.DevMsg = ResourceVN.Error_ValidateData;
                errorResult.UserMsg = ResourceValidate.Required;
                errorResult.DataError = errorsdata;

                throw errorResult;
            }
        }

        /// <summary>
        ///  Validate riêng 
        /// </summary>
        /// <param name="record"></param>
        /// <return>nếu có lỗi thì trả về</return>
        /// Created By:Bùi Quang Điệp (14/08/2022)
        protected virtual void ValidateRecord(T record) { }

        /// <summary>
        /// Validate check mã trùng update
        /// </summary>
        /// <param name="record"></param>
        /// <param name="id"></param>
        /// <param name="fixedAssetCode"></param>
        /// Created By:Bùi Quang Điệp (14/08/2022)
        protected virtual void ValidateCheckUpdate(T record, Guid id) { }

        /// <summary>
        ///  Validate check mã trùng
        /// </summary>
        /// <param name="record"></param>
        /// <return>nếu có lỗi thì trả về</return>
        /// Created By:Bùi Quang Điệp (14/08/2022)
        protected virtual void ValidateCheckCreate(T record) { }

        /// <summary>
        /// Lấy mã mới nhất
        /// </summary>
        /// <returns></returns>
        /// Create By:Bùi Quang Điệp( 07/10/2022)
        public string GetCodeAsset()
        {
            var recordCodes = _baseDL.GetNewAsset();

            // tách chuỗi thành số
            var results = new List<string>();
            var resultsPrefix = new List<string>();
            var recordCodeNew = "";
            var prefix = string.Empty;
            var stringResult = "";
            foreach (var assetCode in recordCodes)
            {
                var checknumber = Regex.Match(assetCode, @"\d+").Value;
                var checkprefix = Regex.Replace(assetCode, @"[\d-]", string.Empty);
                if (decimal.Parse(checknumber) < hundreds)
                {
                    if (decimal.Parse(checknumber) < dozens)
                    {
                        checknumber = "0" + checknumber;
                    }
                    else
                    {
                        checknumber = "00" + checknumber;
                    }

                }

                results.Add(checknumber);
                resultsPrefix.Add(checkprefix);

            }
            if (resultsPrefix[0] == resultsPrefix[1])
            {
                if (decimal.Parse(results[0]) > decimal.Parse(results[1]))
                {
                    stringResult = recordCodes[0];
                }
                else
                {
                    stringResult = recordCodes[1];
                }
            }
            else
            {
                stringResult = recordCodes[1];
            }




            if (stringResult != "")
            {
                //  prefix = Regex.Replace(stringResult, @"[\d-]", string.Empty);
                // tách chuỗi ra lấy phần prefix
                string[] arrListStr = Regex.Split(stringResult, @"\d");
                prefix = arrListStr[0];
                var suffixes = stringResult.Substring((int)prefix.Length);
                // assetNew = Regex.Match(stringResult.ToString(), @"\d{}").Value;
                // Hàm kiển tra có phải là số hay không
                Regex regex = new Regex(@"^[-+]?[0-9]*.?[0-9]+$");
                if (!regex.IsMatch(suffixes[suffixes.Length - 1].ToString()))
                {
                    // Nếu có xuất hiện chữ thì thêm số 0 vào cuối
                    recordCodeNew = prefix + suffixes + "0";
                }
                else
                {
                    decimal number = 0;
                    var text = string.Empty;
                    var checkText = true;
                    // Kiểm tra có kí tự trong chuỗi suffixes hay không
                    for (int i = 0; i < suffixes.Length; i++)
                    {
                        if (!regex.IsMatch(suffixes[i].ToString()))
                        {
                            checkText = false;
                        }
                    }
                    if (!checkText)
                    {
                        text = suffixes.Substring(0, suffixes.Length - 1);
                        number = int.Parse(suffixes[suffixes.Length - 1].ToString());
                    }
                    else
                    {
                        number = decimal.Parse(suffixes);
                    }
                    number = number + 1;
                    if (number < hundreds)
                    {
                        if (text.IsNullOrEmpty())
                        {
                            if (number < dozens)
                            {
                                recordCodeNew = prefix + "00" + number;
                            }
                            else
                            {
                                recordCodeNew = prefix + "0" + number;
                            }
                        }
                        else
                        {
                            recordCodeNew = prefix + text + number;
                        }
                    }
                    else
                    {
                        recordCodeNew = prefix + number;
                    }
                }



            }
            else
            {
                recordCodeNew = prefix + "001";
            }

            return recordCodeNew;
        }

        #endregion
    }

}
