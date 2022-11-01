import { createRouter, createWebHistory } from 'vue-router'
import LoginView from '../views/LoginView.vue'
import HomeView from '../views/HomeView.vue'
import Asset from '../components/asset/TheAsset.vue'
import WriteIncrease from '../components/asset/WriteIncrease.vue'
const routes = [
  {
    path: '/login',
    name: 'login',
    component:LoginView
  },
  {
    path: '/home',
    name: 'home',
    component:HomeView
  },
  {
    path : '/asset',
    name : 'asset',
    component : Asset
  },
  {
    path : '/asset/writeincrease',
    name : 'writeincrease',
    component : WriteIncrease
  }
]

const router = createRouter({
  history: createWebHistory(),
  routes
})

export default router
