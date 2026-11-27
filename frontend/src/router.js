import { createRouter, createWebHistory } from 'vue-router'
import AppealList from './pages/AppealList.vue'
import AppealNew from './pages/AppealNew.vue'
import Login from './pages/Login.vue'

const routes = [
  { path: '/', component: AppealList },
  { path: '/new', component: AppealNew },
  { path: '/login', component: Login },
]

export const router = createRouter({
  history: createWebHistory(),
  routes,
})
