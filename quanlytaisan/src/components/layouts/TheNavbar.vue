<template>
    <div :class="{'setnavbar' :isShowNavbar}" class="navbar">

        <div class="navbar-header">
            <div class="navbar-header__item" style="position:relative;">
                <div class="navbar-header__logo icon tooltip" title="" @click="reloadSetWidth">
                    <Tooltip tooltiptext="Mở menu" positiontooltip="left"/>
                </div>
                <div v-show="!isShowNavbar" class="navbar-header__text">MISA QLTS</div>
            </div>
        </div>
        <div class="navbar-body">
            <router-link to="/home">
                <div class="navbar-body__item tooltip" >
                    <div :class="{'icon-overice__active' : (isShowIcon)}" class="navbar-body__logo icon icon-overice iconleft icon__size-22x2 opacity" title="Tổng quan"></div>
                    <div v-show="!isShowNavbar" class="navbar-body__text opacity">Tổng quan</div>
                     <Tooltip tooltiptext="Tổng quan" positiontooltip="left"/>
                </div>
            </router-link>
            <router-link to="/asset" :class="{'router-link-active' : assetIncrement}">
                <div class="navbar-body__item tooltip asset" @click="isShowAssetDetail = !isShowAssetDetail" >
                    <div class="navbar-body__logo icon icon-asset__active iconleft icon__size-22 opacity" ></div>
                    <div v-show="!isShowNavbar" class="navbar-body__text opacity">Tài sản</div>
                    <div v-if="!isShowNavbar" class="navbar-body__logo icon icon-down opacity" @mouseover="hoverDown0 = true" @mouseleave="hoverDown0 = false">
                    </div>
                     <Tooltip tooltiptext="Tài sản" positiontooltip="left"/>
                     <Tooltip  v-if="!isShowNavbar" :class="{'hoverDown':hoverDown0}" tooltiptext="Xem thêm" positiontooltip="left"/>
                </div>
                 <!-- item con -->
                 <div class="assetDetail" v-if="isShowAssetDetail &&!isShowNavbar">
                            <ul>
                                <router-link to="/asset/writeIncrease" class="a" @click="assetIncrement = true"><li><span class="iconArrow"><i class="fa-solid fa-arrow-right-long"></i></span>Ghi tăng</li></router-link>
                                <li>Thay đổi thông  tin</li>
                                <li>Đánh giá lại</li>
                                <li>Tính hao mòn</li>
                                <li>Điều chuyển tài sản</li>
                                <li>Ghi giảm</li>
                                <li>Kiểm kê</li>
                                <li>Khác</li>
                            </ul>
                        </div>
            </router-link>
            <div class="navbar-body__item tooltip" >
                <div class="navbar-body__logo icon icon-asset1 iconleft icon__size-21 opacity" ></div>
                <div v-show="!isShowNavbar" class="navbar-body__text opacity">Tài sản HT-ĐB</div>
                <div v-show="!isShowNavbar" class="navbar-body__logo icon icon-down opacity" @mouseover="hoverDown1 = true" @mouseleave="hoverDown1 = false"></div>
                 <Tooltip tooltiptext="Tài sản hạ tầng đường bộ" positiontooltip="left"/>
                 <Tooltip  v-if="!isShowNavbar" :class="{'hoverDown':hoverDown1}" tooltiptext="Xem thêm" positiontooltip="left"/>
            </div>
            <div class="navbar-body__item tooltip" >
                <div class="navbar-body__logo icon icon-tool iconleft icon__size-22x2 opacity" ></div>
                <div v-show="!isShowNavbar" class="navbar-body__text opacity">Công cụ dụng cụ</div>
                <div v-show="!isShowNavbar" class="navbar-body__logo icon icon-down opacity" @mouseover="hoverDown2 = true" @mouseleave="hoverDown2 = false"></div>
                 <Tooltip tooltiptext="Công cụ dụng cụ" positiontooltip="left"/>
                 <Tooltip  v-if="!isShowNavbar" :class="{'hoverDown':hoverDown2}" tooltiptext="Xem thêm" positiontooltip="left"/>
            </div>
            <div class="navbar-body__item tooltip">
                <div class="navbar-body__logo icon icon-category iconleft icon__size-17 opacity" ></div>
                <div v-show="!isShowNavbar" class="navbar-body__text opacity">Danh Mục</div>
                 <Tooltip tooltiptext="Danh mục" positiontooltip="left"/>
            </div>
            <div class="navbar-body__item tooltip">
                <div class="navbar-body__logo icon icon-search iconleft icon__size-23x2 opacity" ></div>
                <div v-show="!isShowNavbar" class="navbar-body__text opacity">Tra cứu</div>
                <div v-show="!isShowNavbar" class="navbar-body__logo icon icon-down opacity" @mouseover="hoverDown3 = true" @mouseleave="hoverDown3 = false"></div>
                 <Tooltip tooltiptext="Tra cứu" positiontooltip="left"/>
                 <Tooltip  v-if="!isShowNavbar" :class="{'hoverDown':hoverDown3}" tooltiptext="Xem thêm" positiontooltip="left"/>
            </div>
            <div class="navbar-body__item tooltip">
                <div class="navbar-body__logo icon icon-report iconleft icon__size-22x2 opacity" ></div>
                <div v-show="!isShowNavbar" class="navbar-body__text opacity">Báo cáo</div>
                <div v-show="!isShowNavbar" class="navbar-body__logo icon icon-down opacity" @mouseover="hoverDown4 = true" @mouseleave="hoverDown4 = false"></div>
                <Tooltip  v-if="!isShowNavbar" :class="{'hoverDown':hoverDown4}" tooltiptext="Xem thêm" positiontooltip="left"/>
                 <Tooltip tooltiptext="Báo cáo" positiontooltip="left"/>
            </div>
        </div>
        <div class="navbar-size"  @click="setNavbar()" v-show="!isShowNavbar">
                <div class="navbar-size__icon icon icon-zoomout icon__size-22 tooltip" @mouseover="isHover=true" @mouseleave="isHover=false" >
                    <Tooltip tooltiptext="Thu nhỏ" positiontooltip="left" :class="{'showHover' :isHover}" />
                </div>
        </div>


    </div>
    
</template>
<script>
import Tooltip from '../base/BaseTooltip.vue'
export default {
    name: "TheNavbar",
   components:{
    Tooltip
   },
      data() {
            return {
                hoverDown0:false,
                hoverDown1:false,
                hoverDown2:false,
                hoverDown3:false,
                hoverDown4:false,
               isShowAssetDetail:false,
                isHover:false,
                isShowNavbar: false,
                isShowIcon:false,
                assetIncrement:false
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
.showHover{
    opacity: 1 !important;
}
.hoverDown{
    opacity: 1 !important;
}
.assetDetail{
    width: 100%;
    position: relative;
    background-color: #ffffff17;
    color: #ffffff61;
    margin-top: -15px;
    transition: all 3s ease;
}
li{
    padding-left: 45px;
}
a{
    color: #ffffff61 !important;
}
.iconArrow{
    color: #fff;
    position: absolute;
    left: 15px;
    display: none;
}
</style>