<template>
  <div class="h-screen pt-16 flex overflow-hidden bg-background relative">
    <!-- Lava Lamp Background (Subtle) -->
    <div class="lava-container opacity-30">
      <div class="blob blob-1"></div>
      <div class="blob blob-2"></div>
    </div>

    <!-- Sidebar (Sessions) -->
    <div class="hidden md:flex w-80 flex-col border-r border-border bg-surface/20 backdrop-blur-xl relative z-10">
      <div class="p-4 border-b border-border">
        <Button class="w-full" variant="outline" @click="chatStore.newChat()">
          <svg class="w-4 h-4 mr-2" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M12 4v16m8-8H4"></path></svg>
          {{ $t('chat.newChat') }}
        </Button>
      </div>
      <div class="flex-grow overflow-y-auto p-2 space-y-1 custom-scrollbar">
        <button
          v-for="session in chatStore.sessions"
          :key="session.id"
          @click="chatStore.loadMessages(session.id)"
          :class="[
            'w-full text-left px-3 py-2.5 rounded-lg text-sm transition-all duration-200',
            chatStore.currentSessionId === session.id 
              ? 'bg-emerald-500/10 text-emerald-500 font-medium border border-emerald-500/20 shadow-sm' 
              : 'text-muted hover:text-main hover:bg-surface/50'
          ]"
        >
          <div class="truncate font-medium">{{ session.title || 'Untitled Chat' }}</div>
          <div class="text-[10px] text-muted opacity-70 mt-1 flex items-center">
            <svg class="w-3 h-3 mr-1" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M12 8v4l3 3m6-3a9 9 0 11-18 0 9 9 0 0118 0z"></path></svg>
            {{ new Date(session.updatedAt).toLocaleDateString() }}
          </div>
        </button>
      </div>
    </div>

    <!-- Main Chat Area -->
    <div class="flex-grow flex flex-col bg-transparent overflow-hidden relative z-10">
      <!-- Chat Header (Mobile) -->
      <div class="md:hidden p-4 border-b border-border flex items-center justify-between bg-surface/40 backdrop-blur-xl sticky top-0 z-10">
        <div class="flex items-center">
          <Button size="sm" variant="ghost" @click="isHistoryOpen = true" class="!p-2 mr-2">
            <svg class="w-6 h-6" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M4 6h16M4 12h16M4 18h16"></path></svg>
          </Button>
          <h2 class="text-main font-bold flex items-center">
             IlmAI Chat
          </h2>
        </div>
        <Button size="sm" variant="ghost" @click="chatStore.newChat()" class="!p-2">
           <svg class="w-5 h-5" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M12 4v16m8-8H4"></path></svg>
        </Button>
      </div>

      <!-- Mobile History Drawer Overlay -->
      <Transition
        enter-active-class="transition duration-300 ease-out"
        enter-from-class="opacity-0"
        enter-to-class="opacity-100"
        leave-active-class="transition duration-200 ease-in"
        leave-from-class="opacity-100"
        leave-to-class="opacity-0"
      >
        <div v-if="isHistoryOpen" class="fixed inset-0 z-[100] md:hidden">
          <!-- Backdrop -->
          <div class="absolute inset-0 bg-black/60 backdrop-blur-sm" @click="isHistoryOpen = false"></div>
          
          <!-- Drawer Content -->
          <div class="absolute inset-y-0 left-0 w-[280px] bg-background border-r border-border flex flex-col shadow-2xl animate-in slide-in-from-left duration-300">
            <div class="p-5 border-b border-border flex items-center justify-between bg-surface/50">
              <h3 class="font-bold text-main tracking-tight italic">Historija razgovora</h3>
              <button @click="isHistoryOpen = false" class="p-2 text-muted hover:text-main transition-colors rounded-lg hover:bg-surface/80">
                <svg class="w-6 h-6" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="1.5" d="M6 18L18 6M6 6l12 12"></path></svg>
              </button>
            </div>
            
            <div class="p-4 border-b border-border">
              <Button class="w-full" variant="outline" @click="() => { chatStore.newChat(); isHistoryOpen = false; }">
                <svg class="w-4 h-4 mr-2" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M12 4v16m8-8H4"></path></svg>
                {{ $t('chat.newChat') }}
              </Button>
            </div>

            <div class="flex-grow overflow-y-auto p-2 space-y-1 custom-scrollbar">
              <button
                v-for="session in chatStore.sessions"
                :key="session.id"
                @click="() => { chatStore.loadMessages(session.id); isHistoryOpen = false; }"
                :class="[
                  'w-full text-left px-3 py-3 rounded-lg text-sm transition-all duration-200',
                  chatStore.currentSessionId === session.id 
                    ? 'bg-emerald-500/10 text-emerald-500 font-medium border border-emerald-500/20 shadow-sm' 
                    : 'text-muted hover:text-main hover:bg-surface/50'
                ]"
              >
                <div class="truncate font-medium">{{ session.title || 'Untitled Chat' }}</div>
                <div class="text-[10px] text-muted opacity-70 mt-1 flex items-center">
                  <svg class="w-3 h-3 mr-1" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M12 8v4l3 3m6-3a9 9 0 11-18 0 9 9 0 0118 0z"></path></svg>
                  {{ new Date(session.updatedAt).toLocaleDateString() }}
                </div>
              </button>
            </div>
          </div>
        </div>
      </Transition>

      <!-- Messages -->
      <div ref="messageContainer" class="flex-grow overflow-y-auto p-4 md:p-8 space-y-8 scroll-smooth custom-scrollbar">
        <div v-if="chatStore.messages.length === 0" class="h-full flex flex-col items-center justify-center text-center space-y-6 max-w-lg mx-auto animate-in fade-in zoom-in duration-700">
          <div class="w-20 h-20 rounded-3xl bg-emerald-500/10 flex items-center justify-center text-emerald-500 shadow-inner">
             <svg class="w-10 h-10" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M8 10h.01M12 10h.01M16 10h.01M9 16H5a2 2 0 01-2-2V6a2 2 0 012-2h14a2 2 0 012 2v8a2 2 0 01-2 2h-5l-5 5v-5z"></path></svg>
          </div>
          <div class="space-y-2">
            <h2 class="text-3xl font-extrabold text-main">{{ $t('chat.welcomeGreeting') || 'Postavite pitanje IlmAI-u' }}</h2>
            <p class="text-muted leading-relaxed">{{ $t('home.description') }}</p>
          </div>
        </div>

        <div v-for="msg in chatStore.messages" :key="msg.id" :class="['flex w-full', msg.role === 'user' ? 'justify-end' : 'justify-start hover-trigger']">
          <div :class="[
            'max-w-[85%] md:max-w-[75%] rounded-3xl p-5 text-sm md:text-base leading-relaxed group transition-all duration-300',
            msg.role === 'user' 
              ? 'bg-emerald-500 text-white rounded-tr-sm shadow-lg shadow-emerald-500/10' 
              : 'bg-surface/60 backdrop-blur-md border border-border text-main rounded-tl-sm shadow-sm'
          ]">
            <div class="whitespace-pre-wrap">{{ msg.content }}</div>
            
            <!-- Sources -->
            <div v-if="msg.sources && msg.sources.length > 0" class="mt-5 pt-4 border-t border-border space-y-3">
              <div class="text-[10px] font-bold uppercase tracking-widest text-muted/60 flex items-center">
                <svg class="w-3 h-3 mr-1.5" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M12 6.253v13m0-13C10.832 5.477 9.246 5 7.5 5S4.168 5.477 3 6.253v13C4.168 18.477 5.754 18 7.5 18s3.332.477 4.5 1.253m0-13C13.168 5.477 14.754 5 16.5 5c1.747 0 3.332.477 4.5 1.253v13C19.832 18.477 18.247 18 16.5 18c-1.746 0-3.332.477-4.5 1.253"></path></svg>
                {{ $t('chat.sources') }}
              </div>
              <div class="flex flex-wrap gap-2">
                <div v-for="(src, idx) in msg.sources" :key="idx" class="text-[10px] bg-background/50 border border-border px-2.5 py-1.5 rounded-lg text-muted font-medium hover:border-emerald-500/50 transition-colors cursor-default">
                  {{ src.reference || src.title }}
                </div>
              </div>
            </div>
          </div>
        </div>

        <div v-if="chatStore.sending" class="flex justify-start">
          <div class="bg-surface/60 backdrop-blur-md border border-border rounded-3xl rounded-tl-sm p-5 shadow-sm">
            <div class="flex space-x-1.5">
              <div class="w-2 h-2 bg-emerald-500/40 rounded-full animate-bounce"></div>
              <div class="w-2 h-2 bg-emerald-500/60 rounded-full animate-bounce [animation-delay:0.2s]"></div>
              <div class="w-2 h-2 bg-emerald-500/80 rounded-full animate-bounce [animation-delay:0.4s]"></div>
            </div>
          </div>
        </div>
      </div>

      <!-- Input Area -->
      <div class="p-4 md:p-8 bg-surface/10 backdrop-blur-2xl border-t border-border/30">
        <form @submit.prevent="handleSend" class="max-w-4xl mx-auto relative group">
          <textarea
            v-model="input"
            rows="1"
            :placeholder="$t('chat.placeholder')"
            @keydown.enter.exact.prevent="handleSend"
            class="w-full bg-surface border border-border rounded-2xl px-5 py-4 pr-16 text-main placeholder-muted/50 focus:outline-none focus:ring-2 focus:ring-emerald-500 focus:border-transparent transition-all shadow-xl shadow-black/5 resize-none min-h-[56px] max-h-[200px] overflow-hidden"
            @input="(e) => { 
              const el = e.target as HTMLTextAreaElement;
              el.style.height = 'auto';
              el.style.height = el.scrollHeight + 'px';
            }"
          ></textarea>
          <button
            type="submit"
            :disabled="!input.trim() || chatStore.sending"
            class="absolute right-3 top-1/2 -translate-y-1/2 p-2.5 bg-emerald-500 text-white rounded-xl hover:bg-emerald-600 disabled:opacity-30 disabled:grayscale transition-all transform hover:scale-105 active:scale-95 shadow-lg shadow-emerald-500/20"
          >
            <svg class="w-5 h-5" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M5 10l7-7m0 0l7 7m-7-7v18"></path></svg>
          </button>
        </form>
        <p class="text-center text-[10px] text-muted opacity-60 mt-4 uppercase tracking-widest font-bold">{{ $t('chat.disclaimer') || 'IlmAI može dati netačne informacije. Provjerite izvore.' }}</p>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref, onMounted, watch, nextTick } from 'vue'
import { useRoute } from 'vue-router'
import { useChatStore } from '@/stores/chat'
import { useI18n } from 'vue-i18n'
import Button from '@/components/ui/Button.vue'

const chatStore = useChatStore()
const { locale } = useI18n()
const route = useRoute()
const input = ref('')
const messageContainer = ref<HTMLElement | null>(null)
const isHistoryOpen = ref(false)

const handleSend = async () => {
  if (!input.value.trim() || chatStore.sending) return
  const text = input.value
  input.value = ''
  await chatStore.sendMessage(text, locale.value)
  scrollToBottom()
}

const scrollToBottom = () => {
  nextTick(() => {
    if (messageContainer.value) {
      messageContainer.value.scrollTop = messageContainer.value.scrollHeight
    }
  })
}

watch(() => chatStore.messages.length, () => scrollToBottom())

onMounted(async () => {
  await chatStore.loadSessions()
  
  // Handle prompt from query parameter (e.g. from Search page)
  if (route.query.prompt) {
    input.value = route.query.prompt as string
    handleSend()
  }
})
</script>
