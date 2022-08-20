<template>
    <div :class="{'setnavbar' :isShowNavbar}" class="navbar">

        <div class="navbar-header">
            <div class="navbar-header__item" style="position:relative;">
                <div class="navbar-header__logo icon tooltip" title="" @click="reloadSetWidth">
                    <Tooltip tooltiptext="Quản lý tài sản" positiontooltip="left"/>
                </div>
                <div v-show="!isShowNavbar" class="navbar-header__text">MISA QLTS</div>
            </div>
        </div>
        <div class="navbar-body">
            <router-link to="/">
                <div class="navbar-body__item tooltip" >
                    <div :class="{'icon-overice__active' : (isShowIcon)}" class="navbar-body__logo icon icon-overice iconleft icon__size-22x2 opacity" title="Tổng quan"></div>
                    <div v-show="!isShowNavbar" class="navbar-body__text opacity">Tổng quan</div>
                     <Tooltip tooltiptext="Tổng quan" positiontooltip="left"/>
                </div>
            </router-link>
            <router-link to="/table">
                <div class="navbar-body__item tooltip" >
                    <div class="navbar-body__logo icon icon-asset__active iconleft icon__size-22 opacity" ></div>
                    <div v-show="!isShowNavbar" class="navbar-body__text opacity">Tài sản</div>
                    <div v-show="!isShowNavbar" class="navbar-body__logo icon icon-down opacity"></div>
                     <Tooltip tooltiptext="Tài sản" positiontooltip="left"/>
                </div>
            </router-link>
            <div class="navbar-body__item tooltip" >
                <div class="navbar-body__logo icon icon-asset1 iconleft icon__size-21 opacity" ></div>
                <div v-show="!isShowNavbar" class="navbar-body__text opacity">Tài sản HT-ĐB</div>
                <div v-show="!isShowNavbar" class="navbar-body__logo icon icon-down opacity"></div>
                 <Tooltip tooltiptext="Tài sản hạ tầng đường bộ" positiontooltip="left"/>
            </div>
            <div class="navbar-body__item tooltip" >
                <div class="navbar-body__logo icon icon-tool iconleft icon__size-22x2 opacity" ></div>
                <div v-show="!isShowNavbar" class="navbar-body__text opacity">Công cụ dụng cụ</div>
                <div v-show="!isShowNavbar" class="navbar-body__logo icon icon-down opacity"></div>
                 <Tooltip tooltiptext="Công cụ dụng cụ" positiontooltip="left"/>
            </div>
            <div class="navbar-body__item tooltip">
                <div class="navbar-body__logo icon icon-category iconleft icon__size-17 opacity" ></div>
                <div v-show="!isShowNavbar" class="navbar-body__text opacity">Danh Mục</div>
                 <Tooltip tooltiptext="Danh mục" positiontooltip="left"/>
            </div>
            <div class="navbar-body__item tooltip">
                <div class="navbar-body__logo icon icon-search iconleft icon__size-23x2 opacity" ></div>
                <div v-show="!isShowNavbar" class="navbar-body__text opacity">Tra cứu</div>
                <div v-show="!isShowNavbar" class="navbar-body__logo icon icon-down opacity"></div>
                 <Tooltip tooltiptext="Tra cứu" positiontooltip="left"/>
            </div>
            <div class="navbar-body__item tooltip">
                <div class="navbar-body__logo icon icon-report iconleft icon__size-22x2 opacity" ></div>
                <div v-show="!isShowNavbar" class="navbar-body__text opacity">Báo cáo</div>
                <div v-show="!isShowNavbar" class="navbar-body__logo icon icon-down opacity"></div>
                 <Tooltip tooltiptext="Báo cáo" positiontooltip="left"/>
            </div>
        </div>
        <div class="navbar-size tooltip"  @click="setNavbar()" v-show="!isShowNavbar">
            
                <div class="navbar-size__icon icon icon-zoomout icon__size-22">
                    <Tooltip tooltiptext="Thu nhỏ" positiontooltip="left"/>
                </div>
        </div>


    </div>
    
</template>
<script>
import Tooltip from '../base/TheTooltip.vue'
export default {
    name: "TheNavbar",
   components:{
    Tooltip
   },
      data() {
            return {
                isShowNavbar: false,
                isShowIcon:false
            };
        },
    methods: {
        //
        /**
         * Xử lý sự kiện navbar
         *  Truyền dữ liệu thay đổi navbar
         *  Author : Bùi Quang Điệp
         *  Date 12/08/2022
         */
        setNavbar() {
            try {
                this.isShowNavbar = true;
                  this.emitter.emit("setWidth");
            } catch (error) {
                console.log(error);
            }

        },
        /**
         * Xử lý sự kiện hiện thị lại kích thước ban đầu
         *  Author : Bùi Quang Điệp
         *  Date 12/08/2022
         */
        reloadSetWidth(){
        try {
                this.isShowNavbar = false;
                  this.emitter.emit("setWidthClear");
            } catch (error) {
                console.log(error);
            }
        }
         /**
         * Xử lý sự kiện click thì hiển thị icon active
         *  Author : Bùi Quang Điệp
         *  Date 12/08/2022
         */
        // ClickShowIcon(e){
        //    e.childNodes[0].classList.add("")
        // }
      
    }
}
</script>
<style scoped>

</style>