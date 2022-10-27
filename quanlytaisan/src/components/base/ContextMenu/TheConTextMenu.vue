<template>
  <div
    class="context-menu"
    ref="popper"
    v-show="isVisible"
    tabindex="-1"
    @contextmenu.capture.prevent
  >
    <div class="">
     
      <div
        class="menu__item"
        @click="
          this.$emit('edit');
          close();
        "
      >
        <div
          class="menu__item--icon icon icon-edit icon__size-18"
          style="margin: 0"
        ></div>
        <div class="menu__item--text">Sửa</div>
      </div>
      <div
        class="menu__item"
        @click="
          this.$emit('remove');
          close();
        "
      >
        <div class="menu__item--icon icon icon-remove icon__size-18"></div>
        <div class="menu__item--text">Xóa</div>
      </div>
      <div
        class="menu__item"
        @click="
          this.$emit('replication');
          close();
        "
        v-if="replication"
      >
        <div class="menu__item--icon icon icon-replication icon__size-18"></div>
        <div class="menu__item--text">Nhân bản</div>
      </div>
    </div>
  </div>
</template>

<script>
import Popper from "popper.js";
export default {
  components: {},
  data() {
    return {
      opened: false,
    };
  },

  computed: {
    isVisible() {
      return this.opened;
    },
  },
  methods: {
    open(evt) {
      this.opened = true;

      if (this.popper) {
        this.popper.destroy();
      }

      this.popper = new Popper(this.setPosition(evt), this.$refs.popper, {
        placement: "right-start",
        modifiers: {
          preventOverflow: {
            boundariesElement: document.querySelector(this.boundariesElement),
          },
        },
      });

      // Recalculate position
      this.$nextTick(() => {
        this.popper.scheduleUpdate();
      });
    },
    close() {
      this.opened = false;
    },
    setPosition(evt) {
      const left = evt.clientX;
      const top = evt.clientY;
      const right = left + 1;
      const bottom = top + 1;
      const clientWidth = 1;
      const clientHeight = 1;

      function getBoundingClientRect() {
        return {
          left,
          top,
          right,
          bottom,
        };
      }

      const obj = {
        getBoundingClientRect,
        clientWidth,
        clientHeight,
      };
      return obj;
    },
  },
  beforeUnmount() {
    if (this.popper !== undefined) {
      this.popper.destroy();
    }
  },
  props: {
    replication: {
      Type: Boolean,
      default: true,
    },
  },
};
</script>

<style lang="scss" scoped>
.context-menu {
  position: fixed !important;
  z-index: 999;
  overflow: hidden;
  background-color: #fff;
  border-radius: 4px;
  border: 2px solid #afafaf5c;
  box-shadow: #0000004d 1px 1px 1px;
  &:focus {
    outline: none;
  }
}
</style>
