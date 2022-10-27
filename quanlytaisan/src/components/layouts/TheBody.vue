<template>
    <div class="page-body-wrapper"  :class="{'setWidth':isSetWidth}" >
        <Header tabindex="4" />
        <router-view></router-view>
    </div>

</template>
<script>
import Header from './TheHeader.vue'


export default {
    name: "TheHeader",
    components: {
        Header
    },
    data() {
        return {
         
            isFocus: true,
            removeHandlerName: false,
            file: "",
            tooltipSearch: "",
            isTooltipShow: false,
            buttonText: ['Xóa', 'Không'],
            isShowDialogNotify: false,
            itemAssetDetailCheck: [],
            isShowDialog: false,
            title: "Sửa tài sản",
            titleWarning: [],
            isShowSuccess: false,
            isSetWidth: false


        };
    },
    mounted() {
       // Nhận dữ liệu set lại width navbar
       this.emitter.on("setWidth", () => {
                
                // gán giá trị width lại cho body
                this.isSetWidth = true
            }),
            // Nhận dữ liệu reset width navbar
            this.emitter.on("setWidthClear", () => {
                // gán giá trị width lại cho body
                this.isSetWidth = false
            })
    }

}
</script>
<style>
@import url(../../css/main.css);
</style>