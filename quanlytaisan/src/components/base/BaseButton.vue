<template>
  <!-- Button text -->
  <button
    v-if="
      buttonIcon == false && focusCircle == false && buttonIconMulti == false
    "
    class="btn tooltip"
    :class="[{ active: isActiveButton }, { focusSave: focusSave }, classButton]"
    @click="$emit('click')"
    :id="id"
    :tabindex="tabindex"
  >
    <div>
      {{ textButton }}
    </div>
    <Tooltip
      v-if="tooltipShow"
      :tooltiptext="textTooltip"
      :positiontooltip="positiontooltip"
    />
  </button>
  <button
    v-if="buttonIcon == false && focusCircle && buttonIconMulti == false"
    class="btn"
    :class="[{ active: isActiveButton }, classButton]"
    @click="$emit('click')"
    @keydown.tab.prevent="$emit('keydownTab')"
    :id="id"
    :tabindex="tabindex"
  >
    <div>
      {{ textButton }}
    </div>
  </button>
  <!-- Button icon -->
  <button
    class="btnfunction distance tooltip"
    v-if="buttonIcon && buttonIconMulti == false"
    @click="$emit('click')"
    :style="styleButton"
  >
    <div class="icon icon__size-18" :class="classIcon"></div>
    <Tooltip :tooltiptext="textTooltip" :positiontooltip="positiontooltip" class="tooltipIcon" />

    <input
      type="file"
      class="inputfile"
      v-if="inputInline"
      @input="this.$emit('input', $event)"
    />
  </button>
  <!-- Button multi icon -->
  <button
    class="btnfunction-multi btnfunction tooltip"
    v-if="buttonIconMulti"
    @click="$emit('click')"
  >
    <div class="group-icon">
      <div class="icon icon__size-40" :class="classIcon"></div>
      <div class="icon icon__size-18" :class="classIconMulti"></div>
      <Tooltip :tooltiptext="textTooltip" :positiontooltip="positiontooltip" class="tooltipIconMulti" />
    </div>
  </button>
</template>

<script>
import Tooltip from "./BaseTooltip.vue";
export default {
  name: "QuanlytaisanTheButton",

  data() {
    return {};
  },
  components: {
    Tooltip,
  },

  mounted() {},
  props: {
    buttonIcon: {
      Type: Boolean,
      default: false,
    },
    isActiveButton: {
      Type: Boolean,
    },
    textButton: {
      Type: String,
    },
    classButton: {
      Type: String,
    },
    classIcon: {
      Type: String,
    },
    classIconMulti: {
      Type: String,
    },
    textTooltip: {
      Type: String,
    },
    id: {
      Type: String,
    },
    tabindex: {
      Type: Number,
    },
    focusCircle: {
      Type: Boolean,
      default: false,
    },
    buttonIconMulti: Boolean,
    focusSave: Boolean,
    buttonIconText: Boolean,
    inputInline: Boolean,
    tooltipShow: Boolean,
    styleButton: String,
    positiontooltip: {
      type: String,
      default: "Bottom",
    },
  },

  methods: {},
};
</script>

<style lang="css" scoped>
.inputfile {
  width: 30px;
  height: 30px;
  opacity: 0;

  position: absolute;
  z-index: 1;
}
.group-icon {
  display: flex;
}
.btnfunction-multi {
  width: 70px !important;
  overflow: hidden;
  display: flex;
  align-items: center;
  padding: 0 10px;
  margin-left: 15px;
}
.tooltipIcon{
  margin-top: 60px;
}
.tooltipIconMulti{
  margin-top: 30px;
}
</style>
