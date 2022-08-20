<template>
<div class ="container">
    <Navbar/>
    <Body/>
</div>
      

</template>

<script>
import Navbar from "./components/layouts/TheNavbar.vue"
import Body from "./components/layouts/TheBody.vue"
export default {
    name: 'QuanlytaisanAssetView',
    components:{
        Navbar,Body
    },
    data() {
        return {
            
        };
    },

    methods: {        
        
    },
};
</script>

<style scoped>

</style>