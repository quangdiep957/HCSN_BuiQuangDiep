using Dapper;
using MISA.QLSP.Common.Entities.Entities;
using MISA.QLTS.Common.Entities.DTO;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.QLTS.DL
{
    public class AssetDL:BaseDL<FixedAsset>,IAssetDL
    {
        /// <summary>
        /// Xóa tài sản
        /// </summary>
        /// <param name="fixedAssetID"></param>
        /// <returns>Xóa tài sản</returns>
        /// Create By : Bùi Quang Điệp (22/08/2022)
        //public int DeleteAsset(Guid fixedAssetID)
        //{
        //    // 1 Khai báo thông tin Database
        //    var connectionString = "Host=localhost;Database=misa.web07.hcsn.diep;port=3306;User Id=root;password=Quangdiep@2001";

        //    // 2 Khởi tạo kết nối tới Mysql
        //    var sqlConection = new MySqlConnection(connectionString);
        //    // 3 Lấy dữ liệu
        //    var sqlCommand = "DELETE FROM fixed_asset WHERE FixedAssetID = @FixedAssetID";
        //    var parameters = new DynamicParameters();
        //    parameters.Add("FixedAssetID", fixedAssetID);
        //    var res = sqlConection.Execute(sql: sqlCommand, param: parameters);
        //    return res;
        //}

        public PagingData<FixedAsset> FilterAsset(string? where, int pageSize = 10, int pageNumber = 1 )
        {

            // 1 Khai báo thông tin Database
            var connectionString = "Host=localhost;Database=misa.web07.hcsn.diep;port=3306;User Id=root;password=Quangdiep@2001";

            // 2 Khởi tạo kết nối tới Mysql
            var sqlConection = new MySqlConnection(connectionString);

            //// 3 Lấy dữ liệu
            var sqlCommand = "Proc_asset_GetPaging";
            // Chuẩn bị tham số đầu vào cho stored procedure
            var parameters = new DynamicParameters();
            parameters.Add("@v_Offset", (pageNumber - 1) * pageSize);
            parameters.Add("@v_Limit", pageSize); 
            parameters.Add("@v_Sort", "ModifiedDate DESC");
            parameters.Add("@v_Where", where);

            // Thực hiện gọi vào DB để chạy stored procedure với tham số đầu vào ở trên
            var multipleResults = sqlConection.QueryMultiple(sqlCommand, parameters, commandType: System.Data.CommandType.StoredProcedure);

            // Xử lý kết quả trả về từ DB
            if (multipleResults != null)
            {
                var employees = multipleResults.Read<FixedAsset>().ToList();
                var totalCount = multipleResults.Read<long>().Single();
                var res = new PagingData<FixedAsset>()
                {
                    Data = employees,
                    TotalCount = totalCount
                };
                return res;
            }
            else
            {
                return new PagingData<FixedAsset>();
            }
        }

        /// <summary>
        /// lấy toàn bộ dữ liệu tài sản
        /// </summary>
        /// <returns>danh sách tài sản</returns>
        /// Created By : Bùi Quang Điêp (20/8/2022)
        //public IEnumerable<FixedAsset> Get()
        //{
        //    // 1 Khai báo thông tin Database
        //    var connectionString = "Host=localhost;Database=misa.web07.hcsn.diep;port=3306;User Id=root;password=Quangdiep@2001";

        //    // 2 Khởi tạo kết nối tới Mysql
        //    var sqlConection = new MySqlConnection(connectionString);

        //    //// 3 Lấy dữ liệu
        //    var sqlCommand = "SELECT *, fac.FixedAssetCategoryName,d.DepartmentName FROM fixed_asset fa INNER JOIN department d ON fa.DepartmentID = d.DepartmentID INNER JOIN fixed_asset_category fac ON fa.FixedAssetCategoryID = fac.FixedAssetCategoryID";
        //    var assets = sqlConection.Query<FixedAsset>(sql: sqlCommand);

        //    //// 4 Trả về kết quả
        //    return assets;
        //}

        public string GetNewAsset()
        {
            // 1 Khai báo thông tin Database
            var connectionString = "Host=localhost;Database=misa.web07.hcsn.diep;port=3306;User Id=root;password=Quangdiep@2001";

            // 2 Khởi tạo kết nối tới Mysql
            var sqlConection = new MySqlConnection(connectionString);

            // 3 Lấy dữ liệu
            var sqlCommand = "SELECT MAX(FixedAssetCode) FROM fixed_asset";
            var assetCode = sqlConection.QueryFirst<object>(sql: sqlCommand);

            return assetCode.ToString();
        }

        //public int Post(FixedAsset asset)
        //{

        //    DateTime date = DateTime.Now;
        //    asset.createdDate = date;
        //    asset.modifiedDate = date;
        //    // 1 Khai báo thông tin Database
        //    var connectionString = "Host=localhost;Database=misa.web07.hcsn.diep;port=3306;User Id=root;password=Quangdiep@2001";

        //    // 2 Khởi tạo kết nối tới Mysql
        //    var sqlConection = new MySqlConnection(connectionString);
        //    asset.fixedAssetID = Guid.NewGuid();
        //    // 3 Lấy dữ liệu
        //    var sqlCommand = "Proc_Fixed_Asset_OneInsert";
        //    var parameters = new DynamicParameters();
        //    parameters.Add("FixedAssetID", asset.fixedAssetID);
        //    parameters.Add("FixedAssetCode", asset.fixedAssetCode);
        //    parameters.Add("FixedAssetName", asset.fixedAssetName);
        //    parameters.Add("DepartmentID", asset.departmentID);
        //    parameters.Add("FixedAssetCategoryID", asset.fixedAssetCategoryID);
        //    parameters.Add("Cost", asset.cost);
        //    parameters.Add("Quantity", asset.quantity);
        //    parameters.Add("DepreciationRate", asset.depreciationRate);
        //    parameters.Add("TrackedYear", asset.trackedYear);
        //    parameters.Add("LifeTime", asset.lifeTime);
        //    parameters.Add("CreatedDate", asset.createdDate);
        //    parameters.Add("CreatedBy", asset.createdBy);
        //    parameters.Add("ModifiedDate",asset.modifiedDate);
        //    parameters.Add("DepreciationYear", asset.depreciationYear);
        //    var res = sqlConection.Execute(sql: sqlCommand, param: asset, commandType: CommandType.StoredProcedure);
        //    return res;
        //}

        /// <summary>
        /// Sửa thông tin tài sản
        /// </summary>
        /// <param name="fixedAssetID"> ID tài sản cần chỉnh sửa</param>
        /// <param name="asset"></param>
        /// <returns></returns>
        /// Create By:Bùi Quang Điệp( 18/08/2022)
        public int UpdateAsset(Guid fixedAssetID, FixedAsset asset)
        {
            DateTime date = DateTime.Now;
            asset.modifiedDate = date;
            // 1 Khai báo thông tin Database
            var connectionString = "Host=localhost;Database=misa.web07.hcsn.diep;port=3306;User Id=root;password=Quangdiep@2001";

            // 2 Khởi tạo kết nối tới Mysql
            var sqlConection = new MySqlConnection(connectionString);
            // 3 Lấy dữ liệu
            //var sqlCommand = "Proc_Fixed_Asset_Update";
            var sqlCommand = "UPDATE fixed_asset SET FixedAssetName=@FixedAssetName,DepartmentID=@DepartmentID,FixedAssetCategoryID=@FixedAssetCategoryID,PurchaseDate=@PurchaseDate,Cost=@Cost,Quantity =@Quantity,DepreciationRate=@DepreciationRate,TrackedYear=@TrackedYear,LifeTime=@LifeTime,CreatedBy=@CreatedBy,CreatedDate=@CreatedDate,ModifiedBy=@ModifiedBy,ModifiedDate=@ModifiedDate,ProductionDate=@ProductionDate WHERE FixedAssetID =@FixedAssetID";
            var parameters = new DynamicParameters();
            asset.fixedAssetID = fixedAssetID;
            parameters.Add("@FixedAssetID", asset.fixedAssetID);
            parameters.Add("@FixedAssetCode", asset.fixedAssetCode);
            parameters.Add("@FixedAssetName", asset.fixedAssetName);
            parameters.Add("@DepartmentID", asset.departmentID);
            parameters.Add("@FixedAssetCategoryID", asset.fixedAssetCategoryID);
            parameters.Add("@Cost", asset.cost);
            parameters.Add("@Quantity", asset.quantity);
            parameters.Add("@DepreciationRate", asset.depreciationRate);
            parameters.Add("@TrackedYear", asset.trackedYear);
            parameters.Add("@LifeTime", asset.lifeTime);
            parameters.Add("@CreatedBy", asset.createdBy);
            parameters.Add("@ModifiedBy", asset.modifiedDate);
            parameters.Add("@PurchaseDate", asset.purchaseDate);
            parameters.Add("@DepreciationYear", asset.depreciationYear);
            var res = sqlConection.Execute(sql: sqlCommand, param: asset);
            return res;
        }
    }
}
