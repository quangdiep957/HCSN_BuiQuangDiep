   import Config from "./config";
   /***
         * Format giá tiền
         * Author:Bùi Quang Điệp
         * Date:13/08/2022
         * 
         */
export function formatCash(val){
    if(val){
        const str = val.toString();
        // Kiểm tra xem có dấu phân cách phần thập phân hay không
        if(str.includes (Config.FormatCost.Decimal))
        {
            return val
        }
        else{
            return str.replace(/^0+/, '').replace(/\D/g, "").replace(/\B(?=(\d{3})+(?!\d))/g, Config.FormatCost.Number)
        }
       
    }
    else{
        return 0;
    }
}
export function formatDecimal(val)
{
    if(val.toString().includes('.'))
    {
        return val.toString().split(".").join(Config.FormatCost.Decimal);
    }
    else{
        return val.toString().split(",").join(Config.FormatCost.Decimal);
    }
}

export function unFormatDecimal(val)
{
    if(!val.toString().includes('.'))
    {
        return val.toString().split(",").join('.');
    }
}
   /***
         * SettimeOut
         * Author:Bùi Quang Điệp
         * Date:13/08/2022
         * 
         */
export function timeOut(){
    var hide= true;
  setTimeout(()=>{
    hide = false
  },10000)
    return hide;
}

   /***
         * Format ngày tháng
         * Author:Bùi Quang Điệp
         * Date:13/08/2022
         * 
         */
export function formatDate(date,formatType) {
    
    var d = new Date(date);
    var day = d.getDate();
    var month = d.getMonth() + 1;
    var year = d.getFullYear();
    if (day < 10) {
        day = "0" + day;
    }
    if (month < 10) {
        month = "0" + month;
    }
    if(formatType == 'DD/MM/YYYY')
    {
        date = day + "/" + month + "/" + year;
    }
    else if(formatType == 'MM/DD/YYYY')
    {
        date = month + "/" + day + "/" + year;
    }
    else if(formatType == 'YYYY/MM/DD')
    {
        date = year + "/" + month + "/" + day;
    }
    else if(formatType == 'YYYY/DD/MM')
    {
        date = year + "/" + day + "/" + month;
    }
   

    return date;
}
