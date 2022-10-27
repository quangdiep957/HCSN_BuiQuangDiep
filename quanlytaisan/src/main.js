import { createApp } from 'vue'
import App from './App.vue'
import router from './router'
import { VueCookieNext } from 'vue-cookie-next'
import mitt from 'mitt'
import cors from 'cors'
import vi from 'element-plus/es/locale/lang/vi'
import ElementPlus from "element-plus"
import "element-plus/theme-chalk/index.css"
const emitter = mitt();
const app = createApp(App);
app.config.globalProperties.emitter = emitter;
// set default config
VueCookieNext.config({ expire: '7d' })

// set global cookie
VueCookieNext.setCookie('theme', 'default')
VueCookieNext.setCookie('hover-time', { expire: '1s' })
app.use(router,cors,VueCookieNext);
app.use(ElementPlus,{locale:vi});
app.mount('#app');


