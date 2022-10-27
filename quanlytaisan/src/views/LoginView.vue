<template>
 <div class="background">  
  <div class="dialog">
    <div class="dialog-left"></div>
    <div class="dialog-right">
      <div class="dialog-right__header">
        <div class="dialog-right__logo"></div>
        <div class="dialog-right__text">Đăng nhập để làm việc với <span style="font-weight:bold">MISA QLTS</span></div>
      </div>
      <div class="dialog-right__body">
        <form @keypress.enter="login">
              <div class="group-input">
              <span v-show="isShowMessage"  style="color:red;position:absolute;top:-20px">{{message}}</span>
              <input type="text" class="input" v-model="userName" placeholder="Tên đăng nhập" />
              <div>
                <input :type=typeInput class="input" v-model ="password" placeholder="password" />
              <span class="eyePassword" v-show="isShowEye" @click="isShowEye = !isShowEye"><i class="fa-solid fa-eye"></i></span>
              <span class="eyePassword" v-show="isShowEye==false"  @click="isShowEye = !isShowEye"><i class="fa-sharp fa-solid fa-eye-slash"></i></span>
              </div>
              <button class="input" style="background-color:#1a73e8;color:#fff" @click="login" >Đăng nhập</button>
              <div style="color:#1a73e8;text-align: center;">Quên mật khẩu?</div>
            </div>
        </form>
       
         

      </div>

    </div>
  </div>
 </div>

</template>
<script>
  import { VueCookieNext } from 'vue-cookie-next'
import {API} from '@/js/callapi'
import Resource from '@/js/resource'
export default {
  components:{
  },
  data() {
   
    return {
      userName:'',
    password:'',
    message:'',
    isShowMessage:false,
    isShowEye:false,
    typeInput:'password'
     
    }
  },
  watch:{
    'isShowEye':function(newValue,oldValue)
    {
      console.log(oldValue);
      if(newValue == false)
      {
        this.typeInput ="password"
      }
      else{
        this.typeInput ="text"
      }
    }
  },

  

    methods: {
  
        /**
        * Xử lý sự kiện login
        * Author : Bùi Quang Điệp
        * Date:29/09/2022
        */
    login()
    {
      try {
        if(this.userName != '' && this.password != '')
        {
              var data ={
              UserName : this.userName,
              Password : this.password
            }
            API.post(Resource.APIs.UserLogIn,
            JSON.stringify(data),)
            .then(res=>{
              
              VueCookieNext.setCookie('login', res.data)
              VueCookieNext.setCookie("userName", this.userName);
              var token = VueCookieNext.getCookie('login') 
              console.log(token);
              //localStorage.setItem('YourItem', res.data)
              this.emitter.emit("redirect");
              

            })
            .catch(error=>{
              if(error.response.data == false)
              {
                this.isShowMessage = true;
                this.message="Tài khoản hoặc mật khẩu không chính xác!";
                setTimeout(()=>{
                  this.isShowMessage = false;
                  this.message = "";
                },5000)
              }
              console.log(error);
            })
        }
        else{
          this.message = "Vui lòng nhập đầy đủ thông tin!";
        }
      
      } catch (error) {
        
        console.log(error);
      }
    }
  },
}
</script>
<style scoped>
.background{
  background-image: url(../assets/icon/Op3-Background-1920.png);
  width: 100%;
  height: 100%;
  position: absolute;
  top: 0;
  left: 0;
  z-index: 99998;
}
.dialog{
  display: flex;
    width:  50%;
    height: 70%;
    position: absolute;
    left: 50%;
    top:50%;
    min-width: 770px;
    transform: translate(-50%,-50%);
    background-color:#fff;
    border-radius: 3px;
    z-index: 99999;
    
}
.dialog-right{
  width: 50%;
  padding: 0 20px;
}
.dialog-left{
  background-image: url(../assets/icon/img_Login-Local.png);
  background-repeat: no-repeat;
  background-size: cover;
  width: 50%;
  height: 100%;
}.dialog-right__header{
  margin: 40px 30px 30px 30px;
}
.dialog-right__logo{
  background-image: url(../assets/icon/LogoMISA_VN.svg);
  height: 50px;
  background-position: 50%;
  background-repeat: no-repeat;
}
.group-input{

}
.input{
    outline: none;
    height: 50px;
    border: 1px solid #afafaf;
    border-radius:3px ;
    padding:0;
    margin: 0;
    cursor: pointer;
    text-indent: 14px;
    font-size: 14px;
    width: 100%;
    margin-bottom: 20px;
}
.input:focus{
    border-color: #1aa4c8;
}
.dialog-right__text{
  margin-top: 30px;

  text-align: center;
}
.input::placeholder{
    font-style:normal !important ;
   
   
}
.eyePassword{
  position: absolute;
  right: 20px;
  top:85px
}

</style>