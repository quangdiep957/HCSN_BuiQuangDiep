<template>
  <div class="container-asset">
    <div class="header">
      <span class="title-header">Ghi tăng tài sản</span>
      <div class="group-button">
        <Button
          classIcon="bntfunction__remove icon-remove"
          styleButton="margin-right:15px !important"
          positiontooltip="top"
          v-if="showRemove"
          textTooltip="Xóa"
          buttonIcon="true"
          @click="HandleRemove"
        />

        <Button textButton="Thêm" @click="btnShowDialog()" />
        <Button
          buttonIconMulti="true"
          :classIcon="iconDisplay"
          :classIconMulti="'icon-down-black'"
          @click="showDisplay = true"
          positiontooltip="top"
          textTooltip="chiều dọc"
        />
      </div>
      <div class="group-display" v-if="showDisplay">
        <div class="display-item" @click="zoomMaster('horizontal')">
          <div
            class="icon icon__size-20 icon_vertical"
            style="position: relative; top: 3px; margin-right: 10px"
          ></div>
          Giao diện ngang
        </div>
        <div class="display-item" @click="zoomMaster">
          <div
            class="icon icon__size-20 icon_horizontal"
            style="position: relative; top: 3px; margin-right: 5px"
          ></div>
          Giao diện dọc
        </div>
      </div>
    </div>
    <div class="body">
      <splitpanes horizontal class="default-theme" size="100">
        <pane :size="masterSize">
          <div class="body_table" style="height: 100%; padding-bottom: 20px">
            <div class="bode_table--header">
              <Input
                inputSearch="true"
                placeholder="Tìm kiếm theo số chứng từ, nội dung"
                class="input_search"
                :styleInput="styleObject"
                @keyDownEnter="handlerSearch"
                :focus="isFocus"
                @focus="isFocus = isFocus = false"
                :focusInput="false"
                @keyDown="checkInput"
              />
              <div class="group_icon">
                <div
                  class="icon icon__size-18 icon_print tooltip"
                  style="position: relative; left: 10px"
                >
                  <ToolTip
                    style="margin-left: -10px; margin-top: 20px"
                    tooltiptext="In"
                    positiontooltip="right"
                  />
                </div>
                <div
                  class="icon icon__size-13 icon_dots tooltip"
                  style="position: absolute; right: 5px"
                >
                  <ToolTip
                    style="margin-left: -50px; margin-top: 20px"
                    tooltiptext="Lựa chọn"
                    positiontooltip="right"
                  />
                </div>
              </div>
            </div>
            <Table
              :dataLabel="dataLabel"
              :urlTable="urlTable"
              :classTable="true"
              colspan="1"
              :showReplication="false"
              @showDialog="btnShowDialog"
              :dataEntry="dataEntry"
              @sendDataEntry="dataEntry = false"
              @dataTick="assetIncrement"
              @itemDialog="itemDialog"
              @deleteAsset="dataDelete"
              :showTable="showTableIncrement"
              @hideTable="showTableIncrement = false"
              @pageNumber="getPageNumber"
              @dataPageSize="getDataPageSize"
              :focusTable="true"
              :originalTick="true"
            />
          </div>
        </pane>
        <pane :size="detailSize">
          <div class="body_table" style="height: 100%">
            <div class="tooltip resize">
              <ToolTip
                style="left: 47%; margin-left: -10px; margin-top: 10px"
                tooltiptext="Thay đổi chiều cao"
                positiontooltip="right"
              />
            </div>
            <div class="display-flex">
              <div class="header_table--two title-header">
                Thông tin chi tiết
              </div>
              <div style="position: absolute; right: 10px" class="tooltip">
                <div
                  class="icon icon__size-18 icon_zoom"
                  @click="zoomDetail"
                ></div>
                <ToolTip
                  style="margin-left: -50px; margin-top: 5px"
                  tooltiptext="Phóng to"
                  positiontooltip="right"
                />
              </div>
            </div>

            <Table
              :dataLabel="dataLabelAsset"
              :urlTable="urlTableDataID"
              :classTable="true"
              :canPaging="false"
              :showReplication="false"
              colspan="3"
              :showTable="showTable"
              @hideTable="showTable = false"
              :styleImage="styleImageDetail"
              :styleTable="styleTable"
              @itemDialog="getItemAsset"
              :originalTick="false"
              :isShowContextMenu="false"
            />
          </div>
        </pane>
      </splitpanes>
    </div>
  </div>
  <Dialog
    v-if="isShowDialog"
    @btnCloseDialog="isShowDialog = false"
    :item="itemDetail"
    :handler="handler"
    @handlerName="showSuccess"
    :labelDialog="labelDialog"
  />
  <Notify
    :isShowNumber="isShowNumber"
    :errorName="errorName"
    @handlerName="showSuccess"
    v-if="isShowDialogNotify"
    :dataError="titleWarning"
    :domain="domain"
    @isShowDialogNotify="isShowDialogNotifyFuntion()"
    @removeData="isRemove = true"
    :dataAsset="dataTicks"
    @removeDialog="isShowDialogNotify = false"
    :buttonNames="buttonText"
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
import { Splitpanes, Pane } from "splitpanes";
import "splitpanes/dist/splitpanes.css";
import Button from "@/components/base/BaseButton.vue";
import Input from "@/components/base/BaseInput.vue";
import Table from "@/components/base/BaseTable.vue";
import Dialog from "@/components/asset/TheDialogIncrement.vue";
import Resource from "@/js/resource";
import Enum from "@/js/enum";
import ToolTip from "@/components/base/BaseTooltip.vue";
import { API } from "@/js/callApi";
import Notify from "@/components/base/BaseDialogNotify.vue";

export default {
  name: "QuanlytaisanWriteIncrease",

  data() {
    return {
      // dataLabel:[
      //     { tooltipText:'',tooltip:'',textalign:'',name: "", label: "checkBox" , maxWidth:'maxWidth' },
      //     { tooltipText:'Số thứ tự',tooltip:'block',textalign:'center',name: "STT", label: "STT" , maxWidth:'maxWidthN' },
      //     { tooltipText:'',tooltip:'none',textalign:'left',name: "Số chứng từ", label: "Số chứng từ" },
      //     { tooltipText:'',tooltip:'none',textalign:'left',name: "Ngày chứng từ", label: "Ngày chứng từ" },
      //     { tooltipText:'',tooltip:'none',textalign:'left',name: "Ngày ghi tăng", label: "Ngày ghi tăng" },
      //     { tooltipText:'',tooltip:'none',textalign:'left',name: "Bộ phận sử dụng", label: "Bộ phận sử dụng" },
      //     { tooltipText:'',tooltip:'none',textalign:'right',name: "Tổng nguyên giá", label: "Tổng nguyên giá" },
      //     { tooltipText:'',tooltip:'none',textalign:'right',name: "Nội dung", label: "Nội dung" },

      // ],
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
          name: "Số chứng từ",
          label: "Số chứng từ",
          nameValue: "fixedAssetIncrementCode",
          classColor: true,
          click: true,
        },
        {
          tooltipText: "",
          tooltip: "none",
          textalign: "center",
          name: "Ngày chứng từ",
          label: "Ngày chứng từ",
          nameValue: "dateVocher",
          formatDate: true,
        },
        {
          tooltipText: "",
          tooltip: "none",
          textalign: "center",
          name: "Ngày ghi tăng",
          label: "Ngày ghi tăng",
          nameValue: "dateIncrement",
          formatDate: true,
        },
        {
          tooltipText: "",
          tooltip: "none",
          textalign: "right",
          name: "Nguyên giá",
          label: "Nguyên giá",
          nameValue: "price",
          formatNumber: true,
          summaryLoad: "sumPriceIncrement",
        },
        {
          tooltipText: "",
          tooltip: "none",
          textalign: "left",
          name: "Nội dung",
          label: "Nội dung",
          nameValue: "description",
          styleCss: "min-width:600px !important;padding-left:15px",
        },
        {
          tooltipText: "",
          tooltip: "none",
          textalign: "center",
          name: "Chức năng",
          label: "Chức năng",
          replication: false,
          styleCss: "max-width:90px !important",
        },
      ],
      dataLabelAsset: [
        // { tooltipText:'',tooltip:'',textalign:'',name: "", label: "checkBox" , maxWidth:'maxWidth'},
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
          styleCss: "padding-right:20px",
        },
      ],
      urlTable: Resource.APIs.FixedAssetIncrementFilter,
      urlTableDataID: "",
      showDisplay: false,
      styleObject: {
        width: "calc(30% - 2px)",
        "margin-bottom": "15px",
      },
      isShowDialog: false,
      itemDetail: [],
      idAssetIncrement: "",
      showTable: true,
      labelDialog: "",
      handler: "",
      dataTicks: [],
      titleWarning: "",
      isShowDialogNotify: false,
      isRemove: false,
      buttonText: ["Xóa", "Không"],
      errorName: "",
      dataAssetDetail: [],
      domain: Resource.APIs.FixedAssetIncrementDelete,
      isShowDialogBudget: false,
      labelDialogBudget: "",
      heightMax: false,
      removeHandlerName: false,
      isShowSuccess: false,
      handlerName: "",
      masterSize: 70,
      searchArray: {
        keyword: "",
        pageSize: "20",
        pageNumber: "1",
        handlerName: "",
        errorName: "",
      },
      showTableIncrement: false,
      showRemove: false,
      styleImage: {
        height: "60px",
        "margin-top": "40px",
      },
      styleImageDetail: {
        height: "60px",
      },
      styleTable: {
        height: "calc(100% - 40px) !important",
      },
      horizontal: false,
      iconDisplay: "icon_horizontal",
    };
  },
  components: {
    Button,
    Input,
    Table,
    Splitpanes,
    Pane,
    Dialog,
    Notify,
    ToolTip,
  },
  mounted() {
    this.emitter.on("closeDialogBudget", () => {
      this.isShowDialogBudget = false;
    });
    this.emitter.on("handlerName", (item) => {
      this.showSuccess(item);
    });
    this.emitter.on("titleWarning", (item) => {
      //this.titleWarning = item;
      console.log(item);
    });
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
     *  Nhận dữ liệu lọc
     * Author : Bùi Quang Điệp
     * Date:10/08/2022
     */
    async handlerSearch(item) {
      try {
        debugger
        const maxLength = 50;
        if (event != null || event != undefined) {
          var keyword = event.currentTarget.value;
          if (keyword != undefined) {
            if (keyword.length > maxLength) {
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
              httpSearch + `pageNumber=${this.searchArray.pageNumber}`;
          }
        }

        this.urlTable = Resource.APIs.FixedAssetIncrementFilter + httpSearch;
        this.showTableIncrement = true;
      } catch (error) {
        console.log(error);
      }
    },
    /**
     *   Nhận data cần xóa
     * Author : Bùi Quang Điệp
     * Date:10/08/2022
     */
    dataDelete(item) {
      this.dataTicks = [];
      this.dataTicks.push(item);
      this.HandleRemove();
    },
    /**
     *   Nhận data tài sản cần sửa
     * Author : Bùi Quang Điệp
     * Date:10/08/2022
     */
    getItemAsset(item) {
      this.dataAssetDetail = item;
      this.dataAssetDetail.fixedAssetIncrementID = this.idAssetIncrement;
      // show form dialog chỉnh sửa
      this.isShowDialogBudget = true;
      this.labelDialogBudget = "Sửa tài sản " + `${item.fixedAssetName}`;
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
     * Xử lý sự kiện xóa
     * Author : Bùi Quang Điệp
     * Date:10/08/2022
     */
    HandleRemove() {
      try {
        this.titleWarning = [];
        this.errorName = "";
        this.isShowDialogNotify = true;
        var quantity = this.dataTicks.length;
        if (quantity == 0) {
          (this.buttonText = [Resource.Label.Agree]),
            this.titleWarning.push(Resource.Warning.NoData);
        } else if (quantity == 1) {
          (this.buttonText = [Resource.Label.Delete, Resource.Label.No]),
            this.titleWarning.push(Resource.Warning.DeleteIncrement);
          this.isShowNumber = false;
          this.errorName = this.dataTicks[0].fixedAssetIncrementCode + "?";
        } else {
          if (quantity < 10) {
            quantity = "0" + quantity;
          }
          (this.buttonText = [Resource.Label.Delete, Resource.Label.No]),
            (this.errorName = quantity);
          this.titleWarning.push(Resource.Warning.DeleteIncrements);
          this.isShowNumber = true;
        }
      } catch (error) {
        console.log(error);
      }
    },
    zoomDetail() {
      if (this.detailSize == 100) {
        this.masterSize = 70;
        this.detailSize = 30;
      } else {
        this.masterSize = 0;
        this.detailSize = 100;
      }
    },
    zoomMaster(display) {
      if (display == "horizontal") {
        this.iconDisplay = "icon_vertical";
        this.horizontal = true;
        this.masterSize = 70;
        this.detailSize = 30;
      } else {
        this.iconDisplay = "icon_horizontal";
        this.masterSize = 100;
        this.detailSize = 0;
      }
      this.showDisplay = false;
    },

    /**
     * Sự kiện show dialog
     * Author : Bùi Quang Điệp
     * Date :09 /08/ 2021
     */
    btnShowDialog() {
      try {
        // đặt lại input về rỗng
        this.itemDetail = [];
        this.handler = Resource.CommandType.Add;
        // Lấy mã tài sản mới nhất truyền vào input
        API.get(Resource.APIs.NewCodeIncrement)
          .then((res) => {
            this.labelDialog = Resource.TitleDialog.AddIncrement;
            console.log(res);
            this.itemDetail.fixedAssetIncrementCode = res.data;
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
     * Sự kiện nhận dữ liệu khi được click
     * Author : Bùi Quang Điệp
     * Date :09 /08/ 2021
     */
    assetIncrement(item) {
      this.dataTicks = item;
      if (this.dataTicks.length > 1) {
        this.showRemove = true;
      } else {
        this.showRemove = false;
      }
      if (this.dataTicks.length >= 1) {
        this.idAssetIncrement = item[0].fixedAssetIncrementID;
        this.urlTableDataID =
          Resource.APIs.FixedAssetIncrements +
          `AssetMulti?id=${this.idAssetIncrement}`;
        this.showTable = true;
      } else {
        this.idAssetIncrement = "00000000-0000-0000-0000-000000000000";
        this.urlTableDataID =
          Resource.APIs.FixedAssetIncrements +
          `AssetMulti?id=${this.idAssetIncrement}`;
        this.showTable = true;
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
  },
};
</script>

<style lang="css" scoped>
.container-asset {
  padding: 15px 15px 0 15px;
  background-color: #f4f7ff;
  border-top: 1px solid rgb(73 73 73 / 31%);
  height: calc(100% - 42px);
}
.header {
  background-color: #f4f7ff;

  padding: 0 0 15px 0 !important;
}
.title-header {
  font-weight: bold;
  font-size: 18px;
}
.group-button {
  align-items: center;
  display: flex;
  height: 100%;
}
.body {
  height: calc(100vh - 42px - 75px);
}

.body_table {
  width: 100%;
  height: 50%;
  background-color: #fff;
  position: relative;
  z-index: 1;
}
.bode_table--header {
  display: flex;
  padding: 15px 15px 0 15px;
}
.header_table--two {
  height: 40px;
  line-height: 40px;
  margin-left: 15px;
  font-size: 14px !important;
}

.group_icon {
  display: flex;
  position: absolute;
  right: 0px;
  align-items: center;
  width: 63px;
  height: 35px;
}
.group-display {
  position: absolute;
  right: 15px;
  top: 100px;
  z-index: 999;
  box-shadow: -2px 5px 5px -2px #ababab;
  font-size: 13px;
}
.display-item {
  height: 30px;
  width: 100%;
  background-color: #fff;
  padding: 10px;
  display: flex;
  align-items: center;
}
.display-item:hover {
  background-color: rgb(211 245 254);
}
</style>
