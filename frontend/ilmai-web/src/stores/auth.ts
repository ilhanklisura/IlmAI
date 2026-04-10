import { defineStore } from 'pinia'
import { ref, computed } from 'vue'
import { api } from '@/lib/api'

interface User { id: string; username: string; email: string; firstName?: string; lastName?: string; preferredLanguage: string; isEmailVerified: boolean; roles: string[] }

export const useAuthStore = defineStore('auth', () => {
  const token = ref<string | null>(localStorage.getItem('ilmai_token'))
  const user = ref<User | null>(null)
  const loading = ref(false)

  const isAuthenticated = computed(() => !!token.value)
  const isAdmin = computed(() => user.value?.roles?.includes('admin') ?? false)

  async function login(username: string, password: string) {
    loading.value = true
    try {
      const res = await api.auth.login({ username, password })
      token.value = res.token
      user.value = res.user
      localStorage.setItem('ilmai_token', res.token)
    } finally { loading.value = false }
  }

  async function register(data: any) {
    loading.value = true
    try {
      const res = await api.auth.register(data)
      token.value = res.token
      user.value = res.user
      localStorage.setItem('ilmai_token', res.token)
    } finally { loading.value = false }
  }

  async function fetchUser() {
    if (!token.value) return
    try { user.value = await api.auth.me() } catch { logout() }
  }

  function logout() {
    token.value = null
    user.value = null
    localStorage.removeItem('ilmai_token')
  }

  return { token, user, loading, isAuthenticated, isAdmin, login, register, fetchUser, logout }
})
