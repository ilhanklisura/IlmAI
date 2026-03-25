<template>
  <div class="min-h-[calc(100vh-64px)] flex items-center justify-center p-4">
    <div class="absolute inset-0 hero-gradient opacity-10 pointer-events-none"></div>
    
    <div class="w-full max-w-md relative z-10 animate-in fade-in slide-in-from-bottom-8 duration-700">
      <Card class="!p-8 shadow-2xl">
        <div class="text-center mb-10">
          <div class="inline-flex items-center justify-center p-4 bg-emerald-500/10 rounded-3xl mb-4 group hover:scale-110 transition-transform cursor-pointer" @click="$router.push('/')">
             <svg class="w-8 h-8 text-emerald-500" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M12 2L2 7l10 5 10-5-10-5zM2 17l10 5 10-5M2 12l10 5 10-5"></path></svg>
          </div>
          <h1 class="text-3xl font-extrabold text-main mb-2 tracking-tight">{{ $t('auth.login') }}</h1>
          <p class="text-muted">{{ $t('app.subtitle') }}</p>
        </div>

        <form @submit.prevent="handleLogin" class="space-y-6">
          <div class="space-y-2">
            <label class="text-sm font-semibold text-muted tracking-wide ml-1 uppercase text-[11px]">{{ $t('auth.username') }}</label>
            <Input
              v-model="form.username"
              type="text"
              required
              class="w-full"
            />
          </div>

          <div class="space-y-2">
            <label class="text-sm font-semibold text-muted tracking-wide ml-1 uppercase text-[11px]">{{ $t('auth.password') }}</label>
            <Input
              v-model="form.password"
              type="password"
              required
              class="w-full"
            />
          </div>

          <div v-if="error" class="text-red-500 text-sm bg-red-500/10 border border-red-500/20 p-4 rounded-xl flex items-start space-x-3">
             <svg class="w-5 h-5 mt-0.5 shrink-0" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M12 8v4m0 4h.01M21 12a9 9 0 11-18 0 9 9 0 0118 0z"></path></svg>
             <span>{{ error }}</span>
          </div>

          <Button type="submit" size="lg" class="w-full !rounded-2xl shadow-lg shadow-emerald-500/20" :loading="auth.loading">
            {{ $t('auth.login') }}
          </Button>

          <div class="text-center pt-2">
            <p class="text-muted text-sm">
              {{ $t('auth.noAccount') }}
              <router-link to="/register" class="text-emerald-500 font-bold hover:underline ml-1 decoration-2 underline-offset-4">
                {{ $t('auth.register') }}
              </router-link>
            </p>
          </div>
        </form>
      </Card>
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref } from 'vue'
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
  password: ''
})

const handleLogin = async () => {
  error.value = ''
  try {
    await auth.login(form.value.username, form.value.password)
    router.push('/')
  } catch (err: any) {
    error.value = err.message || 'Login failed'
  }
}
</script>
