<template>
  <div class="group-input size-33" :style="styleCustomer">
    <div class="margin-text">{{ label }}<span style="color: red"> *</span></div>
    <div class="form-date" :class="{ a: isFocusInputHide }">
      <!-- <input type="date" 
		            min="01/01/1997" max="31/12/2030" 
                  class="input-date hide" 
                  v-model="data" 
                  @input="changeDate"
               >
               <input type="text" :tabindex=tabindex @focus="isFocusInputHide = true" @blur="isFocusInputHide = false"
                  :placeholder=placeholder
                  class="input-date inputTextDate" 
                  ref="date"
                  v-model="dataText"
                  @keydown="formatDateText(this.formatType)"
                  @input="demo"
               > -->
      <el-date-picker
        :tabindex="tabindex"
        @focus="isFocusInputHide = true"
        @blur="isFocusInputHide = false"
        @change="this.$emit('Date', data)"
        v-model="data"
        type="date"
        :placeholder="formatType"
        :format="formatType"
      />
      <span class="icon icon-24 icon-calendar btn-date"></span>
    </div>
  </div>
</template>

<script>
import { formatDate } from "@/js/common";

export default {
  name: "BaseDate",

  data() {
    return {
      isFocusInputHide: false,
      data: "",
      dataText: "",
      dateArray: ["_", "_", "/", "_", "_", "/", "_", "_", "_", "_"],
      separation1: true,
      separation: true,
      deleteChange: false,
    };
  },
  props: {
    label: String,
    tabindex: String,
    dataDate: String,
    placeholder: String,
    formatType: String,
    refType: String,
    focusDate: Boolean,
    styleCustomer: String,
  },

  mounted() {
    this.data = this.dataDate;
    if (this.dataDate == undefined) {
      this.data = new Date();
    }
    this.dataText = formatDate(this.data, this.formatType);
  },
  beforeUpdate() {
    if (this.focusDate == true) {
      this.$refs["dateTime"].focus;
    }
  },

  methods: {
    /**
     * hàm validate ngày tháng
     * Author : Bùi Quang Điệp
     * Date:15 /09/2022
     */
    formatDateText(format) {
      try {
        this.deleteChange = false;
        if (event.code == "Tab") {
          return;
        }
        if (event.code == "Backspace") {
          this.deleteChange = true;
          return;
        }
        if (this.dateArray.length > 3) {
          this.separation = false;
        }
        if (this.dateArray.length > 5) {
          this.separation1 = false;
        }
        var text = "";
        if (this.dateArray.length > 10) {
          this.dateArray = ["_", "_", "/", "_", "_", "/", "_", "_", "_", "_"];
          this.separation = true;
          this.separation1 = true;
        }
        var number = String.fromCharCode(event.keyCode),
          valid = /^[0-9]+$/.test(number);

        if (!valid && event.keyCode != 8) {
          event.preventDefault();
        } else {
          event.preventDefault();
          if (event.keyCode != 8) {
            // gán số vừa nhập vào mảng
            this.dateArray.push(number);
          }
          if (format == "DMY") {
            // kiểm tra số đầu tiên lớn hơn 3 thì thêm số 0 đằng trước
            if (this.dateArray[0] > 3) {
              this.dateArray.pop();
              this.dateArray.push("0");
              this.dateArray.push(number);
            }
            if (this.dateArray[0] == 0) {
              if (this.dateArray[1] == 0) {
                this.dateArray.pop();
                this.dateArray.push("_");
              }
            }
            if (this.dateArray[0] == 3) {
              if (this.dateArray[1] > 1) {
                this.dateArray.pop();
              }
            }
            // Kiểm tra nếu đã nhập đầy đủ 2 số đầu tự động thêm dấu phân cách
            if (this.dateArray[1] != undefined && this.separation == true) {
              this.separation = false;
              this.dateArray.push("/");
            }
            // Kiểm tra tháng số đầu tiên không  được lớn hơn 1
            if (this.dateArray[3] > 1) {
              this.dateArray.pop();
              this.dateArray.push("0");
              this.dateArray.push(number);
            }
            if (this.dateArray[3] == 0) {
              if (this.dateArray[4] == 0) {
                this.dateArray.pop();
                this.dateArray.push("_");
              }
            }
            if (this.dateArray[3] == 1) {
              if (this.dateArray[4] > 2) {
                this.dateArray.pop();
                this.dateArray.push("_");
              }
            }
            if (this.dateArray[4] != undefined && this.separation1 == true) {
              this.separation1 = false;
              this.dateArray.push("/");
            }
            if (this.dateArray[6] > 2 || this.dateArray[6] == 0) {
              this.dateArray.pop();
              this.dateArray.push("_");
            }
            if (this.dateArray[6] == 2) {
              if (this.dateArray[7] > 0 && this.dateArray[7] != undefined) {
                this.dateArray.pop();
                this.dateArray.push("_");
              }
              if (this.dateArray[8] > 2 && this.dateArray[8] != undefined) {
                this.dateArray.pop();
                this.dateArray.push("_");
              }
              if (this.dateArray[9] > 2 && this.dateArray[9] != undefined) {
                this.dateArray.pop();
                this.dateArray.push("_");
              }
            }
            if (this.dateArray[6] < 2) {
              if (this.dateArray[7] > 9 && this.dateArray[7] != undefined) {
                this.dateArray.pop();
                this.dateArray.push("_");
              }
              if (this.dateArray[8] > 9 && this.dateArray[8] != undefined) {
                this.dateArray.pop();
                this.dateArray.push("_");
              }
              if (this.dateArray[9] > 9 && this.dateArray[9] != undefined) {
                this.dateArray.pop();
                this.dateArray.push("_");
              }
            }
          }
          if (format == "YMD") {
            // Kiểm tra năm đầu tiên số đầu không được lớn hơn 2
            if (this.dateArray[0] == 2 || this.dateArray[0] == 0) {
              if (this.dateArray[1] > 0 && this.dateArray[1] != undefined) {
                this.dateArray.pop();
              }
              if (this.dateArray[2] > 2 && this.dateArray[2] != undefined) {
                this.dateArray.pop();
              }
              if (this.dateArray[3] > 2 && this.dateArray[3] != undefined) {
                this.dateArray.pop();
              }
            }

            if (this.dateArray[0] < 2) {
              if (this.dateArray[1] > 9 && this.dateArray[1] != undefined) {
                this.dateArray.pop();
              }
              if (this.dateArray[2] > 9 && this.dateArray[2] != undefined) {
                this.dateArray.pop();
              }
              if (this.dateArray[3] > 9 && this.dateArray[3] != undefined) {
                this.dateArray.pop();
              }
            }
            // Kiểm tra nếu đủ năm thì thêm dấu phân cách
            if (this.separation1 == true && this.dateArray[3] != undefined) {
              this.separation1 = false;
              this.dateArray.push("/");
            }
            if (this.dateArray[5] > 1) {
              this.dateArray.pop();
              this.dateArray.push("0");
              this.dateArray.push(number);
            }
            if (this.dateArray[5] == 0) {
              if (this.dateArray[6] == 0) {
                this.dateArray.pop();
              }
            }
            if (this.dateArray[5] == 1) {
              if (this.dateArray[6] > 2) {
                this.dateArray.pop();
              }
            }
            // Kiểm tra nếu đã nhập đầy đủ 2 số tự động thêm dấu phân cách
            if (this.dateArray[6] != undefined && this.separation == true) {
              this.separation = false;
              this.dateArray.push("/");
            }
            // Kiểm tra ngày số đầu tiên không  được lớn hơn 3
            if (this.dateArray[8] > 3) {
              this.dateArray.pop();
              this.dateArray.push("0");
              this.dateArray.push(number);
            }
            if (this.dateArray[8] == 0) {
              if (this.dateArray[9] == 0) {
                this.dateArray.pop();
              }
            }
            if (this.dateArray[8] == 3) {
              if (this.dateArray[9] > 1) {
                this.dateArray.pop();
              }
            }
          }
          if (format == "MDY") {
            // kiểm tra số đầu tiên lớn hơn 1 thì thêm số 0 đằng trước
            if (this.dateArray[0] > 1) {
              this.dateArray.pop();
              this.dateArray.push("0");
              this.dateArray.push(number);
            } else {
              this.dateArray.pop();
              this.dateArray.push(number);
            }
            if (this.dateArray[0] == 0) {
              if (this.dateArray[1] == 0) {
                this.dateArray.pop();
              }
            }
            if (this.dateArray[0] == 1) {
              if (this.dateArray[1] > 2) {
                this.dateArray.pop();
              }
            }
            // Kiểm tra nếu đã nhập đầy đủ 2 số đầu tự động thêm dấu phân cách
            if (this.dateArray[1] != undefined && this.separation == true) {
              this.separation = false;
              this.dateArray.push("/");
            }
            // Kiểm tra ngày số đầu tiên không  được lớn hơn 3
            if (this.dateArray[3] > 3) {
              this.dateArray.pop();
              this.dateArray.push("0");
              this.dateArray.push(number);
            }
            if (this.dateArray[3] == 0) {
              if (this.dateArray[4] == 0) {
                this.dateArray.pop();
              }
            }
            if (this.dateArray[3] == 3) {
              if (this.dateArray[4] > 1) {
                this.dateArray.pop();
              }
            }
            if (this.dateArray[4] != undefined && this.separation1 == true) {
              this.separation1 = false;
              this.dateArray.push("/");
            }
            if (this.dateArray[6] > 2 || this.dateArray[6] == 0) {
              this.dateArray.pop();
            }
            if (this.dateArray[6] == 2) {
              if (this.dateArray[7] > 0 && this.dateArray[7] != undefined) {
                this.dateArray.pop();
              }
              if (this.dateArray[8] > 2 && this.dateArray[8] != undefined) {
                this.dateArray.pop();
              }
              if (this.dateArray[9] > 2 && this.dateArray[9] != undefined) {
                this.dateArray.pop();
              }
            }
            if (this.dateArray[6] < 2) {
              if (this.dateArray[7] > 9 && this.dateArray[7] != undefined) {
                this.dateArray.pop();
              }
              if (this.dateArray[8] > 9 && this.dateArray[8] != undefined) {
                this.dateArray.pop();
              }
              if (this.dateArray[9] > 9 && this.dateArray[9] != undefined) {
                this.dateArray.pop();
              }
            }
          }

          if (event.keyCode == 8) {
            this.dateArray.pop();
          }
          for (let i = 0; i < this.dateArray.length; i++) {
            text = text + this.dateArray[i];
          }
          this.dataText = text;

          // Kiểm tra nếu đã đúng định dạng thì cập nhật vào data
          if (this.dateArray.length > 9) {
            text = text.replace("/", "-");
            text = text.replace("/", "-");
            var newDate = text;
            var dateParts = "";
            if (format == "DMY") {
              dateParts = text.split("-");
              newDate =
                dateParts[1] + "-" + Number(dateParts[0]) + "-" + dateParts[2];
            }
            if (format == "YMD") {
              dateParts = text.split("-");
              newDate =
                dateParts[1] + "-" + Number(dateParts[2]) + "-" + dateParts[0];
            }
            this.$emit("Date", newDate);
            this.dateArray = ["_", "_", "/", "_", "_", "/", "_", "_", "_", "_"];
          }
          if (this.dateArray.length == 0) {
            this.separation = true;
            this.separation1 = true;
          }
        }
      } catch (error) {
        console.log(error);
      }
    },

    /**
     * hàm truyền dữ liệu về cho cha khi ngày tháng thay đổi
     * Author : Bùi Quang Điệp
     * Date:15 /09/2022
     */
    changeDate() {
      try {
        var format = formatDate(this.data, this.formatType);
        this.dataText = format;
        this.$emit("Date", this.data);
      } catch (error) {
        console.log(error);
      }
    },
  },
};
</script>

<style lang="css" scoped>
.a {
  border: 1px solid #1aa4c8 !important;
}
@import url(../../css/component/datimePicker.css);
</style>
