<template>
                          <!-- combobox -->
                          
                        <div  class="combobox">
                            <div v-on:click="btnClickShow()" class="combobox__control" :class="{'combobox__width':(dialog_icon)}" v-if="!ComboboxQuantity">
                                <div v-show="icon" class="combobox__icon--left icon icon-filter icon__size-14 tooltip">   <Tooltip tooltiptext="Lọc dữ liệu" positiontooltip="bottom" style="margin-top:10px"/></div>
                                <input type="text" name="" id="" class="input combobox__input" v-bind:value="value_input" v-bind:placeholder="val">
                                <div class="combobox__icon--right icon icon-down1 icon__size-5 tooltip"><Tooltip tooltiptext="hiển thị" positiontooltip="bottom" style="margin-top:10px"/></div>
                            </div>
                             <div v-show="isShow" class="combobox__option" v-if="optionTable==false" @mouseleave="isShow=false">
                                <div v-for="item in option" :key="item" class="combobox__option--item" v-on:click="btnTickedBody(item)" :class="{'combobox__option--active':(isTicked == item)}">
                                    <span v-if="isTicked == item" class="combobox__option--item-icon"><i class="fa-solid fa-check"></i></span>
                                    <div v-if="typeCombobox=='department'" class="combobox__option--item-text">{{item.DepartmentName}}</div>
                                    <div v-if="typeCombobox=='category'" class="combobox__option--item-text">{{item.FixedAssetCategoryName}}</div>
                                    
                                </div>
                             
                            </div>
                            <div class="combobox__control combox--number" v-if="ComboboxQuantity" v-on:click="isShowCombo = true">
                                            <input type="text" name="" id="" class="input combobox__input--number" v-bind:value="value_quantity">
                                            <div class="combobox__icon--right icon icon-down1 icon__size-5 tooltip"
                                                title="chọn để chọn các thành phần"></div>
                            </div>
                            <div v-show="isShowCombo" class="combobox__option combobox_quantity" v-if="optionTable==false" @mouseleave="isShowCombo=false">
                                <div v-for="item in combobox_quantity" :key="item" class="combobox__option--item" v-on:click="btnTickedQuantity(item)" :class="{'combobox__option--active':(isTicked == item)}">
                                    <div class="combobox__option--item-text" style="padding:0;margin-left:40%">{{ item}}</div>
                                </div>
                             
                            </div>
                            
                           
                           <div v-show="isShow" class="combobox__option remove__Combobox" v-if="optionTable" @mouseleave="isShow=false">
                                <div class="combobox__option--table">
                                    <table style="width: 100%;" class ="table">
                                        <thead class ="combobox__table--header">
                                            <tr>
                                                <th class ="text-align-center" style="width:60px">Mã</th>
                                                <th class ="text-align-left" style="width:calc(100% - 60px)">Tên</th>
                                            </tr>
                                            
                                        </thead>
                                        <tbody v-if="typeCombobox=='department'">
                                            <tr :class="{'combobox__option--active':(isTicked == item)}" v-for="item in option" :key="item"  v-on:click="btnTicked(item)" >
                                                <td class ="text-align-center idValue">{{item.DepartmentCode}}</td>
                                                <td  class ="text-align-left NameValue">{{item.DepartmentName}}</td>
                                            </tr>
                                          
                                        </tbody>
                                        <tbody v-if="typeCombobox=='category'">
                                            <tr :class="{'combobox__option--active':(isTicked == item)}" v-for="item in option" :key="item"  v-on:click="btnTicked(item)" >
                                                <td class ="text-align-center idValue">{{item.FixedAssetCategoryCode}}</td>
                                                <td  class ="text-align-left NameValue">{{item.FixedAssetCategoryName}}</td>
                                            </tr>
                                          
                                        </tbody>
                                    </table>
                                
                                
                            </div>
                            </div>
                    
                        </div>
                        
</template>
<script>
// import axios from "axios"
import Tooltip from "../base/TheTooltip.vue"
export default{
    
    name : "ComboBox",
    components:{
        Tooltip
    },
    data() {
        return {
            isShow : false,
            isTicked : "",
            isShowCombo:false,
            value_input : "",
            value_quantity:"10",
            combobox_quantity:[10,20,30,40],
              searchArray :{
                keyword : "",
                categoryAssetID : "",
                departmentID : "",
                dataPage:{
                    pageSize:""
                }

            }
          
        }
    },
    props:{
        icon: {
            type : Boolean
        },
        val:{
            type : String
        },
        option:{
            type: Array
        },
        dialog_icon:{
              type : Boolean
        },
        optionTable:{
            type:Boolean,
            default : false
        },
        ComboboxQuantity:{
            type:Boolean,
            default:false
        },
        typeCombobox:{
            type:String
        },
        dataOption:{
            type:Array
        }
        

    },
    methods:{
        btnClickShow(){
            this.isShow = true;
        },

        btnTicked(item){
            this.isTicked  = item ;
            this.isShow = false;
            if(this.typeCombobox=="department")
            {
                this.value_input = item.DepartmentCode;
                this.$emit('dataName', item);
            }
            if(this.typeCombobox == "category"){
                  this.value_input = item.FixedAssetCategoryCode;
                 this.$emit('dataName', item);
            }

        },
        btnTickedBody(item){
            this.isTicked  = item ;
            this.isShow = false;
            if(this.typeCombobox=="department")
            {
                this.value_input = item.DepartmentName;
                this.$emit('dataDepartment', item);
            }
            if(this.typeCombobox == "category"){
                  this.value_input = item.FixedAssetCategoryName;
                 this.$emit('dataCategory', item);
            }

        },
          btnTickedQuantity(item){
             this.isTicked  = item ;
            this.isShowCombo = false;
            this.value_quantity = item;
            this.dataPage = item;
            this.emitter.emit('dataPageSize', this.dataPage);
        }

    },
    // created(){
    //   try {
    //     var me = this; 
    //     console.log("a");
    //     axios.get("https://amis.manhnv.net/api/v1/Employees").then((response) => {
    //     console.log(response.data);
    //     me.employee = response.data;
    //   }).catch(res=>{
    //     console.log(res);
    //   })
    //     // cách 2 dùng fetch
    //     fetch("")
    //   } catch (error) {
    //     console.log(error)
    //   }
    
   
    // },
    

}
</script>

<style scoped>
@import url(../../css/component/combobox.css);
@import url(../../css/component/input.css);
#app{
    background-color: #f4f7ff;
}
</style>
