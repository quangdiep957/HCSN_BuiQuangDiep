<template>
  <div
    class="dialog__handle"
    id="dialog__handle"
    @keydown.esc="closeKeyESC"
    ref="dialog"
  >
    <div class="dialog">
      <div class="dialog__header">
        <h3 id="handle">{{ title }}</h3>
        <div
          @click="this.$emit('btnCloseDialog')"
          class="icon icon-close icon__size-18 tooltip"
          id="btn--close"
        >
          <Tooltip
            tooltiptext="Đóng(ESC)"
            positiontooltip="left"
            style="margin-left: 15px"
          />
        </div>
      </div>
      <div class="dialog__body">
        <div class="m-row">
          <Input
            @data="setData"
            :focus="isFocus"
            @isFocus="isFocus = false"
            classBorderInput="group-input size-33"
            @mouseover="showTooltip"
            @mouseleave="hideTooltip"
            labelInput=" Mã tài sản"
            :tooltipInput="requiredData.fixedAssetCode"
            :isShowTooltipRequired="isShowTooltipRequired"
            v-model="dataItemDetail.fixedAssetCode"
            :dataRequired="requiredData.fixedAssetCode"
            properties="fixedAssetCode"
            required="required"
            tabindex="1001"
            @blur="blurHandle"
            :checkRequiredAll="checkRequiredAll"
            :disable="disabled"
            :focusRequired="dataRequiredTop[0]"
            :maxLength="20"
            @errorLength="showTooltipErrorLength"
            @pass="hideTooltipErrorLength"
            :dataChange="dataChange"
            @dataChange="dataChange = false"
            @keydown="capitalizationText()"
          />
          <!-- <div class="margin-text">
              Mã tài sản<span style="color: red">*</span>
              <Tooltip class="tooltipRequiredDetail" :tooltiptext=requiredData.fixedAssetCode positiontooltip="top"
                v-if="isShowTooltipRequired && requiredData.fixedAssetCode!= ''" />
            </div>
            <input v-model="dataItemDetail.fixedAssetCode" type="text" id="fixedAssetCode" class="input input-width "
              ref="fixedAssetCode" required tabindex="1001" @blur="checkRequired('fixedAssetCode')"
              @input="capitalizationText()" /> -->

          <Input
            classBorderInput="group-input size-66 margin-lr-16"
            @mouseover="showTooltip"
            @mouseleave="hideTooltip"
            labelInput="Tên tài sản"
            :tooltipInput="requiredData.fixedAssetName"
            :isShowTooltipRequired="isShowTooltipRequired"
            v-model="dataItemDetail.fixedAssetName"
            :dataRequired="requiredData.fixedAssetName"
            properties="fixedAssetName"
            required="required"
            tabindex="1002"
            @blur="blurHandle"
            :checkRequiredAll="checkRequiredAll"
            @keydown="capitalizationText()"
            :maxLength="255"
            :focusRequired="dataRequiredTop[0]"
          />
        </div>
        <div class="m-row">
          <div
            class="group-input size-33 tooltip tooltipRequired"
            @mouseover="showTooltip($event)"
            @mouseleave="hideTooltip($event)"
          >
            <div class="margin-text">
              Mã bộ phận sử dụng<span style="color: red"> *</span>
              <Tooltip
                class="tooltipRequiredDetail"
                :tooltiptext="requiredData.departmentID"
                positiontooltip="top"
                v-if="isShowTooltipRequired && requiredData.departmentID != ''"
              />
            </div>
            <ComboBox
              val="Chọn mã bộ phận sử dụng"
              :urlAPI="urlDepartment"
              dialog_icon="true"
              optionTable="true"
              tabindex="1003"
              :isCheck="departmentCheck"
              @isCheck="departmentCheck = true"
              @checkFalse="isCheck = false"
              @dataName="dataDepartment"
              nameTable="Department"
              :dataItemCombobox="dataItemDetailFake"
              @removeData="dataItemDetailFake.departmentID = ''"
              @checkRequired="checkRequiredComboboxDepartment"
              :borderRed="borderRedDepartment"
            />
            <!-- <button class="combobox__control combobox__width">
                            <input type="text" name="Mã bộ phận sử dụng" id="" class="input input-width tooltip absolute" required value="" placeholder="Chọn mã bộ phận sử dụng" tabindex="1003">
                            <div class="icon icon-down1 icon__size-5 combobox__icon"></div>
                        </button> -->
          </div>

          <div class="group-input size-66 margin-lr-16">
            <div class="margin-text">Tên bộ phận sử dụng</div>
            <input
              type="text"
              name="Tên bộ phận sử dụng"
              id=""
              class="input input-width tooltip"
              v-bind:value="dataItemDetail.departmentName"
              disabled
              @input="capitalizationText()"
            />
          </div>
        </div>
        <div class="m-row">
          <div
            class="group-input size-33 tooltip tooltipRequired"
            @mouseover="showTooltip($event)"
            @mouseleave="hideTooltip($event)"
          >
            <div class="margin-text">
              Mã loại tài sản<span style="color: red"> *</span>
              <Tooltip
                class="tooltipRequiredDetail"
                :tooltiptext="requiredData.fixedAssetCategoryID"
                positiontooltip="top"
                v-if="
                  isShowTooltipRequired &&
                  requiredData.fixedAssetCategoryID != ''
                "
              />
            </div>
            <ComboBox
              val="Chọn mã loại tài sản"
              :urlAPI="urlCategory"
              dialog_icon="true"
              optionTable="true"
              tabindex="1004"
              :isCheck="categoryCheck"
              @isCheck="categoryCheck = true"
              @checkFalse="categoryCheck = false"
              @dataName="dataCategory"
              nameTable="FixedAssetCategory"
              :dataItemCombobox="dataItemDetailFake"
              @removeData="dataItemDetailFake.fixedAssetCategoryID = ''"
              @checkRequired="checkRequiredComboboxCategory"
              :borderRed="borderRedCategory"
            />
          </div>

          <div class="group-input size-66 margin-lr-16">
            <div class="margin-text">Loại tài sản</div>
            <input
              type="text"
              name="Loại tài sản"
              id=""
              class="input input-width tooltip"
              v-bind:value="dataItemDetail.fixedAssetCategoryName"
              disabled
              @input="capitalizationText()"
            />
          </div>
        </div>
        <div class="m-row">
          <InputNumber
            classBorderInput="group-input size-33"
            @mouseover="showTooltip"
            @mouseleave="hideTooltip"
            classInput="text-align-right absolute"
            labelInput="Số lượng"
            :tooltipInput="requiredData.quantity"
            :isShowTooltipRequired="isShowTooltipRequired"
            v-model="dataItemDetail.quantity"
            :dataRequired="requiredData.quantity"
            properties="quantity"
            required="required"
            tabindex="1005"
            @blur="blurHandle"
            :focusRequired="dataRequiredTop[0]"
            :inputNumber="true"
            :checkRequiredAll="checkRequiredAll"
          />
          <InputNumber
            :focusRequired="dataRequiredTop[0]"
            Type="number"
            :showIcon="false"
            classBorderInput="group-input size-33"
            @mouseover="showTooltip"
            @mouseleave="hideTooltip"
            classInput="text-align-right absolute"
            labelInput="Nguyên giá"
            :tooltipInput="requiredData.cost"
            :isShowTooltipRequired="isShowTooltipRequired"
            v-model="dataItemDetail.cost"
            :dataRequired="requiredData.cost"
            :disable="disabled"
            properties="cost"
            required="required"
            tabindex="1006"
            @blur="blurHandle"
            :checkRequiredAll="checkRequiredAll"
          />
          <InputNumber
            :focusRequired="dataRequiredTop[0]"
            Type="number"
            classBorderInput="group-input size-33 margin-right-10"
            @mouseover="showTooltip"
            @mouseleave="hideTooltip"
            classInput="text-align-right absolute"
            labelInput="Số năm sử dụng"
            :disable="disabled"
            :tooltipInput="requiredData.lifeTime"
            :isShowTooltipRequired="isShowTooltipRequired"
            v-model="dataItemDetail.lifeTime"
            :dataRequired="requiredData.lifeTime"
            properties="lifeTime"
            required="required"
            tabindex="1007"
            @blur="blurHandle"
            :inputNumber="true"
            :checkRequiredAll="checkRequiredAll"
          />
        </div>
        <div class="m-row">
          <InputNumber
            Type="number"
            :showIcon="false"
            :focusRequired="dataRequiredTop[0]"
            classBorderInput="group-input size-33"
            @mouseover="showTooltip"
            @mouseleave="hideTooltip"
            classInput="text-align-right absolute"
            labelInput=" Tỉ lệ hao mòn (%)"
            :tooltipInput="requiredData.depreciationRate"
            :isShowTooltipRequired="isShowTooltipRequired"
            v-model="dataItemDetail.depreciationRate"
            :dataRequired="requiredData.depreciationRate"
            :dataChange="dataChange"
            @dataChange="dataChange = false"
            properties="depreciationRate"
            required="required"
            tabindex="1008"
            @blur="blurHandle"
            :disable="disabled"
            :checkRequiredAll="checkRequiredAll"
          />
          <InputNumber
            Type="number"
            :showIcon="false"
            :focusRequired="dataRequiredTop[0]"
            classBorderInput="group-input size-33"
            @mouseover="showTooltip"
            @mouseleave="hideTooltip"
            classInput="text-align-right absolute"
            labelInput="Giá trị hao mòn năm"
            :tooltipInput="requiredData.depreciationYear"
            :isShowTooltipRequired="isShowTooltipRequired"
            :dataChange="dataChange"
            @dataChange="dataChange = false"
            :disable="disabled"
            v-model="dataItemDetail.depreciationYear"
            :dataRequired="requiredData.depreciationYear"
            properties="depreciationYear"
            required="required"
            tabindex="1009"
            @blur="blurHandle"
            :checkRequiredAll="checkRequiredAll"
          />

          <div class="group-input size-33 margin-right-10">
            <div class="margin-text">Năm theo dõi</div>
            <input
              type="text"
              name="Năm theo dõi"
              required
              id=""
              class="input input-width tooltip text-align-right"
              value="2021"
              disabled
            />
          </div>
        </div>

        <div class="m-row" style="margin-bottom: 25px">
          <DateTime
            label="Ngày mua"
            tabindex="1010"
            :dataDate="dataItemDetail.purchaseDate"
            @Date="getDatePurchaseDate"
            :formatType="formatDateBase"
          />
          <DateTime
            label="Ngày bắt đầu sử dụng"
            tabindex="1011"
            :dataDate="dataItemDetail.productionDate"
            @Date="getDateProductionDate"
            :formatType="formatDateBase"
            @Tab="focusSave = true"
          />

          <div class="group-input size-33 margin-right-10"></div>
        </div>
      </div>
      <div class="dialog__footer">
        <Button
          @click="saveData()"
          classButton="btn-save active"
          :tabindex="1012"
          textButton="Lưu"
          :focusSave="focusSave"
        />
        <Button
          @keydownTab="this.isFocus = true"
          @click="closeDialog"
          focusCircle="true"
          classButton="btn-cancel"
          :tabindex="1013"
          textButton="Hủy"
          id="btn-cancel"
        />
      </div>
    </div>
  </div>

  <Notify
    v-if="DialogNotify"
    :dataError="errors"
    @isShowDialogNotify="DialogNotify = false"
    :buttonNames="buttonNames"
    @isSaveData="saveData"
    @removeDialog="removeDialog"
    :errorName="errorName"
    :isShowNumber="isShowNumber"
  />
</template>
<script>
import Resource from "@/js/resource";
import Enum from "../../js/enum";
import ComboBox from "./BaseComboBox.vue";
import Notify from "./BaseDialogNotify.vue";
import Tooltip from "./BaseTooltip.vue";
import Button from "./BaseButton.vue";
import Input from "./BaseInput.vue";
import InputNumber from "./BaseInputNumber.vue";
import DateTime from "../base/BaseDate.vue";
import { formatCash, formatDate, unFormatDecimal } from "../../js/common.js";
import { API } from "../../js/callapi";
import Config from "@/js/config";
export default {
  name: "TheDialog",
  components: {
    ComboBox,
    Notify,
    Tooltip,
    Button,
    Input,
    DateTime,
    InputNumber,
  },
  data() {
    return {
      formatDateBase: "DMY",
      focusSave: false,
      assetCode: "",
      isChange: false,
      focusDate: false,
      dataItemDetail: {
        fixedAssetID: "",
        fixedAssetCode: "",
        fixedAssetName: "",
        departmentID: "",
        fixedAssetCategoryID: "",
        quantity: 0,
        cost: 0,
        lifeTime: 0,
        depreciationRate: 0,
        depreciationYear: "",
        purchaseDate: new Date(),
        productionDate: new Date(),
        status: "",
      },
      display: "none",
      isShowMenuDateStart: false,
      isShowMenuDateDepreciation: false,
      categoryCheck: false,
      departmentCheck: false,
      isFocusInputHide: false,
      isFocusInputHide1: false,
      urlDepartment: Resource.APIs.Departments,
      urlCategory: Resource.APIs.FixedAssetCategorys,
      dataName: {
        AssetCode: "Mã tài sản",
        AssetName: "Tên tài sản",
        Price: "Nguyên giá",
        YearDate: "Số năm sử dụng",
        Quantity: "Số lượng",
        Purchasedate: "Ngày mua",
        Startday: "Ngày bắt đầu",
        Depreciation: "Giá trị hao mòn năm",
        Atrophy: "Tỉ lệ hao mòn",
      },

      buttonNames: [Resource.Label.Agree],
      errors: [],
      DialogNotify: false,
      date: {
        Startday: new Date(),
        Depreciation: new Date(),
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
          Department: "",
        },
      ],
      optionCBBox: [],
      optionCBBox1: [],
      errorName: "",
      dataItemDetailFake: [],
      requiredData: {
        fixedAssetCode: "",
        fixedAssetName: "",
        quantity: "",
        depreciationYear: "",
        depreciationRate: "",
        cost: "",
        departmentID: "",
        fixedAssetCategoryID: "",
        lifeTime: "",
      },
      opacity: 0,
      isShowTooltipRequired: false,
      borderRed: false,
      isCheckRequiredComboboxDepartment: false,
      isCheckRequiredComboboxCategory: false,
      isFocus: true,
      checkRequiredAll: false,
      dataChange: false,
      productionDateFake: "",
      dataRequiredTop: [],
      isShowNumber: false,
      disabled: false,
    };
  },
  methods: {
    formatDate,
    /**
     * Validate dữ liệu
     * Author : Bùi Quang Điệp
     * Date:10/08/2022
     */
    validation() {
      try {
        var isCheckRequired = false;
        var res = this.dataItemDetail;
        var date = new Date();
        var day = date.getDate() + 1;
        date = date.setDate(day);
        console.log(res);
        this.errors = [];
        this.errorName = "";
        if (!res.fixedAssetCode) {
          isCheckRequired = true;
        } else {
          // Không cho phép nhập quá 20 kí tự
          if (res.fixedAssetCode.length > 20) {
            this.errors.push(Resource.Required.LengthAssetCode);
          }
        }
        if (!res.fixedAssetName) {
          isCheckRequired = true;
        } else {
          // Không cho phép nhập quá 255 kí tự
          if (res.fixedAssetName.length > 255) {
            this.errors.push(Resource.Required.LengthAssetName);
          }
        }
        if (!res.quantity) {
          isCheckRequired = true;
        } else {
          if (res.quantity <= 0) {
            isCheckRequired = true;
            this.errors.push(Resource.Required.MinQuantity);
          }
          if (res.quantity > 10000) {
            isCheckRequired = true;
            this.errors.push(Resource.Required.MaxQuantity);
          }
        }
        if (!res.lifeTime) {
          isCheckRequired = true;
        } else {
          if (res.lifeTime > 100) {
            isCheckRequired = true;
            this.errors.push(Resource.Required.MaxLifeTime);
          }
        }
        if (!res.depreciationYear) {
          isCheckRequired = true;
        }
        if (!res.depreciationRate) {
          isCheckRequired = true;
        }
        if (!res.cost) {
          isCheckRequired = true;
        } else {
          if (res.cost <= 0) {
            isCheckRequired = true;
            this.errors.push(Resource.Required.MinCost);
          }
        }
        if (!res.purchaseDate) {
          isCheckRequired = true;
        } else {
          if (new Date(res.purchaseDate) > date) {
            isCheckRequired = true;
            this.errors.push(Resource.Required.MaxPurchaseDate);
          }
        }
        if (!res.productionDate) {
          isCheckRequired = true;
        } else {
          if (new Date(res.productionDate) > date) {
            isCheckRequired = true;
            this.errors.push(Resource.Required.MaxProductionDate);
          }
        }
        return isCheckRequired;
      } catch (error) {
        console.log(error);
      }
    },

    /**
     * Validate tất cả dữ liệu khi người dùng không nhập
     * Author : Bùi Quang Điệp
     * Date:10/08/2022
     */
    validationAll() {
      this.checkRequiredAll = true;
      if (!this.dataItemDetail.fixedAssetCode) {
        this.requiredData.fixedAssetCode = Resource.Required.FixedAssetCode;
        this.dataRequiredTop.push("fixedAssetCode");
      }

      if (!this.dataItemDetail.fixedAssetName) {
        this.requiredData.fixedAssetName = Resource.Required.FixedAssetName;
        this.dataRequiredTop.push("fixedAssetName");
      }

      if (!this.dataItemDetail.quantity) {
        this.requiredData.quantity = Resource.Required.Quantity;
        this.dataRequiredTop.push("quantity");
      }

      if (!this.dataItemDetail.depreciationYear) {
        this.requiredData.depreciationYear = Resource.Required.DepreciationYear;
        this.dataRequiredTop.push("depreciationYear");
      }

      if (!this.dataItemDetail.lifeTime) {
        this.requiredData.lifeTime = Resource.Required.LifeTime;
        this.dataRequiredTop.push("lifeTime");
      }

      if (!this.dataItemDetail.depreciationRate) {
        this.requiredData.depreciationRate = Resource.Required.DepreciationRate;
        this.dataRequiredTop.push("depreciationRate");
      }

      if (!this.dataItemDetail.cost) {
        this.requiredData.cost = Resource.Required.Cost;
        this.dataRequiredTop.push("cost");
      }

      if (this.isCheckRequiredComboboxDepartment == false) {
        this.requiredData.departmentID = Resource.Required.DepartmentID;
        this.borderRedDepartment = true;
        this.dataRequiredTop.push("departmentID");
      } else {
        this.requiredData.departmentID = "";
        this.borderRedDepartment = false;
      }

      if (this.isCheckRequiredComboboxCategory == false) {
        this.requiredData.fixedAssetCategoryID =
          Resource.Required.FixedAssetCategoryID;
        this.borderRedCategory = true;
        this.dataRequiredTop.push("fixedAssetCategoryID");
      } else {
        this.requiredData.fixedAssetCategoryID = "";
        this.borderRedCategory = false;
      }
      if (this.dataRequiredTop.length == 0) {
        if (this.errors.length >= 1) {
          this.errorName = "";
          this.DialogNotify = true;
        }
      }
      this.isShowTooltipRequired = true;
      this.opacity = 1;
      setTimeout(() => {
        this.opacity = 0;
        this.isShowTooltipRequired = false;
      }, Enum.SetTimeOut.TimeOut);
    },

    /**
     * Validate dữ liệu theo nghiệp vụ
     * Author : Bùi Quang Điệp
     * Date:10/08/2022
     */
    validationMajor() {
      var lifeTime = (1 / this.dataItemDetail.lifeTime) * 100;
      var depreciationRate = this.unFormDecimal(
        this.dataItemDetail.depreciationRate
      );
      if (lifeTime.toString().includes(".")) {
        lifeTime = lifeTime.toFixed(1);
      }
      if (depreciationRate.toString().includes(".")) {
        depreciationRate = parseFloat(depreciationRate).toFixed(1);
      }
      if (lifeTime != depreciationRate) {
        this.errors.push(Resource.Required.BusinessValidationDepreciationRate);
      }
      if (
        this.dataItemDetail.depreciationRate /
          this.dataItemDetail.depreciationYear >
        this.dataItemDetail.cost
      ) {
        this.errors.push(Resource.Required.BusinessValidationDepreciationYear);
      }
    },

    /**
     * xử lý sự kiện khi ấn lưu dữ liệu
     * Author : Bùi Quang Điệp
     * Date:10/08/2022
     */
    saveData() {
      try {
        var isCheckRequired = this.validation();
        if (isCheckRequired == false) {
          this.validationMajor();
          if (this.errors.length >= 1) {
            this.DialogNotify = true;
          } else {
            this.opacity = 0;
            // tạo object loại kiểu json
            var cost = this.unFormNumber(this.dataItemDetail.cost);
            var depreciationYear = this.unFormNumber(
              this.dataItemDetail.depreciationYear
            );
            var dataAPI = {
              fixedAssetCode: this.dataItemDetail.fixedAssetCode,
              fixedAssetName: this.dataItemDetail.fixedAssetName,
              fixedAssetCategoryName:
                this.dataItemDetail.FixedAssetCategoryName,
              departmentID: this.dataItemDetail.departmentID,
              fixedAssetCategoryID: this.dataItemDetail.fixedAssetCategoryID,
              cost: cost,
              quantity: this.unFormNumber(this.dataItemDetail.quantity),
              depreciationRate: this.unFormDecimal(
                this.dataItemDetail.depreciationRate
              ),
              lifeTime: this.unFormNumber(this.dataItemDetail.lifeTime),
              depreciationYear: depreciationYear,
              purchaseDate: this.dataItemDetail.purchaseDate,
              productionDate: this.dataItemDetail.productionDate,
            };

            // Nếu thành công thì call API xử lý thêm dữ liệu
            // Kiểm tra xem đang là chức năng thêm hay chỉnh sửa

            if (this.handler == Resource.CommandType.Edit) {
              if (this.isChange == true) {
                API.put(
                  Resource.APIs.Assets +
                    `?FixedAssetID=${this.dataItemDetail.fixedAssetID}&fixedAssetCode=${this.dataItemDetail.fixedAssetCode}`,
                  JSON.stringify(dataAPI)
                )
                  .then((response) => {
                    this.$emit("btnCloseDialog");
                    this.emitter.emit(
                      "LoadData",
                      this.dataItemDetail.fixedAssetCode
                    );
                    console.log(response);
                    this.$emit("handlerName", Resource.name.edit);
                  })
                  .catch((error) => {
                    console.log(error.message);
                    if (
                      error.response.data.handle ==
                      Enum.FormModeHandler.Required
                    ) {
                      // Lỗi nhập liệu phía người dùng
                      this.DialogNotify = true;
                      this.errors = [];
                      (this.buttonNames = [Resource.Label.Agree]),
                        this.errors.push(error.response.data.dataError[0]);
                    }
                    // Nếu lỗi trùng mã thì hiển thị thông báo cho người dùng
                    if (
                      error.response.data.handle ==
                      Enum.FormModeHandler.DoubleKey
                    ) {
                      this.DialogNotify = true;
                      this.errors = [];
                      (this.buttonNames = [Resource.Label.Agree]),
                        (this.isShowNumber = true);
                      this.errors.push(Resource.Errors.DoubleKey);
                      this.errorName = error.response.data.dataError[0];
                      // gọi Api Lấy mã mới cho người dùng
                      API.get(Resource.APIs.NewCode)
                        .then((res) => {
                          this.dataChange = true;
                          this.dataItemDetail.fixedAssetCode = res.data;
                        })
                        .catch((res) => {
                          console.log(res);
                        });
                    }

                    // Lỗi phía backend
                    if (
                      error.response.data.handle ==
                      Enum.FormModeHandler.Exception
                    ) {
                      this.DialogNotify = true;
                      this.errors = [];
                      (this.buttonNames = [Resource.Label.Agree]),
                        (this.isShowNumber = true);
                      this.errors.push(error.response.data.userMsg);
                    }
                  });
              } else {
                this.$emit("btnCloseDialog");
                this.$emit("handlerName", Resource.name.noEdit);
                this.$emit("removeHandlerName");
              }
            }

            // Kiểm tra nếu là thêm mới thì xử lý
            if (
              this.handler == Resource.CommandType.Add ||
              this.handler == Resource.CommandType.Replication
            ) {
              API.post(Resource.APIs.Assets, JSON.stringify(dataAPI))
                .then((response) => {
                  this.$emit("btnCloseDialog");
                  this.emitter.emit(
                    "LoadData",
                    this.dataItemDetail.fixedAssetCode
                  );
                  console.log(response);
                  if (this.handler == Resource.CommandType.Add)
                    this.$emit("handlerName", Resource.name.add);
                  else {
                    this.$emit("handlerName", Resource.name.replication);
                  }
                })
                .catch((error) => {
                  console.log(error.message);

                  if (
                    error.response.data.handle == Enum.FormModeHandler.Required
                  ) {
                    // Lỗi nhập liệu phía người dùng
                    this.DialogNotify = true;
                    this.errors = [];
                    (this.buttonNames = [Resource.Label.Agree]),
                      this.errors.push(error.response.data.dataError[0]);
                  }
                  // Nếu lỗi trùng mã thì hiển thị thông báo cho người dùng
                  if (
                    error.response.data.handle == Enum.FormModeHandler.DoubleKey
                  ) {
                    this.DialogNotify = true;
                    this.errors = [];
                    (this.buttonNames = [Resource.Label.Agree]),
                      (this.isShowNumber = true);
                    this.errors.push(Resource.Errors.DoubleKey);
                    this.errorName = error.response.data.dataError[0];

                    // gọi Api Lấy mã mới cho người dùng
                    API.get(Resource.APIs.NewCode)
                      .then((res) => {
                        this.dataChange = true;
                        this.dataItemDetail.fixedAssetCode = res.data;
                      })
                      .catch((res) => {
                        console.log(res);
                      });
                  }

                  // Lỗi phía backend
                  if (
                    error.response.data.handle == Enum.FormModeHandler.Exception
                  ) {
                    this.DialogNotify = true;
                    this.errors = [];
                    (this.buttonNames = [Resource.Label.Agree]),
                      (this.isShowNumber = true);
                    this.errors.push(error.response.data.userMsg);
                  }
                });
            }
          }
        } else {
          this.validationAll();
        }
      } catch (error) {
        console.log(error);
      }
    },
    /**
     * Required những dữ liệu cần thiết
     * Author : Bùi Quang Điệp
     * Date:10/08/2022
     */
    checkRequired(name, event) {
      try {
        // Kiểm tra name có dữ liệu hay không
        if (name != "") {
          if (!event.value) {
            event.classList.add("border-red");
            // this.isRequired = true;
          } else {
            //  this.isRequired = false;
            this.requiredData[name] = "";
            event.classList.remove("border-red");
          }
         // this.dataItemDetail[name] = event.value;
        }
      } catch (error) {
        console.log(error);
      }
    },
    /**
     * Kiểm tra xem nhập liệu đầu vào có phải là số hay không
     * Author : Bùi Quang Điệp
     * Date:10/08/2022
     */
    checkNumber(event, name) {
      try {
        var number = String.fromCharCode(event.keyCode),
          valid = /^[0-9]+$/.test(number);
        if (
          this.dataItemDetail[name] != "" &&
          this.dataItemDetail[name] != undefined
        ) {
          if (this.dataItemDetail[name].length > 18) event.preventDefault();
        }
        if (!valid) {
          if (
            event.keyCode == 190 ||
            event.keyCode == 188 ||
            event.keyCode == 8 ||
            event.code == "Tab"
          ) {
            return;
          } else {
            event.preventDefault();
          }
        }
      } catch (error) {
        console.log();
      }
    },

    /**
     * Định dạng dấu phân cách hàng nghìn cho số lượng khi nhập liệu
     * Author : Bùi Quang Điệp
     * Date:10/08/2022
     */
    formatNumberInput(val, item) {
      try {
        val = this.unFormNumber(this.dataItemDetail[item]);
        const mustyNumber = 1000;
        if (val >= mustyNumber) {
          if (item == "cost") {
            this.dataItemDetail.cost = formatCash(val);
          }
          // if (item == "quantity") {
          //   this.dataItemDetail.quantity = formatCash(val);
          // }
          if (item == "depreciation") {
            this.dataItemDetail.depreciationYear = formatCash(val);
          }
          if (item == "lifeTime") {
            this.dataItemDetail.lifeTime = formatCash(val);
          }
        }

        // this.calculatedepreciationYear();
      } catch (error) {
        console.log(error);
      }
    },

    /**
     * Lấy dữ liệu bộ phận sử dụng
     * Author : Bùi Quang Điệp
     * Date:10/08/2022
     */
    dataDepartment(e) {
      try {
        this.dataItemDetail.departmentName = e.DepartmentName;
        this.dataItemDetail.departmentID = e.DepartmentID;
      } catch (error) {
        console.log(error);
      }
    },
    /**
     * Lấy dữ liệu loại tài sản
     * Author : Bùi Quang Điệp
     * Date:10/08/2022
     */

    dataCategory(e) {
      try {
        this.dataItemDetail.fixedAssetCategoryName = e.FixedAssetCategoryName;
        this.dataItemDetail.fixedAssetCategoryID = e.FixedAssetCategoryID;
      } catch (error) {
        console.log(error);
      }
    },

    /**
     * Tính tự động giá trị hao mòn năm
     * Author : Bùi Quang Điệp
     * Date:10/08/2022
     */
    calculatedepreciationYear() {
      try {
        if (
          this.dataItemDetail.cost == "" ||
          this.dataItemDetail.cost == undefined
        ) {
          this.dataItemDetail.cost = 0;
        }
        if (
          this.dataItemDetail.depreciationRate == "" ||
          this.dataItemDetail.depreciationRate == undefined
        ) {
          this.dataItemDetail.depreciationRate = 0;
        }
        const fixed = 2; // phân cách phần thập phân lấy 2 số
        var cost = this.unFormNumber(this.dataItemDetail.cost);
        var depreciationRate = parseFloat(
          this.dataItemDetail.depreciationRate
        ).toFixed(fixed);
        this.dataItemDetail.depreciationYear = formatCash(
          (cost * (depreciationRate / 100)).toFixed(0)
        );
        this.dataChange = true;
      } catch (error) {
        console.log(error);
      }
    },
    /**
     * Hủy bỏ định dạng number
     * Author : Bùi Quang Điệp
     * Date:10/08/2022
     */
    unFormNumber(val) {
      try {
        if (/^[0-9]+$/.test(val) == false) {
          val = val.toString().split(".").join("");
          if (val.includes(",")) {
            val = val.toString().split(",").join(".");
            val = val.toString().split(".").join("");
          }
          return val;
        } else return val;
      } catch (error) {
        console.log(error);
      }
    },

    /**
     * Hủy bỏ định dạng number phần thập phân
     * Author : Bùi Quang Điệp
     * Date:10/08/2022
     */
    unFormDecimal(val) {
      try {
        val = val.toString();
        if (/^[0-9]+$/.test(val) == false) {
          if (val.includes(",")) {
            val = val.toString().split(",").join(".");
          }
          return val;
        } else return val;
      } catch (error) {
        console.log(error);
      }
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
      try {
        var text = event.currentTarget.value;
        if (text != "") {
          text = text.charAt(0).toLocaleUpperCase() + text.slice(1);
        }
        event.currentTarget.value = text;
      } catch (error) {
        console.log(error);
      }
    },

    /**
     * Đóng dialog thêm mới và show thông báo nếu người dùng hủy lưu khi chỉnh sửa
     * Author : Bùi Quang Điệp
     * Date:10/08/2022
     */
    closeDialog() {
      try {
        if (this.handler == Resource.CommandType.Edit) {
          if (this.isChange) {
            this.DialogNotify = true;
            this.errors = [];

            (this.buttonNames = [
              Resource.Label.Save,
              Resource.Label.NotSave,
              Resource.Label.Cancel,
            ]),
              this.errors.push(Resource.Warning.Close);
          } else {
            this.$emit("btnCloseDialog");
          }
        } else {
          this.$emit("btnCloseDialog");
        }
      } catch (error) {
        console.log(error);
      }
    },

    /**
     * Nếu người phím ESC thì đóng form
     * Author : Bùi Quang Điệp
     * Date:10/08/2022
     */
    closeKeyESC() {
      try {
        this.closeDialog();
      } catch (error) {
        console.log(error);
      }
    },

    /**
     * Đóng cả 2 dialog
     * Author : Bùi Quang Điệp
     * Date:15/08/2022
     */
    removeDialog() {
      try {
        this.DialogNotify = false;
        this.$emit("btnCloseDialog");
      } catch (error) {
        console.log(error);
      }
    },

    /**
     * nhận dữ liệu thay đổi và cập nhật lại ngày mua
     * Author : Bùi Quang Điệp
     * Date:15/08/2022
     */
    getDatePurchaseDate(item) {
      try {
        item.setDate( item.getDate() +1);
        this.dataItemDetail.purchaseDate = (item);
      } catch (error) {
        console.log(error);
      }
    },

    /**
     * Cập nhật lại dữ liệu khi có thay đối
     * Author : Bùi Quang Điệp
     * Date:15/08/2022
     */
    updateData(value, name) {
      try {
        this.dataItemDetail[name] = value;
      } catch (error) {
        console.log(error);
      }
    },

    /**
     * nhận dữ liệu thay đổi và cập nhật lại ngày bắt đầu sử dụng
     * Author : Bùi Quang Điệp
     * Date:15/08/2022
     */
    getDateProductionDate(item) {
      try {
        item.setDate( item.getDate() +1);
        this.dataItemDetail.productionDate = (item);
      } catch (error) {
        console.log(error);
      }
    },
    /**
     * Hiển thị tooltip required khi hover
     * Author : Bùi Quang Điệp
     * Date:07/09/2022
     */
    showTooltip(event) {
      try {
        event.currentTarget.classList.add("activeTooltip");
        this.isShowTooltipRequired = true;
      } catch (error) {
        console.log(error);
      }
    },

    /**
     * ẩn tooltip required khi bỏ hover
     * Author : Bùi Quang Điệp
     * Date:07/09/2022
     */
    hideTooltip(event) {
      try {
        event.currentTarget.classList.remove("activeTooltip");
        this.isShowTooltipRequired = false;
      } catch (error) {
        console.log(error);
      }
    },

    /**
     * Check required combobox
     * Author : Bùi Quang Điệp
     * Date:07/09/2022
     */
    checkRequiredComboboxDepartment(item) {
      try {
        this.isCheckRequiredComboboxDepartment = item;
      } catch (error) {
        console.log(error);
      }
    },

    /**
     * Check required combobox
     * Author : Bùi Quang Điệp
     * Date:07/09/2022
     */
    checkRequiredComboboxCategory(item) {
      try {
        this.isCheckRequiredComboboxCategory = item;
      } catch (error) {
        console.log(error);
      }
    },

    /**
     * Nhận dữ liệu từ sự kiện blur để check
     * Author : Bùi Quang Điệp
     * Date:11/09/2022
     */
    blurHandle(event, name) {
      try {
        this.inputHandler = true;
        this.dataRequiredTop = [];
        this.checkRequired(name, event);
      } catch (error) {
        console.log(error);
      }
    },

    /**
     * Gán lại Data từ input
     * Author : Bùi Quang Điệp
     * Date:11/09/2022
     */
    setData(item, name) {
      try {
        this.dataItemDetail[name] = item;
      } catch (error) {
        console.log(error);
      }
    },

    /**
     * Kiểm tra xem dữ liệu có thay đổi không
     * Author : Bùi Quang Điệp
     * Date:11/09/2022
     */
    checkChange(newValue, oldValue) {
      try {
        if (oldValue != "") {
          if (oldValue != 0 && oldValue) {
            {
              if (newValue != oldValue) {
                if (this.handler == Resource.CommandType.Edit)
                  this.isChange = true;
              }
            }
          }
        }
      } catch (error) {
        console.log(error);
      }
    },

    /**
     * Thông báo cho người dùng khi nhập quá kí tự
     * Author : Bùi Quang Điệp
     * Date:11/09/2022
     */
    showTooltipErrorLength(name, lable, maxlength) {
      this.isShowTooltipRequired = true;
      this.requiredData[name] = lable + Resource.Required.MaxLength + maxlength;
      this.opacity = 1;
      setTimeout(() => {
        this.opacity = 0;
        this.isShowTooltipRequired = false;
      }, Enum.SetTimeOut.TimeOut);
    },
  },
  created() {
    /**
     *  Lấy dữu liệu các combobox đổ vào dialog
     * Author : Bùi Quang Điệp
     * Date:07/09/2022
     */
    if (
      this.handler == Resource.CommandType.Edit ||
      this.handler == Resource.CommandType.Replication
    ) {
      this.dataItemDetail = {
        fixedAssetID: this.item.fixedAssetID,
        fixedAssetCode: this.item.fixedAssetCode,
        fixedAssetName: this.item.fixedAssetName,
        departmentID: this.item.departmentID,
        fixedAssetCategoryID: this.item.fixedAssetCategoryID,
        quantity: this.item.quantity,
        cost: formatCash(this.item.cost),
        lifeTime: this.item.lifeTime,
        depreciationRate: this.item.depreciationRate,
        depreciationYear: formatCash(this.item.depreciationYear),
        purchaseDate: this.item.purchaseDate,
        productionDate: this.item.productionDate,
        status: this.item.status,
      };
      this.dataItemDetailFake = this.dataItemDetail;
    } else {
      this.dataItemDetail.fixedAssetCode = this.item.fixedAssetCode;
    }
    if (this.handler == Resource.CommandType.Edit) {
      if (this.dataItemDetail.status == 1) {
        this.disabled = true;
      } else {
        this.disabled = false;
      }
    }

    //this.dataItemDetailCheck = this.item;
    this.dataItemDetailFake = this.dataItemDetail;
  },
  watch: {
    "dataItemDetail.cost": function (newValue, oldValue) {
      if (newValue != oldValue) {
        if (oldValue != "") {
          if (oldValue != 0) {
            this.calculatedepreciationYear();
          }
        }
      }
      if (oldValue != undefined) {
        if (newValue != oldValue) {
          if (oldValue != "") {
            if (oldValue != 0) {
              if (this.handler == Resource.CommandType.Edit)
                this.isChange = true;
            }
          }
        }
      }
    },
    "dataItemDetail.depreciationRate": function (newValue, oldValue) {
      if (newValue != oldValue) {
        if (oldValue != "") {
          if (oldValue != 0) {
            this.calculatedepreciationYear();
          }
        }
      }
      if (oldValue != undefined) {
        if (newValue != oldValue) {
          if (oldValue != "") {
            if (oldValue != 0) {
              if (this.handler == Resource.CommandType.Edit)
                this.isChange = true;
            }
          }
        }
      }
    },
    "dataItemDetail.lifeTime": function (newValue, oldValue) {
      if (oldValue != undefined) {
        if (newValue != oldValue) {
          if (oldValue != "") {
            if (oldValue != 0) {
              this.dataChange = true;
              const fixed = 2;
              this.dataItemDetail.depreciationRate = (
                (1 / Number(unFormatDecimal(this.dataItemDetail.lifeTime))) *
                100
              ).toFixed(fixed);
              if (this.handler == Resource.CommandType.Edit)
                this.isChange = true;
            }
          }
        }
      }
    },
    "dataItemDetail.fixedAssetCode": function (newValue, oldValue) {
      this.checkChange(newValue, oldValue);
    },
    "dataItemDetail.fixedAssetName": function (newValue, oldValue) {
      this.checkChange(newValue, oldValue);
    },
    "dataItemDetail.departmentID": function (newValue, oldValue) {
      if (oldValue != newValue) {
        this.borderRedDepartment = false;
      }
      this.checkChange(newValue, oldValue);
    },
    "dataItemDetail.fixedAssetCategoryID": function (newValue, oldValue) {
      if (oldValue != newValue) {
        this.borderRedCategory = false;
      }
      this.checkChange(newValue, oldValue);
    },
    "dataItemDetail.quantity": function (newValue, oldValue) {
      if (oldValue != 0) this.checkChange(newValue, oldValue);
    },
    "dataItemDetail.depreciationYear": function (newValue, oldValue) {
      this.checkChange(newValue, oldValue);
    },
  },
  mounted() {
    if (
      this.dataItemDetail.purchaseDate == "" &&
      this.dataItemDetail.productionDate == ""
    ) {
      this.dataItemDetail.purchaseDate = new Date();
      this.dataItemDetail.productionDate = new Date();
    }
    this.dataItemDetail.purchaseDate = formatDate(
      this.dataItemDetail.purchaseDate
    );
    this.dataItemDetail.productionDate = formatDate(
      this.dataItemDetail.productionDate
    );
    this.formatDateBase = Config.FormatDate.Value;
  },

  updated() {
    this.$refs["dialog"].focus();
    /**
     *  Khi combobox thay đổi thì kiểm tra nếu đã có dữ liệu thì bỏ tooltip
     * Author : Bùi Quang Điệp
     * Date:07/09/2022
     */
    if (this.isCheckRequiredComboboxDepartment == true) {
      this.requiredData.departmentID = "";
      this.borderRedDepartment = false;
    }

    /**
     *  Khi combobox thay đổi thì kiểm tra nếu đã có dữ liệu thì bỏ tooltip
     * Author : Bùi Quang Điệp
     * Date:07/09/2022
     */
    if (this.isCheckRequiredComboboxCategory == true) {
      this.requiredData.fixedAssetCategoryID = "";
      this.borderRedCategory = false;
    }
  },

  props: {
    isShowDialog: Boolean,
    item: Array,
    title: String,
    handler: String,
  },
};
</script>
<style>
.dateActive .vuejs3-datepicker__calendar.vuejs3-green {
  display: v-bind("display") !important;
}

.group-input.tooltip .tooltiptext.tooltipRequiredDetail {
  opacity: v-bind("opacity") !important;
}

.activeTooltip.group-input.tooltip .tooltiptext.tooltipRequiredDetail {
  opacity: 1 !important;
}
</style>
