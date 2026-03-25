import { createRouter, createWebHistory } from 'vue-router'
import { useAuthStore } from '@/stores/auth'

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: [
    { path: '/', name: 'home', component: () => import('@/pages/Home.vue') },
    { path: '/chat', name: 'chat', component: () => import('@/pages/Chat.vue'), meta: { requiresAuth: true } },
    { path: '/search', name: 'search', component: () => import('@/pages/Search.vue') },
    { path: '/login', name: 'login', component: () => import('@/pages/Auth/Login.vue') },
    { path: '/register', name: 'register', component: () => import('@/pages/Auth/Register.vue') },
    { path: '/settings', name: 'settings', component: () => import('@/pages/Settings.vue'), meta: { requiresAuth: true } },
  ]
})

router.beforeEach((to, _from, next) => {
  const auth = useAuthStore()
  if (to.meta.requiresAuth && !auth.isAuthenticated) next({ name: 'login' })
  else next()
})

export default router
