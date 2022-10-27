<template>
  <div class="dialog__handle" id="dialog__handle" @keydown.esc="CloseKeyESC">
    <div class="dialog" ref="dialog">
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
          <div class="dialog__body--label">Thông tin chứng từ</div>
          <div class="voucher__information">
            <div class="m-row">
              <Input
                :focusRequired="dataRequiredTop[0]"
                :focus="isFocus"
                @isFocus="isFocus = false"
                classBorderInput="group-input size-33"
                @mouseover="showTooltip"
                @mouseleave="hideTooltip"
                classInput="text-align-left absolute"
                labelInput="Mã chứng từ"
                :tooltipInput="requiredData.fixedAssetIncrementCode"
                :isShowTooltipRequired="isShowTooltipRequired"
                v-model="itemDetail.fixedAssetIncrementCode"
                :dataRequired="requiredData.fixedAssetIncrementCode"
                :dataChange="dataChange"
                @dataChange="dataChange = false"
                properties="fixedAssetIncrementCode"
                required="required"
                tabindex="1008"
                @blur="blurHandle"
                :checkRequiredAll="checkRequiredAll"
              />
              <DateTime
                label="Ngày chứng từ"
                tabindex="1009"
                :dataDate="itemDetail.dateVocher"
                @Date="getdateVocher"
                :formatType="formatDateBase"
              />
              <DateTime
                label="Ngày ghi tăng"
                tabindex="1010"
                :dataDate="itemDetail.dateIncrement"
                @Date="getDateIncrement"
                :formatType="formatDateBase"
                styleCustomer="margin-right:15px"
              />
            </div>
            <div class="m-row" style="padding-right: 15px">
              <Input
                classBorderInput="group-input size-100"
                classInput="text-align-left"
                v-model="itemDetail.description"
                labelInput="Ghi chú"
                properties="description"
                tabindex="1011"
              />
            </div>
          </div>
          <div class="dialog__body--label">Thông tin chi tiết</div>
          <div class="voucher__detail">
            <div class="voucher__detail--header">
              <div class="m-row content">
                <Input
                  iconInput="input__search--icon__item input__search--icon"
                  :tooltipInput="tooltipSearch"
                  :tooltipShow="isTooltipShow"
                  Type="text"
                  classInput="input__search--text"
                  placeholder="Tìm kiếm theo mã tên tài sản"
                  ref="txtSearch"
                  @keyDownEnter="handlerSearch"
                  :inputSearch="true"
                  :focusInput="false"
                  @keyDown="checkInput"
                  tabindex="1012"
                  :styleInput="styleObject"
                />
                <Button
                  @click="isShowDialogSelect = true"
                  buttonIconText="true"
                  textButton="Chọn tài sản"
                />
              </div>
            </div>
            <div class="voucher__detail--body">
              <Table
                :dataLabel="dataLabel"
                :urlTable="urlTable"
                :classTable="true"
                colspan="6"
                :can-paging="false"
                :showTable="showTable"
                @hideTable="showTable = false"
                :classCustom="'tableBody'"
                @itemDialog="getItemAsset"
                @deleteAsset="deleteAsset"
                @sendData="getDataTable"
                :styleImage="styleImage"
                @sendDataEntry="getDataEntry"
                :dataEntry="isGetDataEntry"
                :summaryColumn="summaryColumns"
                @summaryColumn="summaryColumns = false"
                :changeData="changeData"
                @changeData="getDataTable"
                styleTooltip="true"
                :originalTick="false"
              />
            </div>
          </div>
        </div>
      </div>
      <div class="dialog__footer">
        <Button
          @click="saveData"
          classButton="btn-save active"
          :tabindex="1012"
          textButton="Lưu"
          :focusSave="focusSave"
        />
        <Button
          @keydownTab="isFocus = true"
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

  <DialogSelect
    v-if="isShowDialogSelect"
    @btnCloseDialog="closeDialogSelect"
    :id="key"
    labelDialog="Chọn tài sản ghi tăng"
  />
  <DialogBudget
    v-if="isShowDialogBudget"
    @sendDataBudget="getDataBudget"
    @btnCloseDialog="isShowDialogBudget = false"
    :labelDialog="labelDialogBudget"
    :item="dataAssetDetail"
    :idIncrement="itemDetail.fixedAssetIncrementID"
  />
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
import Table from "@/components/base/BaseTable.vue";
import DateTime from "@/components/base/BaseDate.vue";
import Enum from "@/js/enum";
import Resource from "@/js/resource";
import DialogSelect from "./TheDialogSelectAsset.vue";
import DialogBudget from "./TheDialogBudget.vue";
import Config from "@/js/config";
import Notify from "@/components/base/BaseDialogNotify.vue";
import { API } from "@/js/callapi";
export default {
  name: "QuanlytaisanTheDialogIncrement",

  data() {
    return {
      formatDateBase: "DMY",
      dataRequiredTop: [],
      isChange: false,
      summaryColumns: false,
      dataBudget: [],
      requiredData: {
        fixedAssetIncrementCode: "",
      },
      dataUpdate: [],
      itemDetail: {
        fixedAssetIncrementCode: "",
        dateVocher: "",
        dateIncrement: "",
        description: "",
        fixedAssetIncrementID: "",
      },
      isShowTooltipRequired: false,
      isShowDialogBudget: false,
      labelDialog: "",
      dataChange: false,
      styleObject: {
        width: "calc(30% - 2px)",
        margin: "15px 0 15px 15px",
      },
      styleImage: {
        height: "60px",
      },
      dataLabel: [
        {
          tooltipText: "Số thứ tự",
          tooltip: "block",
          textalign: "center",
          name: "STT",
          label: "STT",
          maxWidth: "maxWidthN",
        },
        {
          tooltipText: "",
          tooltip: "none",
          textalign: "left",
          name: "Mã tài sản",
          label: "Mã tài sản",
          nameValue: "fixedAssetCode",
        },
        {
          tooltipText: "",
          tooltip: "none",
          textalign: "left",
          name: "Tên tài sản",
          label: "Tên tài sản",
          nameValue: "fixedAssetName",
        },
        {
          tooltipText: "",
          tooltip: "none",
          textalign: "left",
          name: "Bộ phận sử dụng",
          label: "Bộ phận sử dụng",
          nameValue: "departmentID",
          table: "department",
          format: true,
        },
        {
          tooltipText: "",
          tooltip: "none",
          textalign: "right",
          name: "Nguyên giá",
          label: "Nguyên giá",
          nameValue: "cost",
          formatNumber: true,
          summaryLoad: "sumPrice",
        },
        {
          tooltipText: "Hao mòn/khấu hao lũy kế",
          tooltip: "block",
          textalign: "right",
          name: "HM,KH lũy kế",
          label: "HM,KH lũy kế",
          nameValue: "depreciationValue",
          formatNumber: true,
          summaryLoad: "sumDepreciation",
          styleCss: "min-width:100px",
        },
        {
          tooltipText: "",
          tooltip: "none",
          textalign: "right",
          name: "Giá trị còn lại",
          label: "Giá trị còn lại",
          nameValue: "residualValue",
          formatNumber: true,
          summaryLoad: "sumAtrophy",
          styleCss: "min-width:100px",
        },
        {
          tooltipText: "",
          tooltip: "none",
          textalign: "center",
          name: "Chức năng",
          label: "Chức năng",
          replication: false,
          styleCss:
            "width:90px !important ; max-width:90px !important ; min-width:90px !important ;position: sticky;right: 0px;background-color: #fff;",
        },
      ],
      isShowDialogSelect: false,
      // urlTable:Resource.APIs.AssetFilter + 'filterId=1',
      dataEntry: [],
      changeData: {},
      isGetDataEntry: true,
      DialogNotify: false,
      buttonNames: [],
      errors: [],
      inputHandler: false,
      summaryIncrement: 0,
      showTable: false,
      getPrice: false,
      searchArray: {
        keyword: "",
        fixedAssetCategoryID: "",
        departmentID: "",
        pageSize: "",
        pageNumber: "",
        handlerName: "",
        errorName: "",
        filterID: 1,
      },
      key: "",
      isFocus: true,
      dataAssetDetail: [
        {
          fixedAssetIncrementID: "",
          fixedAssetID: "",
          dataSource: "",
          cost: 0,
          departmentID: "",
        },
      ],
    };
  },
  props: {
    item: Array,
    handler: String,
  },
  components: {
    Button,
    Tooltip,
    Input,
    Table,
    DateTime,
    DialogSelect,
    DialogBudget,
    Notify,
  },

  watch: {
    "this.itemDetail.fixedAssetIncrementID": function (newValue, oldValue) {
      if (newValue != oldValue) {
        this.key = this.itemDetail.fixedAssetIncrementCode;
      }
    },
    "itemDetail.fixedAssetIncrementCode": function (newValue, oldValue) {
      if (oldValue != "") {
        if (newValue != oldValue) {
          this.isChange = true;
        }
      }
    },
    "itemDetail.description": function (newValue, oldValue) {
      if (oldValue != "") {
        if (newValue != oldValue) {
          this.isChange = true;
        }
      }
    },
    dataUpdate: function (newValue, oldValue) {
      if (oldValue.length != 0) {
        if (JSON.stringify(oldValue) != JSON.stringify(newValue)) {
          this.isChange = true;
        }
      }
    },
  },
  created() {
    // Gán dữ liệu nhận được vào 1 biến ItemDetail
    this.itemDetail = this.item;
    if (this.handler == Resource.CommandType.Edit) {
      this.labelDialog = Resource.TitleDialog.EditIncrement;
      this.urlTable =
        Resource.APIs.FixedAssetIncrements +
        `AssetMulti?id=${this.itemDetail.fixedAssetIncrementID}` +
        `&key=${this.itemDetail.fixedAssetIncrementCode}&filterId=1`;
      this.showTable = true;
    } else if (this.handler == Resource.CommandType.Add) {
      this.urlTable =
        Resource.APIs.FixedAssetIncrements +
        `AssetMulti?` +
        `key=${this.itemDetail.fixedAssetIncrementCode}&filterId=1`;
      this.labelDialog = Resource.TitleDialog.AddIncrement;
      this.showTable = true;
    }
    this.key = this.itemDetail.fixedAssetIncrementCode;
    if (
      this.itemDetail.dateVocher == undefined ||
      this.itemDetail.dateIncrement == undefined
    ) {
      this.itemDetail.dateVocher = new Date();
      this.itemDetail.dateIncrement = new Date();
    }
    this.formatDateBase = Config.FormatDate.Value;
  },

  methods: {
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
          }
          return val;
        } else return val;
      } catch (error) {
        console.log(error);
      }
    },

    /**
     * Nhận dữ liệu danh sách nguồn hình thành
     * Author : Bùi Quang Điệp
     * Date:10/08/2022
     */
    getDataBudget(item) {
      // Kiểm tra xem đã có dữ liệu hay chưa
      if (this.dataBudget.length == 0) {
        this.dataBudget.push(item);
      } else {
        // Kiểm tra xem ID tài sản đã trùng chưa
        for (let i = 0; i < this.dataBudget.length; i++) {
          if (this.dataBudget[i].fixedAssetID == item.fixedAssetID) {
            this.dataBudget.splice(i, 1);
          }
        }
        this.dataBudget.push(item);
      }
      this.changeData = item;
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
    closeDialogSelect() {
      try {
        this.isShowDialogSelect = false;
        this.showTable = true;
      } catch (error) {
        console.log(error);
      }
    },
    /*
     *   Xóa dữ liệu 1 bản ghi
     *   CreateBy: Bùi Quang Điệp
     *   Date : 08/10/2022
     */
    deleteAsset(item) {
      for (let i = 0; i < this.dataUpdate.length; i++) {
        if (this.dataUpdate[i] == item) {
          this.dataUpdate.splice(i, 1);
        }
      }
      for (let i = 0; i < this.dataEntry.length; i++) {
        if (this.dataEntry[i].fixedAssetID == item.fixedAssetID) {
          this.dataEntry.splice(i, 1);
        }
        // Gọi vào table tính lại tổng
        this.summaryColumns = true;
        this.sumIncrement();
      }
    },
    /**
     *  Nhận dữ liệu lọc
     * Author : Bùi Quang Điệp
     * Date:10/08/2022
     */
    async handlerSearch(item) {
      try {
        this.getPrice = true;
        if (event != null || event != undefined) {
          var keyword = event.currentTarget.value;
          if (keyword != undefined) {
            if (keyword.length > 50) {
              // Thông báo cho người dùng không nhập quá 255 kí tự
              this.tooltipSearch = "Không thể nhập quá 255 kí tự !";
              this.isTooltipShow = true;
              setTimeout(() => {
                this.isTooltipShow = false;
              }, Enum.SetTimeOut.TimeOut);
              return;
            } else this.searchArray.keyword = item;
          }
        }

        var httpSearch = ``;
        if (this.searchArray.keyword != undefined) {
          if (this.searchArray.keyword != "") {
            httpSearch = httpSearch + `keyword=${this.searchArray.keyword}`;
          }
          if (this.searchArray.pageSize != "") {
            httpSearch = httpSearch + `pageSize=${this.searchArray.pageSize}&`;
          }
          if (this.searchArray.pageNumber != "") {
            httpSearch =
              httpSearch + `pageNumber=${this.searchArray.pageNumber}`;
          }
          if (this.itemDetail.fixedAssetIncrementID != undefined) {
            this.urlTable =
              Resource.APIs.FixedAssetIncrements +
              `AssetMulti?id=${this.itemDetail.fixedAssetIncrementID}` +
              `&key=${this.itemDetail.fixedAssetIncrementCode}&` +
              httpSearch +
              `&filterId=1`;
            this.showTable = true;
          } else {
            this.urlTable =
              Resource.APIs.FixedAssetIncrements +
              `AssetMulti?` +
              `&key=${this.itemDetail.fixedAssetIncrementCode}&` +
              httpSearch +
              `&filterId=1`;
            this.showTable = true;
          }
        }
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
        this.itemDetail[name] = value;
      } catch (error) {
        console.log(error);
      }
    },

    /**
     *   Nhận data tài sản cần sửa
     * Author : Bùi Quang Điệp
     * Date:10/08/2022
     */
    getItemAsset(item) {
      this.dataAssetDetail = {
        fixedAssetID: item.fixedAssetID,
        dataSource: item.price,
        cost: item.cost,
        DepartmentID: item.departmentID,
      };
      // show form dialog chỉnh sửa
      this.isShowDialogBudget = true;
      this.labelDialogBudget = "Sửa tài sản " + `${item.fixedAssetName}`;
    },

    /**
     *  Tính tổng giá các tài sản trong table
     * Author : Bùi Quang Điệp
     * Date:10/08/2022
     */
    sumIncrement() {
      this.summaryIncrement = 0;
      for (let i = 0; i < this.dataUpdate.length; i++) {
        this.summaryIncrement =
          Number(this.summaryIncrement) + Number(this.dataUpdate[i].cost);
      }
    },

    /**
     *  Tính tổng giá các tài sản của mã ghi tăng
     * Author : Bùi Quang Điệp
     * Date:10/08/2022
     */
    sumIncrementEntry() {
      this.summaryIncrement = 0;
      for (let i = 0; i < this.dataEntry.length; i++) {
        this.summaryIncrement =
          Number(this.summaryIncrement) + Number(this.dataEntry[i].cost);
      }
    },
    /**
     *   Nhận data tài sản
     * Author : Bùi Quang Điệp
     * Date:10/08/2022
     */
    getDataTable(item) {
      try {
        this.dataUpdate = item;
        var isCheck = false;
        for (let i = 0; i < item.length; i++) {
          isCheck = false;
          for (let j = 0; j < this.dataEntry.length; j++) {
            if (item[i].fixedAssetID == this.dataEntry[j].fixedAssetID) {
              if (item[i].cost != this.dataEntry[j].cost) {
                this.dataEntry[j].cost = item[i].cost;
                this.dataEntry[j].price = item[i].price;
              }

              isCheck = true;
            }
          }
          if (!isCheck) {
            this.dataEntry.push(item[i]);
          }
          // Tính tổng
          this.sumIncrement();
          this.changeData = {};
        }
      } catch (error) {
        console.log(error);
      }
    },

    /**
     *  Validation dữ liệu
     * Author : Bùi Quang Điệp
     * Date:10/08/2022
     */
    validation() {
      // validate dữ liệu
      // Check required
      var date = new Date();
      var day = date.getDate() + 1;
      date = date.setDate(day);
      var isCheck = true;
      if (this.itemDetail.fixedAssetIncrementCode == "") {
        isCheck = false;
      } else {
        // Check độ dài
        if (this.itemDetail.fixedAssetIncrementCode.length > 20) {
          isCheck = false;
          this.errors.push(Resource.Required.LengthAssetCode);
          this.buttonNames = [Resource.Label.Close];
        }
      }
      if (
        this.itemDetail.dateIncrement == "" &&
        this.itemDetail.dateVocher == ""
      ) {
        isCheck = false;
      } else {
        if (new Date(this.itemDetail.dateIncrement) > date) {
          isCheck = false;
          this.errors.push(Resource.Required.MaxIncrementDate);
          this.buttonNames = [Resource.Label.Close];
        } else if (new Date(this.itemDetail.dateVocher) > date) {
          isCheck = false;
          this.errors.push(Resource.Required.MaxDateVocher);
          this.buttonNames = [Resource.Label.Close];
        }
      }

      // Kiểm tra ít nhất phải có 1 bản ghi được chọn
      if (this.dataEntry.length < 1) {
        isCheck = false;
        this.errors = [];
        (this.buttonNames = [Resource.Label.Agree]),
          this.errors.push(Resource.Required.MinData);
      }
      return isCheck;
    },

    /**
     *  Validation tất cả dữ liệu khi chưa nhập
     * Author : Bùi Quang Điệp
     * Date:10/08/2022
     */
    validationAll() {
      this.checkRequiredAll = true;
      if (!this.itemDetail.fixedAssetIncrementCode) {
        this.requiredData.fixedAssetIncrementCode =
          Resource.Required.FixedAssetIncrementCode;
        this.dataRequiredTop.push("fixedAssetIncrementCode");
      }
      if (!this.itemDetail.dateIncrement) {
        this.requiredData.dateIncrement = Resource.Required.dateIncrement;
        this.dataRequiredTop.push("dateIncrement");
      }
      if (!this.itemDetail.dateVocher) {
        this.requiredData.dateVocher = Resource.Required.dateVocher;
        this.dataRequiredTop.push("dateVocher");
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
    /*
     *   Hàm xử lý sự kiện khi ấn lưu
     *   CreateBy: Bùi Quang Điệp
     *   Date : 08/10/2022
     */

    saveData() {
      var isCheck = this.validation();
      if (isCheck) {
        // Nếu không có lỗi tạo Json
        // Tính tổng
        //Lấy danh sách ID
        var dataID = [];
        for (let i = 0; i < this.dataEntry.length; i++) {
          dataID.push(this.dataEntry[i].fixedAssetID);
        }
        // Tính tổng
        this.sumIncrementEntry();
        var dataJson = {
          fixedAssetIncrementCode: this.itemDetail.fixedAssetIncrementCode,
          dateVocher: this.itemDetail.dateVocher,
          dateIncrement: this.itemDetail.dateIncrement,
          Description: this.itemDetail.description,
          fixedAssetncrementName: "Ghi tăng",
          price: this.unFormNumber(this.summaryIncrement),
          FixedAssetIDs: dataID,
          FixedAssets: this.dataBudget,
        };
        // kiểm tra handler thực hiện là gì
        if (this.handler == Resource.CommandType.Add) {
          API.post(Resource.APIs.FixedAssetIncrements, JSON.stringify(dataJson))
            .then((res) => {
              console.log(res);
              this.$emit("btnCloseDialog");
              this.emitter.emit("LoadData");
              this.$emit("handlerName", Resource.name.add);
            })
            .catch((error) => {
              console.log(error.message);
              if (error.response.data.handle == Enum.FormModeHandler.Required) {
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
                API.get(Resource.APIs.NewCodeIncrement)
                  .then((res) => {
                    this.dataChange = true;
                    this.itemDetail.fixedAssetIncrementCode = res.data;
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
        } else if (this.handler == Resource.CommandType.Edit) {
          API.put(
            Resource.APIs.FixedAssetIncrements +
              `?FixedAssetID=${this.itemDetail.fixedAssetIncrementID}&fixedAssetCode=${this.itemDetail.fixedAssetIncrementCode}`,
            JSON.stringify(dataJson)
          )
            .then((res) => {
              console.log(res);
              this.$emit("btnCloseDialog");
              this.emitter.emit("LoadData");
              this.$emit("handlerName", Resource.name.edit);
            })
            .catch((error) => {
              if (error.response.data.handle == Enum.FormModeHandler.Required) {
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
                API.get(Resource.APIs.NewCodeIncrement)
                  .then((res) => {
                    this.dataChange = true;
                    this.itemDetail.fixedAssetIncrementCode = res.data;
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
      // chưa nhập đầy đủ dữ liệu
      else {
        this.validationAll();
      }
    },

    /**
     * lấy dữ liệu ban đầu
     * Author : Bùi Quang Điệp
     * Date :09 /08/ 2021
     */
    getDataEntry(item) {
      this.isGetDataEntry = false;
      this.dataEntry = item;
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
     * nhận dữ liệu thay đổi và cập nhật lại ngày mua
     * Author : Bùi Quang Điệp
     * Date:15/08/2022
     */
    getdateVocher(res) {
      try {
        res.setDate(res.getDate() + 1);
        this.itemDetail.dateVocher = res;
      } catch (error) {
        console.log(error);
      }
    },
    /**
     * nhận dữ liệu thay đổi và cập nhật lại ngày mua
     * Author : Bùi Quang Điệp
     * Date:15/08/2022
     */
    getDateIncrement(res) {
      try {
        res.setDate(res.getDate() + 1);
        this.itemDetail.dateIncrement = res;
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
          this.dataItemDetail[name] = event.value;
        }
      } catch (error) {
        console.log(error);
      }
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
  background-color: #f4f7ff !important;
  padding: 15px !important;
  padding-top: 0 !important;
}
.dialog__body--label {
  height: 40px;
  line-height: 40px;
  width: 100%;
}
.voucher__detail {
  width: 100%;
  height: calc(100vh - 500px);
  background-color: #fff;
}
.voucher__detail--body {
  height: calc(100% - 22px);
}
.content {
  align-items: center;
  justify-content: space-between;
  padding-right: 15px;
}
</style>
