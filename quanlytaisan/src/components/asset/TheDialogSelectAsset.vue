<template>
  <div
    class="dialog__handle"
    id="dialog__handle"
    @keydown.esc="closeDialog"
    ref="dialog"
  >
    <div class="dialog">
      <div class="dialog__header">
        <h3 id="handle">{{ labelDialog }}</h3>
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
        <div>
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
                  :focus="isFocus"
                  @focus="isFocus = isFocus = false"
                  @keyDown="checkInput"
                  :styleInput="styleObject"
                />
                <div class="numberTick">
                  Số Bản ghi đã chọn: {{ numberRecord }}
                </div>
              </div>
            </div>
            <div class="voucher__detail--body">
              <Table
                :dataLabel="dataLabel"
                :urlTable="urlTable"
                :classTable="true"
                colspan="6"
                :loadTable="loadTable"
                @hideTable="loadTable = false"
                :canSummary="false"
                :classCustom="'tableBody'"
                moveData="true"
                @dataTick="dataTick"
                @pageNumber="getPageNumber"
                @dataPageSize="getDataPageSize"
                :bodyData="assetIDs"
                :focusTable="true"
                :isShowContextMenu=false
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
</template>

<script>
import Button from "@/components/base/BaseButton.vue";
import Tooltip from "@/components/base/BaseTooltip.vue";
import Input from "@/components/base/BaseInput.vue";
import Table from "@/components/base/BaseTable.vue";
import Enum from "@/js/enum";
import Resource from "@/js/resource";
import Config from "@/js/config";
export default {
  name: "TheDialogSelectAsset",

  data() {
    return {
      dataRequiredTop: [],
      requiredData: {},
      isShowTooltipRequired: false,
      dataItemDetail: [],
      dataTemporary: [],
      loadTable: false,
      dataChange: false,
      styleObject: {
        width: "calc(30% - 2px)",
        margin: "15px 0 15px 15px",
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
          summaryValue: "sumCost",
        },
        {
          tooltipText: "Hao mòn/khấu hao lũy kế",
          tooltip: "block",
          textalign: "right",
          name: "HM,KH lũy kế",
          label: "HM,KH lũy kế",
          nameValue: "depreciationValue",
          formatNumber: true,
          summaryValue: "sumDepreciation",
        },
        {
          tooltipText: "",
          tooltip: "none",
          textalign: "right",
          name: "Giá trị còn lại",
          label: "Giá trị còn lại",
          nameValue: "residualValue",
          formatNumber: true,
          summaryValue: "sumAtrophy",
        },
      ],
      urlTable: "",
      numberRecord: 1,
      searchArray: {
        keyword: "",
        fixedAssetCategoryID: "",
        departmentID: "",
        pageSize: "20",
        pageNumber: "",
        handlerName: "",
        errorName: "",
        filterID: 2,
        status: 0,
        key: "",
      },
    };
  },
  props: {
    labelDialog: String,
    id: String,
    assetIDs: [],
  },
  components: {
    Button,
    Tooltip,
    Input,
    Table,
  },

  mounted() {
    this.key = this.id;
    this.formatDateBase = Config.FormatDate.Value;
  },
  updated() {
    this.key = this.id;
  },
  created() {
    this.urlTable = Resource.APIs.AssetFilter + `status=0` + "&filterID=2";
  },
  methods: {
    /**
     * Đóng dialog thêm mới và show thông báo nếu người dùng hủy lưu khi chỉnh sửa
     * Author : Bùi Quang Điệp
     * Date:10/08/2022
     */
    closeDialog() {
      try {
        this.$emit("btnCloseDialog");
      } catch (error) {
        console.log(error);
      }
    },
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
    /*
     *   Hàm xử lý nhận data khi tick chọn
     *   CreateBy: Bùi Quang Điệp
     *   Date : 08/10/2022
     */
    dataTick(item) {
      this.dataTemporary = [];
      for (let i = 0; i < item.length; i++) {
        this.dataTemporary.push(item[i]);
        //this.$emit('btnCloseDialog');
      }
      this.numberRecord = this.dataTemporary.length;
    },
    /*
     *   Hàm xử lý gửi data vào cho table khi ấn lưu
     *   CreateBy: Bùi Quang Điệp
     *   Date : 08/10/2022
     */
    saveData() {
      // var data = {
      //   key: this.key,
      //   value: this.dataTemporary,
      // };
      // API.post("Caches", JSON.stringify(data))
      //   .then((res) => {
      //     console.log(res);
      //     this.$emit("btnCloseDialog");
      //   })
      //   .catch((error) => {
      //     console.log(error);
      //   });

      // gửi dữ liệu
      this.$emit("btnCloseDialog");
      this.$emit("sendDataAsset", this.dataTemporary);
    },
    /**
     *  Nhận dữ liệu lọc
     * Author : Bùi Quang Điệp
     * Date:10/08/2022
     */
    async handlerSearch(item) {
      try {
        if (item != undefined) {
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
          if (this.searchArray.pageSize != "") {
            httpSearch = httpSearch + `pageSize=${this.searchArray.pageSize}&`;
          }
          if (this.searchArray.pageNumber != "") {
            httpSearch =
              httpSearch + `pageNumber=${this.searchArray.pageNumber}&`;
          }

          httpSearch = httpSearch + `status=${this.searchArray.status}&`;
          if (this.searchArray.filterID != "") {
            httpSearch = httpSearch + `filterID=${this.searchArray.filterID}&`;
          }
        }
        this.urlTable = Resource.APIs.AssetFilter + httpSearch;
        this.loadTable = true;
      } catch (error) {
        console.log(error);
      }
    },
  },
};
</script>

<style lang="css" scoped>
.dialog__body {
  padding-top: 0 !important;
}
.dialog__body--label {
  height: 40px;
  line-height: 40px;
  width: 100%;
}
.voucher__detail {
  width: 100%;
  height: calc(100vh - 220px);
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
</style>
