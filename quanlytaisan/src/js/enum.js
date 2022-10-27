// Các enum dùng chung toàn chương trình
var Enumeration = Enumeration || {};

/*
 * Các mode của form detail
 * Author : Bùi Quang Điệp
 * Date:07/09/2022
 */  
Enumeration.FormMode = {
  Add: 1, // Thêm mới
  Edit: 2, // Sửa
  Delete: 3, // Xóa
  Replication: 4, // Nhân bản
};

/*
 * Các mode của handle lỗi
 * Author : Bùi Quang Điệp
 * Date:07/09/2022
 */
Enumeration.FormModeHandler = {
  Required: 1, // Lỗi nhập liệu phía người dùng
  DoubleKey: 2, // Lỗi trùng mã
  Exception: 4, // lỗi Exception phía backend
};

/*
 * thời gian timeOut
 * Author : Bùi Quang Điệp
 * Date:07/09/2022
 */
Enumeration.SetTimeOut = {
  TimeOut: 5000,
};
/*
 * Các trường cần tính tổng của bảng tài sản
 * Author : Bùi Quang Điệp
 * Date:07/09/2022
 */
Enumeration.SummaryAsset = {
  Cost: 1,
  Quantity: 2,
  Depreciations: 3,
  Atrophy: 4,
};

export default Enumeration;
