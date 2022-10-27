<template>
  <!-- Input nhập liệu -->
  <div
    class="tooltip tooltipRequired"
    :class="classBorderInput"
    @mouseover="this.$emit('mouseover', $event)"
    @mouseleave="this.$emit('mouseleave', $event)"
  >
    <div class="margin-text" v-if="labelInput != undefined">
      {{ labelInput
      }}<span style="color: red" v-if="iconRequired == 'required'"> *</span>
    </div>
    <Tooltip
        :style="styleTooltip"
        class="tooltipRequiredDetail"
        :tooltiptext="tooltipInput"
        positiontooltip="top"
        v-if="dataRequired != '' && positionInputRequired"
      />
    <button
      class="combobox__control combobox__width"
      style="border: none !important"
    >
      <input
        :type="text"
        id=""
        class="input input-width text-align-right absolute"
        :class="{ iconNumber: showIcon }"
        :disabled="disable"
        :ref="properties"
        :attr="required"
        :tabindex="tabindex"
        @input="updateValue"
        @keydown="this.$emit('keydown', $event, properties)"
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
import AutoNumeric from "autonumeric";
import Tooltip from "./BaseTooltip.vue";
import Config from "@/js/config";
export default {
  props: {
    labelInput: {
      type: String
    },
    position:Number,
    positionInputRequired:Boolean,
    showIcon: {
      Type: Boolean,
      default: true,
    },
    tooltipInput: String,
    isShowTooltipRequired: Boolean,
    dataRequired: Array,
    modelValue: {
      type: Number,
      default: 0,
    },
    max: {
      type: Number,
      default: 10000000000000.0,
    },
    decimalPlaces: {
      type: Number,
      default: 0,
    },
    focusInput: {
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

    focusRequired: String,
    focus: Boolean,
    classBorderInput: String,
    classInput: String,
    iconInput: String,
    keyDown: String,
    placeholder: String,
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
    tabindex: Number,
    properties: String,
    tooltipShow: Boolean,
    required: String,
    iconRequired:Boolean,
    checkRequiredAll: Boolean,
    dataChange: Boolean,
    maxLength: Number,
    styleTooltip:Object
  },

  data() {
    return {
      value: "0",
    };
  },
  components: {
    Tooltip,
  },

  methods: {
    /**
     * hàm cập nhật giá trị mới
     * Author: TTDuc (29/08/2022)
     */
    updateValue() {
      try {
        this.$emit("update:modelValue", this.getRawNumberValue());
        this.$emit("blur",this.$refs[this.properties],this.properties, this.position);
      } catch (err) {
        console.log(err);
      }
    },

    /**
     * lấy giá trị gốc
     * Author:TTDuc (29/08/2022)
     */
    getRawNumberValue() {
      let rawTextValue = this._numeric.rawValue;

      let rawNumberValue = Number(rawTextValue);

      return rawNumberValue;
    },
  },

  mounted() {
    // Khởi tạo 1 biến autonumeric
    const numeric = new AutoNumeric(this.$refs[this.properties], {
      digitGroupSeparator: Config.FormatCost.Number,
      decimalCharacter: Config.FormatCost.Decimal,
      currencySymbolPlacement:
        AutoNumeric.options.currencySymbolPlacement.suffix,
      roundingMethod: AutoNumeric.options.roundingMethod.halfUpSymmetric,
      maximumValue: this.max,
      minimumValue: 0,
      allowDecimalPadding: false,
      decimalPlaces: 2,
      unformatOnSubmit: true,
    });

    this._numeric = numeric;

    if (this.modelValue !== 0) {
      this._numeric.set(this.modelValue);
    }
  },
  updated() {
    if (this.checkRequiredAll) {
      this.$emit("blur",this.$refs[this.properties],this.properties, this.position);
    }
  },
  watch: {
    /**
     * set giá trị mặc định cho numeric
     * Author: TTDuc (29/08/2022)
     */
    modelValue() {
      let rawNumberValue = this.getRawNumberValue();
      if (this.modelValue != rawNumberValue) {
        this._numeric.set(this.modelValue);
      }
    },
    "_numeric.value"(newValue) {
      console.log(1);
      this.$emit("update:modelValue", newValue);
    },
  },
};
</script>
<style scoped>
.iconNumber {
  padding-right: 40px;
}
</style>
