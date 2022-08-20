import { createApp } from 'vue'
import App from './App.vue'
import router from './router'
import mitt from 'mitt';
import tooltip from './components/base/TheTooltip.vue'
import cors from 'cors';
const emitter = mitt();
const app = createApp(App);
app.config.globalProperties.emitter = emitter;

app.use(router,tooltip,cors).mount('#app');


