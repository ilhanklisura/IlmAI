<template>
  <div class="min-h-screen flex items-center justify-center p-4 pt-20 relative overflow-hidden">
    <!-- Background Decoration -->
    <div class="absolute inset-0 hero-gradient opacity-10 pointer-events-none"></div>
    
    <div class="w-full max-w-md relative z-10 animate-in fade-in slide-in-from-bottom-8 duration-700 my-12">
      <Card class="!p-8 shadow-2xl">
        <div class="text-center mb-10">
          <div class="inline-flex items-center justify-center p-4 bg-emerald-500/10 rounded-3xl mb-4 group hover:scale-110 transition-transform cursor-pointer" @click="$router.push('/')">
             <svg class="w-8 h-8 text-emerald-500" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M12 2L2 7l10 5 10-5-10-5zM2 17l10 5 10-5M2 12l10 5 10-5"></path></svg>
          </div>
          <h1 class="text-3xl font-extrabold text-main mb-2 tracking-tight">{{ $t('auth.register') }}</h1>
          <p class="text-muted">{{ $t('app.subtitle') }}</p>
        </div>

        <form @submit.prevent="handleRegister" class="space-y-5">
          <div class="grid grid-cols-2 gap-4">
            <div class="space-y-2">
              <label class="text-sm font-semibold text-muted tracking-wide ml-1 uppercase text-[11px]">{{ $t('auth.firstName') }}</label>
              <Input v-model="form.firstName" type="text" :placeholder="$t('auth.firstName')" required />
            </div>
            <div class="space-y-2">
              <label class="text-sm font-semibold text-muted tracking-wide ml-1 uppercase text-[11px]">{{ $t('auth.lastName') }}</label>
              <Input v-model="form.lastName" type="text" :placeholder="$t('auth.lastName')" required />
            </div>
          </div>

          <div class="space-y-2">
            <label class="text-sm font-semibold text-muted tracking-wide ml-1 uppercase text-[11px]">{{ $t('auth.username') }}</label>
            <Input v-model="form.username" type="text" required />
          </div>

          <div class="space-y-2">
            <label class="text-sm font-semibold text-muted tracking-wide ml-1 uppercase text-[11px]">{{ $t('auth.email') }}</label>
            <Input v-model="form.email" type="email" required />
          </div>

          <div class="space-y-4">
            <div class="space-y-2">
              <label class="text-sm font-semibold text-muted tracking-wide ml-1 uppercase text-[11px]">{{ $t('auth.password') }}</label>
              <Input v-slot="{ focus }" v-model="form.password" type="password" required />
              
              <!-- Password Strength Indicator -->
              <div v-if="form.password" class="space-y-2 px-1">
                <div class="flex gap-1 h-1">
                  <div 
                    v-for="i in 3" :key="i"
                    class="flex-1 rounded-full transition-all duration-500"
                    :class="[
                      strength >= i 
                        ? (strength === 1 ? 'bg-red-500' : strength === 2 ? 'bg-amber-500' : 'bg-emerald-500')
                        : 'bg-border/30'
                    ]"
                  ></div>
                </div>
                <div class="flex justify-between items-center text-[10px] uppercase tracking-widest font-black">
                  <span :class="strength === 1 ? 'text-red-500' : strength === 2 ? 'text-amber-500' : 'text-emerald-500'">
                    {{ strength === 1 ? $t('auth.passwordStrength.weak') : strength === 2 ? $t('auth.passwordStrength.medium') : $t('auth.passwordStrength.strong') }}
                  </span>
                  <span class="text-muted/60" v-if="form.password.length < 6">{{ $t('auth.passwordMinLength') }}</span>
                </div>
              </div>
            </div>

            <div class="space-y-2">
              <label class="text-sm font-semibold text-muted tracking-wide ml-1 uppercase text-[11px]">{{ $t('auth.confirmPassword') }}</label>
              <Input v-model="form.passwordConfirm" type="password" required />
              <p v-if="passwordsMismatch" class="text-[10px] text-red-500 font-bold uppercase tracking-wider ml-1">{{ $t('auth.passwordsMismatch') }}</p>
            </div>
          </div>

          <div v-if="error" class="text-red-500 text-sm bg-red-500/10 border border-red-500/20 p-4 rounded-xl flex items-start space-x-3">
             <svg class="w-5 h-5 mt-0.5 shrink-0" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M12 8v4m0 4h.01M21 12a9 9 0 11-18 0 9 9 0 0118 0z"></path></svg>
             <span>{{ error }}</span>
          </div>

          <Button 
            type="submit" 
            size="lg" 
            class="w-full !rounded-2xl shadow-lg shadow-emerald-500/20" 
            :loading="auth.loading"
            :disabled="passwordsMismatch || form.password.length < 6"
          >
            {{ $t('auth.register') }}
          </Button>

          <div class="text-center pt-2">
            <p class="text-muted text-sm">
              {{ $t('auth.hasAccount') }}
              <router-link to="/login" class="text-emerald-500 font-bold hover:underline ml-1 decoration-2 underline-offset-4">
                {{ $t('auth.login') }}
              </router-link>
            </p>
          </div>
        </form>
      </Card>
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref, computed } from 'vue'
import { useRouter } from 'vue-router'
import { useAuthStore } from '@/stores/auth'
import Card from '@/components/ui/Card.vue'
import Input from '@/components/ui/Input.vue'
import Button from '@/components/ui/Button.vue'

const auth = useAuthStore()
const router = useRouter()
const error = ref('')

const form = ref({
  username: '',
  email: '',
  password: '',
  passwordConfirm: '',
  firstName: '',
  lastName: '',
  preferredLanguage: localStorage.getItem('user-locale') || 'bs'
})

const strength = computed(() => {
  const p = form.value.password
  if (!p) return 0
  if (p.length < 6) return 1
  
  let score = 1
  if (p.length >= 8) score++
  if (/[0-9]/.test(p) && /[^A-Za-z0-9]/.test(p)) score++
  
  return Math.min(score, 3)
})

const passwordsMismatch = computed(() => {
  return form.value.passwordConfirm && form.value.password !== form.value.passwordConfirm
})

const handleRegister = async () => {
  error.value = ''
  try {
    await auth.register(form.value)
    router.push({ name: 'verify-email', query: { email: form.value.email } })
  } catch (err: any) {
    error.value = err.message || 'Registration failed'
  }
}
</script>
