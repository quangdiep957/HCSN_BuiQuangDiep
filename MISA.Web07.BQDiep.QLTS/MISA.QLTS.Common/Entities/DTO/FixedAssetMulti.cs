using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.QLTS.Common.Entities.DTO
{
    public class FixedAssetMulti
    {
        /// <summary>
        /// ID tài sản
        /// </summary>
        public Guid FixedAssetID { get; set; }

        /// <summary>
        /// Mã tài sản
        /// </summary>
        public string FixedAssetCode { get; set; }

        /// <summary>
        /// Tên tài sản
        /// </summary>
        public string FixedAssetName { get; set; }

        /// <summary>
        /// Mã bộ phận sử dụng
        /// </summary>
        public Guid DepartmentID { get; set; }

        /// <summary>
        /// Nguyên giá
        /// </summary>
        public decimal Cost { get; set; }

        /// <summary>
        /// danh sách Nguồn hình thành
        /// </summary>

        public string Price { get; set; }

        /// <summary>
        /// số lượng
        /// </summary>
        public decimal Quantity { get; set; }

        /// <summary>
        /// Tỉ lệ hao mòn
        /// </summary>
        public decimal DepreciationRate { get; set; }

        /// <summary>
        /// Số năm sử dụng
        /// </summary>
        public DateTime ProductionDate { get; set; }
    }
}
