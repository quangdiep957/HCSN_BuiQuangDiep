<template>
    <span v-if="tooltiptext != ''" class="tooltiptext" v-bind:attr="position = positiontooltip" :class="classTooltip">{{ tooltiptext }}</span>
</template>

<script>
export default {
    name: "theTooltip",
    props: {
        tooltiptext: {
            type: String,
            default:''
           
        },
        
        positiontooltip: {
            default: "top",
            type: String,
        },
        classTooltip:String
    },

};
</script>
<style scoped>
 @import url(../../css/component/tooltip.css);
</style>
