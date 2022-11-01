using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.QLTS.Common.Entities.DTO
{
    public class PagingData<T>
    {
        /// <summary>
        /// Mảng đối tượng thỏa mãn điều kiện lọc và phân trang
        /// </summary>
        public List<T> Data { get; set; } = new List<T>();

        /// <summary>
        /// Tổng số bản ghi thỏa mãn điều kiện
        /// </summary>
        public long TotalCount { get; set; }

        /// <summary>
        /// Danh sách tổng
        /// </summary>
        public Summary Summary { get; set; }
    }
}

public class Summary
{
    /// <summary>
    /// Tổng giá tiền
    /// </summary>
    public decimal SumPrice { get; set; }

    /// <summary>
    /// Tổng giá trị hao mòn
    /// </summary>
    public decimal SumDepreciation { get; set; }

    /// <summary>
    /// Tổng giá trị còn lại
    /// </summary>
    public decimal SumAtrophy { get; set; }

    /// <summary>
    /// Tổng số lượng
    /// </summary>
    public decimal SumQuantity { get; set; }

    /// <summary>
    /// Hàm khởi tạo
    /// </summary>
    public Summary()
    {

    }

    /// <summary>
    /// Hàm khởi tạo có tham số
    /// </summary>
    /// <param name="sumPrice"></param>
    /// <param name="sumDepreciation"></param>
    /// <param name="sumAtrophy"></param>
    /// <param name="sumQuantity"></param>
    public Summary(decimal sumPrice, decimal sumDepreciation, decimal sumAtrophy, decimal sumQuantity)
    {
        SumAtrophy = sumAtrophy;
        SumQuantity = sumQuantity;
        SumPrice = sumPrice;
        SumDepreciation = sumDepreciation;
    }
}
