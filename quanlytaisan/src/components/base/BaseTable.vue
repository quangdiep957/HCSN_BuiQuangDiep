<template>
  <div
    class="m-row managetable"
    ref="managetable"
    :class="{ tableBody: classCustom == 'tableBody', tableWrite: classTable }"
    :style="styleTable"
  >
    <div class="table__nodata" v-if="isNoData">
      <div class="table__nodata--item">
        <img :style="styleImage" src="../../assets/icon/img-EmptyData-L.svg" />
      </div>
    </div>
    <table class="table" @click="this.$refs.menu.close">
      <thead>
        <tr>
          <th  :style="label.styleCss"
            @mousedown="startMove"
            tooltip--th
            v-for="label in dataLabel"
            :key="label"
            :id="label.maxWidth"
          >
            <div class="buttonHandler" :class="{'margin-right': label.label=='Chức năng'}">
              <button
              v-if="label.label != 'checkBox'"
              class="resize-width tooltip"
              :class="{
                'text-align-left': label.textalign == 'left',
                'text-align-right': label.textalign == 'right',
                'text-align-center': label.textalign == 'center',
              }"
              :id="label.maxWidth"
            >
              {{ label.label }}
              <Tooltip
                :tooltiptext="label.tooltipText"
                positiontooltip="top"
                style="margin-top: 10px"
                v-if="label.tooltip == 'block'"
              />
            </button>
            <button
              :id="label.maxWidth"
              v-if="label.label == 'checkBox'"
              class="resize-width"
            >
              <label class="checkbox"
                ><input
                  @click="isTicksAll()"
                  type="checkbox"
                  v-model="checked" /><span class="tick"></span
              ></label>
            </button>
            </div>
          </th>
        </tr>
      </thead>
      <tbody>
        <tr
          @contextmenu.prevent="
            this.unCheck = true;
            selectTick($event, item, index, false);
            conTextMenu($event);
          "
          v-for="(item, index) in Asset"
          :key="item.fixedAssetID"
          @click="selectTick($event, item, index, false)"
          :class="{
            'table-active': checkTick(item),
            'row-hover': checkHover(item),
          }"
          @dblclick="isShowDialogDetail(item)"
          tabindex="0"
          @keydown="pressKeyMove"
        >
          <td
            v-for="value in dataLabel"
            :key="value"
            :class="{
              'text-align-left': value.textalign == 'left',
              'text-align-right': value.textalign == 'right',
              'text-align-center': value.textalign == 'center',
            }"
            class="hover"
            :style="value.styleCss"
          >
            <label
              v-if="value.label == 'checkBox'"
              @click="selectTick($event, item, index, true)"
              class="checkbox"
              ><input :checked="checkTick(item)" type="checkbox" /><span
                class="tick"
              ></span
            ></label>
            <!-- nếu là cột check box -->
            <span v-else-if="value.label == 'STT'">{{ index + 1 }}</span>
            <!-- format giá tiền hay không -->
            <span
              v-else-if="
                value.format == false ||
                (value.format == undefined &&
                  value.formatDate == undefined &&
                  value.click == undefined &&
                  !value.formatStatus)
              "
              :class="{ tableColor: value.classColor }"
              >{{
                value.formatNumber == true
                  ? formatCash(item[value.nameValue])
                  : item[value.nameValue]
              }}</span
            >
            <span
              v-if="
                value.format == false ||
                (value.format == undefined &&
                  value.formatDate == undefined &&
                  value.click &&
                  !value.formatStatus)
              "
              @click="isShowDialogDetail(item)"
              :class="{ tableColor: value.classColor }"
              >{{
                value.formatNumber == true
                  ? formatCash(item[value.nameValue])
                  : item[value.nameValue]
              }}</span
            >

            <!-- định dạng theo ngày tháng -->
            <span v-else-if="value.formatDate">{{
              formatDate(item[value.nameValue], typeFormatDate)
            }}</span>
            <!-- Chuyển từ ID thành tên -->
            <span v-else-if="value.format">{{
              convertID(item[value.nameValue], value.table)
            }}</span>
            <!-- Tính tổng theo cột -->
            <span
              v-if="
                value.summaryValue != undefined && value.summaryValue == 'price'
              "
              >{{ summaryColumns(value.summaryValue) }}</span
            >
            <label style="margin-left: 25px" v-if="value.formatStatus"
              ><span
                class="icon_active"
                :class="{ icon_not_active: item[value.nameValue] == 0 }"
              ></span
              >{{
                (name =
                  item[value.nameValue] == 1
                    ? (name = "Đang sử dụng")
                    : (name = "Chưa ghi tăng"))
              }}</label
            >
            <div
              class="tablefunction display-flex"
              v-if="value.label == 'Chức năng'"
            >
              <div
                @click="isShowDialogDetail(item)"
                class="icon-mini icon-edit-mini icon__size-14 tooltip tooltipHandler"
                style="margin-left: 15px"
              >
                <Tooltip
                  tooltiptext="Sửa"
                  positiontooltip="top"
                  style="top: 18px; margin-left: 13px"
                  :class="{'tooltipUpdate': styleTooltip}"
                  :classTooltip="tooltipUpdate"
                />
              </div>
              <div
                @click="isShowDialogDetail(item, true)"
                v-if="value.replication != false"
                class="icon-mini icon-replication-mini icon__size-14 tooltip tooltipHandler tooltipHandlerEdit"
                style="margin-left: 10px"
              >
                <Tooltip
                  tooltiptext="Nhân bản"
                  positiontooltip="top"
                  style="margin-left: -35px !important; top: 18px"
                />
              </div>
              <div
                class="icon-mini icon-remove-mini icon__size-14 tooltip tooltipHandler tooltipHandlerRemove"
                @click="HandleRemoveDetail(item)"
                style="margin-left: 10px"
              >
                <Tooltip
                  tooltiptext="Xóa"
                  positiontooltip="top"
                  style="top: 18px; margin-left: -18px !important"
                  :class="{'tooltipDelete': styleTooltip}"
                  :classTooltip="tooltipDelete"
                />
              </div>
            </div>
          </td>
        </tr>
        <tr style="height: 0px; border: none"></tr>
        <tr class="page__table" v-show="isShowSummary">
          <td 
          :style="value.styleCss"
          style="padding:0 !important"
            class="text-align-right sum-quantity bold"
            v-for="(value, index) in dataLabel"
            :key="value"
          >
            <!-- Nhận summary từ backend trả về -->
            <div
              v-if="value.summary != undefined && value.summary != ''"
              class="border-paging"
              style="width: 100% !important"
            >
              {{ formatCash(dataSummary[value.summary]) }}
            </div>
            <!-- Nhận summary từ phía font-end -->
            <div
              v-else-if="
                value.summaryValue != undefined &&
                index < this.dataLabel.length - 1
              "
              class="border-paging"
              style="width: 100% !important"
            >
              {{ formatCash(summaryColum[value.summaryValue]) }}
            </div>
            <div
              v-else-if="
                value.summaryValue != undefined &&
                index == this.dataLabel.length - 1
              "
              class="border-paging"
              style="width: 100% !important ; padding-right: 20px"
            >
              {{ formatCash(summaryColum[value.summaryValue]) }}
            </div>
            <!-- Nhận summary từ phía font-end tính khi load-->
            <div
              v-else-if="
                value.summaryLoad != undefined &&
                index == this.dataLabel.length - 1
              "
              class="border-paging"
              style="width: 100% !important ; padding-right: 20px"
            >
              {{ formatCash(summaryPaging[value.summaryLoad]) }}
            </div>
            <div
              v-else-if="
                value.summaryLoad != undefined &&
                index < this.dataLabel.length - 1
              "
              class="border-paging"
              style="width: 100% !important"
            >
              {{ formatCash(summaryPaging[value.summaryLoad]) }}
            </div>
            <div
              v-else-if="index == this.dataLabel.length - 1"
              class="border-paging"
              style="width: 100% !important;margin-left: 1px"
            ></div>
            <div v-else class="border-paging" style="width: 100%"></div>
          </td>

          <!-- <td class="text-align-right sum-quantity bold">
                        <div class="border-paging">{{ formatCash(sumQuantity) }}</div>
                    </td>
                    <td class="text-align-right sum-price bold">
                        <div class="border-paging">{{ computedSumPrice }}</div>
                    </td>
                    <td class="text-align-right sum-Atrophy bold">
                        <div class="border-paging">{{ formatCash(sumDepreciation) }}</div>
                    </td>
                    <td class="text-align-right sum-price-rimaining bold">
                        <div class="border-paging">{{ formatCash(sumAtrophy) }}</div>
                    </td> -->
          <!-- <td>
                        <div class="border-paging" style=" width: 100%;"></div>
                    </td>
                    <td>
                        <div class="border-paging" style=" width: 100%;"></div>
                    </td> -->
        </tr>
      </tbody>

    </table>
    <ContextMenu
      ref="menu"
      @add="showDialog('add')"
      @edit="isShowDialogDetail(this.dataTicks[0])"
      @replication="isShowDialogDetail(this.dataTicks[0], true)"
      @remove="HandleRemoveDetail(this.dataTicks[0])"
      :replication="showReplication"
    >
    </ContextMenu>
  </div>
  <div
    class="display-flex paging"
    v-show="isShowPaging"
    :class="{ setWidthPage: isSetWidth, pagingWrite: classTable }"
  >
    <div class="display-flex paging_item">
      <p class="page__table--text">
        Tổng số : <span style="font-weight: bold">{{ sumItem }}</span> bản ghi
      </p>
      <Combobox ComboboxQuantity="true" @dataPageSize="getDataPageSize" />
      <div class="page__table--number display-flex">
        <div class="page--number-item">
          <div class="icon icon-back icon__size-8 tooltip" @click="BackPage">
            <Tooltip tooltiptext="Trang trước" positiontooltip="bottom" />
          </div>
        </div>
        <div
          v-for="item in numberPageArray"
          :key="item"
          class="page--number-item"
          @click="btnPageNumber(item)"
          :class="{ 'number-active': item == checkNumber(item) }"
        >
          {{ item }}
        </div>

        <div class="page--number-item">
          <div @click="NextPage" class="icon icon-next icon__size-8 tooltip">
            <Tooltip tooltiptext="Trang sau" positiontooltip="bottom" />
          </div>
        </div>
      </div>
    </div>
  </div>
  <Notify v-bind:dataError="titleWarning" v-if="isShowDialogNotify" />

  <div class="m-loading" v-show="isLoading">
    <div class="loader"></div>
  </div>
</template>

<script>
import { formatCash, formatDate } from "../../js/common.js";
import ContextMenu from "./ContextMenu/TheConTextMenu.vue";
import Notify from "./BaseDialogNotify.vue";
import Tooltip from "./BaseTooltip.vue";
import { API } from "@/js/callapi";
import Combobox from "./BaseComboBox.vue";
import Resource from "@/js/resource";
import Config from '@/js/config.js';
export default {
  name: "QuanlytaisanTheTable",

  components: {
    Tooltip,
    Combobox,
    ContextMenu,
    Notify,
  },

  data() {
    return {
      dataSelect: [],
      dataAssetDetail: [],
      isShowSummary: true,
      isShowPaging: true,
      tickCheckbox: true,
      isTickPage: 1,
      unCheck: false,
      isLoading: false,
      isNoData: false,
      isClickRow: "",
      Asset: [],
      sumQuantity: 0,
      sumPrice: 0,
      sumDepreciation: 0,
      sumAtrophy: 0,
      isShowContentMenu: false,
      show: false,
      dataTicks: [],
      IndexFirst: 0,
      IndexLast: 0,
      sumItem: 0,
      titleWarning: [],
      isShowDialogNotify: false,
      dataDepartment: [],
      dataCategory: [],
      search: "",
      assetCode: "",
      checked: false,
      recordNumber: "20",
      numberPage: "1",
      numberEntry: 0,
      numberPageArray: [],
      numberTwo: 2,
      isCheckNumber: 1,
      isSetWidth: false,
      tickedOld: false,
      replication: false,
      number: 1,
      indexRow: 0,
      isRowHover: [],
      backgroundColor: "rgba(26, 164, 200, .2)",
      width: "",
      draggable: false,
      startOffset: 0,

      dataWidth: [],
      dataSummary: [],
      summaryColum: {},
      countNumber: 0,
      typeFormatDate:'DMY',
      summaryPaging: {
        sumPrice: 0,
        sumDepreciation: 0,
        sumAtrophy: 0,
      },
    };
  },
  props: {
    originalTick:{
      type:Boolean,
      default:true
    },
    styleTooltip:{
      type:Boolean,
      default:false
    },
    styleTable: Object,
    showReplication: {
      Type: Boolean,
      default: true,
    },
    dataLabel: Array,
    urlTable: String,
    classTable: Boolean,
    colspan: Number,
    classCustom: String,
    dataAsset: Array,
    showTable: Boolean,
    styleImage: Object,
    summaryColumn: Boolean,
    changeData: {
      Type:Object,
      default:{}
    },
    moveData: {
      Type: Boolean,
      default: false,
    },
    canPaging: {
      Type: Boolean,
      default: true,
    },
    canSummary: {
      Type: Boolean,
      default: true,
    },
    dataEntry: {
      type: Boolean,
      default: false,
    },
  },
  computed: {
    /**
     *  format giá tiền
     * Author : Bùi Quang Điệp
     * Date:10/08/2022
     */
    computedSumPrice() {
      try {
        return formatCash(this.sumPrice);
      } catch (error) {
        console.log(error);
        return 0;
      }
    },
  },

  methods: {
    /**
     *
     */
    formatCash,
    formatDate,
     isEmptyObject(obj){
    return JSON.stringify(obj) === '{}'
},
    /**
     *  Lấy số trang được hiển thị
     * Author : Bùi Quang Điệp
     * Date:10/08/2022
     */
    getDataPageSize(item) {
      this.$emit("dataPageSize", item);
      this.recordNumber = item;
    },

    /**
     *  Truyền dữ liệu 1 hàng vào 1 mảng
     * Author : Bùi Quang Điệp
     * Date:10/08/2022
     */
    btnRowTable(item) {
      console.log(item);
    },

    /**
     *  Nhận dữ liệu và show dialog nhân bản
     * Author : Bùi Quang Điệp
     * Date:10/08/2022
     */
    isShowDialogDetail(item, replication) {
      if (item.fixedAssetID != undefined) {
        API.get(Resource.APIs.Asset + `${item.fixedAssetID}`)
          .then((res) => {
            console.log(res);

            if (replication == true) {
              this.emitter.emit("replication", res.data);
              replication = false;
            } else {
              this.emitter.emit("itemDialog", res.data);
              this.$emit("itemDialog", item);
              this.emitter.emit("handlerEdit");
            }
          })
          .catch((error) => {
            console.log(error);
          });
      } else {
        this.$emit("itemDialog", item);
      }
    },
    /**
     * Xử lý sự kiện nhấn phím ctrl thì chọn nhiều
     * Author : Bùi Quang Điệp
     * Date:10/08/2022
     * @param(e,item,index) e là sự kiện được click vào
     *            item là truyền row vừa click trên table
     *            index là số thứ tự của row đó trong table
     */
    selectTick(e, item, index, checkbox) {
      try {
        if (this.tickCheckbox == true) {
          e.preventDefault();
          // Kiểm tra xem có giữ phím ctrl hay không
          // Nếu có thì thêm vào mảng và không reset lại mảng
          // Ngược lại thì reset mảng
          if (e.ctrlKey == true) {
            var dataTicksFake = [];

            for (let i = 0; i < this.dataTicks.length; i++) {
              if (item == this.dataTicks[i]) {
                this.dataTicks.splice(i, 1);
                this.isRowHover = [];
                dataTicksFake = this.dataTicks;
              }
            }

            if (dataTicksFake.length == 0) {
              this.dataTicks.push(item);
            } else {
              this.dataTicks = dataTicksFake;
            }
          } else if (e.shiftKey == true) {
            /**
             * Xử lý sự kiện nhấn phím Shift  thì chọn hàng loạt đến vị trí click
             * Author : Bùi Quang Điệp
             * Date:10/08/2022
             * */
            e.preventDefault();
            this.IndexLast = index;
            var tg = "";
            // Kiểm tra vị trí nếu vị trí click bên trên thì đổi ngược lại vị trí index
            if (this.IndexLast < this.IndexFirst) {
              tg = this.IndexLast;
              this.IndexLast = this.IndexFirst;
              this.IndexFirst = tg;
            }
            // Gán số lượng được chọn về rỗng
            this.dataTicks = [];
            for (var i = this.IndexFirst; i <= this.IndexLast; i++) {
              this.dataTicks.push(this.Asset[i]);
            }
          } else {
            // Khi click vào input checkbox
            if (checkbox == true) {
              this.tickCheckbox = false;
              var checkAdd = true;
              for (let i = 0; i < this.dataTicks.length; i++) {
                if (this.dataTicks[i] == item) {
                  checkAdd = false;
                  this.dataTicks.splice(i, 1);
                }
              }
              if (checkAdd == true) this.dataTicks.push(item);
            } else {
              if (this.dataTicks[0] == item) {
                if (this.unCheck == true) {
                  this.unCheck = false;
                } else {
                  this.dataTicks = [];
                }
              }

              // Nếu không phải thì xóa tài sản trước đó và thêm mới tài sản vừa chọn
              else {
                if (checkbox == false) {
                  this.dataTicks = [];
                } else {
                  this.tickCheckbox = true;
                }
                this.dataTicks.push(item);
                this.IndexFirst = index;
              }
            }

            // Kiểm tra nếu số lượng tick hết = số lượng bản ghi đang hiển thị thì tick all
          }
          if (this.dataTicks.length == this.Asset.length) {
            this.checked = true;
          } else {
            this.checked = false;
          }

          this.indexRow = index + 1;
          this.isRowHover = "";
          this.emitter.emit("titleWarning", this.dataTicks);
          this.$emit("dataTick", this.dataTicks);
        } else {
          this.tickCheckbox = true;
        }
        // Kiểm tra nếu điều kiện khi chọn thì chuyển dữ liệu vào bộ nhớ tạm
        if (this.moveData) {
          this.emitter.emit("dataTick", this.dataTicks);
        }
      } catch (error) {
        console.log(error);
      }
    },
    /**
     * Kiểm tra xem row nào đã được tick
     * Author : Bùi Quang Điệp
     * Date:10/08/2022
     */
    checkTick(item) {
      try {
        for (const value of this.dataTicks) {
          if (item == value) {
            return item;
          }
        }
      } catch (error) {
        console.log(error);
      }
    },
    /**
     * Tick tất cả
     * Author : Bùi Quang Điệp
     * Date:10/08/2022
     */
    isTicksAll() {
      try {
        this.checked = !this.checked;
        if (this.checked == true) {
          this.dataTicks = [];
          for (const item of this.Asset) {
            this.dataTicks.push(item);
          }
          this.emitter.emit("titleWarning", this.dataTicks);
          this.$emit("dataTick", this.dataTicks);
        } else {
          this.dataTicks = [];
          this.emitter.emit("titleWarning", this.dataTicks);
          this.$emit("dataTick", this.dataTicks);
          return;
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

    HandleRemoveDetail(item) {
      try {
        if (this.dataTicks.length > 0) {
          this.dataTicks = [];
        }
        this.emitter.emit("deleteAsset", item);
        this.$emit("deleteAsset", item);
      } catch (error) {
        console.log(error);
      }
    },

    /**
     * Lấy tên bộ phận sử dụng
     * Author : Bùi Quang Điệp
     * Date:17/08/2022
     */
    nameDepartment(id) {
      try {
        for (const item of this.dataDepartment) {
          if (item.DepartmentID == id) {
            return item.DepartmentName;
          }
        }
      } catch (error) {
        console.log(error);
      }
    },

    /**
     * Tính số lượng trang
     * Author : Bùi Quang Điệp
     * Date:17/08/2022
     */
    NumberPage(sumItem) {
      try {
        if (sumItem != undefined) {
          this.numberPage = parseInt(sumItem / this.recordNumber);
          if (this.numberPage == 0) {
            this.numberPage = this.numberPage + 1;
          }
          if (sumItem - this.numberPage * this.recordNumber > 0) {
            this.numberPage = this.numberPage + 1;
          }
        }
      } catch (error) {
        console.log(error);
      }
    },

    /**
     * Chuyển trang tiếp theo
     * Author : Bùi Quang Điệp
     * Date:17/08/2022
     */
    NextPage() {
      try {
        this.number = this.number + 1;
        if (this.number >= this.numberPage) {
          this.number = 1;
        }

        this.showPaging();
        this.btnPageNumber(this.numberPageArray[this.isTickPage]);
      } catch (error) {
        console.log(error);
      }
    },

    /**
     * Hiển thị paging
     * Author : Bùi Quang Điệp
     * Date:17/08/2022
     */
    showPaging() {
      try {
        this.isTickPage = 0;
        this.numberPageArray = [];
        if (this.numberPage < 3) {
          for (let i = 1; i <= this.numberPage; i++) {
            this.numberPageArray.push(i);
          }
          this.isTickPage = 1;
        } else {
          if (this.number >= 2) {
            this.isTickPage = 2;
            this.numberPageArray.push("1");
            this.numberPageArray.push("...");
          }
          for (let i = this.number; i < this.number + 2; i++) {
            if (i < this.number + 2) {
              this.numberPageArray.push(i);
            }
          }
          //  Những số k hiển thị để thành dấu ...
          if (this.number + 1 < this.numberPage) {
            this.numberPageArray.push("...");
            // lấy số sau gần nhất
            for (let i = 1; i <= this.numberPage; i++) {
              if (i > this.numberPage - 1) {
                this.numberPageArray.push(i);
              }
            }
          } else {
            this.numberPageArray.push("...");
          }
        }
      } catch (error) {
        console.log(error);
      }
    },

    /**
     * Chuyển trang về phái trước
     * Author : Bùi Quang Điệp
     * Date:17/08/2022
     */
    BackPage() {
      try {
        this.number = this.number - 1;
        if (this.number < 1) {
          this.number = 1;
        }

        this.showPaging();
        this.btnPageNumber(this.numberPageArray[this.isTickPage]);
      } catch (error) {
        console.log(error);
      }
    },

    /**
     * chọn số trang muốn hiển thị
     * Author : Bùi Quang Điệp
     * Date:17/08/2022
     */
    btnPageNumber(item) {
      try {
        this.isCheckNumber = item;
        // truyền số trang muốn hiển thị
        //this.emitter.emit("pageNumber", item);
        this.$emit("pageNumber", item);
      } catch (error) {
        console.log(error);
      }
    },

    /**
     * Lấy tên loại tài sản
     * Author : Bùi Quang Điệp
     * Date:17/08/2022
     */
    convertID(id, name) {
      try {
        if (name == "department") {
          for (const item of this.dataDepartment) {
            if (item.DepartmentID == id) {
              return item.DepartmentName;
            }
          }
        } else if (name == "category") {
          for (const item of this.dataCategory) {
            if (item.FixedAssetCategoryID == id) {
              return item.FixedAssetCategoryName;
            }
          }
        }
      } catch (error) {
        console.log(error);
      }
    },

    /*
     * Hàm lấy dữ liệu đưa vào table
     * Author : Bùi Quang Điệp
     * Date : 14/08/2022
     */

    getDataTable() {
      try {
        this.summaryColum = [];
        this.countNumber = 0;
        this.isLoading = true;
        var domain = this.urlTable;
        if (domain == "") {
          // Thông báo lỗi
          this.isNoData = false;
          this.isLoading = false;
        } else {
          if (this.search != "") {
            domain = domain + this.search;
          }
          API.get(domain)
            .then((response) => {
              this.Asset =
                response.data.data != undefined
                  ? response.data.data
                  : response.data;
              console.log(response);
              this.dataSummary = response.data.summary;
              this.sumItem = response.data.totalCount;
              if (response.data.totalCount == undefined) {
                this.sumItem = this.Asset.length;
              }
              this.NumberPage(this.sumItem);
              this.showPaging();
              // focus dòng đầu tiên
              if(this.originalTick)
              {
                this.selectTick(event, this.Asset[0], 0, false);
              }
              this.isLoading = false;
              this.checked = false;
              if (this.dataEntry) {
                this.$emit("sendDataEntry", this.Asset);
              }
              this.$emit("sendData", this.Asset);
              if (this.Asset.length == 0) {
                this.isNoData = true;
                this.summaryPaging = {
                  sumPrice: 0,
                  sumDepreciation: 0,
                  sumAtrophy: 0,
                };
              } else {
                this.isNoData = false;
              }
              this.sumColumnsTable();
            })
            .catch((error) => {
              this.isLoading = false;
              this.isNoData = true;
              this.Asset = [];
              this.summaryPaging = {
                sumPrice: 0,
                sumDepreciation: 0,
                sumAtrophy: 0,
              };
              console.log(error);
            });
        }
      } catch (error) {
        console.log(error);
      }
    },

    /*
     *tính tổng các trường trong table
     * Author : Bùi Quang Điệp
     * Date : 09/08/2022
     */
    sumColumnsTable() {
      // set lại các biến tổng về 0
      this.sumQuantity = 0;
      this.sumPrice = 0;
      this.sumDepreciation = 0;
      this.sumAtrophy = 0;
      this.sumPriceIncrement = 0;
      var yearNow = new Date().getFullYear();
      // sử dụng vòng lặp để tính tổng
      const fixed = 0;
      for (const item of this.Asset) {
        this.sumQuantity = this.sumQuantity + item.quantity;
        this.sumPrice = Number(this.sumPrice) + Number(item.cost);
        var yearproductionDate = new Date(item.productionDate).getFullYear();
        var yearUse = yearNow - yearproductionDate;
        if (yearUse == 0) {
          yearUse = 1;
        }
        item.depreciationValue = (
          item.cost *
          ((item.depreciationRate * yearUse) / 100)
        ).toFixed(fixed);
        this.sumDepreciation =
          this.sumDepreciation + Number(item.depreciationValue);
        item.residualValue = item.cost - item.depreciationValue;
        this.sumAtrophy = this.sumAtrophy + item.residualValue;
        if (item.price != undefined && item.cost == undefined) {
          this.sumPriceIncrement = this.sumPriceIncrement + item.price;
        }
      }
      this.summaryPaging = {
        sumQuantity: this.sumQuantity,
        sumPrice: this.sumPrice,
        sumDepreciation: this.sumDepreciation,
        sumAtrophy: this.sumAtrophy,
        sumPriceIncrement: this.sumPriceIncrement,
      };
    },

    /*
     * Hàm xử lý tính tổng
     * Author : Bùi Quang Điệp
     * Date : 10/09/2022
     */
    summaryColumns(name) {
      // Kiểm tra chạy đủ dòng thì dừng
      if (this.countNumber == 0) {
        // Kiểm tra nếu chưa khai báo thì mặc định là 0
        if (this.summaryColum[name] == undefined) {
          this.summaryColum[name] = 0;
        }
        for (let i = 0; i < this.Asset.length; i++) {
          this.summaryColum[name] =
            this.summaryColum[name] + this.Asset[i][name];
        }
        this.countNumber++;
      }
    },

    /*
     *   hàm xử lý chức năng di chuyển lên xuống 1 dòng table
     * Author : Bùi Quang Điệp
     * Date : 10/09/2022
     */
    moveRowTable() {
      try {
        // nhận giá trị index đang đứng hiện tại
        // this.indexRow
        var item = this.Asset[this.indexRow];
        this.isRowHover = item;
      } catch (error) {
        console.log(error);
      }
    },

    /*
     *   hàm xử lý sự kiện ấn phím di chuyển lên xuống
     * Author : Bùi Quang Điệp
     * Date : 10/09/2022
     */
    pressKeyMove() {
      try {
        var isShowHover = false;
        event.preventDefault();
        if (event.code == "ArrowUp") {
          if (this.indexRow == 0) {
            this.indexRow = this.Asset.length;
            this.$refs["managetable"].scrollTop =
              this.$refs["managetable"].scrollHeight;
          } else if (this.indexRow < 10) {
            this.$refs["managetable"].scrollTop = 0;
          }
          this.indexRow = this.indexRow - 1;
          isShowHover = true;
        }
        if (event.code == "ArrowDown") {
          if (this.indexRow == this.Asset.length) {
            this.indexRow = 0;
            this.$refs["managetable"].scrollTop = 0;
          } else if (this.indexRow >= 10) {
            this.$refs["managetable"].scrollTop =
              this.$refs["managetable"].scrollTop + 400;
          }

          this.indexRow = this.indexRow + 1;
          isShowHover = true;
        }

        if (event.code == "Enter") {
          this.selectTick(
            event,
            this.Asset[this.indexRow - 1],
            this.indexRow - 1,
            false
          );

          this.backgroundColor = "rgba(26, 164, 200, .2)";
        }

        if (isShowHover == true) {
          isShowHover = false;
          this.isRowHover = this.Asset[this.indexRow - 1];
        }

        console.log(event);
      } catch (error) {
        console.log(error);
      }
    },

    /*
     *  hàm xử lý check row được hover
     * Author : Bùi Quang Điệp
     * Date : 09/08/2022
     */
    checkHover(item) {
      try {
        if (item == this.isRowHover) {
          this.backgroundColor = "#fff";
          return item;
        }
      } catch (error) {
        console.log(error);
      }
    },

    /*
     *  hàm xử lý chọn number page
     * Author : Bùi Quang Điệp
     * Date : 09/08/2022
     */
    checkNumber(item) {
      try {
        if (item == this.isCheckNumber) {
          return item;
        }
      } catch (error) {
        console.log(error);
      }
    },
    /*
     *  hàm xử lý tính lại width
     * Author : Bùi Quang Điệp
     * Date : 11/09/2022
     */
    move(e) {
      try {
        if (this.draggable) {
          // tính lại width
          this.width = this.startOffset + e.pageX;
          var column = e.srcElement;
          for (let i = 0; i < this.dataWidth.length; i++) {
            if (this.dataWidth[i].name == column) {
              this.dataWidth[i].width = this.width;
            }
          }
          this.setWidthTable(column);
        }
      } catch (error) {
        console.log(error);
      }
    },

    /*
     *  hàm xử lý khi sự kiện mousedown được kích hoạt
     * Author : Bùi Quang Điệp
     * Date : 11/09/2022
     */
    // startMove(e) {
    //   this.dataWidth = [];
    //   try {
    //     var check = true;
    //     // lấy width hiện tại của column được chọn
    //     this.width = e.currentTarget.clientWidth;
    //     this.draggable = true;
    //     // lấy giá trị chênh lệch width
    //     this.startOffset = this.width - e.pageX;
    //     // lấy comlumn được chọn
    //     var colums = e.srcElement;
    //     // gán vào 1 object thông tin của column đó và width ban đầu
    //     var data = {
    //       name: colums,
    //       width: this.width,
    //     };
    //     if (this.dataWidth.length < 1) {
    //       this.dataWidth.push(data);
    //     } else {
    //       for (let i = 0; i < this.dataWidth.length; i++) {
    //         if (this.dataWidth[i].name == colums) {
    //           this.dataWidth[i].width = this.width;
    //           check = true;
    //           return;
    //         } else {
    //           check = false;
    //         }
    //       }

    //       if (check == false) {
    //         this.dataWidth.push(data);
    //       }
    //     }
    //   } catch (error) {
    //     console.log(error);
    //   }
    // },

    /*
     *  hàm xử lý sự kiện dừng chuột
     * Author : Bùi Quang Điệp
     * Date : 11/09/2022
     */
    stopMove() {
      try {
        this.draggable = false;
      } catch (error) {
        console.log(error);
      }
    },

    /*
     *  Set lại width của column được chọn khi có sự thay đổi
     * Author : Bùi Quang Điệp
     * Date : 11/09/2022
     */
    setWidthTable(column) {
      try {
        if (this.dataWidth.length > 0) {
          for (let i = 0; i <= this.dataWidth.length; i++) {
            if (this.dataWidth[i].name == column) {
              this.dataWidth[i].name.parentElement.style.width =
                this.dataWidth[i].width + "px";
            }
          }
        }
      } catch (error) {
        console.log(error);
      }
    },

    /*
     *  hàm xử lý mở context menu
     * Author : Bùi Quang Điệp
     * Date : 16/09/2022
     */
    conTextMenu(event) {
      this.$refs["menu"].open(event);
    },

    /*
     * Gọi Hàm xử lý sự click contextmenu
     * Author : Bùi Quang Điệp
     * Date : 14/08/2022
     */
    showDialog() {
      this.$emit("showDialog");
      this.emitter.emit("showDialog");
    },

    /*
     * Lấy dữ liệu từ chuỗi json
     * Author : Bùi Quang Điệp
     * Date : 14/08/2022
     */
    //  getJson(id)
    // {
    //     try {
    //     API.get(Resource.APIs.FixedAssetIncrements + `AssetMulti?id=${id}`)
    //     .then(res=>{
    //
    //         this.dataAssetDetail = res.data;

    //     })
    //     .catch(error=>{
    //         console.log(error);
    //     })
    //     if( this.dataAssetDetail.length >0)
    //     {
    //         for (let i=0;i<this.dataAssetDetail.length;i++)
    //         {
    //         var summary=0;
    //          summary = summary + this.dataAssetDetail[i].cost;
    //         }
    //         return  summary;
    //     }
    //     } catch (error) {
    //      console.log(error);
    //     }

    // }
  },

  created() {
    if (this.urlTable == "") {
      this.isNoData = true;
    }
    // kiểm tra xem có show paging hay không
    if (this.canPaging != undefined) {
      if (!this.canPaging) {
        this.isShowPaging = this.canPaging;
      }
    }
    if (this.canSummary != undefined) {
      if (!this.canSummary) {
        this.isShowSummary = this.canSummary;
      }
    }
    /*
     * Gọi Hàm lấy dữ liệu đưa vào table
     * Author : Bùi Quang Điệp
     * Date : 14/08/2022
     */
    if (this.dataAsset != undefined) {
      this.summaryColum = {};
      this.Asset = this.dataAsset;
      this.sumItem = this.Asset.length;
      this.NumberPage(this.sumItem);
    } else this.getDataTable();

    // Lấy thông tin bộ phận sử dụng từ API
    try {
      API.get(Resource.APIs.Departments)
        .then((res) => {
          console.log(res);
          this.dataDepartment = res.data;
        })
        .catch((res) => {
          console.log(res);
        });
    } catch (error) {
      console.log(error);
    }

    // lấy thông tin loại tài sản từ API
    try {
      API.get(Resource.APIs.FixedAssetCategorys)
        .then((res) => {
          console.log(res);
          this.dataCategory = res.data;
        })
        .catch((res) => {
          console.log(res);
        });
    } catch (error) {
      console.log(error);
    }
    this.showPaging();

    this.typeFormatDate = Config.FormatDate.Value;
  },

  /*
   * kích hoạt khi bỏ clik chuột
   * Author : Bùi Quang Điệp
   * Date : 14/08/2022
   */
  unmounted() {
    try {
      document.removeEventListener("mouseup", this.stopMove);
    } catch (error) {
      console.log(error);
    }

    // document.removeEventListener('mousemove', this.stopMove);
  },
  beforeUpdate() {
    if (this.dataAsset != undefined) {
      this.Asset = this.dataAsset;
      this.sumItem = this.Asset.length;
      this.NumberPage(this.sumItem);
    }
  },
  updated() {
    if (this.urlTable != "" && this.showTable) {
      this.$emit("hideTable");
      this.getDataTable();
    }

    // Khi có sự thay đổi thì cập nhật lại giá tiền
    if (this.summaryColumn) {
      this.$emit("summaryColumn");
      this.sumColumnsTable();
    }

      /**
     *  Cập nhật lại giá tiền và nguồn hình thành khi được thay đổi
     * Author : Bùi Quang Điệp
     * Date:10/08/2022
     */
      if (!this.isEmptyObject(this.changeData)) {
        for (const asset of this.Asset) {
          if (asset.fixedAssetID == this.changeData.fixedAssetID) {
              asset.cost = this.changeData.sumCost;
              asset.price = this.changeData.dataSource;
          }
        }
        this.$emit("changeData",this.Asset);
        this.sumColumnsTable();
      }
  },

  /*
   * kích hoạt khi click chuột
   * Author : Bùi Quang Điệp
   * Date : 14/08/2022
   */
  mounted() {
    try {
      document.addEventListener("mouseup", this.stopMove);
      document.addEventListener("mousemove", this.move);

      // Nhận lời gọi loading lại data từ dialog
      this.emitter.on("LoadData", (assetCode) => {
        // Gọi hàm load data
        this.titleWarning = [];
        this.dataTicks = [];
        this.assetCode = "";
        this.assetCode = assetCode;
        this.getDataTable();
      }),
        // this.emitter.on("search", (search) => {
        //
        //     this.search = search;
        //     // Gọi hàm load data
        //     this.getDataTable();
        // }),

        // Nhận số lượng bản ghi hiển thị

        // Nhận dữ liệu set lại width navbar
        this.emitter.on("setWidth", () => {
          // gán giá trị width lại cho body
          this.isSetWidth = true;
        }),
        // Nhận dữ liệu reset width navbar
        this.emitter.on("setWidthClear", () => {
          // gán giá trị width lại cho body
          this.isSetWidth = false;
        }),
        // Nhận dữ liệu tickAll
        this.emitter.on("tickAll", () => {
          this.isTicksAll();
        });
      // Nhận lệnh xóa tất cả
      this.emitter.on("removeAll", () => {
        this.Asset = [];
      }),
        this.emitter.on("switchName", (value) => {
          if (value.name == "summary") {
            if (value.item) {
              this.isShowSummary = true;
            } else {
              this.isShowSummary = false;
            }
          } else if (value.name == "paging") {
            if (value.item) {
              this.isShowPaging = true;
            } else {
              this.isShowPaging = false;
            }
          }
        });
    } catch (error) {
      console.log(error);
    }
  },
};
</script>

<style lang="css" scoped>
.row-hover {
  background-color: rgba(26, 164, 200, 0.2) !important;
}

tbody tr:hover {
  background-color: v-bind("backgroundColor");
}
#maxWidth {
  width: 39px !important;
}
#maxWidthN {
  width: 40px !important;
}
.tableColor {
  color: blue;
}
.icon_active {
  border-radius: 50%;
  background-color: #1aa4c8;
  width: 6px;
  height: 6px;
  margin-left: -20px;
  margin-top: 3px;
  position: absolute;
  margin-right: 0 !important;
}
.icon_not_active {
  background-color: #ffae04 !important;
}
.buttonHandler{
  height: 100%;
    box-sizing: border-box;
    margin: 0px;
    padding: 0px;
    border-left: none;
    position: relative;
}
.tooltipUpdate{
  top: -5px !important;
    left: -20px !important;
}
.tooltipDelete{
  top: -5px !important;
    left: 25px !important;
}
.margin-right{
  margin-right: -1px;
  top: -1px;
    border-top: 1px solid #cdd3d6;
    border-bottom: 3px solid #f5f5f5;
}
</style>
