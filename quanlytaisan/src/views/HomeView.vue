<template>
<TheDialogIncrementVue 
v-model="dataItemDetail.fixedAssetCode"
/>
</template>


<script>
  import {API} from '../js/callApi'
  import Resource  from '@/js/resource';
  import TheDialogIncrementVue from '@/components/base/BaseInputNumber.vue';
export default {
 name: 'QuanlytaisanHomeView',

 data() {
   return {
    a:'demo',
     dataItemDetail :{
      fixedAssetCode:'1223'
     }
   };
 },
 components:{
  TheDialogIncrementVue
 },

 created() {
  API.get(Resource.APIs.Departments)
    .then(response => {
      
      var data = response.data
      console.log(data);
    })
    .catch(e => {
      
      console.log(e);
    })
 },

};
</script>

<style lang="css" scoped>

.block {
 padding: 30px 0;
 text-align: center;
 border-right: solid 1px var(--el-border-color);
 flex: 1;
}
.block:last-child {
 border-right: none;
}
.demonstration {
 display: block;
 color: var(--el-text-color-secondary);
 font-size: 14px;
 margin-bottom: 20px;
}
</style>