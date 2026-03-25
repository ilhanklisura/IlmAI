<template>
  <Teleport to="body">
    <transition
      enter-active-class="transition duration-300 ease-out"
      enter-from-class="opacity-0 scale-95"
      enter-to-class="opacity-100 scale-100"
      leave-active-class="transition duration-200 ease-in"
      leave-from-class="opacity-100 scale-100"
      leave-to-class="opacity-0 scale-95"
    >
      <div v-if="isOpen" class="fixed inset-0 z-[10000] flex items-center justify-center p-4">
        <!-- Backdrop -->
        <div class="absolute inset-0 bg-slate-950/80 backdrop-blur-md" @click="close"></div>
        
        <!-- Modal Content -->
        <div class="relative w-full max-w-md bg-surface border border-border/40 rounded-3xl shadow-[0_32px_64px_-16px_rgba(0,0,0,0.5)] overflow-hidden">
          <div class="px-6 py-5 border-b border-border/30 flex items-center justify-between bg-main/[0.02]">
            <h2 class="text-xl font-bold text-main flex items-center">
              <svg class="w-5 h-5 mr-3 text-emerald-500" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M10.325 4.317c.426-1.756 2.924-1.756 3.35 0a1.724 1.724 0 002.573 1.066c1.543-.94 3.31.826 2.37 2.37a1.724 1.724 0 001.065 2.572c1.756.426 1.756 2.924 0 3.35a1.724 1.724 0 00-1.066 2.573c.94 1.543-.826 3.31-2.37 2.37a1.724 1.724 0 00-2.572 1.065c-.426 1.756-2.924 1.756-3.35 0a1.724 1.724 0 00-2.573-1.066c-1.543.94-3.31-.826-2.37-2.37a1.724 1.724 0 00-1.065-2.572c-1.756-.426-1.756-2.924 0-3.35a1.724 1.724 0 001.066-2.573c-.94-1.543.826-3.31 2.37-2.37.996.608 2.296.07 2.572-1.065z"></path><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M15 12a3 3 0 11-6 0 3 3 0 016 0z"></path></svg>
              {{ $t('nav.settings') }}
            </h2>
            <button @click="close" class="text-muted hover:text-main transition-colors p-2 rounded-lg hover:bg-main/5">
              <svg class="w-6 h-6" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M6 18L18 6M6 6l12 12"></path></svg>
            </button>
          </div>
          
          <div class="p-8 space-y-8">
            <!-- Theme Setting -->
            <div class="space-y-4">
              <label class="block text-xs font-bold text-muted uppercase tracking-widest">{{ $t('settings.theme') }}</label>
              <div class="grid grid-cols-2 gap-4">
                <button 
                  @click="setTheme('dark')"
                  :class="['flex flex-col items-center justify-center space-y-2 p-5 rounded-2xl border transition-all duration-300', theme === 'dark' ? 'bg-emerald-500/10 border-emerald-500 text-emerald-500' : 'bg-main/5 border-border/50 text-muted hover:border-main/20']"
                >
                  <svg class="w-6 h-6" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M20.354 15.354A9 9 0 018.646 3.646 9.003 9.003 0 0012 21a9.003 9.003 0 008.354-5.646z"></path></svg>
                  <span class="font-semibold">Dark</span>
                </button>
                <button 
                  @click="setTheme('light')"
                  :class="['flex flex-col items-center justify-center space-y-2 p-5 rounded-2xl border transition-all duration-300', theme === 'light' ? 'bg-emerald-500/10 border-emerald-500 text-emerald-500' : 'bg-main/5 border-border/50 text-muted hover:border-main/20']"
                >
                  <svg class="w-6 h-6" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M12 3v1m0 16v1m9-9h-1M4 12H3m15.364-6.364l-.707.707M6.343 17.657l-.707.707m12.728 0l-.707-.707M6.343 6.343l-.707-.707m12.728 12.728L5.657 5.657"></path></svg>
                  <span class="font-semibold">Light</span>
                </button>
              </div>
            </div>
            
            <!-- Language Setting -->
            <div class="space-y-4">
              <label class="block text-xs font-bold text-muted uppercase tracking-widest">{{ $t('settings.language') }}</label>
              <div class="grid grid-cols-2 gap-4">
                <button 
                  @click="setLanguage('bs')"
                  :class="['flex flex-col items-center justify-center space-y-2 p-5 rounded-2xl border transition-all duration-300', currentLang === 'bs' ? 'bg-emerald-500/10 border-emerald-500 text-emerald-500' : 'bg-main/5 border-border/50 text-muted hover:border-main/20']"
                >
                  <svg class="w-8 h-auto rounded shadow-sm mb-1" viewBox="0 0 16 8" xmlns="http://www.w3.org/2000/svg">
                    <rect width="16" height="8" fill="#002395"/>
                    <polygon points="4.4,0 12.4,0 12.4,8" fill="#fecb00"/>
                  </svg>
                  <span class="text-sm font-bold uppercase tracking-wider">Bosanski</span>
                </button>
                <button 
                  @click="setLanguage('en')"
                  :class="['flex flex-col items-center justify-center space-y-2 p-5 rounded-2xl border transition-all duration-300', currentLang === 'en' ? 'bg-emerald-500/10 border-emerald-500 text-emerald-500' : 'bg-main/5 border-border/50 text-muted hover:border-main/20']"
                >
                  <svg class="w-8 h-auto rounded shadow-sm mb-1" viewBox="0 0 60 30" xmlns="http://www.w3.org/2000/svg">
                    <rect width="60" height="30" fill="#012169"/>
                    <path d="M0,0 L60,30 M60,0 L0,30" stroke="#fff" stroke-width="6"/>
                    <path d="M0,0 L60,30 M60,0 L0,30" stroke="#C8102E" stroke-width="4"/>
                    <path d="M30,0 v30 M0,15 h60" stroke="#fff" stroke-width="10"/>
                    <path d="M30,0 v30 M0,15 h60" stroke="#C8102E" stroke-width="6"/>
                  </svg>
                  <span class="text-sm font-bold uppercase tracking-wider">English</span>
                </button>
              </div>
            </div>
          </div>
          
          <div class="px-8 py-6 bg-main/[0.02] border-t border-border/30">
            <Button variant="primary" size="lg" class="w-full !rounded-2xl" @click="close">{{ $t('common.save') }}</Button>
          </div>
        </div>
      </div>
    </transition>
  </Teleport>
</template>

<script setup lang="ts">
import { ref, onMounted, watch } from 'vue'
import { useI18n } from 'vue-i18n'
import Button from '@/components/ui/Button.vue'

const { locale } = useI18n()
const props = defineProps<{ isOpen: boolean }>()
const emit = defineEmits(['close'])

const theme = ref(localStorage.getItem('theme') || 'dark')
const currentLang = ref(locale.value)

const close = () => {
  emit('close')
}

const setTheme = (t: string) => {
  theme.value = t
  localStorage.setItem('theme', t)
  updateTheme()
}

const updateTheme = () => {
  const isLight = theme.value === 'light'
  document.documentElement.classList.toggle('light', isLight)
  document.documentElement.classList.toggle('dark', !isLight)
  document.documentElement.style.colorScheme = isLight ? 'light' : 'dark'
  
  // Directly set body background as a fallback
  document.body.style.backgroundColor = isLight ? '#f8fafc' : '#0f172a'
}

const setLanguage = (lang: string) => {
  currentLang.value = lang
  locale.value = lang
  localStorage.setItem('user-locale', lang)
}

onMounted(() => {
  updateTheme()
})

watch(() => props.isOpen, (newVal) => {
  if (newVal) {
    document.body.style.overflow = 'hidden'
  } else {
    document.body.style.overflow = ''
  }
})
</script>
