// Resource dùng chung toàn chương trình
var Resource = Resource || {};

/*
 * Các method khi gọi axios
 * Author : Bùi Quang Điệp
 * Date:07/09/2022
 */
Resource.APIMethod = {
  Get: "Get",
  Post: "Post",
  Put: "Put",
  Delete: "Delete",
};

/*
 * kiểu dữ liệu khi gọi  API
 * Author : Bùi Quang Điệp
 * Date:07/09/2022
 */
Resource.ContentType = {
  json: "application/json",
};

/*
 * Các trường thông báo required
 * Author : Bùi Quang Điệp
 * Date:07/09/2022
 */
Resource.Required = {
  MaxLength: " không được nhập quá ",
  DoubleKey: "Mã tài sản  đã có trong hệ thống!",
  FixedAssetCode: "Mã tài sản không được bỏ trống .",
  FixedAssetName: "Tên tài sản không được bỏ trống.",
  Quantity: "Số lượng không được bằng không.",
  DepreciationYear: "Giá trị hao mòn không được bằng không.",
  LifeTime: "Số năm sử dụng không được bằng không.",
  DepreciationRate: "Tỉ lệ hao mòn không được bằng không.",
  Cost: "Nguyên giá không được bằng không.",
  DepartmentID: "Mã phòng ban không được để trống.",
  FixedAssetCategoryID: "Mã loại tài sản không được để trống.",
  LengthAssetCode: "Mã tài sản không được quá 20 kí tự.",
  LengthAssetName: "Tên tài sản không được quá 255 kí tự.",
  BusinessValidationDepreciationRate: `Tỉ lệ hao mòn phải bằng phải bằng 1 / số năm sử dụng`,
  BusinessValidationDepreciationYear: `Hao mòn năm phải nhỏ hơn nguyên giá`,
  MinQuantity: "Số lượng không được nhỏ hơn hoặc bằng không",
  MaxQuantity: "Số lượng không được lớn hơn 10000",
  MaxLifeTime: "Số năm sử dụng không được lớn hơn 100",
  MinCost: "Nguyên giá không được nhỏ hơn hoặc bằng không",
  MaxPurchaseDate: "Ngày mua không được lớn hơn ngày hiện tại",
  MaxDateVocher: "Ngày chứng từ không được lớn hơn ngày hiện tại",
  MaxIncrementDate: "Ngày ghi tăng không được lớn hơn ngày hiện tại",
  MaxProductionDate: "Ngày sử dụng không được lớn hơn ngày hiện tại",
  FixedAssetIncrementCode: "Mã ghi tăng không được bỏ trống.",
  MinData: "Cần chọn ít nhất 1 tài sản!",
  MinBudget: "Nguồn hình thành không được trùng!",
  RequiredBudget: "Nguồn hình thành không được bỏ trống!",
  RequiredCost: "Nguyên giá không được bằng 0!",
};

/*
 * Tiêu đề form dialog
 * Author : Bùi Quang Điệp
 * Date:07/09/2022
 */
Resource.TitleDialog = {
  EditIncrement: "Sửa chứng từ ghi tăng",
  AddIncrement: "Thêm chứng từ ghi tăng",
};

/*
 * Thông tin cảnh báo
 * Author : Bùi Quang Điệp
 * Date:07/09/2022
 */
Resource.Warning = {
  Close:
    "Thông tin thay đổi sẽ không được cập nhật nếu bạn không lưu. Bạn có muốn lưu các thay đổi này?",
  NoData: "Bạn cần chọn tài sản trước khi xóa!",
  Delete: `Bạn có muốn xóa tài sản `,
  DeleteIncrement: `Bạn có muốn xóa chứng từ `,
  Deletes: `Tài sản đã được chọn. Bạn có muốn xóa các tài sản khỏi danh sách?`,
  DeleteIncrements: `Chứng từ đã được chọn. Bạn có muốn xóa các chứng từ khỏi danh sách?`,
  AssetActive: " đã phát sinh chứng từ ghi tăng có mã ",
};

/*
 * Tên các chức năng
 * Author : Bùi Quang Điệp
 * Date:07/09/2022
 */
Resource.name = {
  edit: "Sửa",
  add: "Thêm",
  delete: "Xóa",
  replication: "Nhân bản",
  noEdit: "Dữ liệu không thay đổi ",
};
/*
 * Các commandType của toolbar
 * Author : Bùi Quang Điệp
 * Date:07/09/2022
 */
Resource.CommandType = {
  Add: "add", // Thêm mới
  Edit: "edit", // Chỉnh sửa
  Delete: "delete", //Xóa
  Replication: "replication", // Nhân bản
};

/*
 * Các Label của button
 * Author : Bùi Quang Điệp
 * Date:07/09/2022
 */
Resource.Label = {
  Agree: "Đồng ý",
  unAgree: "Không đồng ý",
  Cancel: "Hủy",
  Save: "Lưu",
  NotSave: "Không lưu",
  Close: "Đóng",
  Delete: "Xóa",
  No: "Không",
};
/*
 * Các action trên form detail
 * Author : Bùi Quang Điệp
 * Date:07/09/2022
 */
Resource.CommandForm = {
  Save: "Save",
  Cancel: "Cancel",
};

/*
 * API Domain
 * Author : Bùi Quang Điệp
 * Date:07/09/2022
 */
Resource.Domain = "http://localhost:13846/api/v1/";

/*
 * Các danh sách đầu API
 * Author : Bùi Quang Điệp
 * Date:07/09/2022
 */
Resource.APIs = {
  Assets: "FixedAssets/",
  AssetDelete: "FixedAssets/delete",
  FixedAssetIncrementDelete: "FixedAssetIncrements/delete",
  AssetFilter: "FixedAssets/filter?",
  Asset: "FixedAssets/Detail?id=",
  Departments: "Departments",
  FixedAssetCategorys: "FixedAssetCategorys",
  NewCode: "FixedAssets/CodeAsset",
  NewCodeIncrement: "FixedAssetIncrements/CodeAsset",
  DepartmentSearch: "Departments/search?keyword=",
  BudgetSearch: "Budgets/search?keyword=",
  FixedAssetCategorySearch: "FixedAssetCategorys/search?keyword=",
  UserLogOut: "Users/logout",
  UserLogIn: "Users/login",
  UserCheck: "Users/checktoken",
  FixedAssetIncrements: "FixedAssetIncrements/",
  FixedAssetIncrementFilter: "FixedAssetIncrements/filter?",
  Budgets: "Budgets/",
};
/*
 *  APIs Error Message
 * Author : Bùi Quang Điệp
 * Date:07/09/2022
 */
Resource.Errors = {
  DoubleKey: " đã tồn tại trong hệ thống. Vui lòng kiểm tra lại.",
};

/*
 * Các kiểu định dạng ngày tháng
 * Author : Bùi Quang Điệp
 * Date:07/09/2022
 */
Resource.Date = {
  DMY: "DD/MM/YYYY",
  YMD: "YYYY/MM/DD",
  MDY: "MM/DD/YYYY",
  YDM: "YYYY/DD/MM",
};
/*
 * Các icon form dialog
 * Author : Bùi Quang Điệp
 * Date:07/09/2022
 */
Resource.PopupIcons = {
  notice: "notice",
  error: "error",
  warning: "warning",
  deleteWarning: "deleteWarning",
  confirm: "confirm",
};

/*
 * ID của các bảng cần xóa
 * Author : Bùi Quang Điệp
 * Date:07/09/2022
 */
Resource.IDTable = {
  fixedAssetIncrementID: "fixedAssetIncrementID",
  fixedAssetID: "fixedAssetID",
};

export default Resource;
