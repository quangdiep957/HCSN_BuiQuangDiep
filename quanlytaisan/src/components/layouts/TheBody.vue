<template>
    <div class="page-body-wrapper" :class="{'setWidth':isSetWidth}">
        <Header />
        <div class="bodydata">
            <div class="m-row m-row__form" style="">
                <div class="m-row__left display-flex">
                    <!-- tạo input search -->
                    <div class="input-search" style="margin-right:10px" >
                        <div class="icon input__search--icon__item input__search--icon" data-toggle="tìm kiếm"></div>
                        <input type="text" name="" id="" class="input input__search--text" @keydown.enter="HandlerSearch"
                            placeholder="Tìm kiếm tài sản">
                    </div>
                    <ComboBox val="Loại tài sản" v-bind:option=optionCategory icon="true"  typeCombobox="category" @dataCategory="HandlerSearch" @remove="HandlerSearch" />
                    <ComboBox val="Bộ phận sử dụng" v-bind:option=optionDepartment icon="true" typeCombobox="department" @dataDepartment="HandlerSearch" @remove="HandlerSearch" />
                </div>

                <div class="m-row__right display-flex">
                    <div @click="btnShowDialog()" class="btn btn__icon" id="btn--add">
                        <div class="btn__icon--item">
                            <div class="icon icon-add icon__size-8"></div>
                            <div class="btn--text">Thêm tài sản</div>
                        </div>
                    </div>
                    <button class="btnfunction distance tooltip">
                        <div class="bntfunction__more icon icon-more icon__size-18"></div>
                         <Tooltip tooltiptext="Chỉnh sửa" positiontooltip="bottom"/>
                    </button>
                    <button class="btnfunction tooltip" @click="HandleRemove()">
                        <div class="bntfunction__remove icon icon-remove icon__size-18"></div>
                         <Tooltip tooltiptext="Xóa" positiontooltip="bottom"/>
                    </button>
                </div>
            </div>
            <!-- Thêm table  -->
            <router-view></router-view>
        </div>
    </div>
    <TheDialog v-if="isShowDialog" @BtnCloseDialog="isShowDialog = false" :title="title"
    v-bind:item="itemAssetDetail" :handler=handler @handlerName="ShowSuccess" />
  <Notify v-if="isShowDialogNotify" :dataError=titleWarning @isShowDialogNotify ="isShowDialogNotifyFuntion()" @removeData="isRemove =true" :dataAsset=dataTicks :buttonNames="['Xóa','Không']" />  
    <div class="toast__box" v-show="isShowSuccess == true">
                    <div class="toast__box--item">
                    <div class="toast__box--body"><div class ="toast__box--item-icon"><div class="icon icon-tick-white icon__size-11x2"></div></div></div>
                        <div class="toast__box--item-text">{{handlerName}} dữ liệu thành công</div>
                    </div>
                </div>
</template>
<script>
import Header from './TheHeader.vue'
import ComboBox from '../base/TheComboBox.vue'
import TheDialog from '../base/TheDialog.vue'
import Tooltip from '../base/TheTooltip.vue'
import Notify from '../base/TheDialogNotify.vue'
import axios from 'axios'
export default {
    name: "TheHeader",
    components: {
        Header, ComboBox, TheDialog,Tooltip,Notify
    },
    data() {
        return {
            isShowDialogNotify : false,
            isShowDialog: false,
            title:"Sửa tài sản",
            titleWarning:[],
            dataTicks:[],
            optionDepartment: [],
            optionCategory: [],
            itemAssetDetail :[],
            isSetWidth:false,
            handler:"",
             isShowSuccess:false,
            searchArray :{
                keyword : "",
                fixedAssetCategoryID : "",
                departmentID : "",
                pageSize:"",
                pageNumber:"",
                handlerName:""

            }
            ,
            dataReplication:[],
            isRemove :false
        };
    },
    methods: {
        /**
         * Sự kiện show dialog 
         * Author : Bùi Quang Điệp
         * Date :09 /08/ 2021
         */
        btnShowDialog(){
            try {
            // đặt lại input về rỗng
             this.itemAssetDetail=[]
            this.handler="add";
             // Lấy mã tài sản mới nhất truyền vào input
             axios.get('http://localhost:13846/api/v1/FixedAssets/CodeAsset')
             .then(res=>{
                 this.isShowDialog = true ;
                 this.title="Thêm tài sản";
                    console.log(res);

            // Kiểm tra nếu là nhân bản thì truyền data vào
            if(this.dataReplication != undefined)
            {
                this.itemAssetDetail = this.dataReplication;
                this.dataReplication = [];
            }
             this.itemAssetDetail.fixedAssetCode = res.data;
             })
             .catch(res=>{
                console.log(res);
             })

            
             
            } catch (error) {
                console.log(error);
            }
           
        },
         /**
         * Xử lý sự kiện xóa
         * Author : Bùi Quang Điệp
         * Date:10/08/2022
         */
        HandleRemove(){
            try {
                
            this.titleWarning = [];
             this.isShowDialogNotify = true;
           var quantity = this.dataTicks.length;
            if(quantity == 0){
                this.titleWarning.push("Bạn cần chọn tài sản trước khi xóa !");
            }
            else if(quantity==1){
                  this.titleWarning.push(`Bạn có thật sự muốn xóa tài sản ${this.dataTicks[0].fixedAssetName}`);
            }
            else{
                this.titleWarning.push(`Bạn có thật sự muốn xóa ${quantity} tài sản ?`);
            }

            } catch (error) {
                console.log(error);
            }
           
        },
        /**
        *   Xóa thành công thì ẩn dialog và xóa dữ liệu cũ
         * Author : Bùi Quang Điệp
         * Date:10/08/2022
         */
        isShowDialogNotifyFuntion(){
            
            this.isShowDialogNotify = false;
            if(this.isRemove)
            {
                this.dataTicks = [];
                this.isRemove = false;
            }

        },

         /**
        *  Nhận dữ liệu lọc
         * Author : Bùi Quang Điệp
         * Date:10/08/2022
         */
        HandlerSearch(item){
            
            if(item=="")
            {
                this.searchArray.departmentID = "";
                this.searchArray.fixedAssetCategoryID = "";
            }
            if(item.DepartmentID != undefined)
            {   
                
                 this.searchArray.departmentID= item.DepartmentID;
            }
            if(item.FixedAssetCategoryID != undefined)
            {   
                
                 this.searchArray.fixedAssetCategoryID= item.FixedAssetCategoryID;
            }
            if(event.currentTarget.value != undefined)
            {
                this.searchArray.keyword= event.currentTarget.value;
            }
            if(item.pageSize != undefined)
            {
                this.searchArray.pageSize = item;
            }
            if(item.pageNumber!=undefined)
            {
                this.searchArray.pageNumber = item;
            }

            this.emitter.emit("search",this.searchArray);
        },
        /**
         * Show thông báo thêm mới hoặc sửa thành công
         * Author : Bùi Quang Điệp
         * Date :14 /08/ 2021
         */
        ShowSuccess(item){
           
            if(item == "Sửa")
            {
                this.handlerName="Sửa"
            }
            else{
                 this.handlerName="Thêm"
            }
             this.isShowSuccess = true;
             
              setTimeout(()=>{

                this.isShowSuccess = false },3000);

        }
        
    },
     /**
         * Nhận dữ liệu từ combobox
         * Author : Bùi Quang Điệp
         * Date :14 /08/ 2021
         */
    created(){
        // Lấy thông tin bộ phận sử dụng từ API
        try {
            axios.get("http://localhost:13846/api/v1/Departments")
            .then(res=>{
                console.log(res);
                this.optionDepartment = res.data;
                 this.emitter.emit("Department",res.data);
                
            })
            .catch(res=>{
                  console.log(res);
            })
           
            
        } catch (error) {
            console.log(error);
        }

        // lấy thông tin loại tài sản từ API
         try {
            
            axios.get("http://localhost:13846/api/v1/FixedAssetCategorys")
            .then(res=>{
                console.log(res);
                this.optionCategory = res.data;
                // truyền dữ liệu sang table
                 this.emitter.emit("Category",res.data);
                
            })
            .catch(res=>{
                  console.log(res);
            })
           
            
        } catch (error) {
            console.log(error);
        }
    }
    
    ,
     mounted(){
         /**
         * Sự kiện nhận dữu liệu từ EventBus 
         * Author : Bùi Quang Điệp
         * Date :09 /08/ 2021
         */
        try {
            
            // nhận dữ liệu từ table
             this.emitter.on("itemDialog", (item) => {
                // gán giá trị mở dialog bằng true
             this.isShowDialog = true,
              this.title="Sửa tài sản";
              
             // truyền data detail vào cho mảng 
             this.itemAssetDetail = item
             this.handler = "edit";
            }),
            // Nhận dữ liệu set lại width navbar
              this.emitter.on("setWidth", () => {
                // gán giá trị width lại cho body
             this.isSetWidth = true
            }),
            // Nhận dữ liệu reset width navbar
             this.emitter.on("setWidthClear", () => {
                // gán giá trị width lại cho body
             this.isSetWidth = false
            }),
            // Nhận dữ liệu từ Cần xóa từ table
             this.emitter.on("titleWarning", (item) => {
             // truyền data vào cho mảng 
             
             this.dataTicks = item
            }),
            this.emitter.on("dataPageSize",(item)=>{
                  // truyền data vào cho mảng 
                    this.searchArray.pageSize = item
                    this.HandlerSearch(item)
            }),
            this.emitter.on("pageNumber",item=>{
                // truyền data vào cho mảng 
                this.searchArray.pageNumber = item
                this.HandlerSearch(item)
            })
            this.emitter.on("replication",item=>{
                // Nhận dữ liệu cần nhân bản
                
                this.dataReplication =  item;
                this.btnShowDialog();
            })

        } catch (error) {
            console.log(error);
        }
           
                
          
    },
}
</script>
<style>
@import url(../../css/main.css);
</style>