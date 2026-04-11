const getEnvVar = (val: any, fallback: string) => {
  if (!val || val === 'undefined' || val === 'null') return fallback
  return val
}

const API_URL = getEnvVar(import.meta.env.VITE_API_URL, 'http://localhost:5030')
const API_BASE = getEnvVar(import.meta.env.VITE_API_BASE, `${API_URL}/api`)


function getToken(): string | null { return localStorage.getItem('ilmai_token') }

async function request<T>(method: string, path: string, body?: unknown): Promise<T> {
  const headers: Record<string, string> = { 'Content-Type': 'application/json' }
  const token = getToken()
  if (token) headers['Authorization'] = `Bearer ${token}`

  const res = await fetch(`${API_BASE}${path}`, {
    method,
    headers,
    body: body ? JSON.stringify(body) : undefined
  })

  if (res.status === 204) {
    return null as T
  }

  if (!res.ok) {
    const err = await res.json().catch(() => ({ message: res.statusText }))
    throw new Error(err.message || res.statusText)
  }

  const text = await res.text()
  return text ? JSON.parse(text) : ({} as T)
}

export const api = {
  auth: {
    login: (data: { username: string; password: string }) => request<any>('POST', '/auth/login', data),
    register: (data: any) => request<any>('POST', '/auth/register', data),
    me: () => request<any>('GET', '/auth/me'),
    changePassword: (data: { currentPassword: string; newPassword: string; confirmNewPassword: string }) => request<any>('POST', '/auth/change-password', data),
    verifyEmail: (data: { email: string; code: string }) => request<any>('POST', '/auth/verify-email', data),
    resendCode: (email: string) => request<any>('POST', '/auth/resend-code', { email }),
    logout: () => request<any>('POST', '/auth/logout'),
  },
  admin: {
    getStats: () => request<any>('GET', '/admin/stats'),
    getUsers: () => request<any[]>('GET', '/admin/users'),
    getAnalytics: () => request<any>('GET', '/admin/analytics'),
    getActivityAnalytics: () => request<any>('GET', '/admin/activity-analytics'),
    getLogs: (level?: string) => request<any[]>('GET', `/admin/logs${level ? '?level=' + level : ''}`),
    exportLogsUrl: (format: string) => `${API_URL}/api/admin/logs/export/${format}`,
    toggleUser: (id: string) => request<any>('PATCH', `/admin/users/${id}/toggle-active`),
    resetPassword: (id: string) => request<any>('POST', `/admin/users/${id}/reset-password`),
    updateUserRole: (id: string, roleName: string) => request<any>('PATCH', `/admin/users/${id}/role`, { roleName }),
  },
  chat: {
    send: (data: { sessionId?: string; question: string; language: string }) => request<any>('POST', '/chat/send', data),
    sessions: () => request<any[]>('GET', '/chat/sessions'),
    messages: (sessionId: string) => request<any[]>('GET', `/chat/sessions/${sessionId}/messages`),
    deleteSession: (sessionId: string) => request<any>('DELETE', `/chat/sessions/${sessionId}`),
    renameSession: (id: string, title: string) =>
      request<any>('PUT', `/chat/sessions/${id}/title`, { title })
  },
  quran: {
    search: (query: string, lang = 'bs') => request<any[]>('GET', `/quran/search?query=${encodeURIComponent(query)}&language=${lang}`),
    surahs: () => request<any[]>('GET', '/quran/surahs'),
  },
  hadith: {
    search: (query: string, lang = 'bs') => request<any[]>('GET', `/hadith/search?query=${encodeURIComponent(query)}&language=${lang}`),
  },
  daily: {
    get: (lang = 'bs') => request<any>('GET', `/daily?language=${lang}`),
  },
  search: {
    query: (data: { query: string; language: string }) => request<any[]>('POST', '/search', data),
  },
  settings: {
    get: () => request<any>('GET', '/settings'),
    update: (data: any) => request<any>('PUT', '/settings', data),
  },
}
