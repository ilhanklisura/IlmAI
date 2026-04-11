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
        <div 
          class="relative w-full max-w-md border rounded-3xl shadow-[0_32px_64px_-16px_rgba(0,0,0,0.5)] overflow-hidden flex flex-col max-h-[90vh] transition-colors duration-300"
          :class="[theme === 'light' ? 'bg-white border-slate-200' : 'bg-[#0f172a] border-white/10']"
        >
          <div class="px-6 py-5 border-b flex items-center justify-between shrink-0" :class="[theme === 'light' ? 'bg-slate-50 border-slate-100' : 'bg-white/5 border-white/5']">
            <h2 class="text-xl font-bold flex items-center" :class="[theme === 'light' ? 'text-slate-900' : 'text-white']">
              <svg class="w-5 h-5 mr-3 text-emerald-500" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M10.325 4.317c.426-1.756 2.924-1.756 3.35 0a1.724 1.724 0 002.573 1.066c1.543-.94 3.31.826 2.37 2.37a1.724 1.724 0 001.065 2.572c1.756.426 1.756 2.924 0 3.35a1.724 1.724 0 00-1.066 2.573c.94 1.543-.826 3.31-2.37 2.37a1.724 1.724 0 00-2.572 1.065c-.426 1.756-2.924 1.756-3.35 0a1.724 1.724 0 00-2.573-1.066c-1.543.94-3.31-.826-2.37-2.37a1.724 1.724 0 00-1.065-2.572c-1.756-.426-1.756-2.924 0-3.35a1.724 1.724 0 001.066-2.573c-.94-1.543.826-3.31 2.37-2.37.996.608 2.296.07 2.572-1.065z"></path><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M15 12a3 3 0 11-6 0 3 3 0 016 0z"></path></svg>
              {{ $t('nav.settings') }}
            </h2>
            <button @click="close" class="text-muted hover:text-main p-2 rounded-lg hover:bg-main/5 transition-colors">
              <svg class="w-6 h-6" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M6 18L18 6M6 6l12 12"></path></svg>
            </button>
          </div>
          
          <div class="p-8 space-y-8 overflow-y-auto">
            <!-- Admin Section -->
            <div v-if="auth.isAdmin" class="p-4 bg-emerald-500/10 border border-emerald-500/20 rounded-2xl space-y-3">
              <div class="flex items-center justify-between">
                <span class="text-[10px] font-black uppercase tracking-widest text-emerald-500">{{ $t('admin.nav.administration') }}</span>
                <div class="w-1.5 h-1.5 rounded-full bg-emerald-500 animate-pulse"></div>
              </div>
              <router-link 
                to="/admin" 
                @click="close"
                class="flex items-center justify-center space-x-2 w-full py-3 bg-emerald-500 text-white text-sm font-bold rounded-xl shadow-lg shadow-emerald-500/20 hover:bg-emerald-600 transition-all active:scale-95"
              >
                <svg class="w-4 h-4" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M9 19v-6a2 2 0 00-2-2H5a2 2 0 00-2 2v6a2 2 0 002 2h2a2 2 0 002 2zm0 0h6v-1a6 6 0 00-9-5.197M13 7a4 4 0 11-8 0 4 4 0 018 0z" /></svg>
                <span>{{ $t('admin.nav.openDashboard') }}</span>
              </router-link>
            </div>

            <!-- Theme Setting -->
            <div class="space-y-4">
              <label class="block text-xs font-bold uppercase tracking-widest" :class="[theme === 'light' ? 'text-slate-500' : 'text-slate-400']">{{ $t('settings.theme') }}</label>
              <div class="grid grid-cols-2 gap-4">
                <button 
                  @click="setTheme('dark')"
                  :class="['flex flex-col items-center justify-center space-y-2 p-5 rounded-2xl border transition-all duration-300', theme === 'dark' ? 'bg-emerald-500/10 border-emerald-500 text-emerald-500' : 'bg-slate-500/5 border-transparent text-slate-400 hover:border-slate-300']"
                >
                  <svg class="w-6 h-6" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M20.354 15.354A9 9 0 018.646 3.646 9.003 9.003 0 0012 21a9.003 9.003 0 008.354-5.646z"></path></svg>
                  <span class="font-semibold text-sm">{{ $t('settings.dark') }}</span>
                </button>
                <button 
                  @click="setTheme('light')"
                  :class="['flex flex-col items-center justify-center space-y-2 p-5 rounded-2xl border transition-all duration-300', theme === 'light' ? 'bg-emerald-500/10 border-emerald-500 text-emerald-500' : 'bg-slate-500/5 border-transparent text-slate-400 hover:border-slate-500']"
                >
                  <svg class="w-6 h-6" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M12 3v1m0 16v1m9-9h-1M4 12H3m15.364-6.364l-.707.707M6.343 17.657l-.707.707m12.728 0l-.707-.707M6.343 6.343l-.707-.707m12.728 12.728L5.657 5.657"></path></svg>
                  <span class="font-semibold text-sm">{{ $t('settings.light') }}</span>
                </button>
              </div>
            </div>
            
            <!-- Language Setting -->
            <div class="space-y-4 pt-4 border-t" :class="[theme === 'light' ? 'border-slate-100' : 'border-white/5']">
              <label class="block text-xs font-bold uppercase tracking-widest" :class="[theme === 'light' ? 'text-slate-500' : 'text-slate-400']">{{ $t('settings.language') }}</label>
              <div class="grid grid-cols-2 gap-4">
                <button 
                  @click="setLanguage('bs')"
                  :class="['flex flex-col items-center justify-center space-y-2 p-4 rounded-2xl border transition-all duration-300', currentLang === 'bs' ? 'bg-emerald-500/10 border-emerald-500 text-emerald-500' : 'bg-slate-500/5 border-transparent text-slate-400']"
                >
                  <svg class="w-7 h-auto rounded shadow-sm" viewBox="0 0 16 8" xmlns="http://www.w3.org/2000/svg">
                    <rect width="16" height="8" fill="#002395"/>
                    <polygon points="4.4,0 12.4,0 12.4,8" fill="#fecb00"/>
                  </svg>
                  <span class="text-[10px] font-bold uppercase tracking-wider">Bosanski</span>
                </button>
                <button 
                  @click="setLanguage('en')"
                  :class="['flex flex-col items-center justify-center space-y-2 p-4 rounded-2xl border transition-all duration-300', currentLang === 'en' ? 'bg-emerald-500/10 border-emerald-500 text-emerald-500' : 'bg-slate-500/5 border-transparent text-slate-400']"
                >
                  <svg class="w-7 h-auto rounded shadow-sm" viewBox="0 0 60 30" xmlns="http://www.w3.org/2000/svg">
                    <rect width="60" height="30" fill="#012169"/>
                    <path d="M0,0 L60,30 M60,0 L0,30" stroke="#fff" stroke-width="6"/>
                    <path d="M0,0 L60,30 M60,0 L0,30" stroke="#C8102E" stroke-width="4"/>
                    <path d="M30,0 v30 M0,15 h60" stroke="#fff" stroke-width="10"/>
                    <path d="M30,0 v30 M0,15 h60" stroke="#C8102E" stroke-width="6"/>
                  </svg>
                  <span class="text-[10px] font-bold uppercase tracking-wider">English</span>
                </button>
              </div>
            </div>

            <!-- Change Password Section -->
            <div v-if="auth.isAuthenticated" class="space-y-4 pt-4 border-t" :class="[theme === 'light' ? 'border-slate-100' : 'border-white/5']">
              <label class="block text-xs font-bold uppercase tracking-widest" :class="[theme === 'light' ? 'text-slate-500' : 'text-slate-400']">{{ $t('settings.changePassword') }}</label>
              <div class="space-y-4">
                <div class="space-y-1.5">
                  <label class="text-[10px] font-semibold ml-1 uppercase" :class="[theme === 'light' ? 'text-slate-400' : 'text-slate-500']">{{ $t('settings.currentPassword') }}</label>
                  <Input v-model="passwordForm.currentPassword" type="password" class="w-full" />
                </div>
                <div class="space-y-1.5">
                  <label class="text-[10px] font-semibold ml-1 uppercase" :class="[theme === 'light' ? 'text-slate-400' : 'text-slate-500']">{{ $t('settings.currentPassword') }}</label>
                  <Input v-model="passwordForm.newPassword" type="password" class="w-full" />
                </div>
                <div class="space-y-1.5">
                  <label class="text-[10px] font-semibold ml-1 uppercase" :class="[theme === 'light' ? 'text-slate-400' : 'text-slate-500']">{{ $t('settings.confirmPassword') }}</label>
                  <Input v-model="passwordForm.confirmNewPassword" type="password" class="w-full" />
                </div>

                <div v-if="passwordSuccess" class="text-emerald-500 text-[11px] font-bold bg-emerald-500/10 p-3 rounded-xl border border-emerald-500/20">
                  {{ $t('settings.passwordChanged') }}
                </div>
                <div v-if="passwordError" class="text-red-500 text-[11px] font-bold bg-red-500/10 p-3 rounded-xl border border-red-500/20">
                  {{ passwordError }}
                </div>

                <Button 
                  @click="handleChangePassword" 
                  :loading="changingPassword"
                  variant="primary" 
                  class="w-full !rounded-xl !py-3 text-sm shadow-lg shadow-emerald-500/20"
                >
                  {{ $t('settings.passwordChangeBtn') }}
                </Button>
              </div>
            </div>
          </div>
          
          <div class="px-8 py-6 border-t shrink-0" :class="[theme === 'light' ? 'bg-slate-50 border-slate-100' : 'bg-white/5 border-white/5']">
            <Button variant="primary" size="lg" class="w-full !rounded-2xl" @click="close">{{ $t('common.close') }}</Button>
          </div>
        </div>
      </div>
    </transition>
  </Teleport>
</template>

<script setup lang="ts">
import { ref, onMounted, watch } from 'vue'
import { useI18n } from 'vue-i18n'
import { useAuthStore } from '@/stores/auth'
import { api } from '@/lib/api'
import Button from '@/components/ui/Button.vue'
import Input from '@/components/ui/Input.vue'

const { locale } = useI18n()
const props = defineProps<{ isOpen: boolean }>()
const emit = defineEmits(['close'])
const auth = useAuthStore()

const theme = ref(localStorage.getItem('theme') || 'dark')
const currentLang = ref(locale.value)

// Password form state
const passwordForm = ref({
  currentPassword: '',
  newPassword: '',
  confirmNewPassword: ''
})
const changingPassword = ref(false)
const passwordSuccess = ref(false)
const passwordError = ref('')

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

const handleChangePassword = async () => {
  passwordError.value = ''
  passwordSuccess.value = false

  if (passwordForm.value.newPassword !== passwordForm.value.confirmNewPassword) {
    passwordError.value = locale.value === 'bs' ? 'Nove lozinke se ne poklapaju.' : 'New passwords do not match.'
    return
  }

  if (passwordForm.value.newPassword.length < 6) {
    passwordError.value = locale.value === 'bs' ? 'Nova lozinka mora imati najmanje 6 karaktera.' : 'New password must be at least 6 characters.'
    return
  }

  changingPassword.value = true
  try {
    await api.auth.changePassword(passwordForm.value)
    passwordSuccess.value = true
    passwordForm.value = { currentPassword: '', newPassword: '', confirmNewPassword: '' }
    setTimeout(() => { passwordSuccess.value = false }, 5000)
  } catch (err: any) {
    passwordError.value = err.message || (locale.value === 'bs' ? 'Greška pri promjeni lozinke.' : 'Error changing password.')
  } finally {
    changingPassword.value = false
  }
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

<style scoped>
/* Restored original logic and aesthetics */
</style>
