import { defineStore } from 'pinia'
import { ref, computed } from 'vue'
import { api } from '@/lib/api'
import i18n from '@/plugins/i18n'

interface User { id: string; username: string; email: string; firstName?: string; lastName?: string; preferredLanguage: string; isEmailVerified: boolean; roles: string[] }

export const useAuthStore = defineStore('auth', () => {
  const token = ref<string | null>(localStorage.getItem('ilmai_token'))
  const user = ref<User | null>(null)
  const loading = ref(false)

  const isAuthenticated = computed(() => !!token.value)
  const isAdmin = computed(() => user.value?.roles?.includes('admin') ?? false)

  function syncLanguage(lang?: string) {
    if (lang) {
      localStorage.setItem('user-locale', lang)
      // @ts-ignore - locale is a ref in Composition API mode
      if (i18n.global.locale.value !== lang) {
        // @ts-ignore
        i18n.global.locale.value = lang
      }
    }
  }

  async function login(username: string, password: string) {
    loading.value = true
    try {
      const res = await api.auth.login({ username, password })
      token.value = res.token
      user.value = res.user
      localStorage.setItem('ilmai_token', res.token)
      syncLanguage(res.user.preferredLanguage)
    } finally { loading.value = false }
  }

  async function register(data: any) {
    loading.value = true
    try {
      const res = await api.auth.register(data)
      token.value = res.token
      user.value = res.user
      localStorage.setItem('ilmai_token', res.token)
      syncLanguage(res.user.preferredLanguage)
    } finally { loading.value = false }
  }

  async function fetchUser() {
    if (!token.value) return
    try { 
      const userData = await api.auth.me()
      user.value = userData
      syncLanguage(userData.preferredLanguage)
    } catch { logout() }
  }

  async function logout() {
    try {
      if (token.value) await api.auth.logout()
    } catch (err) {
      console.error('Logout API failed:', err)
    } finally {
      token.value = null
      user.value = null
      localStorage.removeItem('ilmai_token')
      // Redirect to home and force a reload to clear all state
      window.location.href = '/'
    }
  }

  return { token, user, loading, isAuthenticated, isAdmin, login, register, fetchUser, logout }
})
