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
                <button class="btn btn-confirm" ref="btnConfirm" tabindex="1" @click="HandleAsset()" >
                    {{buttonNames[0]}}
                </button>
                <button v-if="buttonNames[1] != undefined " class="btn btn-cancel" ref="btnCancel" style="margin:0 10px;" @keydown.tab.prevent="isFocusConfirm()" tabindex="2" @click="RemoveDialog()">
                   {{buttonNames[1]}}
                </button>
                  <button v-if="buttonNames[2] != undefined " class="btn btn-cancel" @keydown.tab.prevent="isFocusConfirm()" tabindex="3" @click="this.$emit('isShowDialogNotify')">
                    {{buttonNames[2]}}
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
        dataAsset:Array,
        buttonNames:Array,
        handleButton:String
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

        /***
         * xử lý chức năng xóa , xóa nhiều hoặc trả về giá trị lưu cho dialog
         * Author:Bùi Quang Điệp
         * Date:15/08/2022
         * 
         */
        HandleAsset(){
            try {
                if(this.buttonNames.length==1)
                {
                         this.$emit("isShowDialogNotify");
                }
                // Kiểm tra chức năng là lưu hay thêm mới
                if(this.buttonNames[2] != undefined)
                {
                        this.$emit("isSaveData");
                }
                else{
                            // Xóa 1 bản ghi
                        if(this.dataAsset.length == 1)
                        {
                            axios.post(`http://localhost:13846/api/v1/FixedAssets/delete?id=${this.dataAsset[0].fixedAssetID}`)
                            .then(res=>{
                                    console.log(res);
                                    this.$emit("isShowDialogNotify");
                                    this.emitter.emit("LoadData");
                            })
                            .catch(res=>{
                                console.log(res);
                            })
                        }
                        // Xóa nhiều bản ghi
                        if(this.dataAsset.length>1){
                            var idAssets = [];
                            for(let i =0 ;i<this.dataAsset.length ; i++)
                            {
                                idAssets.push(this.dataAsset[i].fixedAssetID);
                            }
                            axios.post(`http://localhost:13846/api/v1/FixedAssets/deleteMulti`,idAssets)
                            .then(res=>{
                                console.log(res);
                                    this.$emit("isShowDialogNotify");
                                    this.emitter.emit("LoadData");
                            })
                            .catch(res=>{
                                console.log(res);
                            })
                        }
                }
                
                

            } catch (error) {
                console.log(error);
            }
        },

        /***
         * Truyền dữu liệu đóng về cho thằng chứa Dialog này
         * Author:Bùi Quang Điệp
         * Date:15/08/2022
         * 
         */
         RemoveDialog(){
                    
                    try {
                        if(this.buttonNames[2] != undefined)
                        {
                             this.$emit("RemoveDialog");
                        }
                        else{
                            this.$emit("isShowDialogNotify");
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