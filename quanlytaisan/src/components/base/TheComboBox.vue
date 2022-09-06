<template>
  <!-- combobox -->

  <div class="combobox">
    <div
      v-on:click="btnClickShow()"
      class="combobox__control cbtooltip"
      :class="{ combobox__width: dialog_icon }"
      v-if="!ComboboxQuantity"
    >
      <div
        v-show="icon"
        class="combobox__icon--left icon icon-filter icon__size-14 tooltip"
      >
        <Tooltip
          tooltiptext="Lọc dữ liệu"
          positiontooltip="bottom"
          style="margin-top: 10px"
        />
      </div>
      <input
        type="text"
        name=""
        id=""
        class="input combobox__input"
        v-bind:value="value_input"
        v-bind:placeholder="val"
      />
      <div class="combobox__icon--right icon icon-down1 icon__size-5 tooltip">
        <Tooltip
          tooltiptext="hiển thị"
          positiontooltip="bottom"
          style="margin-top: 10px"
        />
          <!-- vị trí tooltip -->
        <div id="TooltipComBoBox" class="tooltip" v-show="isShowTooltip">
          <Tooltip
              :tooltiptext="optionText"
                positiontooltip="bottom"
                id ="tooltipText"
              />
           </div>
      </div>
    </div>
    <div
      v-show="isShow"
      class="combobox__option"
      v-if="optionTable == false"
      v-clickoutside="hideListData"
    >
      <div
      v-for="(item,index) in option" :key="item"
        class="combobox__option--item tooltip tooltipCombobox"
        @click="btnTickedBody(item)"
        ref="item"
        @mouseover="GetPos(item,index)"
        @mouseleave="RemovePos()"
        :class="{ 'combobox__option--active': isTicked == item }"
        style="position: relative">
        <span v-if="isTicked == item" class="combobox__option--item-icon"
          ><i class="fa-solid fa-check"></i
        ></span>
        <div v-if="typeCombobox == 'department'" class="combobox__option--item-text">
          {{ item.DepartmentName}}
        </div>
        <div v-if="typeCombobox == 'category'" class="combobox__option--item-text">
          {{ item.FixedAssetCategoryName }}
        </div>
   
      </div>
    </div>
    <!-- combobox phân trang   -->
    <div
      class="combobox__control combox--number"
      v-if="ComboboxQuantity"
      v-on:click="isShowCombo = true"
    >
      <input
        type="text"
        name=""
        id=""
        class="input combobox__input--number"
        v-bind:value="value_quantity"
      />
      <div
        class="combobox__icon--right icon icon-down1 icon__size-5 tooltip"
        title="chọn để chọn các thành phần"
      ></div>
    </div>
    <div
      v-show="isShowCombo"
      class="combobox__option combobox_quantity"
      v-if="optionTable == false"
      @mouseleave="isShowCombo = false"
    >
      <div
        v-for="item in combobox_quantity"
        :key="item"
        class="combobox__option--item"
        v-on:click="btnTickedQuantity(item)"
        :class="{ 'combobox__option--active': isTicked == item }"
      >
        <div class="combobox__option--item-text" style="padding: 0; margin-left: 40%">
          {{ item }}
        </div>
      </div>
    </div>

    <!-- option combobox table  -->

    <div v-show="isShow" class="combobox__option remove__Combobox" v-if="optionTable">
      <div class="combobox__option--table">
        <table style="width: 100%" class="table">
          <thead class="combobox__table--header">
            <tr>
              <th class="text-align-center" style="width: 60px">Mã</th>
              <th class="text-align-left" style="width: calc(100% - 60px)">Tên</th>
            </tr>
          </thead>
          <tbody v-if="typeCombobox == 'department'">
            <tr
              :class="{ 'combobox__option--active': isTicked == item }"
              v-for="item in option"
              :key="item"
              v-on:click="btnTicked(item)"
            >
              <td class="text-align-center idValue">{{ item.DepartmentCode }}</td>
              <td class="text-align-left NameValue">{{ item.DepartmentName }}</td>
            </tr>
          </tbody>
          <tbody v-if="typeCombobox == 'category'">
            <tr
              :class="{ 'combobox__option--active': isTicked == item }"
              v-for="item in option"
              :key="item"
              v-on:click="btnTicked(item)"
            >
              <td class="text-align-center idValue">{{ item.FixedAssetCategoryCode }}</td>
              <td class="text-align-left NameValue">{{ item.FixedAssetCategoryName }}</td>
            </tr>
          </tbody>
        </table>
      </div>
    </div>
  </div>


</template>
<script>
/**
 * Gán sự kiện nhấn click chuột ra ngoài combobox data (ẩn data list đi)
 * Bùi Quang Điệp (27/08/2022)
 */
const clickoutside = {
  mounted(el, binding) {
    try {
      el.clickOutsideEvent = (event) => {
     
           // Nếu element hiện tại không phải là element đang click vào
      // Hoặc element đang click vào không phải là button trong combobox hiện tại thì ẩn đi.
        if(el.previousElementSibling != null)
        {
          if (
        !((
            el === event.target || // click phạm vi của combobox__data
            el.contains(event.target) || //click vào element con của combobox__data
            (el.previousElementSibling.contains(event.target))
          ))
      ) {
        // Thực hiện sự kiện tùy chỉnh:
        binding.value(event, el);
      }
        }
        
      
     
      
      
    };
      
    } catch (error) {
      console.log(error)
      
    }
   
    document.body.addEventListener("click", el.clickOutsideEvent);
  },
  beforeUnmount: (el) => {
    console.log("beforeUnmount");
    document.body.removeEventListener("click", el.clickOutsideEvent);
  },
};
// import axios from "axios"
import Tooltip from "../base/TheTooltip.vue";
export default {
  name: "ComboBox",
  components: {
    Tooltip,
  },
  directives: {
    clickoutside,
  },

  data() {
    return {
      left:"0px",
      top:"0px",
      opacity:0,
      isShow: false,
      isTicked: "",
      isShowCombo: false,
      value_input: "",
      value_quantity: "20",
      combobox_quantity: [10, 20, 30, 40,50],
      optionText:"",
      isShowTooltip : false,
      handlerCombobox:true,
      handlerOne:false,
      handlerTwo:false,
      itemDetail:[],
      searchArray: {
        keyword: "",
        categoryAssetID: "",
        departmentID: "",
        dataPage: {
          pageSize: "",

        },
      },
    };
  },
  props: {
    icon: {
      type: Boolean,
    },
    val: {
      type: String,
    },
    option: {
      type: Array,
    },
    dialog_icon: {
      type: Boolean,
    },
    optionTable: {
      type: Boolean,
      default: false,
    },
    ComboboxQuantity: {
      type: Boolean,
      default: false,
    },
    typeCombobox: {
      type: String,
    },
    dataOption: {
      type: Array,
    },
    isShowInDialog: {
      type: Boolean,
    },
    dataItemCombobox :{
      type:Array
    },
    categoryCheck:{
      type: Boolean,
    },
    departmentCheck:{
      type: Boolean,
    }
  },

  methods: {
     /**
      * click hiển thị option Combobox
      * Author : Bùi Quang Điệp
      * Date:10/08/2022
      * */
    btnClickShow() {
      debugger;
      this.isShow = true;
    },

      /**
      * click ra ngoài thì ẳn combobox
      * Author : Bùi Quang Điệp
      * Date:10/08/2022
      * */
    hideListData() {
      this.isShow = false;
    },

      /**
      * Khi chọn 1 hàng thì focus vào hàng đó
      * Author : Bùi Quang Điệp
      * Date:10/08/2022
      * */
    btnTicked(item) {
      debugger
      this.isTicked = item;
      this.isShow = false;
      // Kiểm tra xem combobox đang hiển thị bảng nào
      if (this.typeCombobox == "department") {
        this.value_input = item.DepartmentCode;
        this.$emit("dataName", item);
        if(this.departmentCheck == true)
        {
          this.$emit("departmentCheckfalse");
          this.$emit("RemoveDataOne");
        }
        
      }
      if (this.typeCombobox == "category") {
        this.value_input = item.FixedAssetCategoryCode;
        this.$emit("dataName", item);
        if(this.categoryCheck == true)
        {
          this.$emit("categoryCheckfalse");
          this.$emit("RemoveDataTwo");
        }
        
      }
   
    },

      /**
      * click hiểm thị option Combobox của combobox ngoài giao diện chính
      * Author : Bùi Quang Điệp
      * Date:10/08/2022
      * */
    btnTickedBody(item) {
      if(this.isTicked == item)
      {
        this.isTicked = "";
        this.value_input = "";
         this.isShow = false;
        this.$emit("remove", this.isTicked);
      }
      else{
        this.isTicked = item;
      this.isShow = false;
      if (this.typeCombobox == "department") {
        this.value_input = item.DepartmentName;
        this.$emit("dataDepartment", item);
      }
      if (this.typeCombobox == "category") {
        this.value_input = item.FixedAssetCategoryName;
        this.$emit("dataCategory", item);
      }
      }
      
    },

      /**
      * click hiểm thị option Combobox của combobox chọn số lượng bản ghi
      * Author : Bùi Quang Điệp
      * Date:10/08/2022
      * */
    btnTickedQuantity(item) {
      this.isTicked = item;
      this.isShowCombo = false;
      this.value_quantity = item;
      this.dataPage = item;
      this.emitter.emit("dataPageSize", this.dataPage);
    },


  /**
      * lấy vị trí hiện tại cảu combobox Option
      * Author : Bùi Quang Điệp
      * Date:10/08/2022
      * */
    GetPos(item,index) {
      debugger
      this.opacity = 1;
         //const left = this.$refs.item[index].getBoundingClientRect().left;
         const top = this.$refs.item[index].getBoundingClientRect().top;
        // Gán giá trị tooltip
        
        if(item.FixedAssetCategoryName != undefined)
        {
          if(item.FixedAssetCategoryName.length >20)
          {
            this.isShowTooltip = true;
            this.optionText = item.FixedAssetCategoryName;
          }
         
        }
        else{
          if(item.DepartmentName.length >20)
          {
            this.isShowTooltip = true;
            this.optionText = item.DepartmentName;
          }
          
        }
      

        // Gán vị trí của tooltip combobox
        this.left = 30 + "px";
        this.top = top - 90 + "px";

    },

    /**
      * Khi không hover thì đóng tooltip và xóa text cũ
      * Author : Bùi Quang Điệp
      * Date:10/08/2022
      * */
    RemovePos(){
      this.isShowTooltip = false;
      this.optionText = "";

    }

   
  },

    /**
      * Gán dữ liệu cho các combobox trong dialog khi sửa tài sản
      * Author : Bùi Quang Điệp
      * Date:25/08/2022
      * */
  beforeUpdate(){
      debugger
       if(this.dataItemCombobox != undefined)
      {
            if(this.optionTable && this.dataItemCombobox.departmentID != "" && this.dataItemCombobox.fixedAssetCategoryID != "")
        {
            if(this.typeCombobox == "category") 
                      {
                        if(this.categoryCheck == false){
                          for(let i =0 ;i<this.option.length ;i++)
                                {
                                  if(this.option[i].FixedAssetCategoryID == this.dataItemCombobox.fixedAssetCategoryID)
                                  {
                                    this.$emit("categoryCheck");
                                      this.btnTicked(this.option[i]);
                                      
                                  }
                                }
                        }
                          
                      }
                    else{
                      if(this.departmentCheck == false)
                      {
                        for(let i =0 ;i<this.option.length ;i++)
                      {
                        if(this.option[i].DepartmentID == this.dataItemCombobox.departmentID)
                        {
                          this.$emit("departmentCheck");
                            this.btnTicked(this.option[i]);
                           

                            
                        }
                      }
                      }
                       
                    }
                
          
     
                  
          }
     
      }
     
    }
};
</script>

<style scoped>
@import url(../../css/component/combobox.css);
@import url(../../css/component/input.css);
#app {
  background-color: #f4f7ff;
}
#TooltipComBoBox {
        left : v-bind('left');
        top : v-bind('top');
    }
#tooltipText{
  opacity: v-bind('opacity');
  position:initial;
}
</style>
