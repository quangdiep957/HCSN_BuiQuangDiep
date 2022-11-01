<template>
  <!-- combobox -->

  <div class="combobox">
    <div
      v-on:click="btnClickShow()"
      class="combobox__control cbtooltip"
      :class="{ combobox__width: dialog_icon }"
      v-if="!ComboboxQuantity"
    >
      <div
        v-show="icon"
        class="combobox__icon--left icon icon-filter icon__size-14 tooltip"
      >
        <Tooltip
          tooltiptext="Lọc dữ liệu"
          positiontooltip="bottom"
          style="margin-top: 10px"
        />
      </div>
      <input
        type="text"
        name=""
        ref="inputControl"
        id=""
        class="input combobox__input"
        v-model="value_input"
        v-bind:placeholder="val"
        :tabindex="tabindex"
        @keydown="focusCombobox"
        @input="searchComboBox"
        :class="{ 'border-red': borderRed }"
      />
      <div
        v-if="checkRemove == false"
        class="combobox__icon--right icon icon-down1 icon__size-5 tooltip"
        @mouseover="isShowTooltipOption = true"
        @mouseleave="isShowTooltipOption = false"
      >
        <Tooltip
          tooltiptext="hiển thị"
          positiontooltip="bottom"
          style="margin-top: 14px"
          v-if="isShowTooltipOption"
        />
        <!-- vị trí tooltip -->
        <div id="TooltipComBoBox" v-show="isShowTooltip">
          <Tooltip
            :tooltiptext="optionText"
            positiontooltip="bottom"
            id="tooltipText"
          />
        </div>
      </div>
      <div
        v-if="checkRemove && isShowRemove"
        class="combobox__icon--right tooltip"
        @mouseover="isShowTooltipOption = true"
        @mouseleave="isShowTooltipOption = false"
      >
        <Tooltip
          tooltiptext="Bỏ chọn"
          positiontooltip="bottom"
          v-if="isShowTooltipOption"
        />
        <!-- vị trí tooltip -->
        <div
          class="remove--option tooltip"
          @click="removeTick"
          style="position: absolute; right: 6px; top: 7px"
        >
          <i style="margin: 3px 5.5px" class="fa-duotone fa-xmark"></i>
        </div>
      </div>
    </div>
    <div
      v-show="isShow"
      class="combobox__option"
      v-if="optionTable == false"
      v-clickoutside="hideListData"
      :class="{
        classComboboxCustom: classComboboxCustom == 'classComboboxCustom',
      }"
    >
      <div
        v-for="(item, index) in option"
        :key="item"
        class="combobox__option--item tooltip tooltipCombobox"
        @click="btnTickedBody(item)"
        ref="item"
        @mouseover="getPos(item, index)"
        @mouseleave="removePos()"
        :class="{ 'combobox__option--active': isTicked == item }"
        style="position: relative"
      >
        <span v-if="isTicked == item" class="combobox__option--item-icon"
          ><i class="fa-solid fa-check"></i
        ></span>
        <div class="combobox__option--item-text">
          <div ref="item-text" class="option--item-text">{{ item[name] }}</div>
        </div>
      </div>
    </div>
    <!-- combobox phân trang   -->
    <div
      class="combobox__control combox--number"
      v-if="ComboboxQuantity"
      v-on:click="isShowCombo = true"
    >
      <input
        type="text"
        name=""
        id=""
        class="input combobox__input--number"
        v-bind:value="value_quantity"
      />
      <div
        class="combobox__icon--right icon icon-down1 icon__size-5 tooltip"
        title="chọn để chọn các thành phần"
      ></div>
    </div>
    <div
      v-show="isShowCombo"
      class="combobox__option combobox_quantity"
      v-if="optionTable == false"
      @mouseleave="isShowCombo = false"
    >
      <div
        v-for="item in combobox_quantity"
        :key="item"
        class="combobox__option--item"
        v-on:click="btnTickedQuantity(item)"
        :class="{ 'combobox__option--active': isTicked == item }"
      >
        <div
          class="combobox__option--item-text"
          style="padding: 0; margin-left: 40%"
        >
          {{ item }}
        </div>
      </div>
    </div>

    <!-- option combobox table  -->

    <div
      v-show="isShow"
      class="combobox__option remove__Combobox"
      v-if="optionTable"
      v-clickoutside="hideListData"
      @keydown="pressKeyMove"
      ref="option"
      tabindex="0"
    >
      <div class="combobox__option--table" ref="combobox__option--table">
        <table style="width: 100%" class="table">
          <thead class="combobox__table--header">
            <tr>
              <th class="text-align-center" style="width: 60px">Mã</th>
              <th class="text-align-left" style="width: calc(100% - 60px)">
                Tên
              </th>
            </tr>
          </thead>
          <tbody>
            <tr
              @focus="focusRowTable"
              v-for="(item, index) in option"
              :key="item"
              :class="{
                'combobox__option--active': isTicked == item,
                'row-hover': checkHover(item),
              }"
              ref="rowTable"
              @click="btnTicked(item, index)"
            >
              <td class="text-align-center idValue">{{ item[code] }}</td>
              <td class="text-align-left NameValue">{{ item[name] }}</td>
            </tr>
            <tr style="height: 0px; border: none"></tr>
          </tbody>
        </table>
      </div>
      <div class="m-loading" v-show="isLoading">
        <div class="loader"></div>
      </div>
    </div>
  </div>
</template>
<script>
/**
 * Gán sự kiện nhấn click chuột ra ngoài combobox data (ẩn data list đi)
 * Bùi Quang Điệp (27/08/2022)
 */
const clickoutside = {
  mounted(el, binding) {
    try {
      el.clickOutsideEvent = (event) => {
        // Nếu element hiện tại không phải là element đang click vào
        // Hoặc element đang click vào không phải là button trong combobox hiện tại thì ẩn đi.
        if (el.previousElementSibling != null) {
          if (
            !(
              el === event.target || // click phạm vi của combobox__data
              el.contains(event.target) || //click vào element con của combobox__data
              el.previousElementSibling.contains(event.target)
            )
          ) {
            // Thực hiện sự kiện tùy chỉnh:
            binding.value(event, el);
          }
        }
      };
    } catch (error) {
      console.log(error);
    }

    document.body.addEventListener("click", el.clickOutsideEvent);
  },
  beforeUnmount: (el) => {
    console.log("beforeUnmount");
    document.body.removeEventListener("click", el.clickOutsideEvent);
  },
};
// import axios from "axios"
import Tooltip from "./BaseTooltip.vue";
import Resource from "@/js/resource";
import { API } from "@/js/callApi";
export default {
  name: "ComboBox",
  components: {
    Tooltip,
  },
  directives: {
    clickoutside,
  },

  data() {
    return {
      isLoading: false,
      checkRemove: false,
      name: "",
      idOld: "",
      isShowTooltipOption: false,
      id: "",
      code: "",
      left: "0px",
      top: "0px",
      opacity: 0,
      isShow: false,
      isTicked: "",
      isShowCombo: false,
      value_input: "",
      value_quantity: "20",
      combobox_quantity: [10, 20, 30, 40, 50],
      optionText: "",
      isShowTooltip: false,
      handlerCombobox: true,
      handlerOne: false,
      handlerTwo: false,
      isShowHover: false,
      itemDetail: [],
      indexRow: 0,
      checkFocus: true,
      isRowHover: [],
      isFocus: true,
      isShowOption: true,
      searchArray: {
        keyword: "",
        categoryAssetID: "",
        departmentID: "",
      },
      dataPage: "",
      option: [],
      optionReserve: [],
    };
  },
  props: {
    icon: {
      type: Boolean,
    },
    val: {
      type: String,
    },
    // option: {
    //   type: Array,
    // },
    dialog_icon: {
      type: Boolean,
    },
    optionTable: {
      type: Boolean,
      default: false,
    },
    ComboboxQuantity: {
      type: Boolean,
      default: false,
    },
    nameTable: {
      type: String,
    },
    dataOption: {
      type: Array,
    },
    isShowInDialog: {
      type: Boolean,
    },
    dataItemCombobox: {
      type: Array,
    },
    isCheck: {
      type: Boolean,
    },
    borderRed: {
      type: Boolean,
    },
    isShowRemove: {
      type: Boolean,
      default: true,
    },
    tabindex: String,
    urlAPI: String,
    classComboboxCustom: String,
    position: String,
  },

  methods: {
    /*
     *  hàm xử lý tìm kiếm theo tên hoặc mã
     * Author : Bùi Quang Điệp
     * Date : 09/08/2022
     */
    searchComboBox() {
      try {
        this.option = [];
        var keyword = this.value_input;
        if (keyword == "") {
          this.option = this.optionReserve;
        } else {
          var domain = Resource.APIs.FixedAssetCategorySearch + keyword;
          if (this.nameTable == "Department") {
            domain = Resource.APIs.DepartmentSearch + keyword;
          }
          this.isLoading = true;
          API.get(domain)
            .then((res) => {
              console.log(res);
              this.option = res.data;
              this.$emit("updateData", res);
              this.isLoading = false;
            })
            .catch((res) => {
              this.isLoading = false;
              console.log(res);
            });
        }
      } catch (error) {
        console.log(error);
      }
    },

    /**
     * click hiển thị option Combobox
     * Author : Bùi Quang Điệp
     * Date:10/08/2022
     * */
    btnClickShow() {
      try {
        this.isShow = true;
      } catch (error) {
        console.log(error);
      }
    },

    /**
     * click ra ngoài thì ẳn combobox
     * Author : Bùi Quang Điệp
     * Date:10/08/2022
     * */
    hideListData() {
      try {
        this.isShow = false;
      } catch (error) {
        console.log(error);
      }
    },

    /**
     * Khi chọn 1 hàng thì focus vào hàng đó
     * Author : Bùi Quang Điệp
     * Date:10/08/2022
     * */
    btnTicked(item, index) {
      try {
        this.isTicked = item;
        if (this.isTicked != []) {
          this.isShowOption = true;
        }
        if (this.isShowOption == true) {
          this.isShow = false;
        }

        // Kiểm tra xem combobox đang hiển thị bảng nào
        if (this.position != undefined) {
          this.value_input = item[this.name];
        } else {
          this.value_input = item[this.code];
        }

        this.$emit("checkRequired", true, this.position);
        if (this.isCheck == true) {
          this.$emit("removeData");
          this.$emit("checkFalse");
        }
        this.$emit("dataName", item);

        this.indexRow = index + 1;
        this.isRowHover = "";
        this.isShowOption = true;

        if (this.checkFocus == true) {
          this.$refs.inputControl.focus();
        }
      } catch (error) {
        console.log(error);
      }
    },

    /**
     * click hiển thị option Combobox của combobox ngoài giao diện chính
     * Author : Bùi Quang Điệp
     * Date:10/08/2022
     * */
    btnTickedBody(item) {
      try {
        if (this.isTicked == item) {
          // Gán giá trị khi xóa ID
          this.idOld = item[this.id];
          // xóa trường hợp nguồn tiền
          this.$emit("removeBudget", this.position);
          this.isTicked[this.id] = "";
          this.value_input = "";
          this.isShow = false;
          this.$emit("remove", this.isTicked);

          if (this.isShowRemove == true) {
            this.checkRemove = false;
          }
          // Kiểm tra xem ID có bị Xóa không nếu có thì Gán lại giá trị của ID đó
          if (this.idOld != "" && this.isTicked[this.id] == "") {
            item[this.id] = this.idOld;
            this.idOld = "";
          }

          this.isTicked = "";
        } else {
          if (this.isShowRemove) {
            this.checkRemove = true;
          }

          this.isTicked = item;
          this.isShow = false;

          this.value_input = item[this.name];
          if (this.position != undefined) {
            this.$emit("dataComBoBoxSearch", item, this.position);
          } else {
            this.$emit("dataComBoBoxSearch", item);
          }
        }
      } catch (error) {
        console.log(error);
      }
    },

    /**
     * click hiển thị option Combobox của combobox chọn số lượng bản ghi
     * Author : Bùi Quang Điệp
     * Date:10/08/2022
     * */
    btnTickedQuantity(item) {
      try {
        this.isTicked = item;
        this.isShowCombo = false;
        this.value_quantity = item;
        this.dataPage = item;
        this.$emit("dataPageSize", this.dataPage);
        this.dataPage = "";
      } catch (error) {
        console.log(error);
      }
    },

    /**
     * lấy vị trí hiện tại của combobox Option
     * Author : Bùi Quang Điệp
     * Date:10/08/2022
     * */
    async getPos(item, index) {
      try {
        this.opacity = 1;
        //const left = this.$refs.item[index].getBoundingClientRect().left;
        const top = this.$refs.item[index].getBoundingClientRect().top;
        // Gán giá trị tooltip
        // gán độ dài max
        var width = await this.$refs["item-text"][index].offsetWidth;
        var maxWidth = (await this.$refs["item"][index].offsetWidth) - 45;
        if (item.FixedAssetCategoryName != undefined) {
          if (maxWidth <= width) {
            this.isShowTooltip = true;
            this.optionText = item.FixedAssetCategoryName;
          } else {
            width = 0;
            this.isShowTooltip = false;
          }
        } else {
          if (width >= maxWidth) {
            this.isShowTooltip = true;
            this.optionText = item.DepartmentName;
          } else {
            width = 0;
            this.isShowTooltip = false;
          }
        }

        // Gán vị trí của tooltip combobox
        if (this.optionText != "") {
          this.left = 30 + "px";
        }

        this.top = top - 60 + "px";
      } catch (error) {
        console.log(error);
      }
    },

    /**
     * Khi không hover thì đóng tooltip và xóa text cũ
     * Author : Bùi Quang Điệp
     * Date:10/08/2022
     * */
    removePos() {
      try {
        this.isShowTooltip = false;
        this.optionText = "";
      } catch (error) {
        console.log(error);
      }
    },

    /*
     *
     * xử lý sự kiện focus combobox thì sẽ hiện thị combobox option và chọn phần tử đầu tiên
     * Author : Bùi Quang Điệp
     * Date:10/08/2022
     * */
    focusCombobox() {
      try {
        if (event.code == "ArrowDown") {
          this.isShowOption = false;
          this.isShow = true;
          this.$refs["option"].focus();
        }
        //this.searchComboBox();
      } catch (error) {
        console.log(error);
      }
    },

    /*
     *
     * xử lý sự kiện ấn phím lên xuống di chuyển combobox option
     * Author : Bùi Quang Điệp
     * Date:10/08/2022
     * */
    pressKeyMove() {
      try {
        var isShowHover = false;
        event.preventDefault();
        if (event.code == "ArrowUp") {
          if (this.indexRow == 0) {
            this.indexRow = this.option.length;
            this.$refs["combobox__option--table"].scrollTop =
              this.$refs["combobox__option--table"].scrollHeight;
          } else {
            if (this.indexRow + 1 <= 3) {
              this.$refs["combobox__option--table"].scrollTop = 0;
            } else {
              if ((this.indexRow - 1) % 3 == 0 && this.indexRow > 2)
                this.$refs["combobox__option--table"].scrollTop =
                  this.$refs["combobox__option--table"].scrollTop - 120;
            }
          }

          this.indexRow = this.indexRow - 1;
          isShowHover = true;
        }
        if (event.code == "ArrowDown") {
          if (this.indexRow == this.option.length) {
            this.indexRow = 0;
            this.$refs["combobox__option--table"].scrollTop = 0;
          } else {
            if (this.indexRow + 1 >= this.option.length) {
              this.$refs["combobox__option--table"].scrollTop =
                this.$refs["combobox__option--table"].scrollTop + 0;
            } else {
              if (this.indexRow % 3 == 0 && this.indexRow > 2)
                this.$refs["combobox__option--table"].scrollTop =
                  this.$refs["combobox__option--table"].scrollTop + 120;
            }
          }

          this.indexRow = this.indexRow + 1;
          isShowHover = true;
        }

        if (event.code == "Enter") {
          this.btnTicked(this.option[this.indexRow - 1], this.indexRow - 1);

          this.backgroundColor = "rgba(26, 164, 200, .2)";
        }

        if (isShowHover == true) {
          isShowHover = false;
          this.isRowHover = this.option[this.indexRow - 1];
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
          this.backgroundColor = "rgba(26, 164, 200, .2)";
          return item;
        }
      } catch (error) {
        console.log(error);
      }
    },

    /*
     *  hàm xử lý bỏ chọn combobox khi click
     * Author : Bùi Quang Điệp
     * Date : 09/08/2022
     */
    removeTick() {
      try {
        this.btnTickedBody(this.isTicked);
        this.checkRemove = false;
        this.searchComboBox();
      } catch (error) {
        console.log(error);
      }
    },

    /*
     *  hàm xử lý focus vào row đầu tiên trong combobox table
     * Author : Bùi Quang Điệp
     * Date : 09/08/2022
     */
    focusRowTable() {},
  },
  /**
   * Nhận dữ liệu từ combobox
   * Author : Bùi Quang Điệp
   * Date :14 /08/ 2021
   */
  created() {
    // Lấy thông tin bộ phận sử dụng từ API
    try {
      if (this.urlAPI != undefined) {
        API.get(this.urlAPI)
          .then((res) => {
            console.log(res);
            this.option = res.data;
            this.optionReserve = res.data;
            //this.emitter.emit("Department",res.data);
          })
          .catch((res) => {
            console.log(res);
          });
      }
    } catch (error) {
      console.log(error);
    }
  },

  /**
   * Gán dữ liệu cho các combobox trong dialog khi sửa tài sản
   * Author : Bùi Quang Điệp
   * Date:25/08/2022
   * */
  beforeUpdate() {
    try {
      if (this.dataItemCombobox != undefined) {
        if (
          this.optionTable &&
          this.dataItemCombobox.departmentID != "" &&
          this.dataItemCombobox.fixedAssetCategoryID != ""
        ) {
          var checkValue = true;
          if (this.isCheck == false) {
            this.checkFocus = false;
            for (let i = 0; i < this.option.length; i++) {
              var idChange = this.id.charAt(0).toLowerCase() + this.id.slice(1);

              if (this.option[i][this.id] == this.dataItemCombobox[idChange]) {
                this.$emit("isCheck");
                this.btnTicked(this.option[i], i);
                checkValue = false;
              }
            }
            if (checkValue == true) {
              this.checkFocus = true;
            }
          } else {
            this.checkFocus = true;
          }
        } else if (this.dataItemCombobox != "") {
          checkValue = true;
          if (this.isCheck == false) {
            this.checkFocus = false;
            for (let i = 0; i < this.option.length; i++) {
              if (this.option[i][this.id] == this.dataItemCombobox) {
                this.$emit("isCheck", this.position);
                this.btnTicked(this.option[i]);
                checkValue = false;
              }
            }
            if (checkValue == true) {
              this.checkFocus = true;
            }
          } else {
            this.checkFocus = true;
          }
        }
      }
    } catch (error) {
      console.log(error);
    }
  },
  mounted() {
    try {
      // gán Tên bảng cho từng combobox
      if (this.nameTable != "") {
        this.name = this.nameTable + "Name";
        this.id = this.nameTable + "ID";
        this.code = this.nameTable + "Code";
      }

      // Mặc định required là false
      this.$emit("checkRequired", false);
    } catch (error) {
      console.log(error);
    }
  },
};
</script>

<style scoped>
@import url(../../css/component/comboBox.css);
@import url(../../css/component/input.css);

#app {
  background-color: #f4f7ff;
}

#TooltipComBoBox {
  left: v-bind("left");
  top: v-bind("top");
}

#tooltipText {
  opacity: v-bind("opacity");
  position: initial;
}
.row-hover {
  background-color: rgba(26, 164, 200, 0.2) !important;
}
input {
  text-overflow: ellipsis;
  padding-right: 20px;
}
.option--item-text {
  position: unset;
  white-space: nowrap;
  text-overflow: ellipsis;
  overflow: hidden;
  width: 100%;
}
.m-loading {
  position: absolute !important;
  width: 100%;
  height: 100%;
  opacity: 0.8 !important;
  background-color: rgba(46, 37, 37, 0.485);
}
</style>
