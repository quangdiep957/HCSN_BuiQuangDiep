import { createRouter, createWebHistory } from 'vue-router'
import AboutView from '../views/AboutView.vue'
import table from '../components/base/TheTable.vue'
const routes = [
  {
    path: '/',
    name: 'about',
    component:AboutView
  },
  {
    path : '/table',
    name : 'table',
    component : table
  }
]

const router = createRouter({
  history: createWebHistory(),
  routes
})

export default router
