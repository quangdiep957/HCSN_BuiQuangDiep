<template>
   <!-- dialog cảnh báo -->
    <div class="dialog__handle" id="dialog__notify">
        <div class="dialog__notify">
            <div class="dialog__notify--header">
                <div class="icon-warning"><i class="fa-solid fa-triangle-exclamation"></i></div>
                <div  class ="text--notify">
                    <p v-for="item in dataError" :key="item">{{item}}</p>
                </div>
            </div>
            <div class="dialog__notify--footer">
                <button class="btn btn-confirm" ref="btnConfirm" tabindex="1" @click="DeleteAsset()" >
                    Đồng ý
                </button>
                <button class="btn btn-cancel" style="margin:0 10px;" @keydown.tab.prevent="isFocusConfirm()" tabindex="2" @click="this.$emit('isShowDialogNotify')">
                    Không
                </button>
            </div>
        </div>
    </div>
</template>

<script>
import axios from 'axios';
export default {
    name: 'TheDialogNotify',

    data() {
        return {
            ShowDialogNotify:true,
        };
    },

    mounted() {
        this.$refs['btnConfirm'].focus();

    },
    props:{
        dataError:Array,
        dataAsset:Array
    },

    methods: {
        /***
         * Focus vào ô Xác nhận
         * Author:Bùi Quang Điệp
         * Date:13/08/2022
         * 
         */
        isFocusConfirm(){
        this.$refs['btnConfirm'].focus();
        },

        DeleteAsset(){
            debugger
            try {
                // Xóa 1 bản ghi
                if(this.dataAsset.length >= 1)
                {
                    axios.post(`http://localhost:13846/api/v1/FixedAssets/delete?fixedAssetID=${this.dataAsset[0].fixedAssetID}`)
                    .then(res=>{
                        console.log(res);
                            this.$emit("isShowDialogNotify");
                            this.emitter.emit("LoadData");
                    })
                    .catch(res=>{
                         console.log(res);
                    })
                }

               
              
            } catch (error) {
                console.log(error);
            }
        }
    },
    
        
};
</script>

<style lang="css" scoped>

</style>