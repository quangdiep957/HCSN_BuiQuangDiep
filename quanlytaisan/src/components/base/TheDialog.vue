<template>
  <!-- dialog form sửa tài chính -->
  <div class="dialog__handle" id="dialog__handle">
    <div class="dialog">
      <div class="dialog__header">
        <h3 id="handle">{{ title }}</h3>
        <div @click="this.$emit('BtnCloseDialog')" class="icon icon-close icon__size-18 tooltip" id="btn--close">
          <Tooltip tooltiptext="Đóng" positiontooltip="left" style="margin-left:15px" />
        </div>
      </div>
      <div class="dialog__body">
        <div class="m-row">
          <div class="group-input size-33 tooltip tooltipRequired" @mouseover="showTooltip($event)"
            @mouseleave="hideTooltip($event)">
            <div class="margin-text">
              Mã tài sản<span style="color: red">*</span>
              <Tooltip class="tooltipRequiredDetail" :tooltiptext=requiredData.fixedAssetCode positiontooltip="top"
                v-if="isShowTooltipRequired && requiredData.fixedAssetCode!= ''" />
            </div>
            <input v-model="dataItemDetail.fixedAssetCode" type="text" id="fixedAssetCode" class="input input-width "
              ref="fixedAssetCode" required tabindex="1001" @blur="checkRequired('fixedAssetCode')"
              @input="capitalizationText()" />
          </div>

          <div class="group-input size-66 margin-lr-16 tooltip tooltipRequired" @mouseover="showTooltip($event)"
            @mouseleave="hideTooltip($event)">
            <div class="margin-text">
              Tên tài sản <span style="color: red">*</span>
              <Tooltip class="tooltipRequiredDetail" :tooltiptext=requiredData.fixedAssetName positiontooltip="top"
                v-if="isShowTooltipRequired && requiredData.fixedAssetName!=''" />
            </div>
            <input v-model="dataItemDetail.fixedAssetName" type="text" name="Tên tài sản" id="fixedAssetName"
              class="input input-width tooltip" required placeholder="Nhập tên tài sản" tabindex="1002"
              ref="fixedAssetName" @blur="checkRequired('fixedAssetName')" @input="capitalizationText()" />
          </div>
        </div>
        <div class="m-row">
          <div class="group-input size-33 tooltip tooltipRequired" @mouseover="showTooltip($event)"
            @mouseleave="hideTooltip($event)">
            <div class="margin-text">
              Mã bộ phận sử dụng<span style="color: red">*</span>
              <Tooltip class="tooltipRequiredDetail" :tooltiptext=requiredData.departmentID positiontooltip="top"
                v-if="isShowTooltipRequired && requiredData.departmentID!=''" />
            </div>
            <ComboBox val="Chọn mã bộ phận sử dụng" :option="optionCBBox" dialog_icon="true" optionTable="true"
              :isCheck="departmentCheck" @isCheck="departmentCheck=true" @checkFalse="isCheck=false"
              @dataName="DataDepartment" nameTable="Department" :dataItemCombobox=dataItemDetailFake
              @removeData="dataItemDetailFake.departmentID=''" @checkRequired="checkRequiredComboboxDepartment"
              :borderRed="borderRedDepartment" />
            <!-- <button class="combobox__control combobox__width">
                            <input type="text" name="Mã bộ phận sử dụng" id="" class="input input-width tooltip absolute" required value="" placeholder="Chọn mã bộ phận sử dụng" tabindex="1003">
                            <div class="icon icon-down1 icon__size-5 combobox__icon"></div>
                        </button> -->
          </div>

          <div class="group-input size-66 margin-lr-16">
            <div class="margin-text">Tên bộ phận sử dụng</div>
            <input type="text" name="Tên bộ phận sử dụng" id="" class="input input-width tooltip"
              v-bind:value="dataItemDetail.departmentName" disabled @input="capitalizationText()" />
          </div>
        </div>
        <div class="m-row">
          <div class="group-input size-33 tooltip tooltipRequired" @mouseover="showTooltip($event)"
            @mouseleave="hideTooltip($event)">
            <div class="margin-text">
              Mã loại sản phẩm<span style="color: red">*</span>
              <Tooltip class="tooltipRequiredDetail" :tooltiptext=requiredData.fixedAssetCategoryID positiontooltip="top"
                v-if="isShowTooltipRequired && requiredData.fixedAssetCategoryID!=''" />
            </div>
            <ComboBox val="Chọn mã loại sản phẩm" v-bind:option="optionCBBox1" dialog_icon="true" optionTable="true"
              :isCheck="categoryCheck" @isCheck="categoryCheck=true" @checkFalse="categoryCheck=false"
              @dataName="DataCategory" nameTable="FixedAssetCategory" :dataItemCombobox=dataItemDetailFake
              @removeData="dataItemDetailFake.fixedAssetCategoryID=''" @checkRequired="checkRequiredComboboxCategory"
              :borderRed="borderRedCategory" />
          </div>

          <div class="group-input size-66 margin-lr-16">
            <div class="margin-text">Loại tài sản</div>
            <input type="text" name="Loại tài sản" id="" class="input input-width tooltip"
              v-bind:value="dataItemDetail.fixedAssetCategoryName" disabled @input="capitalizationText()" />
          </div>
        </div>
        <div class="m-row">
          <div class="group-input size-33 tooltip tooltipRequired" @mouseover="showTooltip($event)"
            @mouseleave="hideTooltip($event)">
            <div class="margin-text">
              Số lượng<span style="color: red">*</span>
              <Tooltip class="tooltipRequiredDetail" :tooltiptext=requiredData.quantity positiontooltip="top"
                v-if="isShowTooltipRequired && requiredData.quantity!=''" />
            </div>
            <button class="combobox__control combobox__width" style="border: none !important">
              <input type="text" name="Số lượng" number required id="quantity"
                class="input input-width tooltip text-align-right absolute" placeholder="" style="padding-right: 40px"
                v-model="dataItemDetail.quantity" tabindex="1005" ref="quantity"
                @input="formatNumberInput(dataItemDetail.quantity, 'quantity')" @keypress="CheckNumber($event)"
                @blur="checkRequired('quantity')" />
              <div class="combobox__icon">
                <div class="header-group__icon--item icon icon-up icon__size-8"></div>
                <div class="header-group__icon--item icon icon-down2 icon__size-8"></div>
              </div>
            </button>
          </div>
          <div class="group-input size-33 tooltip tooltipRequired" @mouseover="showTooltip($event)"
            @mouseleave="hideTooltip($event)">
            <div class="margin-text">
              Nguyên giá<span style="color: red">*</span>
              <Tooltip class="tooltipRequiredDetail" :tooltiptext=requiredData.cost positiontooltip="top"
                v-if="isShowTooltipRequired && requiredData.cost !=''" />
            </div>
            <input type="text" name="Nguyên giá" number required id="cost"
              class="input input-width tooltip text-align-right" v-model="dataItemDetail.cost" tabindex="1006"
              ref="cost" @blur="checkRequired('cost')" @input="formatNumberInput(dataItemDetail.cost, 'cost')" />
          </div>
          <div class="group-input size-33 margin-right-10 tooltip tooltipRequired" @mouseover="showTooltip($event)"
            @mouseleave="hideTooltip($event)">
            <div class="margin-text">
              Số năm sử dụng <span style="color: red">*</span>
              <Tooltip class="tooltipRequiredDetail" :tooltiptext=requiredData.lifeTime positiontooltip="top"
                v-if="isShowTooltipRequired && requiredData.lifeTime!=''" />
            </div>
            <button class="combobox__control combobox__width">
              <input type="text" name="Số năm sử dụng" number required id="yearuse" ref="lifeTime"
                class="input input-width tooltip text-align-right absolute" placeholder="" style="padding-right: 40px"
                v-model="dataItemDetail.lifeTime" tabindex="1007" 
                @blur="checkRequired('lifeTime')" />
                />
              <div class="combobox__icon">
                <div class="header-group__icon--item icon icon-up icon__size-8"></div>
                <div class="header-group__icon--item icon icon-down2 icon__size-8"></div>
              </div>
            </button>
          </div>
        </div>
        <div class="m-row">
          <div class="group-input size-33 tooltip tooltipRequired" @mouseover="showTooltip($event)"
            @mouseleave="hideTooltip($event)">
            <div class="margin-text">
              Tỉ lệ hao mòn (%) <span style="color: red">*</span>
              <Tooltip class="tooltipRequiredDetail" :tooltiptext=requiredData.depreciationRate positiontooltip="top"
                v-if="isShowTooltipRequired && requiredData.depreciationRate!=''" />
            </div>
            <input type="text" name="Tỉ lệ hao mòn" number required ref="depreciationRate" id="txtAtrophy"
              class="input input-width tooltip text-align-right" v-model="dataItemDetail.depreciationRate"
              tabindex="1008" @blur="checkRequired('depreciationRate')" />
          </div>
          <div class="group-input size-33 tooltip tooltipRequired" @mouseover="showTooltip($event)"
            @mouseleave="hideTooltip($event)">
            <div class="margin-text">
              Giá trị hao mòn năm<span style="color: red">*</span>
              <Tooltip class="tooltipRequiredDetail" :tooltiptext=requiredData.depreciationYear positiontooltip="top"
                v-if="isShowTooltipRequired && requiredData.depreciationYear!=''" />
            </div>
            <input type="text" name="Giá trị hao mòn" number required id="depreciationYear"
              class="input input-width tooltip text-align-right" v-model="dataItemDetail.depreciationYear"
              ref="depreciationYear" tabindex="1009" @blur="checkRequired('depreciationYear')"
              @input="formatNumberInput(dataItemDetail.depreciationYear, 'depreciation')" />
          </div>
          <div class="group-input size-33 margin-right-10">
            <div class="margin-text">Năm theo dõi</div>
            <input type="text" name="Năm theo dõi" required id="" class="input input-width tooltip text-align-right"
              value="2021" disabled />
          </div>
        </div>

        <div class="m-row" style="margin-bottom: 25px">
          <div class="group-input size-33">
            <div class="margin-text">
              Ngày mua <span style="color: red">*</span>
            </div>
            <div class="combobox__control combobox__width  input-date" tabindex="1010"
              @focus="showMenuDateDepreciation">
              <!-- <input
                type="text"
                name="Ngày mua"
                id="purchasedate"
                required
                class="input input-width tooltip absolute"
                calender
                v-model="dataItemDetail.Startday"
                placeholder=""
                tabindex="1010"
                ref="purchasedate"
                 @blur="checkRequired(this.$refs['purchasedate'])"
              />  -->
              <datepicker format="MM/dd/yyyy" v-model="date.Depreciation" :hideInput="false" :full-month-name="true"
                :inline="false" :minimum-view="'day'" :maximum-view="'day'" typeable="true"
                :class="{'dateActive':isShowMenuDateDepreciation}"></datepicker>
              <div class="icon icon-calendar icon__size-18 combobox__icon"></div>
            </div>
          </div>
          <div class="group-input size-33">
            <div class="margin-text">
              Ngày bắt đầu sử dụng <span style="color: red">*</span>
            </div>
            <div class="combobox__control combobox__width input-date" tabindex="1011" @focus="showMenuDateStartDay">
              <!-- <input
                type="text"
                name="Ngày bắt đầu sử dụng"
                id="startday"
                required
                class="input input-width tooltip absolute"
                calender
                value=""
                placeholder=""
                ref="startday"
                tabindex="1011"
                @blur="checkRequired(this.$refs['startday'])"
              /> -->
              <datepicker format="MM/dd/yyyy" typeable="true" v-model="date.Startday" :hideInput="false"
                :minimum-view="'day'" :class="{'dateActive':isShowMenuDateStart}" :maximum-view="'day'"
                :full-month-name="true" :inline="false">
              </datepicker>
              <div class="icon icon-calendar icon__size-18 combobox__icon"></div>
            </div>
          </div>
          <div class="group-input size-33 margin-right-10"></div>
        </div>
      </div>
      <div class="dialog__footer">
        <button @click="Validation()" class="btn btn-save active" tabindex="1012">Lưu</button>
        <button class="btn btn-cancel" id="btn-cancel" tabindex="1013" @keydown.tab.prevent="isFocusInput()"
          @click="CloseDialog">
          Hủy
        </button>
      </div>
    </div>
  </div>

  <Notify v-if="DialogNotify" v-bind:dataError="errors" @isShowDialogNotify="DialogNotify = false"
    :buttonNames="buttonNames" @isSaveData="SaveData" @RemoveDialog="RemoveDialog" :errorName="errorName" />


</template>
<script>
import ComboBox from "../base/TheComboBox.vue"
import Notify from "../base/TheDialogNotify.vue"
import Datepicker from 'vuejs3-datepicker'
import Tooltip from '../base/TheTooltip.vue'
import { formatCash } from '../../js/common.js'

import axios from "axios"
export default {
  name: "TheDialog",
  components: {
    ComboBox, Notify, Datepicker, Tooltip

  },

  data() {
    return {
      dataItemDetail: [],
      display: "none",
      isShowMenuDateStart: false,
      isShowMenuDateDepreciation: false,
      categoryCheck: false,
      departmentCheck: false,
      dataName:
      {
        AssetCode: 'Mã tài sản',
        AssetName: "Tên tài sản",
        Price: 'Nguyên giá',
        YearDate: 'Số năm sử dụng',
        Quantity: 'Số lượng',
        Purchasedate: 'Ngày mua',
        Startday: 'Ngày bắt đầu',
        Depreciation: 'Giá trị hao mòn năm',
        Atrophy: 'Tỉ lệ hao mòn'

      },

      buttonNames: ['Đồng ý'],
      errors: [],
      DialogNotify: false,
      date: {
        Startday: new Date(),
        Depreciation: new Date()
      },
      Department: "",
      Category: "",
      items: [
        {
          AssetCode: "",
          AssetName: "",
          Categories: "",
          PartsUsed: "",
          Quantity: 0,
          Price: 0,
          Atrophy: 0,
          Depreciation: 0,
          Category: "",
          Department: ""
        }
      ],
      optionCBBox: [],
      optionCBBox1: [],
      errorName: "",
      dataItemDetailFake: [],
      requiredData:
      {
        fixedAssetCode: "",
        fixedAssetName: "",
        quantity: "",
        depreciationYear: "",
        depreciationRate: "",
        cost: "",
        departmentID: "",
        fixedAssetCategoryID: "",
        lifeTime:""
      },
      opacity: 0,
      isShowTooltipRequired: false,
      borderRed: false,
      isCheckRequiredComboboxDepartment: false,
      isCheckRequiredComboboxCategory: false
    }
  },
  methods: {
    /**
     * Validate dữ liệu
     * Author : Bùi Quang Điệp
     * Date:10/08/2022
     */
    Validation() {

      var isCheckRequired = false;
      var res = this.dataItemDetail;
      console.log(res);
      try {
        this.errors = [];
        if (!res.fixedAssetCode) {
          this.errors.push('Mã tài sản không được bỏ trống .');
          isCheckRequired = true;

        }
        else {
          // Không cho phép nhập quá 50 kí tự
          if (res.fixedAssetCode.length > 50) {
            this.errors.push('Mã tài sản không được quá 36 kí tự.');
          }
        }
        if (!res.fixedAssetName) {
          this.errors.push('Tên tài sản không được bỏ trống .');
          isCheckRequired = true;

        }
        else {
          // Không cho phép nhập quá 50 kí tự
          if (res.fixedAssetCode.length > 100) {
            this.errors.push('Tên tài sản không được quá 36 kí tự.');
          }
        }
        if (!res.quantity) {
          this.errors.push('Số lượng không được bỏ trống .');
          isCheckRequired = true;
        }
        if (!res.lifeTime) {
          this.errors.push('Số năm sử dụng không được bỏ trống .');
          isCheckRequired = true;
        }
        if (!res.depreciationYear) {

          this.errors.push('Giá trị hao mòn không được bỏ trống .');
          isCheckRequired = true;
        }
        if (!res.depreciationRate) {
          this.errors.push('Tỉ lệ hao mòn không được bỏ trống .');
          isCheckRequired = true;
        }
        if (!res.cost) {
          this.errors.push('Nguyên giá không được bỏ trống .');
          isCheckRequired = true;
        }

        if (isCheckRequired == false) {
          if (1 / res.lifeTime != res.depreciationRate) {
            this.errors.push(`Tỉ lệ hao mòn phải bằng phải bằng 1 / số năm sử dụng`);
          }
          if (res.depreciationRate / depreciationYear > cost) {
            this.errors.push(`Hao mòn năm phải lớn hơn nguyên giá`);
          }
          // this.dataItemDetail.depreciationYear = (res.depreciationRate)*(res.price);
          if (this.errors.length >= 1) {

            this.DialogNotify = true;
          }

          else {
            this.opacity = 0;
            // tạo object loại kiểu json
            var cost = this.unFormNumber(this.dataItemDetail.cost);
            var depreciationYear = this.unFormNumber(this.dataItemDetail.depreciationYear);

            var dataAPI = {

              fixedAssetCode: this.dataItemDetail.fixedAssetCode,
              fixedAssetName: this.dataItemDetail.fixedAssetName,
              fixedAssetCategoryName: this.dataItemDetail.FixedAssetCategoryName,
              departmentID: this.dataItemDetail.departmentID,
              fixedAssetCategoryID: this.dataItemDetail.fixedAssetCategoryID,
              cost: cost,
              quantity: this.dataItemDetail.quantity,
              depreciationRate: this.dataItemDetail.depreciationRate,
              lifeTime: this.dataItemDetail.lifeTime,
              depreciationYear: depreciationYear,
            }
            // Nếu thành công thì call API xử lý thêm dữ liệu
            // Kiểm tra xem đang là chức năng thêm hay chỉnh sửa

            if (this.handler == "edit") {
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
                  this.emitter.emit("LoadData", this.dataItemDetail.fixedAssetCode);
                  console.log(response);
                  this.$emit("handlerName", "Sửa");


                })
                .catch((error) => {
                  console.log(error.message);
                });

            }

            // Kiểm tra nếu là thêm mới thì xử lý 
            if (this.handler == "add") {
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
                  this.emitter.emit("LoadData", this.dataItemDetail.fixedAssetCode);
                  console.log(response);
                  this.$emit("handlerName", "Thêm");

                })
                .catch((error) => {

                  console.log(error.message);
                  // Nếu lỗi trùng mã thì hiển thị thông báo cho người dùng
                  if (error.response.data.error == "e1002") {

                    this.DialogNotify = true;
                    this.errors = [];
                    this.buttonNames = ['Đồng ý'],

                      this.errors.push(`Mã tài sản  đã có trong hệ thống !`)
                    this.errorName = error.response.data.data[1];

                  }
                });


            }


          }
        }
        else {

          if (!res.fixedAssetCode) {
            this.requiredData.fixedAssetCode = 'Mã tài sản không được bỏ trống .';
            this.checkRequired('fixedAssetCode');
          }
       
          if (!res.fixedAssetName) {
            this.requiredData.fixedAssetName = 'Tên tài sản không được bỏ trống .';
            this.checkRequired('fixedAssetName');
          }
        
          if (!res.quantity) {
            this.requiredData.quantity = 'Số lượng không được bỏ trống .';
            this.checkRequired('quantity');
          }

          if (!res.depreciationYear) {

            this.requiredData.depreciationYear = 'Giá trị hao mòn không được bỏ trống .';
            this.checkRequired('depreciationYear');
          }
         
          if (!res.lifeTime) {
            this.requiredData.lifeTime = 'Số năm sử dụng không được bỏ trống .';
            this.checkRequired('lifeTime');
            }

          if (!res.depreciationRate) {
            this.requiredData.depreciationRate = 'Tỉ lệ hao mòn không được bỏ trống .';
            this.checkRequired('depreciationRate');
          }
         

          if (!res.cost) {
            this.requiredData.cost = 'Nguyên giá không được bỏ trống .';
            this.checkRequired('cost');
          }

          
          if (this.isCheckRequiredComboboxDepartment == false) {
            this.requiredData.departmentID = 'Mã phòng ban không được để trống .';
            this.borderRedDepartment = true;
          }
          else {
            this.requiredData.departmentID = '';
            this.borderRedDepartment = false;
          }


          if (this.isCheckRequiredComboboxCategory == false) {
            this.requiredData.fixedAssetCategoryID = 'Mã loại tài sản không được để trống .';
            this.borderRedCategory = true;
          }
          else {
            this.requiredData.fixedAssetCategoryID = '';
            this.borderRedCategory = false;
          }

          this.isShowTooltipRequired = true;
          this.opacity = 1;
          setTimeout(() => {
            this.opacity = 0;
            this.isShowTooltipRequired = false;
          }, 2000)


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
    checkRequired(name) {
      debugger
      // Kiểm tra name có dữ liệu hay không
      if (name != '') {
        if (!event.currentTarget.value) {
          this.$refs[name].classList.add("border-red");
          // this.isRequired = true;
        }
        else {
          //  this.isRequired = false;
          this.requiredData[name] = '';
          this.$refs[name].classList.remove("border-red");
        }

      }

    },
    /**
  * Kiểm tra xem nhập liệu đầu vào có phải là số hay không
  * Author : Bùi Quang Điệp
  * Date:10/08/2022
  */
    CheckNumber(e) {
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
    formatNumberInput(val, item) {
      try {

        if (item == "cost") {
          this.dataItemDetail.cost = formatCash(val);
        }
        if (item == "quantity") {
          this.dataItemDetail.quantity = formatCash(val);
        }
        if (item == "depreciation") {
          this.dataItemDetail.depreciationYear = formatCash(val);
        }

        this.calculatedepreciationYear();



      } catch (error) {
        console.log(error);
      }

    },
    /**
     * Focus vào ô input dầu tiên
     * Author : Bùi Quang Điệp
     * Date:10/08/2022
     */
    isFocusInput() {
      this.$refs['fixedAssetCode'].focus();
    },

    /**
   * Lấy dữ liệu bộ phận sử dụng
   * Author : Bùi Quang Điệp
   * Date:10/08/2022
   */
    DataDepartment(e) {

      this.dataItemDetail.departmentName = e.DepartmentName;
      this.dataItemDetail.departmentID = e.DepartmentID;
    },
    /**
   * Lấy dữ liệu loại sản phẩm
   * Author : Bùi Quang Điệp
   * Date:10/08/2022
   */

    DataCategory(e) {
      this.dataItemDetail.fixedAssetCategoryName = e.FixedAssetCategoryName;
      this.dataItemDetail.fixedAssetCategoryID = e.FixedAssetCategoryID;
    },


    /**
     * Tính tự động giá trị hao mòn năm
     * Author : Bùi Quang Điệp
     * Date:10/08/2022
     */
    calculatedepreciationYear() {
      try {
        if (this.dataItemDetail.cost != "" || this.dataItemDetail.depreciationRate != "") {
          var cost = this.unFormNumber(this.dataItemDetail.cost);
          var depreciationRate = (this.dataItemDetail.depreciationRate);
          this.dataItemDetail.depreciationYear = formatCash(cost * depreciationRate);
        }
      }
      catch (error) {
        console.log(error);
      }

    },
    /**
    * Hủy bỏ định dạng number
    * Author : Bùi Quang Điệp
    * Date:10/08/2022
    */
    unFormNumber(val) {
      return val.split('.').join('');
    },

    /**
   * Tự động viết hoa chữ cái đầu cho các trường
   *   - Mã tài sản
   *   - Tên tài sản
   *   - Tên bộ phận sử dụng
   *   - Loại tài sản
   * Author : Bùi Quang Điệp
   * Date:10/08/2022
   */
    capitalizationText() {

      var text = event.currentTarget.value;
      if (text != "") {

        text = text.charAt(0).toLocaleUpperCase() + text.slice(1);

      }
      event.currentTarget.value = text;

    }
    ,
    /**
   * Đóng dialog thêm mới và show thông báo nếu người dùng hủy lưu khi chỉnh sửa
   * Author : Bùi Quang Điệp
   * Date:10/08/2022
   */
    CloseDialog() {
      if (this.handler == "edit") {
        this.DialogNotify = true;
        this.errors = [];
        this.buttonNames = ['Lưu', 'Không Lưu', 'Hủy bỏ'],
          this.errors.push("Thông tin thay đổi sẽ không được cập nhật nếu bạn không lưu. Bạn có muốn lưu các thay đổi này ?")
      }
      else {
        this.$emit('BtnCloseDialog');
      }

    },

    /**
    * Nếu người dùng ấn lưu từ form dialog cảnh báo thì thực hiện lưu
    * Author : Bùi Quang Điệp
    * Date:10/08/2022
    */
    SaveData() {
      this.Validation();
    },

    /**
  * Đóng cả 2 dialog
  * Author : Bùi Quang Điệp
  * Date:15/08/2022
  */
    RemoveDialog() {
      this.DialogNotify = false;
      this.$emit('BtnCloseDialog');
    },

    /**
  * khi focus mở chọn ngày
  * Author : Bùi Quang Điệp
  * Date:15/08/2022
  */
    showMenuDateStartDay() {
      this.display = "block";
      this.isShowMenuDateStart = true;
    },

    showMenuDateDepreciation() {
      this.display = "block";
      this.isShowMenuDateDepreciation = true;
    },

    /**
  * Hiển thị tooltip required khi hover 
  * Author : Bùi Quang Điệp
  * Date:07/09/2022
  */
    showTooltip(event) {
      event.currentTarget.classList.add("activeTooltip");
      this.isShowTooltipRequired = true;
    },

    /**
  * ẩn tooltip required khi bỏ hover
  * Author : Bùi Quang Điệp
  * Date:07/09/2022
  */
    hideTooltip(event) {
      event.currentTarget.classList.remove("activeTooltip");
      this.isShowTooltipRequired = false;
    },

    /**
     * Check required combobox
     * Author : Bùi Quang Điệp
     * Date:07/09/2022
     */
    checkRequiredComboboxDepartment(item) {
      
      this.isCheckRequiredComboboxDepartment = item;
    },

     /**
     * Check required combobox
     * Author : Bùi Quang Điệp
     * Date:07/09/2022
     */
    checkRequiredComboboxCategory(item) {
      this.isCheckRequiredComboboxCategory = item;
    }




  },
  created() {
  /**
  *  Lấy dữu liệu các combobox đổ vào dialog
  * Author : Bùi Quang Điệp
  * Date:07/09/2022
  */
  //gán mảng DataItemDetail vào Cho item để có thể sử dụng v-model
    this.dataItemDetail = this.item
    this.dataItemDetailFake = this.dataItemDetail;
    // Lấy dữ liệu từ api đổ vào combobox
    axios.get("http://localhost:13846/api/v1/Departments")
      .then(res => {
        console.log(res);
        this.optionCBBox = res.data;
      })
      .catch(error => {
        console.log(error);
      })
    axios.get("http://localhost:13846/api/v1/FixedAssetCategorys")
      .then(res => {
        console.log(res);
        this.optionCBBox1 = res.data;
      })
      .catch(error => {
        console.log(error);
      })


  },


  mounted() {
    /**
  *  Khi mở dialog mặc định focus vào input đầu tiên
  * Author : Bùi Quang Điệp
  * Date:07/09/2022
  */
    this.isFocusInput();
    this.dataItemDetail.cost = formatCash(this.dataItemDetail.cost);
    this.dataItemDetail.depreciationYear = formatCash(this.dataItemDetail.depreciationYear);

   


  },
  updated(){
   
  /**
  *  Khi combobox thay đổi thì kiểm tra nếu đã có dữ lieju thì bỏ tooltip
  * Author : Bùi Quang Điệp
  * Date:07/09/2022
  */
    if (this.isCheckRequiredComboboxDepartment == true) {
      this.requiredData.departmentID = '';
      this.borderRedDepartment = false;
    }

   /**
  *  Khi combobox thay đổi thì kiểm tra nếu đã có dữ lieju thì bỏ tooltip
  * Author : Bùi Quang Điệp
  * Date:07/09/2022
  */
    if (this.isCheckRequiredComboboxCategory == true) {
      this.requiredData.fixedAssetCategoryID = '';
      this.borderRedCategory = false;
    }
  },

  props: {
    isShowDialog1: Boolean,
    item: Array,
    title: String,
    handler: String
  },



}
</script>
<style>
.dateActive .vuejs3-datepicker__calendar.vuejs3-green {
  display: v-bind('display') !important;
}

.group-input.tooltip .tooltiptext.tooltipRequiredDetail {
  opacity: v-bind('opacity') !important;
}

.activeTooltip.group-input.tooltip .tooltiptext.tooltipRequiredDetail {
  opacity: 1 !important;
}
</style>