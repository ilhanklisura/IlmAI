<template>
  <div class="min-h-screen flex items-center justify-center p-4 pt-24 relative overflow-hidden bg-background transition-colors duration-500">
    <!-- Adaptive Glow Background -->
    <div class="absolute inset-0 pointer-events-none opacity-40">
      <div class="absolute top-1/2 left-1/2 -translate-x-1/2 -translate-y-1/2 w-[600px] h-[600px] bg-emerald-500/10 blur-[120px] rounded-full"></div>
    </div>
    
    <div class="w-full max-w-[440px] relative z-10 animate-fade-in-up">
      <Card class="!p-8 sm:!p-12 shadow-2xl border-border/50 !bg-surface/60 backdrop-blur-2xl !rounded-[3rem]">
        
        <!-- Header Section -->
        <div class="text-center mb-10">
          <div class="inline-flex items-center justify-center p-5 bg-emerald-500/10 rounded-3xl mb-6 ring-1 ring-emerald-500/20">
             <svg class="w-10 h-10 text-emerald-500" fill="none" stroke="currentColor" viewBox="0 0 24 24">
               <path stroke-linecap="round" stroke-linejoin="round" stroke-width="1.5" d="M3 19a2 2 0 002 2h14a2 2 0 002-2V7a2 2 0 00-2-2H5a2 2 0 00-2 2v12z" />
               <path stroke-linecap="round" stroke-linejoin="round" stroke-width="1.5" d="M12 11l7-7M5 4l7 7" />
             </svg>
          </div>
          <h1 class="text-3xl font-extrabold text-main mb-3 tracking-tight">{{ $t('verifyEmail.title') }}</h1>
          <p class="text-muted text-sm leading-relaxed mb-4">{{ $t('verifyEmail.subtitle') }}</p>
          <div class="inline-flex items-center px-4 py-1.5 bg-background/50 rounded-full border border-border/50">
            <span class="text-emerald-500 font-bold text-xs tracking-wide truncate max-w-[200px]">{{ email }}</span>
          </div>
        </div>

        <div class="space-y-8">
          <!-- Code Input Grid -->
          <div class="grid grid-cols-6 gap-2 sm:gap-3">
            <input
              v-for="(digit, index) in 6"
              :key="index"
              :id="'digit-' + index"
              v-model="codeArray[index]"
              type="text"
              maxlength="1"
              autocomplete="one-time-code"
              inputmode="numeric"
              class="w-full aspect-[3/4] text-center text-2xl font-bold rounded-2xl border border-border bg-background/50 text-main focus:border-emerald-500 focus:ring-4 focus:ring-emerald-500/10 focus:outline-none transition-all placeholder-muted/20"
              placeholder="0"
              @input="handleInput($event, index)"
              @keydown="handleKeyDown($event, index)"
              @paste="handlePaste"
            />
          </div>

          <!-- Error Feedback -->
          <div v-if="error" class="text-red-500 text-sm bg-red-500/10 border border-red-500/20 p-4 rounded-2xl flex items-start space-x-3 animate-in fade-in slide-in-from-top-2">
             <svg class="w-5 h-5 mt-0.5 shrink-0" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M12 8v4m0 4h.01M21 12a9 9 0 11-18 0 9 9 0 0118 0z"></path></svg>
             <span class="font-medium">{{ error }}</span>
          </div>

          <!-- Success Feedback -->
          <div v-if="success" class="text-emerald-500 text-sm bg-emerald-500/10 border border-emerald-500/20 p-5 rounded-2xl flex items-center justify-center space-x-3 animate-in zoom-in duration-300">
             <svg class="w-6 h-6 shrink-0" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M5 13l4 4L19 7"></path></svg>
             <span class="font-bold text-base">{{ $t('verifyEmail.success') }}</span>
          </div>

          <div class="pt-2">
            <Button 
              size="lg" 
              class="w-full !rounded-[1.5rem] !py-4 shadow-xl shadow-emerald-500/20 font-bold text-lg transition-all active:scale-[0.97]" 
              :loading="loading" 
              @click="submitVerify"
              :disabled="!isComplete"
            >
              {{ $t('verifyEmail.button') }}
            </Button>
          </div>

          <!-- Footer Actions -->
          <div class="text-center pt-2 space-y-4">
            <button @click="resendCode" :disabled="resending" class="text-emerald-500 hover:text-emerald-400 disabled:opacity-50 transition-colors text-sm font-bold flex items-center justify-center w-full">
              <svg xmlns="http://www.w3.org/2000/svg" class="w-4 h-4 mr-2" fill="none" viewBox="0 0 24 24" stroke="currentColor">
                <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M4 4v5h.582m15.356 2A8.001 8.001 0 004.582 9m0 0H9m11 11v-5h-.581m0 0a8.003 8.003 0 01-15.357-2m15.357 2H15" />
              </svg>
              {{ resending ? 'Slanje...' : $t('verifyEmail.resend') }}
            </button>

            <button @click="backToLogin" class="text-muted hover:text-main transition-all text-sm font-semibold flex items-center justify-center w-full group">
              <svg class="w-4 h-4 mr-2 group-hover:-translate-x-1 transition-transform" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M10 19l-7-7m0 0l7-7m-7 7h18"></path></svg>
              {{ $t('verifyEmail.backToLogin') }}
            </button>
          </div>
        </div>
      </Card>
      
      <!-- Footer Copyright -->
      <p class="text-center mt-8 text-muted/60 text-[10px] font-bold tracking-[0.2em] uppercase">
        &copy; 2026 IlmAI Platform
      </p>
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref, computed, onMounted } from 'vue'
import { useRouter, useRoute } from 'vue-router'
import { api } from '@/lib/api'
import { useAuthStore } from '@/stores/auth'
import Card from '@/components/ui/Card.vue'
import Button from '@/components/ui/Button.vue'

const router = useRouter()
const route = useRoute()
const auth = useAuthStore()

const email = computed(() => (route.query.email as string) || auth.user?.email || '')
const codeArray = ref(['', '', '', '', '', ''])
const loading = ref(false)
const resending = ref(false)
const error = ref('')
const success = ref(false)

const isComplete = computed(() => codeArray.value.every(d => d !== ''))

const handleInput = (e: Event, index: number) => {
  const input = e.target as HTMLInputElement
  const val = input.value.replace(/[^0-9]/g, '') 

  codeArray.value[index] = val.slice(-1)

  if (val && index < 5) {
    const nextInput = document.getElementById('digit-' + (index + 1))
    nextInput?.focus()
  }
}

const handleKeyDown = (e: KeyboardEvent, index: number) => {
  if (e.key === 'Backspace' && !codeArray.value[index] && index > 0) {
    const prevInput = document.getElementById('digit-' + (index - 1))
    prevInput?.focus()
  }
}

const handlePaste = (e: ClipboardEvent) => {
  const pasteData = e.clipboardData?.getData('text')
  if (pasteData && pasteData.length === 6 && /^\d+$/.test(pasteData)) {
    for (let i = 0; i < 6; i++) {
        codeArray.value[i] = pasteData[i]
    }
    const lastInput = document.getElementById('digit-5')
    lastInput?.focus()
  }
}

const resendCode = async () => {
  if (!email.value) return
  resending.value = true
  error.value = ''
  try {
    await api.auth.resendCode(email.value)
    // Clear the current inputs to avoid confusion
    codeArray.value = ['', '', '', '', '', '']
    document.getElementById('digit-0')?.focus()
  } catch (err: any) {
    error.value = err.message || 'Failed to resend code'
  } finally {
    resending.value = false
  }
}

const submitVerify = async () => {
  if (!isComplete.value) return
  
  error.value = ''
  loading.value = true
  
  try {
    const code = codeArray.value.join('')
    await api.auth.verifyEmail({ email: email.value, code })
    
    if (auth.user) {
      auth.user.isEmailVerified = true
    }
    
    success.value = true
    setTimeout(() => {
      router.push('/')
    }, 1500)
  } catch (err: any) {
    error.value = err.message || 'Verification failed'
  } finally {
    loading.value = false
  }
}

const backToLogin = () => {
  auth.logout()
  router.push('/login')
}

onMounted(() => {
  if (!email.value && !auth.isAuthenticated) {
    router.push('/register')
  }
})
</script>

<style scoped>
input::-webkit-outer-spin-button,
input::-webkit-inner-spin-button {
  -webkit-appearance: none;
  margin: 0;
}
input[type=number] {
  -moz-appearance: textfield;
}
</style>
