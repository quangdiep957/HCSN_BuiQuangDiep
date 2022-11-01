<template>
  <div
    class="dialog__handle"
    id="dialog__handle"
    @keydown.esc="CloseKeyESC"
    ref="dialog"
  >
    <div class="dialog">
      <div class="dialog__header">
        <h3 id="handle">{{ labelDialog }}</h3>
        <div
          @click="closeDialog"
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
        <div>
          <div class="voucher__information">
            <div class="m-row" style="width: calc(70% + 30px)">
              <Input
                Type="number"
                :disable="true"
                classBorderInput="group-input size-100"
                @mouseover="showTooltip"
                @mouseleave="hideTooltip"
                classInput="text-align-left "
                labelInput="Bộ phận sử dụng"
                v-model="dataItemDetail.departmentName"
                :dataRequired="requiredData.departmentName"
                properties="departmentName"
                required="required"
                tabindex="1008"
                :checkRequiredAll="checkRequiredAll"
              />
            </div>
          </div>
          <div
            class="dialog__body--label"
            style="margin-left: 15px; font-weight: bold"
          >
            Nguyên giá
          </div>
          <div class="voucher__detail">
            <div class="voucher__detail--header">
              <div class="m-row" style="height: 40px; align-items: center">
                <div class="size-50">Nguồn hình thành</div>
                <div class="size-20" style="margin-left: 16px">Giá trị</div>
              </div>
            </div>
            <div class="voucher__detail--body">
              <div class="voucher__detail--body-item">
                <div
                  class="m-row"
                  style="height: 55px"
                  v-for="(value, index) in dataUpdate"
                  :key="value"
                >
                  <div class="size-50 tooltip">
                    <div style="position: relative">
                      <Tooltip
                        class="tooltipRequiredDetail"
                        :tooltiptext="requiredDataCombobox[index]"
                        positiontooltip="top"
                        v-if="
                          isShowTooltipRequired &&
                          requiredDataCombobox[index] != ''
                        "
                        style="top: 40px; opacity: 1"
                      />
                    </div>
                    <Combobox
                      val="Chọn nguồn hình thành"
                      :urlAPI="urlBudget"
                      dialog_icon="true"
                      :position="index"
                      :dataItemCombobox="id[index]"
                      :isShowRemove="false"
                      tabindex="1003"
                      classComboboxCustom="classComboboxCustom"
                      @isCheck="removeID"
                      @dataComBoBoxSearch="getDataCombobox"
                      :borderRed="borderRedCombobox[index]"
                      @checkRequired="checkBorderRedCombobox"
                      nameTable="Budget"
                      @removeBudget="removeBudget"
                    />
                  </div>
                  <div class="size-20">
                    <InputNumber
                      Type="number"
                      :showIcon="false"
                      :focusRequired="dataRequiredTop[0]"
                      classBorderInput="group-input size-100"
                      @mouseover="showTooltip"
                      @mouseleave="hideTooltip"
                      classInput="text-align-right "
                      :tooltipInput="requiredData.cost"
                      :isShowTooltipRequired="isShowTooltipRequired"
                      :positionInputRequired="positionInputRequired[index]"
                      v-model="value.cost"
                      :dataRequired="requiredData.cost"
                      properties="cost"
                      required="required"
                      :iconRequired="false"
                      tabindex="1008"
                      @blur="blurHandle"
                      :checkRequiredAll="checkRequiredAll"
                      @input="setSumCost()"
                      :styleTooltip="styleObject"
                      :position="index"
                    />
                  </div>
                  <div
                    class="size-20 display-flex"
                    style="height: 35px; margin-left: 30px"
                  >
                    <div
                      class="icon icon__size-18 icon_augment tooltip"
                      style="position: relative; margin-right: 10px"
                      @click="addItem"
                    >
                      <Tooltip
                        tooltiptext="Thêm nguồn chi phí"
                        positiontooltip="left"
                        style="margin-left: 15px"
                      />
                    </div>
                    <div
                      class="icon icon__size-18 icon_reduce tooltip"
                      v-if="dataUpdate.length >= 2"
                      style="position: relative"
                      @click="reduceItem(index)"
                    >
                      <Tooltip
                        tooltiptext="Bỏ nguồn chi phí"
                        positiontooltip="left"
                        style="margin-left: 15px"
                      />
                    </div>
                  </div>
                </div>
              </div>
              <div class="m-row" style="margin: 15px 0">
                <div class="size-50">
                  <Input
                    :inputNumber="true"
                    :showIcon="false"
                    :disable="true"
                    :titleInput="true"
                    classBorderInput="group-input"
                    @mouseover="showTooltip"
                    @mouseleave="hideTooltip"
                    classInput="text-align-right "
                  />
                </div>
                <div class="size-20">
                  <Input
                    :disable="true"
                    :focusRequired="dataRequiredTop[0]"
                    classBorderInput="group-input size-100"
                    @mouseover="showTooltip"
                    @mouseleave="hideTooltip"
                    classInput="text-align-right "
                    :tooltipInput="requiredData.sumCost"
                    :isShowTooltipRequired="isShowTooltipRequired"
                    v-model="sumCost"
                    :dataRequired="requiredData.sumCost"
                    properties="sumCost"
                  />
                </div>
              </div>
            </div>
          </div>
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
import Button from "@/components/base/BaseButton.vue";
import Tooltip from "@/components/base/BaseTooltip.vue";
import Input from "@/components/base/BaseInput.vue";
import InputNumber from "@/components/base/BaseInputNumber.vue";
import Combobox from "@/components/base/BaseComboBox.vue";
import Notify from "@/components/base/BaseDialogNotify.vue";
import Resource from "@/js/resource";
import { API } from "@/js/callApi";
import { formatCash } from "@/js/common";
export default {
  name: "QuanlytaisanTheDialogIncrement",

  data() {
    return {
      dataRequiredTop: [],
      requiredData: {},
      requiredDataCombobox: [],
      positionInputRequired: [],
      position: 0,
      isShowTooltipRequired: false,
      dataItemDetail: {
        fixedAssetID: "",
        fixedAssetCode: "",
        fixedAssetName: "",
        departmentID: "",
        quantity: 0,
        cost: 0,
        lifeTime: 0,
        depreciationRate: 0,
        depreciationYear: "",
        purchaseDate: "",
        productionDate: "",
        price: "",
      },
      dataChange: false,
      dataCombobox: [],
      errors: [],
      buttonNames: [],
      id: [],
      dataUpdate: [],
      sumCost: 0,

      urlBudget: Resource.APIs.Budgets,
      dataIncrement: [],
      DialogNotify: false,
      isChange: false,
      checkRequiredAll: false,
      borderRedCombobox: [],
      isCheckRequiredCombobox: [],
      styleObject: {
        top: "40px !important",
      },
    };
  },
  props: {
    labelDialog: String,
    item: Array,
    idIncrement: String,
  },
  components: {
    Button,
    Tooltip,
    Input,
    Combobox,
    Notify,
    InputNumber,
  },
  created() {
    this.dataUpdate.idIncrement = this.idIncrement;
    // Lấy dữ liệu bảng budget
    API.get(Resource.APIs.Budgets)
      .then((res) => {
        this.dataBudget = res.data;
      })
      .catch((err) => {
        console.log(err);
      });
    //Nhận dữ liệu
    this.dataItemDetail = {
      fixedAssetID: this.item.fixedAssetID,
      departmentID: this.item.DepartmentID,
      cost: formatCash(this.item.cost),
      price: this.item.dataSource,
    };
    if (this.dataItemDetail.price == null || this.dataItemDetail.price == "") {
      this.dataItemDetail.price = JSON.stringify([
        {
          idIncrement: "",
          budget: "4a79f328-be08-49f4-b0df-fd28072548af",
          cost: this.dataItemDetail.cost,
          fixedAssetID: "",
        },
      ]);
    }
    var price = JSON.parse(this.dataItemDetail.price);
    console.log(price);
    this.dataUpdate = [];
    this.id = [];
    for (let index = 0; index < price.length; index++) {
      this.dataUpdate.push(price[index]);
      this.id.push(price[index].budget);
    }

    this.setSumCost();
    API.get(Resource.APIs.Departments)
      .then((res) => {
        for (let i = 0; i < res.data.length; i++) {
          if (this.dataItemDetail.departmentID == res.data[i].DepartmentID) {
            this.dataItemDetail.departmentName = res.data[i].DepartmentName;
            this.dataChange = true;
          }
        }
      })
      .catch((err) => {
        console.log(err);
      });
  },
  mounted() {
    var price = JSON.parse(this.dataItemDetail.price);
    console.log(price);
    this.dataUpdate = [];
    this.id = [];
    for (let index = 0; index < price.length; index++) {
      this.dataUpdate.push(price[index]);
      this.id.push(price[index].budget);
    }
    this.setSumCost();
    // })
    // .catch((error) => {
    //   console.log(error);
    // });
  },

  updated() {
    this.idIncrements = this.idIncrement;
    /**
     *  Khi combobox thay đổi thì kiểm tra nếu đã có dữ liệu thì bỏ tooltip
     * Author : Bùi Quang Điệp
     * Date:07/09/2022
     */
    for (let i = 0; i < this.isCheckRequiredCombobox.length; i++) {
      if (this.isCheckRequiredCombobox[i] == true) {
        this.borderRedCombobox[i] = false;
      }
    }
  },

  methods: {
    /**
     * Nhận dữ liệu từ sự kiện blur để check
     * Author : Bùi Quang Điệp
     * Date:11/09/2022
     */
    blurHandle(event, name, position) {
      try {
        this.dataRequiredTop = [];
        this.checkRequired(name, event, position);
      } catch (error) {
        console.log(error);
      }
    },
    /**
     * Required những dữ liệu cần thiết
     * Author : Bùi Quang Điệp
     * Date:10/08/2022
     */
    checkRequired(name, event, position) {
      try {
        // Kiểm tra name có dữ liệu hay không
        if (name != "") {
          if (!event.value) {
            event.classList.add("border-red");
          } else {
            event.classList.remove("border-red");
            this.positionInputRequired[position] = false;
          }
          this.checkRequiredAll = false;
        }
      } catch (error) {
        console.log(error);
      }
    },

    /**
     * Định dạng dấu phân cách hàng nghìn cho số lượng khi nhập liệu
     * Author : Bùi Quang Điệp
     * Date:10/08/2022
     */
    formatNumberInput(index, name) {
      try {
        var val = this.unFormNumber(this.dataUpdate[index][name]);
        if (val >= 1000) {
          if (name == "cost") {
            this.dataUpdate[index].cost = formatCash(val);
          }
        }

        // this.calculatedepreciationYear();
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
     * Đóng dialog thêm mới và show thông báo nếu người dùng hủy lưu khi chỉnh sửa
     * Author : Bùi Quang Điệp
     * Date:10/08/2022
     */
    closeDialog() {
      try {
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
      } catch (error) {
        console.log(error);
      }
    },

    checkBorderRedCombobox(item, position) {
      this.isCheckRequiredCombobox[position] = item;
      if (item) {
        this.requiredDataCombobox[position] = "";
      }
    },
    /**
     * Lấy dữ liệu khi chọn combobox
     * Author : Bùi Quang Điệp
     * Date :09 /08/ 2021
     */
    getDataCombobox(item, index) {
      this.dataUpdate[index].budget = item.BudgetID;
      this.id[index] = item.budgetID;
    },

    /**
     * Xóa id trong
     * Author : Bùi Quang Điệp
     * Date :09 /08/ 2021
     */
    removeID(position) {
      this.id.splice(position, 1);
    },

    /**
     * Xóa nguồn tiền khi bỏ chọn
     * Author : Bùi Quang Điệp
     * Date :09 /08/ 2021
     */
    removeBudget(position) {
      this.dataUpdate[position].budget = "";
    },

    /**
     * Hàm validation dữ liệu
     * Author : Bùi Quang Điệp
     * Date :09 /08/ 2021
     */
    validation() {
      // validation
      this.positionInputRequired = [];
      this.requiredDataCombobox = [];
      this.errors = [];
      var isCheck = true;
      // Kiểm tra nguồn hình thành không được trùng nhau
      for (let i = 0; i < this.dataUpdate.length; i++) {
        if (this.dataUpdate[i].budget == "") {
          this.errors = [];
          //this.requiredData.budget = Resource.Required.RequiredBudget;
          this.requiredDataCombobox.push(Resource.Required.RequiredBudget);
          if (this.borderRedCombobox.length == 0) {
            this.borderRedCombobox.push("");
          }
          this.borderRedCombobox[i] = true;
          //this.errors.push(Resource.Required.RequiredBudget);
          isCheck = false;
        } else {
          this.requiredDataCombobox.push("");
        }
        if (this.dataUpdate[i].cost == 0 || this.dataUpdate[i].cost == "") {
          this.errors = [];
          this.positionInputRequired.push(true);
          this.requiredData.cost = Resource.Required.RequiredCost;
          //  this.errors.push(Resource.Required.RequiredCost);
          isCheck = false;
        } else {
          this.positionInputRequired.push(false);
        }
        if (isCheck) {
          var count = 0;
          for (let j = 0; j < this.dataUpdate.length; j++) {
            if (this.dataUpdate[j].budget == this.dataUpdate[i].budget) {
              count++;
            }
            if (count >= 2) {
              this.errors = [];
              this.errors.push(Resource.Required.MinBudget);
              this.DialogNotify = true;
              this.buttonNames = ["Đồng ý"];
              return (isCheck = false);
            } else {
              this.DialogNotify = false;
            }
          }
        }
      }
      return isCheck;
    },

    /**
     * xử lý sự kiện lưu dữ liệu
     * Author : Bùi Quang Điệp
     * Date:10/10/2022
     */
    saveData() {
      try {
        var isCheck = this.validation();
        if (isCheck) {
          // tạo chuỗi json
          var data = {
            fixedAssetID: this.dataItemDetail.fixedAssetID,
            sumCost: this.unFormNumber(this.sumCost),
            dataSource: JSON.stringify(this.dataUpdate),
          };
          // API.put(
          //   Resource.APIs.FixedAssetIncrements + `UpdateCost`,
          //   JSON.stringify(data)
          // ).then((res) => {
          //   console.log(res);
          //   // đóng form
          //   this.$emit("btnCloseDialog");
          //   this.emitter.emit("LoadData");
          //   this.emitter.emit("handlerName", Resource.name.edit);
          // });

          this.$emit("sendDataBudget", data);
          this.$emit("btnCloseDialog");
        } else {
          this.checkRequiredAll = true;
          this.isShowTooltipRequired = true;
        }
      } catch (error) {
        console.log(error);
      }
    },

    /**
     * Nhận dữ liệu chỉnh sửa
     * Author : Bùi Quang Điệp
     * Date :09 /08/ 2021
     */
    itemDialog(item) {
      var assetIncrementID = item.fixedAssetIncrementID;
      API.get(
        Resource.APIs.FixedAssetIncrements +
          `IncrementDetail/?id=${assetIncrementID}`
      )
        .then((res) => {
          this.itemDetail = res.data;
          this.handler = Resource.CommandType.Edit;
          this.isShowDialog = true;
        })
        .catch((err) => {
          console.log(err);
        });
    },
    /**
     * tính tổng nguyên giá
     * Author : Bùi Quang Điệp
     * Date:10/10/2022
     */
    setSumCost() {
      this.sumCost = 0;
      for (let i = 0; i < this.dataUpdate.length; i++) {
        var cost = this.unFormNumber(this.dataUpdate[i].cost);
        this.sumCost = this.unFormNumber(this.sumCost);
        this.sumCost = Number(this.sumCost) + Number(cost);
        this.sumCost = formatCash(this.sumCost);
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
     * Thêm 1 item budget
     * Author : Bùi Quang Điệp
     * Date:10/10/2022
     */
    addItem() {
      this.dataUpdate.push({
        budget: "",
        cost: 0,
        fixedAssetID: this.dataItemDetail.fixedAssetID,
      });
      this.setSumCost();
      this.isChange = true;
    },
    /**
     * Xóa item budget
     * Author : Bùi Quang Điệp
     * Date:10/10/2022
     */
    reduceItem(index) {
      if (this.dataUpdate.length > 1) {
        this.dataUpdate.splice(index, 1);
        this.setSumCost();
      }
    },
    /**
     * Cập nhật lại dữ liệu khi có thay đối
     * Author : Bùi Quang Điệp
     * Date:15/08/2022
     */
    updateData(value, name) {
      try {
        name.cost = Number(value.toString().split(".").join(""));
      } catch (error) {
        console.log(error);
      }
    },
    /**
     * Kiểm tra có phải chuỗi json hay không
     * Author : Bùi Quang Điệp
     * Date:15/08/2022
     */
    isJson(str) {
      try {
        JSON.parse(str);
      } catch (e) {
        return false;
      }
      return true;
    },
  },
};
</script>

<style lang="css" scoped>
.voucher__information {
  width: 100%;
  background-color: #fff;
}
.dialog__body {
  background-color: #fff;
  padding-top: 0 !important;
}
.dialog__body--label {
  height: 20px;
  line-height: 20px;
  width: 100%;
}
.voucher__detail {
  width: 100%;
  height: calc(100vh - 500px);
  background-color: #fff;
}
.voucher__detail--body {
  height: calc(100% - 65px);
}
.content {
  align-items: center;
  justify-content: space-between;
  padding-right: 15px;
}
.voucher__detail--body-item {
  height: 148px;
  overflow-y: auto;
}
</style>
