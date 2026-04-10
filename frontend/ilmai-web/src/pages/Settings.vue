<template>
  <Container class="py-12 space-y-8 max-w-2xl">
    <div class="flex items-center justify-between">
      <h1 class="text-3xl font-bold text-white">{{ $t('settings.title') }}</h1>
      <span class="text-[10px] bg-white/10 text-muted px-2 py-1 rounded-full uppercase tracking-widest font-bold">Verzija 1.5</span>
    </div>
    
    <Card>
      <div class="space-y-6">
        <!-- Language -->
        <div class="space-y-2">
          <label class="block text-sm font-medium text-gray-400">{{ $t('settings.language') }}</label>
          <div class="grid grid-cols-2 gap-4">
            <button 
              @click="locale = 'bs'"
              :class="['px-4 py-3 rounded-lg border text-sm font-medium transition-all', locale === 'bs' ? 'bg-emerald-500/10 border-emerald-500 text-emerald-500' : 'bg-slate-900 border-slate-700 text-gray-400 hover:text-white']"
            >
              Bosanski
            </button>
            <button 
              @click="locale = 'en'"
              :class="['px-4 py-3 rounded-lg border text-sm font-medium transition-all', locale === 'en' ? 'bg-emerald-500/10 border-emerald-500 text-emerald-500' : 'bg-slate-900 border-slate-700 text-gray-400 hover:text-white']"
            >
              English
            </button>
          </div>
        </div>

        <!-- User Info -->
        <div class="space-y-2 pt-4 border-t border-slate-700">
           <label class="block text-sm font-medium text-gray-400">Account</label>
           <div class="bg-slate-900 rounded-lg p-4 border border-slate-700">
              <div class="text-white font-semibold">{{ auth.user?.firstName }} {{ auth.user?.lastName }}</div>
              <div class="text-sm text-gray-500">{{ auth.user?.email }}</div>
           </div>
        </div>
      </div>
      
      <div class="mt-8 flex justify-end">
        <Button @click="saveSettings">
          {{ $t('settings.save') }}
        </Button>
      </div>
    </Card>

    <!-- Change Password -->
    <Card v-if="auth.isAuthenticated">
      <div class="space-y-6">
        <div class="flex items-center space-x-3 mb-2">
          <div class="p-2 bg-emerald-500/10 rounded-xl">
            <svg class="w-5 h-5 text-emerald-500" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M12 15v2m-6 4h12a2 2 0 002-2v-6a2 2 0 00-2-2H6a2 2 0 00-2 2v6a2 2 0 002 2zm10-10V7a4 4 0 00-8 0v4h8z"></path></svg>
          </div>
          <h2 class="text-xl font-bold text-white">{{ $t('settings.changePassword') }}</h2>
        </div>

        <div class="space-y-4">
          <div class="space-y-2">
            <label class="block text-[11px] font-semibold text-muted tracking-wide ml-1 uppercase">{{ $t('settings.currentPassword') }}</label>
            <Input v-model="passwordForm.currentPassword" type="password" class="w-full" />
          </div>
          <div class="space-y-2">
            <label class="block text-[11px] font-semibold text-muted tracking-wide ml-1 uppercase">{{ $t('settings.newPassword') }}</label>
            <Input v-model="passwordForm.newPassword" type="password" class="w-full" />
          </div>
          <div class="space-y-2">
            <label class="block text-[11px] font-semibold text-muted tracking-wide ml-1 uppercase">{{ $t('settings.confirmPassword') }}</label>
            <Input v-model="passwordForm.confirmNewPassword" type="password" class="w-full" />
          </div>
        </div>

        <!-- Success Message -->
        <div v-if="passwordSuccess" class="text-emerald-400 text-sm bg-emerald-500/10 border border-emerald-500/20 p-4 rounded-xl flex items-center space-x-3">
          <svg class="w-5 h-5 shrink-0" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M5 13l4 4L19 7"></path></svg>
          <span>{{ $t('settings.passwordChanged') }}</span>
        </div>

        <!-- Error Message -->
        <div v-if="passwordError" class="text-red-500 text-sm bg-red-500/10 border border-red-500/20 p-4 rounded-xl flex items-start space-x-3">
          <svg class="w-5 h-5 mt-0.5 shrink-0" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M12 8v4m0 4h.01M21 12a9 9 0 11-18 0 9 9 0 0118 0z"></path></svg>
          <span>{{ passwordError }}</span>
        </div>

        <div class="flex justify-end">
          <Button @click="handleChangePassword" :loading="changingPassword" class="!rounded-xl">
            {{ $t('settings.passwordChangeBtn') }}
          </Button>
        </div>
      </div>
    </Card>

    <Card v-else class="text-center py-8">
       <p class="text-muted">Prijavite se da biste promijenili lozinku.</p>
    </Card>
  </Container>
</template>

<script setup lang="ts">
import { ref, onMounted } from 'vue'
import { useI18n } from 'vue-i18n'
import { useAuthStore } from '@/stores/auth'
import { api } from '@/lib/api'
import Container from '@/components/layout/Container.vue'
import Card from '@/components/ui/Card.vue'
import Button from '@/components/ui/Button.vue'
import Input from '@/components/ui/Input.vue'

const { locale } = useI18n()
const auth = useAuthStore()

onMounted(() => {
  console.log('Settings Page Diagnostic:', {
    isAuthenticated: auth.isAuthenticated,
    user: auth.user,
    timestamp: new Date().toISOString()
  })
})

const passwordForm = ref({
  currentPassword: '',
  newPassword: '',
  confirmNewPassword: ''
})
const changingPassword = ref(false)
const passwordSuccess = ref(false)
const passwordError = ref('')

const saveSettings = () => {
  // Logic to save settings if needed via API
  alert('Settings saved locally.')
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
  } catch (err: any) {
    passwordError.value = err.message || (locale.value === 'bs' ? 'Greška pri promjeni lozinke.' : 'Error changing password.')
  } finally {
    changingPassword.value = false
  }
}
</script>
