<template>
    <span class="tooltiptext" v-bind:attr="position = positiontooltip">{{ tooltiptext }}</span>
</template>

<script>
export default {
    name: "theTooltip",
    props: {
        tooltiptext: {
            type: String,
            default: "Tooltip text",
        },
        
        positiontooltip: {
            default: "top",
            type: String,
        },
    },

};
</script>
<style scoped>
 @import url(../../css/component/tooltip.css);
</style>
