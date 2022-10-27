<template>
  <!-- input tìm kiếm -->
  <div
    class="input-search tooltip"
    style="margin-right: 10px"
    v-if="inputSearch"
    :style="styleInput"
  >
    <div class="icon" :class="iconInput"></div>
    <input
      :type="text"
      name=""
      id=""
      class="input"
      :disabled="disable"
      :class="[classInput]"
      @keydown="this.$emit('keyDown', $event)"
      @keypress.enter="this.$emit('keyDownEnter', $event.currentTarget.value)"
      :placeholder="placeholder"
      @blur="this.$emit('blur',$event.currentTarget,properties)"
      ref="txtSearch"
    />
    <Tooltip
      v-if="tooltipShow"
      class="tooltipRequiredDetail showTooltip"
      style="top: -30px"
      :tooltiptext="tooltipInput"
      positiontooltip="top"
    />
  </div>

  <!-- Input nhập liệu -->
  <div
    v-if="inputSearch == false"
    class="tooltip tooltipRequired"
    :class="classBorderInput"
    @mouseover="this.$emit('mouseover', $event)"
    @mouseleave="this.$emit('mouseleave', $event)"
  >
    <div class="margin-text" v-if="labelInput != undefined">
      {{ labelInput
      }}<span style="color: red" v-if="required == 'required'"> *</span>
      <Tooltip
        class="tooltipRequiredDetail"
        :tooltiptext="tooltipInput"
        positiontooltip="top"
        v-if="isShowTooltipRequired && dataRequired != ''"
      />
    </div>
    <input
      :value="modelValue"
      :type="text"
      id=""
      class="input input-width"
      v-if="inputNumber == false"
      :class="classInput"
      :disabled="disable"
      :ref="properties"
      :attr="required"
      :tabindex="tabindex"
     
      @keydown="
        this.$emit('keydown', $event, properties);
        checkLength($event);
      "
      @input="sendDataModel($event)"
      @blur="this.$emit('blur',$event.currentTarget,properties)"
    />
    <button
      class="combobox__control combobox__width"
      style="border: none !important"
      v-if="inputNumber == true"
    >
      <input
         :value="modelValue"
        @input="sendDataModel($event)"
        :type="text"
        id=""
        class="input input-width"
        :class="classInput"
        style="padding-right: 40px"
        :disabled="disable"
        :ref="properties"
        :attr="required"
        :tabindex="tabindex"
        @blur="this.$emit('blur', $event.currentTarget, properties)"
        @keydown="
          this.$emit('keydown', $event, properties);
          checkLength($event);
        "
        
      />
      <p v-if="titleInput" class="label-input">Tổng</p>
      <div class="combobox__icon" v-if="showIcon">
        <div class="header-group__icon--item icon icon-up icon__size-8"></div>
        <div
          class="header-group__icon--item icon icon-down2 icon__size-8"
        ></div>
      </div>
    </button>
  </div>
</template>

<script>
import Tooltip from "./BaseTooltip.vue";

export default {
  name: "QuanlytaisanTheInput",

  data() {
    return {
      data: "",
    };
  },
  components: {
    Tooltip,
  },

  mounted() {
    this.isFocus();

    // if(this.properties=="depreciationRate")
    // {
    //     var a = this.$refs[this.properties].value ;
    //     this.$refs[this.properties].value = a.split(".").join(",");
    // }
  },
  updated() {
    // if(this.properties=="depreciationRate")
    // {
    //     var a = this.$refs[this.properties].value ;
    //     this.$refs[this.properties].value = a.split(".").join(",");
    // }

    /**
     *  Focus vào input còn báo lỗi
     * Author : Bùi Quang Điệp
     * Date:16/09/2022
     */
    try {
      if (this.focusRequired != undefined) {
        this.$refs[this.focusRequired].focus();
      }
    } catch (error) {
      console.log(error);
    }

    /**
     *  chuyển lại data khi thay đổi
     * Author : Bùi Quang Điệp
     * Date:07/09/2022
     */
    try {
      if (this.dataChange == true) {
        this.data = this.dataInput;
        this.$emit("dataChange");
      }
      /**
       *  Khi mở dialog mặc định focus vào input đầu tiên
       * Author : Bùi Quang Điệp
       * Date:07/09/2022
       */
      if (this.focus == true) {
        
        this.$refs[this.properties].focus();
        this.$emit("isFocus");
      }

    //   if (
    //     this.properties == "cost" ||
    //     this.properties == "depreciationYear" ||
    //     this.properties == "lifeTime" ||
    //     this.properties == "sumCost"
    //   ) {
    //     this.data = formatCash(this.data);
    //   }
    //   if (this.properties == "depreciationRate") {
    //     this.data = formatDecimal(this.data);
    //   }
      if (this.checkRequiredAll) {
        this.$emit("blur", this.$refs[this.properties], this.properties);
      }
    } catch (error) {
      console.log(error);
    }
  },
  props: {
    focusInput: {
      Type: Boolean,
      default: true,
    },
    showIcon: {
      Type: Boolean,
      default: true,
    },
    inputSearch: {
      Type: Boolean,
      default: false,
    },
    styleInput: Object,
    inputNumber: {
      Type: Boolean,
      default: false,
    },
    modelValue: String,
    focusRequired: String,
    focus: Boolean,
    classBorderInput: String,
    classInput: String,
    iconInput: String,
    keyDown: String,
    placeholder: String,
    tooltipInput: String,
    labelInput: String,
    isShowTooltipRequired: Boolean,
    dataInput: String,
    titleInput: {
      type: Boolean,
      default: false,
    },
    Type: {
      Type: String,
      default: "text",
    },
    disable: {
      Type: Boolean,
      default: false,
    },
    dataRequired: Array,
    tabindex: Number,
    properties: String,
    tooltipShow: Boolean,
    required: String,
    checkRequiredAll: Boolean,
    dataChange: Boolean,
    maxLength: Number,
  },

  methods: {
     /**
     * gửi dữ liệu khi input
     * Author : Bùi Quang Điệp
     * Date:15/08/2022
     */
    sendDataModel()
    {
        try {
            this.$emit('update:modelValue', event.currentTarget.value);
            this.$emit('input');
            
        } catch (error) {
            console.log(error);
        }
    },
    /**
     * Check độ dài không cho phép nhập quá kí tự
     * Author : Bùi Quang Điệp
     * Date:15/08/2022
     */
    checkLength(event) {
      if (this.maxLength != undefined) {
        if (this.data.length >= this.maxLength) {
          if (event.code == "Backspace" || event.code == "Tab") return;
          else
            this.$emit(
              "errorLength",
              this.properties,
              this.labelInput,
              this.maxLength
            );
          event.preventDefault();
        } else {
          this.$emit("pass", this.properties);
        }
      }
    },
    /**
     *  Khi mở dialog mặc định focus vào input đầu tiên
     * Author : Bùi Quang Điệp
     * Date:07/09/2022
     */
    isFocus() {
      try {
        
        if (this.inputSearch) {
          if (this.focusInput) {
            this.$refs["txtSearch"].focus();
            this.$emit("isFocus");
          }
        }
        else{
          if(this.focus)
          {
            this.$refs[this.properties].focus();
            this.$emit("isFocus");
          }
        }
      } catch (error) {
        console.log(error);
      }
    },
  },
};
</script>

<style lang="css" scoped>
.showTooltip {
  opacity: 1 !important;
}
</style>
