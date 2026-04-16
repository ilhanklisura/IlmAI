<template>
  <div class="min-h-screen bg-background flex">
    <!-- Sidebar -->
    <aside class="w-72 bg-surface border-r border-border/50 flex flex-col fixed inset-y-0 z-50">
      <div class="p-6">
        <router-link to="/admin" class="flex items-center space-x-3 group">
          <div class="w-10 h-10 rounded-xl bg-emerald-500/10 border border-emerald-500/20 flex items-center justify-center">
            <svg class="w-6 h-6 text-emerald-500" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2">
              <path d="M12 3a6 6 0 0 0 9 9 9 9 0 1 1-9-9z" />
            </svg>
          </div>
          <div class="flex flex-col">
            <span class="text-xl font-bold text-main">IlmAI</span>
            <span class="text-[10px] text-emerald-500 font-black uppercase tracking-widest">{{ $t('app.name') }} Admin</span>
          </div>
        </router-link>
      </div>

      <nav class="flex-1 px-4 space-y-1 mt-4">
        <router-link 
          v-for="item in menuItems" 
          :key="item.to"
          :to="item.to"
          class="flex items-center space-x-3 px-4 py-3 rounded-xl font-bold text-sm transition-all duration-200 group"
          :class="[
            $route.path === item.to 
              ? 'bg-emerald-500 text-white shadow-lg shadow-emerald-500/20' 
              : 'text-muted hover:bg-emerald-500/5 hover:text-emerald-500'
          ]"
        >
          <div class="w-5 h-5" v-html="item.icon"></div>
          <span>{{ item.label }}</span>
        </router-link>
      </nav>

      <div class="p-6 border-t border-border/50">
        <router-link to="/" class="flex items-center space-x-3 px-4 py-3 rounded-xl font-bold text-xs text-muted hover:bg-emerald-500/10 hover:text-emerald-500 transition-all mb-2">
          <svg class="w-4 h-4" fill="none" stroke="currentColor" stroke-width="2" viewBox="0 0 24 24">
            <path stroke-linecap="round" stroke-linejoin="round" d="M3 12l2-2m0 0l7-7 7 7M5 10v10a1 1 0 001 1h3m10-11l2 2m-2-2v10a1 1 0 01-1 1h-3m-6 0a1 1 0 001-1v-4a1 1 0 011-1h2a1 1 0 011 1v4a1 1 0 001 1m-6 0h6" />
          </svg>
          <span>{{ $t('common.backToHome') }}</span>
        </router-link>
        
        <button @click="auth.logout()" class="w-full flex items-center space-x-3 px-4 py-3 rounded-xl font-bold text-xs text-muted hover:bg-red-500/10 hover:text-red-500 transition-all">
          <svg class="w-4 h-4" fill="none" stroke="currentColor" stroke-width="2" viewBox="0 0 24 24">
            <path stroke-linecap="round" stroke-linejoin="round" d="M17 16l4-4m0 0l-4-4m4 4H7m6 4v1a3 3 0 01-3 3H6a3 3 0 01-3-3V7a3 3 0 013-3h4a3 3 0 013 3v1" />
          </svg>
          <span>{{ $t('nav.logout') }}</span>
        </button>
      </div>
    </aside>

    <!-- Main Content -->
    <div class="flex-1 ml-72 flex flex-col min-h-screen">
      <!-- Top Bar -->
      <header class="h-16 bg-surface/80 backdrop-blur-md border-b border-border/50 sticky top-0 z-40 px-8 flex items-center justify-between">
        <h1 class="text-sm font-black uppercase tracking-widest text-main">{{ currentPageTitle }}</h1>
        
        <div class="flex items-center space-x-6">
          <!-- Quick Settings -->
          <div class="flex items-center space-x-2 pr-6 border-r border-border/50">
            <!-- Theme Toggle -->
            <button @click="toggleTheme" class="p-2 text-muted hover:text-emerald-500 transition-colors" title="Promijeni temu">
              <svg v-if="theme === 'dark'" class="w-5 h-5" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M12 3v1m0 16v1m9-9h-1M4 12H3m15.364-6.364l-.707.707M6.343 17.657l-.707.707m12.728 0l-.707-.707M6.343 6.343l-.707-.707m12.728 12.728L5.657 5.657" /></svg>
              <svg v-else class="w-5 h-5" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M20.354 15.354A9 9 0 018.646 3.646 9.003 9.003 0 0012 21a9.003 9.003 0 008.354-5.646z" /></svg>
            </button>
            <!-- Language Toggle -->
            <button @click="toggleLanguage" class="flex items-center space-x-1.5 px-2 py-1 rounded-lg hover:bg-emerald-500/10 transition-colors">
              <span class="text-[10px] font-black uppercase text-muted hover:text-emerald-500">{{ currentLang }}</span>
            </button>
          </div>

          <!-- User Info -->
          <div class="flex items-center space-x-4">
            <div class="flex flex-col text-right">
              <span class="text-xs font-bold text-main">{{ auth.user?.firstName }} {{ auth.user?.lastName }}</span>
              <span class="text-[9px] text-emerald-500 font-bold uppercase tracking-widest leading-none">Admin</span>
            </div>
            <div class="w-9 h-9 rounded-xl bg-emerald-500/10 border border-emerald-500/20 flex items-center justify-center text-emerald-500 font-bold text-xs">
              {{ auth.user?.firstName?.[0] }}{{ auth.user?.lastName?.[0] }}
            </div>
          </div>
        </div>
      </header>

      <main class="p-8 flex-grow">
        <router-view v-slot="{ Component }">
          <transition 
            enter-active-class="transition duration-300 ease-out"
            enter-from-class="opacity-0 translate-y-4"
            enter-to-class="opacity-100 translate-y-0"
            mode="out-in"
          >
            <component :is="Component" />
          </transition>
        </router-view>
      </main>

      <!-- Admin Footer -->
      <footer class="mt-auto py-6 px-8 border-t border-border/10 text-center">
        <p class="text-[10px] font-bold text-muted/40 uppercase tracking-[0.2em] flex items-center justify-center flex-wrap gap-2">
          <span>&copy; {{ new Date().getFullYear() }} IlmAI</span>
          <span class="w-1 h-1 rounded-full bg-border/40"></span>
          <span>Admin Panel</span>
          <span class="w-1 h-1 rounded-full bg-border/40"></span>
          <span>{{ $t('admin.dashboard.currentUser') }}: {{ auth.user?.firstName }} {{ auth.user?.lastName }}</span>
        </p>
      </footer>
    </div>
  </div>
</template>

<script setup lang="ts">
import { computed, ref, onMounted } from 'vue'
import { useRoute } from 'vue-router'
import { useAuthStore } from '@/stores/auth'
import { useI18n } from 'vue-i18n'

const route = useRoute()
const auth = useAuthStore()
const { locale, t } = useI18n()

const theme = ref(localStorage.getItem('theme') || 'dark')
const currentLang = computed(() => locale.value)

const menuItems = computed(() => [
  { 
    to: '/admin', 
    label: t('admin.nav.dashboard'), 
    icon: '<svg fill="none" stroke="currentColor" stroke-width="2" viewBox="0 0 24 24"><path d="M3 12l2-2m0 0l7-7 7 7M5 10v10a1 1 0 001 1h3m10-11l2 2m-2-2v10a1 1 0 01-1 1h-3m-6 0a1 1 0 001-1v-4a1 1 0 011-1h2a1 1 0 011 1v4a1 1 0 001 1m-6 0h6"/></svg>'
  },
  { 
    to: '/admin/users', 
    label: t('admin.nav.users'), 
    icon: '<svg fill="none" stroke="currentColor" stroke-width="2" viewBox="0 0 24 24"><path d="M12 4.354a4 4 0 110 5.292M15 21H3v-1a6 6 0 0112 0v1zm0 0h6v-1a6 6 0 00-9-5.197M13 7a4 4 0 11-8 0 4 4 0 018 0z"/></svg>'
  },
  { 
    to: '/admin/analytics', 
    label: t('admin.nav.analytics'), 
    icon: '<svg fill="none" stroke="currentColor" stroke-width="2" viewBox="0 0 24 24"><path d="M9 19v-6a2 2 0 00-2-2H5a2 2 0 00-2 2v6a2 2 0 002 2h2a2 2 0 002 2zm0 0h6v-1a6 6 0 00-9-5.197M13 7a4 4 0 11-8 0 4 4 0 018 0z" /></svg>'
  },
  { 
    to: '/admin/logs', 
    label: t('admin.nav.logs'), 
    icon: '<svg fill="none" stroke="currentColor" stroke-width="2" viewBox="0 0 24 24"><path d="M9 12h6m-6 4h6m2 5H7a2 2 0 01-2-2V5a2 2 0 012-2h5.586a1 1 0 01.707.293l5.414 5.414a1 1 0 01.293.707V19a2 2 0 01-2 2z"/></svg>'
  }
])

const currentPageTitle = computed(() => {
  const item = menuItems.value.find(i => i.to === route.path)
  return item ? item.label : 'Admin'
})

const toggleTheme = () => {
  theme.value = theme.value === 'dark' ? 'light' : 'dark'
  localStorage.setItem('theme', theme.value)
  updateTheme()
}

const updateTheme = () => {
  const isLight = theme.value === 'light'
  document.documentElement.classList.toggle('light', isLight)
  document.documentElement.classList.toggle('dark', !isLight)
  document.body.style.backgroundColor = isLight ? '#f8fafc' : '#0f172a'
}

const toggleLanguage = () => {
  const newLang = locale.value === 'bs' ? 'en' : 'bs'
  locale.value = newLang
  localStorage.setItem('user-locale', newLang)
}

onMounted(() => {
  updateTheme()
})
</script>
