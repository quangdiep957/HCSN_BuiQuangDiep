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
                    <div class="header-group__icon display-flex "  v-click-out-side="hideUser">
                        <div class="header-group__icon--item icon__size-24 icon icon-notify tooltip tooltipMenu"> <Tooltip tooltiptext ="Thông báo" positiontooltip="top"/></div>
                        <div class="header-group__icon--item icon__size-24 icon icon-folder tooltip tooltipMenu"><Tooltip tooltiptext="Thư mục" positiontooltip="top"/></div>
                        <div class="header-group__icon--item icon__size-24 icon icon-support tooltip tooltipMenu"><Tooltip tooltiptext="Hỗ trợ" positiontooltip="top"/></div>
                        <div style=" display: flex; align-items: center;justify-content: center;" @click="isShowUser=true" >
                            <div class="header-group__icon--item icon__size-24 icon icon-user tooltip tooltipMenu" ><Tooltip tooltiptext="Người dùng" positiontooltip="top"/></div>
                            <div class="header-group__icon--item icon__size-8 icon icon-down2" style="padding-right: 5px;margin-left:0"></div>
                        </div>
                        <div class="user" v-show="isShowUser" >
                            <ul>
                                <li class="user-name">{{userName}}</li>
                                <li class="user-item"><span><i class="fa-solid fa-user-tie"></i></span> Thông tin tài khoản</li>
                                <li class="user-item"><span><i class="fa-solid fa-key-skeleton"></i></span>Đổi mật khẩu</li>
                                <li @click="isShowSetting=true;isShowUser=false" class="user-item"><span><i class="fa-solid fa-gear"></i></span> Thiết lập</li>
                                <li class="user-item" @click="removeToken();this.isShowUser=false"><span><i class="fa-duotone fa-arrow-right-from-bracket"></i></span> Đăng xuất</li>
                            </ul>
                        </div>
                        <div class="user" v-show="isShowSetting" >
                            <ul>
                                <li class="user-item"><span><i class="fa-solid fa-user-tie"></i></span> Tính tổng <Switch @changeSwitch ="changeSwitch" name ="summary" class="witch-right" />
                                </li>
                                <li class="user-item"><span><i class="fa-solid fa-key-skeleton"></i></span>Paging <Switch @changeSwitch ="changeSwitch" name ="paging"  class="witch-right" /></li>
                                <li class="user-item" @click="isShowRadio=true"><span><i class="fa-solid fa-gear"></i></span> Định dạng ngày tháng</li>
                                <li class="user-item"><span><i class="fa-solid fa-gear"></i></span> Định dạng giá tiền <Switch @changeSwitch ="changeSwitch" name ="cost"  class="witch-right" /></li>
                            </ul>
                        </div>
                        <div class="radioFormat" v-if="isShowRadio">
                            <span>Chọn kiểu</span>
                            <ul>
                                <li><input type="radio" name="date" checked="checked" @input="sendFormat('DMY')" />DD/MM/YYYY({{formatDate(dateNow,'DMY')}})</li>
                                <li><input type="radio" name="date"  @input="sendFormat('MDY')" />MM/DD/YYYY({{formatDate(dateNow,'MDY')}})</li>
                                <li><input type="radio" name="date"  @input="sendFormat('YDM')" />YYYY/DD/MM({{formatDate(dateNow,'YDM')}})</li>
                                <li><input type="radio" name="date"  @input="sendFormat('YMD')" />YYYY/MM/DD({{formatDate(dateNow,'YMD')}})</li>
                            </ul>
                        </div>
                       
                       
                    </div>
                </div>

            </div>
</template>

<script>

import {formatDate} from   '@/js/common'
import Tooltip from '../base/BaseTooltip.vue'
import { VueCookieNext } from 'vue-cookie-next'
import Resource from '../../js/resource'
import {API} from '../../js/callapi'
import Switch from '../base/BaseSwitch.vue'
import clickOutSide from "@mahdikhashan/vue3-click-outside";
export default {
    name: "TheHeader",
    components: {
        Tooltip,Switch
    },
    directives: {
        clickOutSide,
  },
    data(){
        return{
            isShowUser:false,
            userName:'Tên đăng nhập',
            isShowSetting:false,
            dateNow:new Date(),
            isShowRadio:false
           
        }
    },
    methods:{
         /**
         * ẩn form user khi click ra ngoài
         * Created By: Bùi Quang Điệp
         * Date:01/10/2022
         */
        hideUser()
        {
            
            this.isShowUser=false,
            this.isShowSetting=false,
            this.isShowRadio = false
        },
        formatDate,
         /**
         * gửi dữ liệu khi trạng thái thay đổi
         * Created By: Bùi Quang Điệp
         * Date:01/10/2022
         */
        changeSwitch(item,name)
        {
            var dataSwitch ={
                item:item,
                name:name
            }

            this.emitter.emit('switchName',dataSwitch)
        },

         /**
         * gửi format ngày tháng
         * Created By: Bùi Quang Điệp
         * Date:01/10/2022
         */
        sendFormat(format)
        {
        
            var value = '';
            if(format == 'DMY')
            {
                value = Resource.Date.DMY
            }
            else if(format == 'MDY')
            {
                value = Resource.Date.MDY
            }
            else if(format == 'YDM')
            {
                value = Resource.Date.YDM
            }
            else if(format == 'YMD')
            {
                value = Resource.Date.YMD
            }
            this.emitter.emit("dateFormat",value);
            this.isShowRadio= false;
        },
        /**
         * Xóa token khi ấn đăng xuất
         * Created By: Bùi Quang Điệp
         * Date:01/10/2022
         */
        removeToken()
        { 
           
            var user={
                userName:this.userName,
                password:''
            }
            API.post(Resource.APIs.UserLogOut,user)
          
            .catch(error=>{
                console.log(error);
            })
            // xóa dữ liệu cũ
            this.emitter.emit('removeAll');
            VueCookieNext.removeCookie('login');
            this.emitter.emit("redirect")
        }
    },
    mounted(){
        var name = VueCookieNext.getCookie("userName");
        if(name)
        {
            this.userName = name;
        }
        else{
            this.userName="Tên đăng nhập";
        }
    }
}
</script>
<style>
.user{
    position: absolute;
    width: 300px;
    height: auto;
    right:-5px;
    top:20px;
    background-color: #fff;
    box-shadow: 0 5px 15px 0 rgb(0 0 0 / 5%);
    z-index: 999;
    border-radius: 4px;

}
.user-name{
    text-align: center;
    font-size: 15px;
    border-bottom:1px solid #000 ;
    justify-content: center;
}
li{
    list-style: none;
    height: 40px;
    line-height: 40px;
    display: flex;
    align-items: center;
}
li:hover{
    background-color: rgba(26, 164, 200, .2);
}
.user-item{

}
.user-item span{
    margin: 0 20px;
}
ul{
    padding: 0 ;
}
.witch-right{
    position: absolute;
    right: 20px;
}
.radioFormat{
    position: absolute;
    right: 296px;
    top: 19px;
    z-index: 10000;
    background-color: #fff;
    white-space: nowrap;
    padding: 10px 20px;
}

</style>

