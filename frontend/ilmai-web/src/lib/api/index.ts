const API_BASE = import.meta.env.VITE_API_BASE || 'http://localhost:5030/api'

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
