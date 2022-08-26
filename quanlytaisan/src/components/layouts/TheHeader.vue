<template>
    <div class="header">
                <p class ="title-header">Danh sách tài sản </p>
                <div class="header-right display-flex">
                    <div>Sở tài chính</div>
                    <div class="header-date">
                        <span style="margin: 0 14px">Năm</span><span style="font-weight: bold;">2021 </span>
                        <div class="">
                            <div class="header-group__icon--item icon icon-up icon__size-8"></div>
                            <div class="header-group__icon--item icon icon-down2 icon__size-8"></div>
                        </div>

                    </div>
                    <div class="header-group__icon display-flex ">
                        <div class="header-group__icon--item icon__size-24 icon icon-notify tooltip tooltipMenu"> <Tooltip tooltiptext ="Thông báo" positiontooltip="top"/></div>
                        <div class="header-group__icon--item icon__size-24 icon icon-folder tooltip tooltipMenu"><Tooltip tooltiptext="Thư mục" positiontooltip="top"/></div>
                        <div class="header-group__icon--item icon__size-24 icon icon-support tooltip tooltipMenu"><Tooltip tooltiptext="Hỗ trợ" positiontooltip="top"/></div>
                        <div style=" display: flex; align-items: center;justify-content: center;">
                            <div class="header-group__icon--item icon__size-24 icon icon-user tooltip tooltipMenu"><Tooltip tooltiptext="Người dùng" positiontooltip="top"/></div>
                            <div class="header-group__icon--item icon__size-8 icon icon-down2" style="padding-right: 5px;margin-left:0"></div>
                        </div>
                       
                       
                    </div>
                </div>

            </div>
</template>

<script>
import Tooltip from '../base/TheTooltip.vue'

export default {
    name: "TheHeader",
    components: {
        Tooltip
    }
}
</script>

