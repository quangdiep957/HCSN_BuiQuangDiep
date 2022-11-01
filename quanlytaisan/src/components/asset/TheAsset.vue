<template>
  <div
    class="bodydata"
    @keydown.ctrl.prevent="showDialogCtrl"
    ref="body"
    tabindex="0"
  >
    <div class="m-row m-row__form" style="">
      <div class="m-row__left display-flex">
        <!-- tạo input search -->

        <Input
          iconInput="input__search--icon__item input__search--icon"
          :tooltipInput="tooltipSearch"
          :tooltipShow="isTooltipShow"
          Type="text"
          classInput="input__search--text"
          placeholder="Tìm kiếm theo mã, tên tài sản"
          ref="txtSearch"
          @keyDownEnter="handlerSearch"
          :inputSearch="true"
          :focusInput="false"
          @keyDown="checkInput"
        />
        <ComboBox
          val="Loại tài sản"
          :urlAPI="urlCategory"
          icon="true"
          nameTable="FixedAssetCategory"
          @dataComBoBoxSearch="handlerSearch"
          @remove="handlerSearch"
        />
        <ComboBox
          val="Bộ phận sử dụng"
          :urlAPI="urlDepartment"
          icon="true"
          nameTable="Department"
          @dataComBoBoxSearch="handlerSearch"
          @remove="handlerSearch"
        />
      </div>

      <div class="m-row__right display-flex">
        <Button
          @click="btnShowDialog()"
          buttonIconText="true"
          textButton="Thêm tài sản"
          textTooltip="Ctrl+I"
          tooltipShow="true"
        />
        <Button
          classIcon="bntfunction__more icon-more"
          inputInline="true"
          textTooltip="Nhập Excel"
          buttonIcon="true"
          @input="checkFile"
          @click="Export"
        />
        <Button
          classIcon="bntfunction__remove icon-remove"
          textTooltip="Xóa"
          buttonIcon="true"
          @click="HandleRemove"
        />
      </div>
    </div>
    <!-- Thêm table  -->
    <Table
      :dataLabel="dataLabel"
      :urlTable="urlTable"
      colspan="6"
      :showTable="showTable"
      @hideTable="showTable = false"
      @pageNumber="getPageNumber"
      @dataPageSize="getDataPageSize"
      :focusTable="true"
    />
  </div>
  <TheDialog
    v-if="isShowDialog"
    @btnCloseDialog="isShowDialog = false"
    :title="title"
    v-bind:itemCheck="itemAssetDetailCheck"
    @removeHandlerName="this.removeHandlerName = true"
    v-bind:item="itemAssetDetail"
    :handler="handler"
    @handlerName="showSuccess"
  />
  <Notify
    :domain="domain"
    @handlerName="showSuccess"
    :errorName="errorName"
    :isShowNumber="isShowNumber"
    v-if="isShowDialogNotify"
    @removeDialog="isShowDialogNotify = false"
    :dataError="titleWarning"
    @isShowDialogNotify="isShowDialogNotifyFuntion()"
    :dataAsset="dataTicks"
    :buttonNames="buttonText"
    :lastName="lastName"
    :firstText="firstText"
    :lastText="lastText"
  />
  <div class="toast__box" v-show="isShowSuccess == true">
    <div class="toast__box--item">
      <div class="toast__box--body">
        <div class="toast__box--item-icon">
          <div class="icon icon-tick-white icon__size-11x2"></div>
        </div>
      </div>
      <div class="toast__box--item-text">
        {{ handlerName }}
        <span v-if="removeHandlerName == false">dữ liệu thành công</span>
      </div>
    </div>
  </div>
</template>

<script>
import TheDialog from "../base/TheDialog.vue";
import Notify from "../base/BaseDialogNotify.vue";
import ComboBox from "../base/BaseComboBox.vue";
import Button from "../base/BaseButton.vue";
import Input from "../base/BaseInput.vue";
import Table from "../base/BaseTable.vue";
import Resource from "@/js/resource";
import { API } from "@/js/callApi";
import Enum from "@/js/enum";
export default {
  name: "QuanlytaisanAsset",

  data() {
    return {
      urlTable: Resource.APIs.AssetFilter,
      domain: Resource.APIs.AssetDelete,
      searchArray: {
        keyword: "",
        fixedAssetCategoryID: "",
        departmentID: "",
        pageSize: "",
        pageNumber: "",
        handlerName: "",
        errorName: "",
      },
      dataLabel: [
        {
          tooltipText: "",
          tooltip: "",
          textalign: "",
          name: "",
          label: "checkBox",
          maxWidth: "maxWidth",
        },
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
          name: "Loại sản phẩm",
          label: "Loại tài sản",
          nameValue: "fixedAssetCategoryID",
          format: true,
          table: "category",
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
          name: "Số lượng",
          label: "Số lượng",
          nameValue: "quantity",
          formatNumber: true,
          summaryLoad: "sumQuantity",
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
        },
        {
          tooltipText: "",
          tooltip: "none",
          textalign: "left",
          name: "Trạng thái",
          label: "Trạng thái",
          nameValue: "status",
          formatStatus: true,
          styleCss: "min-width:110px !important",
        },
        {
          tooltipText: "",
          tooltip: "none",
          textalign: "center",
          name: "Chức năng",
          label: "Chức năng",
        },
      ],
      urlDepartment: Resource.APIs.Departments,
      urlCategory: Resource.APIs.FixedAssetCategorys,
      isFocus: true,
      removeHandlerName: false,
      file: "",
      tooltipSearch: "",
      isTooltipShow: false,
      buttonText: ["Xóa", "Không"],
      isShowDialogNotify: false,
      itemAssetDetailCheck: [],
      isShowDialog: false,
      title: "Sửa tài sản",
      titleWarning: [],
      dataTicks: [],
      optionDepartment: [],
      optionCategory: [],
      itemAssetDetail: [],
      isSetWidth: false,
      handler: "",
      isShowSuccess: false,
      isShowNumber: false,
      dataReplication: {},
      isRemove: false,
      showTable: false,
      lastName: "",
      firstText: "",
      lastText: "",
    };
  },
  components: {
    ComboBox,
    Button,
    Input,
    Table,
    TheDialog,
    Notify,
  },
  methods: {
    /*
     *   Hàm xử lý nhận số trang hiển thị
     *   CreateBy: Bùi Quang Điệp
     *   Date : 08/10/2022
     */
    getPageNumber(item) {
      this.searchArray.pageNumber = item;
      this.handlerSearch();
    },
    /*
     *   Hàm xử lý nhận số bản ghi hiển thị
     *   CreateBy: Bùi Quang Điệp
     *   Date : 08/10/2022
     */
    getDataPageSize(item) {
      this.searchArray.pageSize = item;
      this.handlerSearch();
    },
    /**
     * Sự kiện show dialog
     * Author : Bùi Quang Điệp
     * Date :09 /08/ 2021
     */
    btnShowDialog() {
      try {
        // đặt lại input về rỗng
        this.itemAssetDetail = [];
        this.handler = "add";
        // Lấy mã tài sản mới nhất truyền vào input
        API.get(Resource.APIs.NewCode)
          .then((res) => {
            this.title = "Thêm tài sản";
            console.log(res);
            // Kiểm tra nếu là nhân bản thì truyền data vào
            if (this.dataReplication.fixedAssetCode != undefined) {
              this.handler = "replication";
              this.title = "Nhân bản tài sản";
              this.itemAssetDetail = this.dataReplication;
              this.dataReplication = {};
            }
            this.itemAssetDetail.fixedAssetCode = res.data;
            this.isShowDialog = true;
          })
          .catch((res) => {
            console.log(res);
          });
      } catch (error) {
        console.log(error);
      }
    },
    /**
     * Xử lý sự kiện xóa
     * Author : Bùi Quang Điệp
     * Date:10/08/2022
     */
    HandleRemove() {
      try {
        this.titleWarning = [];
        this.errorName = "";
        this.lastName = "";
        this.firstText = "";
        this.lastText = "";
        this.isShowDialogNotify = true;
        // Kiểm tra xem có tài sản nào đang sử dụng hay không
        var quantity = this.dataTicks.length;
        var isCheck = [];
        for (let i = 0; i < quantity; i++) {
          if (this.dataTicks[i].status == 1) {
            isCheck.push(this.dataTicks[i]);
          }
        }
        // có tài sản đã được sử dụng
        if (isCheck.length > 0) {
          API.get(
            Resource.APIs.FixedAssetIncrements +
              `GetNameIncremnt?id=${isCheck[0].fixedAssetID}`
          ).then((res) => {
            this.lastName = res.data;
          });
          (this.buttonText = ["Đồng ý"]), (this.firstText = "Tài sản có mã ");
          this.errorName = isCheck[0].fixedAssetCode;
          this.isShowNumber = true;
          this.titleWarning.push(Resource.Warning.AssetActive);
          this.lastText = ". Không thể xóa!";
        } else {
          if (quantity == 0) {
            (this.buttonText = ["Đồng ý"]),
              this.titleWarning.push(Resource.Warning.NoData);
          } else if (quantity == 1) {
            (this.buttonText = ["Xóa", "Không"]),
              this.titleWarning.push(Resource.Warning.Delete);
            this.isShowNumber = false;
            this.errorName = this.dataTicks[0].fixedAssetName + "?";
          } else {
            if (quantity < 10) {
              quantity = "0" + quantity;
            }
            (this.buttonText = ["Xóa", "Không"]), (this.errorName = quantity);
            this.titleWarning.push(Resource.Warning.Deletes);
            this.isShowNumber = true;
          }
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
    isShowDialogNotifyFuntion() {
      try {
        this.isShowDialogNotify = false;
        if (this.isRemove) {
          this.dataTicks = [];
          this.isRemove = false;
        }
      } catch (error) {
        console.log(error);
      }
    },
    /**
     *   Check file Excel
     * Author : Bùi Quang Điệp
     * Date:10/08/2022
     */
    checkFile(event) {
      try {
        if (event.currentTarget.files != null) {
          this.file = event.currentTarget.files;
          // Kiểm tra xem đúng định dạng Excel hay không
          if (
            this.file.type ===
            "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
          ) {
            // Gọi Api truyền dữ liệu lên
          } else {
            console.log("Chọn đúng định dạng");
          }
        }
      } catch (error) {
        console.log(error);
      }
    },

    /**
     *  Nhận dữ liệu lọc
     * Author : Bùi Quang Điệp
     * Date:10/08/2022
     */
    async handlerSearch(item) {
      try {
        if (item != undefined) {
          if (item.FixedAssetCategoryID == "") {
            this.searchArray.fixedAssetCategoryID = "";
          }
          if (item.DepartmentID != undefined) {
            if (item.DepartmentID == "") {
              this.searchArray.departmentID = "";
            }
            this.searchArray.departmentID = item.DepartmentID;
          }
          if (item.FixedAssetCategoryID != undefined) {
            this.searchArray.fixedAssetCategoryID = item.FixedAssetCategoryID;
          }
          if (event != null || event != undefined) {
            var keyword = event.currentTarget.value;
            if (keyword != undefined) {
              if (keyword.length > 50) {
                // Thông báo cho người dùng không nhập quá 50 kí tự
                this.tooltipSearch = "Không thể nhập quá 50 kí tự !";
                this.isTooltipShow = true;
                setTimeout(() => {
                  this.isTooltipShow = false;
                }, Enum.SetTimeOut.TimeOut);
                return;
              } else this.searchArray.keyword = item;
            }
          }
        }
        var httpSearch = ``;
        if (this.searchArray.keyword != undefined) {
          if (this.searchArray.keyword != "") {
            httpSearch = httpSearch + `keyword=${this.searchArray.keyword}&`;
          }
          if (this.searchArray.fixedAssetCategoryID != "") {
            httpSearch =
              httpSearch +
              `categoryAssetID=${this.searchArray.fixedAssetCategoryID}&`;
          }
          if (this.searchArray.departmentID != "") {
            httpSearch =
              httpSearch + `departmentID=${this.searchArray.departmentID}&`;
          }
        }
        if (this.searchArray.pageSize != "") {
          httpSearch = httpSearch + `pageSize=${this.searchArray.pageSize}&`;
        }
        if (this.searchArray.pageNumber != "") {
          httpSearch = httpSearch + `pageNumber=${this.searchArray.pageNumber}`;
        }
        this.urlTable = Resource.APIs.AssetFilter + httpSearch;
        this.showTable = true;
        // await  this.emitter.emit("search",httpSearch);
      } catch (error) {
        console.log(error);
      }
    },
    /**
     * Show thông báo thêm mới hoặc sửa thành công
     * Author : Bùi Quang Điệp
     * Date :14 /08/ 2021
     */
    showSuccess(item) {
      try {
        this.handlerName = item;

        this.isShowSuccess = true;

        setTimeout(() => {
          this.isShowSuccess = false;
        }, Enum.SetTimeOut.TimeOut);
      } catch (error) {
        console.log(error);
      }
    },

    /**
     * Kiểm tra không cho người dùng nhập kí tự đặc biệt vào input tìm kiếm
     * Author : Bùi Quang Điệp
     * Date :14 /08/ 2021
     */
    checkInput(event) {
      try {
        var keyChar = event.key;
        if (
          keyChar == "'" ||
          keyChar == "`" ||
          keyChar == "!" ||
          keyChar == "@" ||
          keyChar == "#" ||
          keyChar == "$" ||
          keyChar == "%" ||
          keyChar == "^" ||
          keyChar == "&" ||
          keyChar == "*" ||
          keyChar == "(" ||
          keyChar == ")" ||
          keyChar == "-" ||
          keyChar == "_" ||
          keyChar == "+" ||
          keyChar == "=" ||
          keyChar == "/" ||
          keyChar == "~" ||
          keyChar == "<" ||
          keyChar == ">" ||
          keyChar == "," ||
          keyChar == ";" ||
          keyChar == ":" ||
          keyChar == "|" ||
          keyChar == "?" ||
          keyChar == "{" ||
          keyChar == "}" ||
          keyChar == "[" ||
          keyChar == "]" ||
          keyChar == "¬" ||
          keyChar == "£" ||
          keyChar == '"' ||
          keyChar == "\\"
        ) {
          event.preventDefault();
        }
      } catch (error) {
        console.log(error);
      }
    },

    /**
     * Hiển thị form Thêm mới khi nhấn ctrl + A
     * Author : Bùi Quang Điệp
     * Date :14 /08/ 2021
     */
    showDialogCtrl(event) {
      try {
        if (event.code == "KeyI") {
          this.btnShowDialog();
        }
        if (event.code == "KeyA") {
          // Chọn All
          this.emitter.emit("tickAll");
          console.log(event);
        }
      } catch (error) {
        console.log(error);
      }
    },

    // Export(){
    //     axios.post("http://localhost:13846/api/v1/FixedAssets/Export",this.dataTicks)
    //     .then(res=>{
    //         console.log(res);
    //         const blob = new Blob([res.data], { // Make a BLOB object
    //             type : 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet'
    //         });

    //      (blob, "ExcelReport", 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet') // use the DownloadJS library and download the file
    //     })
    //     .catch(error=>{
    //         console.log(error)
    //     })
    // }
  },

  watch: {
    /**
     * Bắt sự kiện thay đổi dialog
     * Author : Bùi Quang Điệp
     * Date :14 /08/ 2021
     */
    isShowDialog: function (newValue, oldValue) {
      if (this.isShowDialog == false) {
        if (newValue != oldValue) {
          this.$refs["body"].focus();
        }
      }
    },

    /**
     * Bắt sự kiện thay đổi HandlerName
     * Author : Bùi Quang Điệp
     * Date :14 /08/ 2021
     */
    removeHandlerName: function (newValue, oldValue) {
      if (oldValue != newValue) {
        setTimeout(() => {
          this.removeHandlerName = false;
        }, Enum.SetTimeOut.TimeOut);
      }
    },
  },

  mounted() {
    /**
     * Sự kiện nhận dữu liệu từ EventBus
     * Author : Bùi Quang Điệp
     * Date :09 /08/ 2021
     */
    try {
      // Nhận lời gọi thêm mới từ table
      this.emitter.on("showDialog", () => {
        this.btnShowDialog();
      }),
        // nhận dữ liệu từ table
        this.emitter.on("itemDialog", (item) => {
          // gán giá trị mở dialog bằng true
          (this.isShowDialog = true), (this.title = "Sửa tài sản");

          // truyền data detail vào cho mảng
          this.itemAssetDetail = item;
          this.itemAssetDetailCheck = item;
          this.handler = "edit";
        }),
        this.emitter.on("titleWarning", (item) => {
          // truyền data vào cho mảng

          this.dataTicks = item;
        }),
        this.emitter.on("replication", (item) => {
          // Nhận dữ liệu cần nhân bản

          this.dataReplication = item;
          this.btnShowDialog();
        });

      // Nhận dữ liệu delete
      this.emitter.on("deleteAsset", (item) => {
        this.dataTicks = [];
        this.dataTicks.push(item);
        this.HandleRemove();
      });
      this.emitter.on("focusBody", () => {
        this.$refs["body"].focus();
      });
    } catch (error) {
      console.log(error);
    }
  },
};
</script>
