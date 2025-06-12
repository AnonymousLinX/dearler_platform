import { createApp } from 'vue'
import App from './App.vue'
import router from './router'
import axios from 'axios';
import { createPinia } from 'pinia';
import './HttpRequests/request'

axios.defaults.baseURL = "http://127.0.0.1:5251/";

const pinia = createPinia()
const app = createApp(App)

app.use(pinia)
.use(router)
.mount('#app')