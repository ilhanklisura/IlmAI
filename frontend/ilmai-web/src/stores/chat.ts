import { defineStore } from 'pinia'
import { ref } from 'vue'
import { api } from '@/lib/api'

interface Message { id: string; role: string; content: string; sources?: any[]; createdAt: string }
interface Session { id: string; title: string; createdAt: string; updatedAt: string; messageCount: number }

export const useChatStore = defineStore('chat', () => {
  const sessions = ref<Session[]>([])
  const currentSessionId = ref<string | null>(null)
  const messages = ref<Message[]>([])
  const loading = ref(false)
  const sending = ref(false)

  async function loadSessions() {
    loading.value = true
    try { sessions.value = await api.chat.sessions() } finally { loading.value = false }
  }

  async function loadMessages(sessionId: string) {
    currentSessionId.value = sessionId
    loading.value = true
    try { messages.value = await api.chat.messages(sessionId) } finally { loading.value = false }
  }

  async function sendMessage(question: string, language: string) {
    sending.value = true
    try {
      const res = await api.chat.send({ sessionId: currentSessionId.value ?? undefined, question, language })
      currentSessionId.value = res.sessionId
      messages.value.push({ id: crypto.randomUUID(), role: 'user', content: question, createdAt: new Date().toISOString() })
      messages.value.push({ id: crypto.randomUUID(), role: 'assistant', content: res.answer, sources: res.sources, createdAt: new Date().toISOString() })
      await loadSessions()
      return res
    } finally { sending.value = false }
  }

  async function deleteSession(sessionId: string) {
    await api.chat.deleteSession(sessionId)

    // ako brišeš trenutno otvoren chat → reset
    if (currentSessionId.value === sessionId) {
      currentSessionId.value = null
      messages.value = []
    }

    // ukloni iz liste (instant UX)
    sessions.value = sessions.value.filter(s => s.id !== sessionId)
  }

  async function renameSession(sessionId: string, title: string) {
    await api.chat.renameSession(sessionId, title)

    const s = sessions.value.find(s => s.id === sessionId)
    if (s) s.title = title
  }

  function newChat() { currentSessionId.value = null; messages.value = [] }

  return { sessions, currentSessionId, messages, loading, sending, loadSessions, loadMessages, sendMessage, deleteSession, renameSession, newChat }
})
