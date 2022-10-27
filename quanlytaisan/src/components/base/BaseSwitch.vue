<template>
   <div class="switch" @click="showSwitch=!showSwitch;this.$emit('changeSwitch',showSwitch,name)">
             <div class="circle" :class="{'circle_show':showSwitch , 'circle_hide':showSwitch==false}"></div>
     </div>
</template>

<script>
export default {
    name: 'QuanlytaisanBaseSwitch',

    data() {
        return {
            showSwitch:true
        };
    },

    mounted() {
        
    },
    props:{
        name:String
    },

    methods: {
        
    },
};
</script>

<style lang="css" scoped>
.switch
{
    width: 52px;
    height: 24px;
    border-radius: 15px;
    background-color: #eee;
    
}
.circle_show{
    background-color: rgb(26 164 200 / 86%);
    left: 25px;
}
.circle_hide{
    background-color: gray;
    left: 3px;
}
.circle{
    width: 20px;
    height: 20px;
    margin: 2px;
    position: relative;
    border-radius: 50%;
    transition: all 0.5s ease;
   
}

</style>