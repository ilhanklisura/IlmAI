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
    {
      path: '/admin',
      component: () => import('@/layouts/AdminLayout.vue'),
      meta: { requiresAuth: true, requiresAdmin: true },
      children: [
        { path: '', name: 'admin-dashboard', component: () => import('@/pages/Admin/Dashboard.vue') },
        { path: 'users', name: 'admin-users', component: () => import('@/pages/Admin/Users.vue') },
        { path: 'analytics', name: 'admin-analytics', component: () => import('@/pages/Admin/Analytics.vue') },
        { path: 'logs', name: 'admin-logs', component: () => import('@/pages/Admin/Logs.vue') }
      ]
    }
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

  // 2. Admin check
  if (to.meta.requiresAdmin && !auth.isAdmin) {
    return next({ name: 'home' })
  }

  // 3. Verification check: If authenticated but NOT verified, force redirect to VerifyEmail
  // (unless already on the VerifyEmail page or login/register or ADMIN page)
  const isAuthPage = ['login', 'register', 'verify-email'].includes(to.name as string)
  const isAdminArea = to.path.startsWith('/admin')
  
  if (auth.isAuthenticated && auth.user && !auth.user.isEmailVerified && !isAuthPage && !isAdminArea) {
    return next({ 
      name: 'verify-email', 
      query: { email: auth.user.email } 
    })
  }

  next()
})

export default router
