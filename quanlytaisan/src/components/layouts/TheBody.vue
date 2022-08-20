<template>
    <div class="page-body-wrapper" :class="{'setWidth':isSetWidth}">
        <Header />
        <div class="bodydata">
            <div class="m-row m-row__form" style="">
                <div class="m-row__left display-flex">
                    <!-- tạo input search -->
                    <div class="input-search tooltip" style="margin-right:10px" >
                        <div class="icon input__search--icon__item input__search--icon" data-toggle="tìm kiếm"> <Tooltip tooltiptext="Tìm kiếm" positiontooltip="bottom"/></div>
                        <input type="text" name="" id="" class="input input__search--text"
                            placeholder="Tìm kiếm tài sản">
                    </div>
                    <ComboBox val="Loại tài sản" v-bind:option=optionCBBox icon="true" />
                    <ComboBox val="Bộ phận sử dụng" v-bind:option=optionCBBox icon="true" />
                </div>

                <div class="m-row__right display-flex">
                    <div @click="btnShowDialog()" class="btn btn__icon tooltip" id="btn--add">
                        <div class="btn__icon--item">
                            <div class="icon icon-add icon__size-8"></div>
                            <div class="btn--text">Thêm tài sản</div>
                        </div>
                         <Tooltip tooltiptext="Thêm mới" positiontooltip="bottom"/>
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
    v-bind:item="itemAssetDetail" />
    <Notify v-if="isShowDialogNotify" :dataError=titleWarning @isShowDialogNotify ="isShowDialogNotify = false" />
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
            optionCBBox: [],
            itemAssetDetail :[],
            isSetWidth:false
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

             // Lấy mã tài sản mới nhất truyền vào input
             axios.get('http://localhost:13846/api/v1/FixedAssets/CodeAsset')
             .then(res=>{
                this.itemAssetDetail.fixedAssetCode = res.data;
                 this.isShowDialog = true ;
                 this.title="Thêm tài sản";
                    console.log(res);
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
                  this.titleWarning.push(`Bạn có thật sự muốn xóa tài sản ${this.dataTicks[0].AssetName}`);
            }
            else{
                this.titleWarning.push(`Bạn có thật sự muốn xóa ${quantity} tài sản ?`);
            }
                
            } catch (error) {
                console.log(error);
            }
           
        },

    },
     /**
         * Nhận dữ liệu từ combobox
         * Author : Bùi Quang Điệp
         * Date :14 /08/ 2021
         */
    created(){
        try {
            
            axios.get("http://localhost:13846/api/Department")
            .then(res=>{
                console.log(res);
                this.optionCBBox = res.data;
                 this.emitter.emit("Department",res.data);
                
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
            }),
            // Nhận dữ liệu set lại width navbar
              this.emitter.on("setWidth", () => {
                // gán giá trị width lại cho body
             this.isSetWidth = true
            })
            // Nhận dữ liệu reset width navbar
             this.emitter.on("setWidthClear", () => {
                // gán giá trị width lại cho body
             this.isSetWidth = false
            })
            // Nhận dữ liệu từ titleWarning từ table
             this.emitter.on("titleWarning", (item) => {
                
             // truyền data vào cho mảng 
             this.dataTicks = item
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