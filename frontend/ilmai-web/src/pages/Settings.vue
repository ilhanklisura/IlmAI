<template>
  <Container class="py-12 space-y-8 max-w-2xl">
    <h1 class="text-3xl font-bold text-white">{{ $t('settings.title') }}</h1>
    
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
  </Container>
</template>

<script setup lang="ts">
import { useI18n } from 'vue-i18n'
import { useAuthStore } from '@/stores/auth'
import Container from '@/components/layout/Container.vue'
import Card from '@/components/ui/Card.vue'
import Button from '@/components/ui/Button.vue'

const { locale } = useI18n()
const auth = useAuthStore()

const saveSettings = () => {
  // Logic to save settings if needed via API
  alert('Settings saved locally.')
}
</script>
