<template>
    <div class="m-row managetable">
         <div class="table__nodata" v-if="isNoData">
                        <div class="table__nodata--item">
                                  
                        <div>Dữ liệu hiện tại đang trống</div>
                        <img style="width: 200px;height: 200px;" src="../../assets/icon/empty-concept-illustration_114360-1233.webp">
                            </div>
                    </div>
                    <table class="table">
                        <thead>
                            <tr>
                                 <th><button class="resize-width"><label  class="checkbox"><input @click="isTicksAll()" type="checkbox" ><span class="tick"></span></label></button></th>
                                <th class="text-align-center tooltip--th "><button class="resize-width b tooltip">STT <Tooltip tooltiptext="Số thứ tự" positiontooltip="top" style="margin-top:10px ;" /></button> </th>
                                <th><button class="resize-width text-align-left">Mã tài sản</button></th>
                                <th><button class="resize-width text-align-left">Tên tài sản</button></th>
                                <th><button class="resize-width text-align-left">Loại sản phẩm </button></th>
                                <th><button class="resize-width text-align-left">Bộ phận sử dụng </button></th>
                                <th><button class="resize-width text-align-right ">Số lượng </button></th>
                                <th><button class="resize-width text-align-right ">Nguyên giá </button></th>
                                <th tooltip--th><button class=" text-align-right resize-width tooltip">HM/KH lũy kế <Tooltip tooltiptext="Hao mòn khấu hao" positiontooltip="top" style="margin-top:10px" /></button></th>
                                <th><button class="resize-width text-align-right ">Giá trị còn lại </button></th>
                                <th><button class="resize-width  text-align-center">Chức năng </button></th>
                            </tr>

                        </thead>
                        <tbody>
                            <tr v-contextmenu:contextmenu  v-for="(item,index) in Asset" :key="item.fixedAssetID" 
                              
                            @click="SelectTick($event,item,index)" 
                            :class="{'table-active':checkTick(item)}"
                             @dblclick="isShowDialogDetail(item)"
                             >
                                <td> <label class="checkbox"><input :checked="checkTick(item)" type="checkbox" ><span class="tick"></span></label></td>
                                <td class="text-align-center">{{index+1}}</td>
                                <td class="text-align-left" id ="IdAsset">{{item.fixedAssetCode}}</td>
                                <td class="text-align-left" id ="nameAsset">{{item.fixedAssetName}}</td>
                                <td class="text-align-left">{{item.fixedAssetCategoryName}}</td>
                                <td class="text-align-left">{{NameDepartment(item.departmentID)}}</td>
                                <td class="text-align-right quantity">{{item.quantity}}</td>
                                <td class="text-align-right price">{{formatCash(item.cost)}}</td>
                                <td class="text-align-right Atrophy">{{(item.depreciationRate)}}</td>
                                <td class="text-align-right price-remaining">{{formatCash(item.trackedYear)}}</td>
                                <td class="text-align-center">
                                    <div class="tablefunction display-flex">
                                        <div class="icon icon-edit icon__size-14 tooltip" style="margin-left:15px"><Tooltip tooltiptext="Sửa" positiontooltip="top" style="left: -22%;top: 20px;" /></div>
                                        <div class="icon icon-detail icon__size-14 tooltip"><Tooltip tooltiptext="Nhân bản" positiontooltip="top" style="left: -2%;top: 20px;" /></div>
                                        <div class="icon icon-remove icon__size-17 tooltip" @click="HandleRemoveDetail(item)"><Tooltip tooltiptext="Xóa" positiontooltip="top" style="left: 46px;top: 20px;" /></div>
                                    </div>
                                </td>

                            </tr>
                            <tr class="page__table">
                                <td class="text-align-left" colspan="6">
                                </td>
                                <td class="text-align-right sum-quantity bold">{{formatCash(sumQuantity)}}</td>
                                <td class="text-align-right sum-price bold">{{ComputedSumPrice}}</td>
                                <td class="text-align-right sum-Atrophy bold">{{formatCash(sumDepreciation)}}</td>
                                <td class="text-align-right sum-price-rimaining bold">{{formatCash(sumAtrophy)}}</td>
                                <td></td>
                            </tr>
                            <div class="display-flex" style="font-size: 11px;margin-left: 17px;top: 91%;z-index: 999;position: fixed;margin-top:15px">
                                        <p class="page__table--text">Tổng số <span style="font-weight: bold;">{{sumItem}}</span>
                                            bản ghi</p>
                                        <Combobox ComboboxQuantity="true" style="width:100px" />
                                        <div class="page__table--number display-flex">
                                            <div class="page--number-item">
                                                <div class="icon icon-back icon__size-8"></div>
                                            </div>
                                            <div class="page--number-item number-active">1</div>
                                            <div class="page--number-item">2</div>
                                            <div class="page--number-item">...</div>
                                            <div class="page--number-item">10</div>
                                            <div class="page--number-item">
                                                <div class="icon icon-next icon__size-8"></div>
                                            </div>
                                        </div>
                                    </div>
                        </tbody>
                    </table>

                </div>
                
                <!-- <div class="menu">
        <div class="menu__item" v-if="isShowContentMenu">
            <div class="menu__item--icon icon icon-add-menu icon__size-18"></div>
            <div class="menu__item--text">Thêm</div>
        </div>
        <div class="menu__item">
            <div class="menu__item--icon icon icon-edit icon__size-18" style="margin:0"></div>
            <div class="menu__item--text">Sửa</div>
        </div>
        <div class="menu__item">
            <div class="menu__item--icon icon icon-remove icon__size-18"></div>
            <div class="menu__item--text">Xóa</div>
        </div>
        <div class="menu__item">
            <div class="menu__item--icon icon icon-replication icon__size-18"></div>
            <div class="menu__item--text">Nhân bản</div>
        </div>
    </div> -->
    <v-contextmenu ref="contextmenu">
    <v-contextmenu-item>  
        <div class="menu__item">
            <div class="menu__item--icon icon icon-add-menu icon__size-18"></div>
            <div class="menu__item--text">Thêm</div>
        </div>
    </v-contextmenu-item>
    <v-contextmenu-item>
        <div class="menu__item">
            <div class="menu__item--icon icon icon-edit icon__size-18" style="margin:0"></div>
            <div class="menu__item--text">Sửa</div>
        </div>
    </v-contextmenu-item>
    <v-contextmenu-item>
        <div class="menu__item">
            <div class="menu__item--icon icon icon-remove icon__size-18"></div>
            <div class="menu__item--text">Xóa</div>
        </div>
    </v-contextmenu-item>
    <v-contextmenu-item>
         <div class="menu__item">
            <div class="menu__item--icon icon icon-replication icon__size-18"></div>
            <div class="menu__item--text">Nhân bản</div>
        </div>
    </v-contextmenu-item>
  </v-contextmenu>
     <Notify v-bind:dataError=titleWarning v-if="isShowDialogNotify" />
     
    <div class="m-loading" v-if="isLoading">
        <div class="loader"></div>
    </div>
</template>

<script>
import {formatCash} from '../../js/common.js'
import { directive, Contextmenu, ContextmenuItem } from "v-contextmenu";
import "v-contextmenu/dist/themes/default.css";
import Notify from '../base/TheDialogNotify.vue'
import Tooltip from '../base/TheTooltip.vue'
import axios from 'axios'
import Combobox from '../base/TheComboBox.vue'
export default {
    name: 'QuanlytaisanTheTable',
directives: {
    contextmenu: directive,
  },
  components: {
    [Contextmenu.name]: Contextmenu,
    [ContextmenuItem.name]: ContextmenuItem,Tooltip,Combobox,
    Notify
  },
    data() {
        return {
            directives: {
                contextmenu: directive,
                isLoading :false,
                isNoData :false,
            },
            components: {
                [Contextmenu.name]: Contextmenu,
                [ContextmenuItem.name]: ContextmenuItem,
            },
            isClickRow :"",
            Asset:[],
             sumQuantity :0,
             sumPrice : 0,
             sumDepreciation :0,
             sumAtrophy : 0,
             isShowContentMenu :false,
              show: false,
              dataTicks:[],
              IndexFirst:0,
              IndexLast :0,
              sumItem :1,
              isTickAll:false,
              titleWarning:[],
              isShowDialogNotify:false,
              dataDepartment:[]
        };
    },
 computed:{
        ComputedSumPrice(){
             return formatCash(this.sumPrice)
        },
         
        
          
    },

    methods: {

        /**
         * 
         */
        formatCash,
         /**
         *  Truyền dữ liệu 1 hàng vào 1 mảng
         * Author : Bùi Quang Điệp
         * Date:10/08/2022
         */
        btnRowTable(item){
            console.log(item);
        },
        isShowDialogDetail(item){
                this.emitter.emit("itemDialog",item);
                this.emitter.emit("handlerEdit","edit");
                
        },  
        /**
         * Xử lý sự kiện nhấn phím ctrl thì chọn nhiều
         * Author : Bùi Quang Điệp
         * Date:10/08/2022
         * @param(e,item,index) e là sự kiện được click vào
         *            item là truyền row vừa click trên table
         *            index là số thứ tự của row đó trong table 
         */
        SelectTick(e,item,index){
            try {
                // Kiểm tra xem có giữ phím ctrl hay không
                // Nếu có thì thêm vào mảng và không reset lại mảng 
                // Ngược lại thì reset mảng
            if(e.ctrlKey == true){
                this.dataTicks.push(item);

            }
             /**
             * Xử lý sự kiện nhấn phím Shitft  thì chọn hàng loạt đến vị trí click
             * Author : Bùi Quang Điệp
             * Date:10/08/2022
             * */
            else if(e.shiftKey == true){
               this.IndexLast = index;
               var tg = "";
               // Kiểm tra vị trí nếu vị trí click bên trên thì đổi ngược lại vị trí index
               if(this.IndexLast < this.IndexFirst){
                     tg = this.IndexLast;
                      this.IndexLast = this.IndexFirst;
                     this.IndexFirst = tg;

               }
               for(var i = this.IndexFirst;i<= this.IndexLast;i++){
                    this.dataTicks.push(this.Asset[i]);
               }
            
            }
            else{
            this.dataTicks =[];
             this.dataTicks.push(item);
                this.IndexFirst = index;

            }
            } catch (error) {
                console.log(error);
            }
            this.emitter.emit("titleWarning",this.dataTicks);
        },
         /**
         * Kiểm tra xem row nào đã được tick
         * Author : Bùi Quang Điệp
         * Date:10/08/2022
         */
        checkTick(item){
            try {
               
                  for (const value of this.dataTicks) {
                        if(item==value)
                        {
                            return item;
                        }
               }
                
            } catch (error) {
                console.log(error);
            }
            this.emitter.emit("titleWarning",this.dataTicks);
        },
         /**
         * Tick tất cả 
         * Author : Bùi Quang Điệp
         * Date:10/08/2022
         */
        isTicksAll(){
            this.isTickAll = !this.isTickAll;
              if(this.isTickAll == true){
                    for (const item of this.Asset) {
                     this.dataTicks.push(item);
                }
                 }
             else{
                    this.dataTicks=[];
                    return
            }
        },

        /**
         * Xử lý sự kiện xóa
         * Author : Bùi Quang Điệp
         * Date:10/08/2022
         */
        
        HandleRemoveDetail(item){
             this.isShowDialogNotify = true;
            this.titleWarning.push(`Bạn có thật sự muốn xóa tài sản ${item.AssetName} này không ?`);

        }
        ,
         /**
         * Lấy tên bộ phận sử dụng
         * Author : Bùi Quang Điệp
         * Date:17/08/2022
         */
        NameDepartment(id){
            for (const item of this.dataDepartment) {
                if(item.DepartmentID == id)
                {
                    return item.DepartmentName;
                }
            }
        }
       
    },

        created(){
                    /*
                    * Hàm lấy dữ liệu đưa vào table
                    * Author : Bùi Quang Điệp
                    * Date : 14/08/2022
                    */

          
            this.isLoading = true;
             axios.get('http://localhost:13846/api/v1/FixedAssets')
                    .then(response =>{
                         this.Asset=response.data;
                         console.log(response);
                          this.isLoading = false; 
                    /*
                    *Hàm tính tổng các trường trong table
                    * Author : Bùi Quang Điệp
                    * Date : 09/08/2022
                    */

                        for (const item of this.Asset) {
                            this.sumQuantity = (this.sumQuantity + item.quantity);
                            this.sumPrice = (this.sumPrice + item.cost);
                            this.sumDepreciation=405;
                            this.sumAtrophy=(this.sumAtrophy + item.trackedYear);
                            this.sumItem++;
                        }
                        
                      
                    })
                    .catch(error => {

                         this.isLoading = false;
                         this.isNoData =true;
                        alert(error);
                    })
                    
        
            
        },
        mounted(){
             this.emitter.on("Department", (item) => {
                
             // truyền data vào cho mảng 
             this.dataDepartment = item
            })
        }

        
};
</script>

<style lang="scss" scoped>

</style>