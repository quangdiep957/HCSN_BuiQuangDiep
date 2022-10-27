<template>
  <!-- dialog cảnh báo -->
  <div class="dialog__handle" id="dialog__notify">
    <div class="dialog__notify">
      <div class="dialog__notify--header">
        <div class="icon-warning">
          <i class="fa-solid fa-triangle-exclamation"></i>
        </div>
        <div v-if="isShowNumber == false" class="text--notify">
          <p v-for="item in dataError" :key="item">
            {{ item }} <b>{{ errorName }}</b>
          </p>
        </div>
        <div v-if="isShowNumber" class="text--notify">
          <p v-for="item in dataError" :key="item">
            {{ firstText }}<b>{{ errorName }}</b> {{ item }}
            <b>{{ lastName }}</b
            >{{ lastText }}
          </p>
        </div>
      </div>
      <div class="dialog__notify--footer">
        <button
          class="btn btn-confirm"
          ref="btnConfirm"
          tabindex="1"
          @click="handleRemove()"
        >
          {{ buttonNames[buttonLabel.buttonLabelZero] }}
        </button>
        <button
          v-if="buttonNames[buttonLabel.buttonLabelOne] != undefined"
          class="btn btn-cancel"
          ref="btnCancel"
          style="margin: 0 10px"
          @keydown.tab.prevent="isFocusConfirm()"
          tabindex="2"
          @click="removeDialog()"
        >
          {{ buttonNames[buttonLabel.buttonLabelOne] }}
        </button>
        <button
          v-if="buttonNames[buttonLabel.buttonLabelTwo] != undefined"
          class="btn btn-cancel"
          @keydown.tab.prevent="isFocusConfirm()"
          tabindex="3"
          @click="this.$emit('isShowDialogNotify')"
        >
          {{ buttonNames[buttonLabel.buttonLabelTwo] }}
        </button>
      </div>
    </div>
  </div>
</template>

<script>
import Resource from '@/js/resource';
import { API } from "../../js/callapi";
export default {
  name: "TheDialogNotify",

  data() {
    return {
      ShowDialogNotify: true,
      buttonLabel:{
        buttonLabelZero:0,
        buttonLabelOne:1,
        buttonLabelTwo:2
      }
    };
  },
  components: {},

  mounted() {
    this.$refs["btnConfirm"].focus();
  },
  props: {
    dataError: Array,
    errorName: String,
    dataAsset: Array,
    buttonNames: Array,
    handleButton: String,
    domain: String,
    firstText: String,
    lastName: String,
    lastText: String,
    isShowNumber: {
      Type: Boolean,
      default: false,
    },
  },

  methods: {
    /***
     * Focus vào ô Xác nhận
     * Author:Bùi Quang Điệp
     * Date:13/08/2022
     *
     */
    isFocusConfirm() {
      this.$refs["btnConfirm"].focus();
    },

    /***
     * xử lý chức năng xóa , xóa nhiều hoặc trả về giá trị lưu cho dialog
     * Author:Bùi Quang Điệp
     * Date:15/08/2022
     *
     */
    handleRemove() {
      try {
        if (this.buttonNames.length == 1) {
          this.$emit("isShowDialogNotify");
        }
        // Kiểm tra chức năng là lưu hay thêm mới
        if (this.buttonNames[this.buttonLabel.buttonLabelTwo] != undefined) {
          this.$emit("isSaveData");
        } else {
          var idAssets = [];
          var property = "";
          if (this.dataAsset[0].fixedAssetID == undefined) {
            property = "fixedAssetIncrementID";
          } else {
            property = "fixedAssetID";
          }
          for (let i = 0; i < this.dataAsset.length; i++) {
            idAssets.push(this.dataAsset[i][property]);
          }
          if (idAssets.length > 0) {
            API.post(this.domain, idAssets)
              .then((res) => {
                console.log(res);
                this.$emit("removeData");
                this.$emit("isShowDialogNotify");
                this.$emit("handlerName",Resource.Label.Delete);
                this.emitter.emit("LoadData");
              })
              .catch((res) => {
                console.log(res);
              });
          }
        }
      } catch (error) {
        console.log(error);
      }
    },

    /***
     * Truyền dữ liệu đóng về cho component chứa Dialog này
     * Author:Bùi Quang Điệp
     * Date:15/08/2022
     *
     */
    removeDialog() {
      try {
        if (this.buttonNames[this.buttonLabel.buttonLabelOne] != undefined) {
          this.$emit("removeDialog");
        } else {
          this.$emit("isShowDialogNotify");
        }
      } catch (error) {
        console.log(error);
      }
    },
  },
};
</script>

<style lang="css" scoped></style>
