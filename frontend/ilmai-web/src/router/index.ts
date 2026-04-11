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
    { path: '/verify-email', name: 'verify-email', component: () => import('@/pages/Auth/VerifyEmail.vue') },
    { path: '/settings', name: 'settings', component: () => import('@/pages/Settings.vue'), meta: { requiresAuth: true } },
  ]
})

router.beforeEach(async (to, _from, next) => {
  const auth = useAuthStore()
  
  // If we have a token but no user loaded yet (e.g. on refresh), wait for fetchUser
  if (auth.token && !auth.user) {
    try {
      await auth.fetchUser()
    } catch (err) {
      console.error('Failed to fetch user in router guard:', err)
      return next({ name: 'login' })
    }
  }

  // 1. Logged in check
  if (to.meta.requiresAuth && !auth.isAuthenticated) {
    return next({ name: 'login' })
  }

  // 2. Verification check: If authenticated but NOT verified, force redirect to VerifyEmail
  // (unless already on the VerifyEmail page or login/register)
  const isAuthPage = ['login', 'register', 'verify-email'].includes(to.name as string)
  
  if (auth.isAuthenticated && auth.user && !auth.user.isEmailVerified && !isAuthPage) {
    return next({ 
      name: 'verify-email', 
      query: { email: auth.user.email } 
    })
  }

  next()
})

export default router
