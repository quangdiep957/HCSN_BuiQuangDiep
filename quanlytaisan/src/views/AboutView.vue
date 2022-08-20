<template>
    <div>
        <div v-contextmenu:contextmenu>right click here</div>
       <v-contextmenu ref="contextmenu">
    <v-contextmenu-item>Menu Item 1</v-contextmenu-item>
    <v-contextmenu-item>Menu Item 2</v-contextmenu-item>
    <v-contextmenu-item>Menu Item 3</v-contextmenu-item>
  </v-contextmenu>
<div class="tooltip">
  <span class="tooltiptext">{{tooltiptext}}</span>
</div>
<div class="tooltip">
  <span class="tooltiptext">{{tooltiptext}}</span>
</div>

<div class="tooltip">
  <span class="tooltiptext">{{tooltiptext}}</span>
</div>

    </div>
</template>

<script>
import { directive, Contextmenu, ContextmenuItem } from "v-contextmenu";
import "v-contextmenu/dist/themes/default.css";
export default {
    name: 'QuanlytaisanAboutView',
 directives: {
    contextmenu: directive,
  },
  components: {
    [Contextmenu.name]: Contextmenu,
    [ContextmenuItem.name]: ContextmenuItem,
  },
    data() {
        return {
            
        };
    },

    methods: {
         onClick (text) {
            alert(`You clicked ${text}!`);
        }
        
    },
   
};
</script>

<style lang="css" scoped>
.tooltip {
  position: relative;
}

.tooltip .tooltiptext {
  visibility: hidden;
  width: 120px;
  background-color: #555;
  color: #fff;
  text-align: center;
  border-radius: 6px;
  padding: 5px 0;
  position: absolute;
  z-index: 1;
  bottom: 125%;
  left: 50%;
  margin-left: -60px;
  opacity: 0;
  transition: opacity 0.3s;
}

.tooltip .tooltiptext::after {
  content: "";
  position: absolute;
  top: 100%;
  left: 50%;
  margin-left: -5px;
  border-width: 5px;
  border-style: solid;
  border-color: #555 transparent transparent transparent;
}

.tooltip:hover .tooltiptext {
  visibility: visible;
  opacity: 1;
}
</style>