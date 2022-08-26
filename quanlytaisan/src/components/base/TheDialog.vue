<template>
  <!-- dialog form sửa tài chính -->
  <div class="dialog__handle" id="dialog__handle">
    <div class="dialog">
      <div class="dialog__header">
        <h3 id="handle">{{title}}</h3>
        <div
          @click="this.$emit('BtnCloseDialog')"
          class="icon icon-close icon__size-18 tooltip"
          id="btn--close">
          <Tooltip tooltiptext="Đóng" positiontooltip="left" style="margin-left:-30px" />
          </div>
      </div>
      <div class="dialog__body">
        <div class="m-row">
          <div class="group-input size-33">
            <div class="margin-text">
              Mã tài sản<span style="color: red">*</span>
            </div>
            <input
              v-model="dataItemDetail.fixedAssetCode"
              type="text"
              id="txtAssetCode"
              class="input input-width"
              required
              ref="txtAssetCode"
              tabindex="1001"
              @blur="checkRequired(this.$refs['txtAssetCode'])"
            />
          </div>

          <div class="group-input size-66 margin-lr-16">
            <div class="margin-text">
              Tên tài sản <span style="color: red">*</span>
            </div>
            <input
              v-model="dataItemDetail.fixedAssetName"
              type="text"
              name="Tên tài sản"
              id="txtNameAsset"
              class="input input-width"
              required
              placeholder="Nhập tên tài sản"
              tabindex="1002"
              ref ="txtNameAsset"
               @blur="checkRequired(this.$refs['txtNameAsset'])"
            />
          </div>
        </div>
        <div class="m-row">
          <div class="group-input size-33">
            <div class="margin-text">
              Mã bộ phận sử dụng<span style="color: red">*</span>
            </div>
            <ComboBox
              val="Chọn mã bộ phận sử dụng"
              v-bind:option="optionCBBox"
              dialog_icon="true"
              optionTable="true"
               @dataName="DataDepartment"
               typeCombobox="department"

               
            />
            <!-- <button class="combobox__control combobox__width">
                            <input type="text" name="Mã bộ phận sử dụng" id="" class="input input-width absolute" required value="" placeholder="Chọn mã bộ phận sử dụng" tabindex="1003">
                            <div class="icon icon-down1 icon__size-5 combobox__icon"></div>
                        </button> -->
          </div>

          <div class="group-input size-66 margin-lr-16">
            <div class="margin-text">Tên bộ phận sử dụng</div>
            <input
              type="text"
              name="Tên bộ phận sử dụng"
              id=""
              class="input input-width"
               v-bind:value="dataItemDetail.departmentName"
              disabled
            />
          </div>
        </div>
        <div class="m-row">
          <div class="group-input size-33">
            <div class="margin-text">
              Mã loại sản phẩm<span style="color: red">*</span>
            </div>
            <ComboBox
              val="Chọn mã loại sản phẩm"
              v-bind:option="optionCBBox1"
              dialog_icon="true"
              optionTable="true"
              @dataName="DataCategory"
              typeCombobox="category"
              
            />
          </div>

          <div class="group-input size-66 margin-lr-16">
            <div class="margin-text">Loại tài sản</div>
            <input
              type="text"
              name="Loại tài sản"
              id=""
              class="input input-width"
              v-bind:value="dataItemDetail.fixedAssetCategoryName"
              disabled
            />
          </div>
        </div>
        <div class="m-row">
          <div class="group-input size-33">
            <div class="margin-text">
              Số lượng<span style="color: red">*</span>
            </div>
            <button
              class="combobox__control combobox__width"
              style="border: none !important"
            >
              <input
                type="text"
                name="Số lượng"
                number
                required
                id="txtQuantity"
                class="input input-width text-align-right absolute"
                placeholder=""
                style="padding-right: 40px"
                v-model="dataItemDetail.quantity"
                tabindex="1005"
                ref="txtQuantity"

              @input="FormatNumberInput(dataItemDetail.quantity,'quantity')"
              @keypress="CheckNumber($event)"
              @blur="checkRequired(this.$refs['txtQuantity'])"
              />
              <div class="combobox__icon">
                <div
                  class="header-group__icon--item icon icon-up icon__size-8"
                ></div>
                <div
                  class="header-group__icon--item icon icon-down2 icon__size-8"
                ></div>
              </div>
            </button>
          </div>
          <div class="group-input size-33">
            <div class="margin-text">
              Nguyên giá<span style="color: red">*</span>
            </div>
            <input
              type="text"
              name="Nguyên giá"
              number
              required
              id="txtPrice"
              class="input input-width text-align-right"
              v-model="dataItemDetail.cost"
              tabindex="1006"
              ref ="txtPrice"
              @blur="checkRequired(this.$refs['txtPrice'])"
              @input="FormatNumberInput(dataItemDetail.cost,'cost')"
            />
          </div>
          <div class="group-input size-33 margin-right-10">
            <div class="margin-text">
              Số năm sử dụng <span style="color: red">*</span>
            </div>
            <button class="combobox__control combobox__width">
              <input
                type="text"
                name="Số năm sử dụng"
                number
                required
                id="yearuse"
                class="input input-width text-align-right absolute"
                placeholder=""
                style="padding-right: 40px"
                v-model="dataItemDetail.lifeTime"
                tabindex="1007"

           
              />
              <div class="combobox__icon">
                <div
                  class="header-group__icon--item icon icon-up icon__size-8"
                ></div>
                <div
                  class="header-group__icon--item icon icon-down2 icon__size-8"
                ></div>
              </div>
            </button>
          </div>
        </div>
        <div class="m-row">
          <div class="group-input size-33">
            <div class="margin-text">
              Tỉ lệ hao mòn (%) <span style="color: red">*</span>
            </div>
            <input
              type="text"
              name="Tỉ lệ hao mòn"
              number
              required
              ref="txtAtrophy"
              id="txtAtrophy"
              class="input input-width text-align-right"
              v-model="dataItemDetail.depreciationRate"
              tabindex="1008"
               @blur="checkRequired(this.$refs['txtAtrophy'])"
            />
          </div>
          <div class="group-input size-33">
            <div class="margin-text">
              Giá trị hao mòn năm<span style="color: red">*</span>
            </div>
            <input
              type="text"
              name="Giá trị hao mòn"
              number
              required
              id="txtdepreciation"
              class="input input-width text-align-right"
              v-model="dataItemDetail.depreciationYear"
              ref="txtdepreciation"
              tabindex="1009"
              @blur="checkRequired(this.$refs['txtdepreciation'])"
                @input="FormatNumberInput(dataItemDetail.depreciationYear,'depreciation')"
              

            />
          </div>
          <div class="group-input size-33 margin-right-10">
            <div class="margin-text">Năm theo dõi</div>
            <input
              type="text"
              name="Năm theo dõi"
              required
              id=""
              class="input input-width text-align-right"
              value="2021"
              disabled
            />
          </div>
        </div>

        <div class="m-row" style="margin-bottom: 25px">
          <div class="group-input size-33">
            <div class="margin-text">
              Ngày mua <span style="color: red">*</span>
            </div>
            <div class="combobox__control combobox__width  input-date" tabindex="1010">
              <!-- <input
                type="text"
                name="Ngày mua"
                id="purchasedate"
                required
                class="input input-width absolute"
                calender
                v-model="dataItemDetail.Startday"
                placeholder=""
                tabindex="1010"
                ref="purchasedate"
                 @blur="checkRequired(this.$refs['purchasedate'])"
              />  -->
              <datepicker format="MM/dd/yyyy" v-model="date.Depreciation" :hideInput="false" :full-month-name="true" typeable = "true"></datepicker>
              <div class="icon icon-calendar icon__size-18 combobox__icon"></div>
            </div>
          </div>
          <div class="group-input size-33">
            <div class="margin-text">
              Ngày bắt đầu sử dụng <span style="color: red">*</span>
            </div>
            <div class="combobox__control combobox__width input-date" tabindex="1011">
              <!-- <input
                type="text"
                name="Ngày bắt đầu sử dụng"
                id="startday"
                required
                class="input input-width absolute"
                calender
                value=""
                placeholder=""
                ref="startday"
                tabindex="1011"
                @blur="checkRequired(this.$refs['startday'])"
              /> -->
              <datepicker format="MM/dd/yyyy" typeable = "true" v-model="date.Startday"  :hideInput="false" :full-month-name="true">
              </datepicker>
              <div class="icon icon-calendar icon__size-18 combobox__icon"></div>
            </div>
          </div>
          <div class="group-input size-33 margin-right-10"></div>
        </div>
      </div>
      <div class="dialog__footer">
        <button @click="Validation()" class="btn btn-save active" tabindex="1012">Lưu</button>
        <button class="btn btn-cancel" id="btn-cancel" tabindex="1013" @keydown.tab.prevent="isFocusInput()" @click="this.$emit('BtnCloseDialog')">
          Hủy
        </button>
      </div>
    </div>
  </div>
  
    <Notify v-if="DialogNotify" v-bind:dataError="errors" @isShowDialogNotify="DialogNotify=false" />
    
</template>
<script>
import ComboBox from "../base/TheComboBox.vue"
import Notify from "../base/TheDialogNotify.vue"
import Datepicker from 'vuejs3-datepicker'
import Tooltip from '../base/TheTooltip.vue'
import {formatCash} from '../../js/common.js'

import axios from "axios"
export default {
    name:"TheDialog",
    components:{
        ComboBox,Notify,Datepicker,Tooltip
       
    },

    data(){
        return{
                dataItemDetail:[],
                dataName:
                    {
                        AssetCode:'Mã tài sản',
                        AssetName:"Tên tài sản",
                        Price :'Nguyên giá',
                        YearDate :'Số năm sử dụng',
                        Quantity : 'Số lượng',
                        Purchasedate:'Ngày mua',
                        Startday :'Ngày bắt đầu',
                        Depreciation:'Giá trị hao mòn năm',
                        Atrophy:'Tỉ lệ hao mòn'

                    }
                ,
                errors:[],
                DialogNotify :false,
                date:{
                  Startday : new Date(),
                  Depreciation : new Date()
                },
                Department:"",
                Category:"",
                items:[
                  {
                      AssetCode:"",
                      AssetName:"",
                      Categories:"",
                      PartsUsed  :"",
                      Quantity :0,
                      Price :0,
                      Atrophy :0,              
                      Depreciation: 0,
                      Category:"",
                      Department:""
                  }
                ],
                optionCBBox:[],
                optionCBBox1:[],
        }
    },
    methods:{
        /**
         * Validate dữ liệu
         * Author : Bùi Quang Điệp
         * Date:10/08/2022
         */
        Validation(){
          debugger
            var isCheckRequired = false;
            var res = this.dataItemDetail;
               console.log(res);
            try {
                    this.errors=[];
                    if(!res.fixedAssetCode)
                    {
                         this.errors.push('Mã tài sản không được bỏ trống .');
                         isCheckRequired =true;
                      
                    }
                    else{
                        // Không cho phép nhập quá 50 kí tự
                         if(res.fixedAssetCode.length >50)
                         {
                             this.errors.push('Mã tài sản không được quá 36 kí tự.');
                         }
                    }
                    if(!res.fixedAssetName)
                    {
                         this.errors.push('Tên tài sản không được bỏ trống .');
                         isCheckRequired =true;
                  
                    }
                     else{
                        // Không cho phép nhập quá 50 kí tự
                         if(res.fixedAssetCode.length >100)
                         {
                             this.errors.push('Tên tài sản không được quá 36 kí tự.');
                         }
                    }
                      if(!res.quantity)
                    {
                         this.errors.push('Số lượng không được bỏ trống .');
                         isCheckRequired =true;
                    }
                    if(!res.lifeTime)
                    {
                         this.errors.push('Số năm sử dụng không được bỏ trống .');
                         isCheckRequired =true;
                    }
                      if(!res.depreciationYear)
                    {

                         this.errors.push('Giá trị hao mòn không được bỏ trống .');
                         isCheckRequired =true;
                    }
                      if(!res.depreciationRate)
                    {
                         this.errors.push('Tỉ lệ hao mòn không được bỏ trống .');
                         isCheckRequired =true;
                    }
                       if(!res.cost)
                    {
                         this.errors.push('Nguyên giá không được bỏ trống .');
                         isCheckRequired =true;
                    }
                    
                    if(isCheckRequired == false){
                          if (1 / res.lifeTime != res.depreciationRate){
                              this.errors.push(`Tỉ lệ hao mòn phải bằng phải bằng 1 / số năm sử dụng`);
                          }
                          // if (res.Atrophy / yeardepreciation > price){
                          //     errorMsgs.push(`Hao mòn năm phải lớn hơn nguyên giá`);
                          // }
                         // this.dataItemDetail.depreciationYear = (res.depreciationRate)*(res.price);
                        if(this.errors.length>=1){
                            
                            this.DialogNotify = true;
                        }
                        
                        else{
                          debugger
                          // tạo object loại kiểu json
                          var cost=(this.dataItemDetail.cost).split('.').join('');
                          var depreciationYear = (this.dataItemDetail.depreciationYear).split('.').join('');
                          var dataAPI={
                             
                                fixedAssetCode: this.dataItemDetail.fixedAssetCode,
                                fixedAssetName: this.dataItemDetail.fixedAssetName,
                                fixedAssetCategoryName: this.dataItemDetail.FixedAssetCategoryName,
                                departmentID: this.dataItemDetail.departmentID,
                                fixedAssetCategoryID:this.dataItemDetail.fixedAssetCategoryID,
                                cost:cost ,
                                quantity: this.dataItemDetail.quantity,
                                depreciationRate: this.dataItemDetail.depreciationRate,
                                lifeTime: this.dataItemDetail.lifeTime,
                                depreciationYear: depreciationYear,
                          }
                                // Nếu thành công thì call API xử lý thêm dữ liệu
                                // Kiểm tra xem đang là chức năng thêm hay chỉnh sửa
                                
                                if(this.handler == "edit"){
                                      axios
                                      .put(
                                        `http://localhost:13846/api/v1/FixedAssets?FixedAssetID=${this.dataItemDetail.fixedAssetID}`,
                                        JSON.stringify(dataAPI),
                                        {
                                          headers: {
                                            "Content-Type": "application/json",
                                          },
                                        }
                                      )
                                      .then((response) => {
                                        this.$emit('BtnCloseDialog');
                                        this.emitter.emit("LoadData",this.dataItemDetail.fixedAssetCode);
                                        console.log(response);
                                       
                                       
                                      })
                                      .catch((error) => {
                                        console.log(error.message);
                                      });
                                       this.$emit("handlerName","Sửa");
                                      
                                      
                                      
    
                               
                                }
                                // Kiểm tra nếu là thêm mới thì xử lý 
                                 if(this.handler== "add"){
                                  axios
                                .post(
                                  "http://localhost:13846/api/v1/FixedAssets",
                                  JSON.stringify(dataAPI),
                                  {
                                    headers: {
                                      "Content-Type": "application/json",
                                    },
                                  }
                                )
                                .then((response) => {
                                  this.$emit('BtnCloseDialog');
                                  this.emitter.emit("LoadData",this.dataItemDetail.fixedAssetCode);
                                  console.log(response);
                                })
                                .catch((error) => {
                                  console.log(error.message);
                                });
                                  this.$emit("handlerName","Thêm");
                              
                                }
                                 
                               
                        }
                    }
                    else{
                      alert("Vui lòng nhập trường còn trống");
                    }
                   
                   
                    
                               
            } catch (error) {
                console.log(error)
            }
        },
         /**
         * Required những dữu liệu cần thiết
         * Author : Bùi Quang Điệp
         * Date:10/08/2022
         */
        checkRequired(res){
            // Kiểm tra nếu input rỗng thì hiển thị border đỏ và thông báo cho người dùng
            if(!res.value)
            {
                res.classList.add("border-red");
                // this.isRequired = true;


            }
            else{
               //  this.isRequired = false;
                res.classList.remove("border-red");
            }
        },
           /**
         * Kiểm tra xem nhập liệu đầu vào có phải là số hay không
         * Author : Bùi Quang Điệp
         * Date:10/08/2022
         */
        CheckNumber(e){
        var number = String.fromCharCode(e.keyCode),
         valid = /^[0-9]+$/.test(number);

        if (!valid) {
            e.preventDefault();
        }
            

        },
         /**
         * Định dạng dấu phân cách hàng nghìn cho số lượng khi nhập liệu
         * Author : Bùi Quang Điệp
         * Date:10/08/2022
         */
        FormatNumberInput(val,item){
            try {
              debugger
                if(item == "cost")
                {
                  this.dataItemDetail.cost = formatCash(val);
                }
                  if(item == "quantity")
                {
                  this.dataItemDetail.quantity = formatCash(val);
                }
                  if(item == "depreciation")
                {
                  this.dataItemDetail.depreciationYear = formatCash(val);
                }
             
                
                

                
            } catch (error) {
                console.log(error);
            }

        },
        /**
         * Focus vào ô input dầu tiên
         * Author : Bùi Quang Điệp
         * Date:10/08/2022
         */
        isFocusInput(){
                this.$refs['txtAssetCode'].focus();
        },
        
        // Lấy dữu liệu cho Tên phòng ban
        DataDepartment(e){
          
          this.dataItemDetail.departmentName =e.DepartmentName;
          this.dataItemDetail.departmentID =e.DepartmentID;
        },
        // Lấy dữu liệu cho Tên loại sản phẩm
        DataCategory(e){
          this.dataItemDetail.fixedAssetCategoryName =e.FixedAssetCategoryName;
          this.dataItemDetail.fixedAssetCategoryID =e.FixedAssetCategoryID;
        }

       
        
    },
    created(){
          
        // gám mảng DataItemDetail vào Cho item để có thể sử dụng v-model
         
                  this.dataItemDetail = this.item
        // Lấy dữ liệu từ api đổ vào combobox
        axios.get("http://localhost:13846/api/v1/Departments")
        .then(res=>{
          console.log(res);
          this.optionCBBox = res.data;
        })
        .catch(error=>{
          console.log(error);
        })
        axios.get("http://localhost:13846/api/v1/FixedAssetCategorys")
        .then(res=>{
          console.log(res);
          this.optionCBBox1 = res.data;
        })
        .catch(error=>{
          console.log(error);
        })


    },


   mounted() {
    
      this.isFocusInput();
       this.dataItemDetail.cost = formatCash(this.dataItemDetail.cost);

  },
   
    props:{
        isShowDialog1 : Boolean,
        item:Array,
        title:String,
        handler:String
    },
  

    
}
</script>