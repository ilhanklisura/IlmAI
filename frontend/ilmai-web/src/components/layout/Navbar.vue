<template>
  <nav class="fixed top-0 left-0 right-0 z-50 bg-surface/80 backdrop-blur-md border-b border-border/50">
    <div class="max-w-[1500px] mx-auto px-4 md:px-8 h-16 flex items-center justify-between">
      <!-- Logo -->
      <router-link to="/" class="flex items-center space-x-3 group">
        <div class="w-10 h-10 rounded-xl bg-emerald-500/10 border border-emerald-500/20 flex items-center justify-center group-hover:border-emerald-500/40 transition-colors">
          <svg class="w-6 h-6 text-emerald-500" width="24" height="24" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round">
            <path d="M12 3a6 6 0 0 0 9 9 9 9 0 1 1-9-9z" />
            <path d="M19 3l-1.5 1.5L16 3l1.5 1.5L16 6l1.5-1.5L19 6l-1.5-1.5z" />
          </svg>
        </div>
        <div class="flex flex-col text-left">
          <span class="text-xl font-bold text-main tracking-tight leading-tight">IlmAI</span>
          <span class="text-[9px] text-muted font-bold uppercase tracking-[0.2em] leading-none">{{ $t('app.subtitle') }}</span>
        </div>
      </router-link>

      <!-- Desktop Nav -->
      <div class="hidden md:flex items-center space-x-6">
          <router-link 
            v-for="link in navLinks" 
            :key="link.to" 
            :to="link.to"
            class="text-sm font-semibold text-muted hover:text-main transition-all duration-200"
            active-class="!text-emerald-500"
          >
            {{ $t(link.label) }}
          </router-link>

        <div class="h-4 w-[1px] bg-border mx-2"></div>

        <template v-if="auth.isAuthenticated">
              <button @click="settingsOpen = true" class="text-muted hover:text-main transition-all p-2 rounded-xl hover:bg-surface">
                <svg class="w-5 h-5" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M10.325 4.317c.426-1.756 2.924-1.756 3.35 0a1.724 1.724 0 002.573 1.066c1.543-.94 3.31.826 2.37 2.37a1.724 1.724 0 001.065 2.572c1.756.426 1.756 2.924 0 3.35a1.724 1.724 0 00-1.066 2.573c.94 1.543-.826 3.31-2.37 2.37a1.724 1.724 0 00-2.572 1.065c-.426 1.756-2.924 1.756-3.35 0a1.724 1.724 0 00-2.573-1.066c-1.543.94-3.31-.826-2.37-2.37a1.724 1.724 0 00-1.065-2.572c-1.756-.426-1.756-2.924 0-3.35a1.724 1.724 0 001.066-2.573c-.94-1.543.826-3.31 2.37-2.37.996.608 2.296.07 2.572-1.065z"></path><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M15 12a3 3 0 11-6 0 3 3 0 016 0z"></path></svg>
              </button>
              
          <!-- Welcome Message (Desktop) -->
          <div class="hidden lg:flex items-center space-x-1.5 text-sm font-medium text-muted/80 px-4">
            <span>{{ $t('nav.welcome') }},</span>
            <span class="text-emerald-500 font-bold">{{ auth.user?.firstName }} {{ auth.user?.lastName }}</span>
          </div>

              <button @click="auth.logout()" class="text-muted hover:text-red-500 transition-all p-2 rounded-xl hover:bg-red-500/10">
                <svg class="w-5 h-5" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M17 16l4-4m0 0l-4-4m4 4H7m6 4v1a3 3 0 01-3 3H6a3 3 0 01-3-3V7a3 3 0 013-3h4a3 3 0 013 3v1"></path></svg>
              </button>
        </template>
        <template v-else>
          <button @click="settingsOpen = true" class="text-muted hover:text-main transition-all mr-2 p-2 rounded-xl hover:bg-surface">
            <svg class="w-5 h-5" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M10.325 4.317c.426-1.756 2.924-1.756 3.35 0a1.724 1.724 0 002.573 1.066c1.543-.94 3.31.826 2.37 2.37a1.724 1.724 0 001.065 2.572c1.756.426 1.756 2.924 0 3.35a1.724 1.724 0 00-1.066 2.573c.94 1.543-.826 3.31-2.37 2.37a1.724 1.724 0 00-2.572 1.065c-.426 1.756-2.924 1.756-3.35 0a1.724 1.724 0 00-2.573-1.066c-1.543.94-3.31-.826-2.37-2.37a1.724 1.724 0 00-1.065-2.572c-1.756-.426-1.756-2.924 0-3.35a1.724 1.724 0 001.066-2.573c-.94-1.543.826-3.31 2.37-2.37.996.608 2.296.07 2.572-1.065z"></path><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M15 12a3 3 0 11-6 0 3 3 0 016 0z"></path></svg>
          </button>
          <router-link to="/login" class="px-5 py-2 bg-emerald-500 text-white text-sm font-bold rounded-xl hover:bg-emerald-600 transition-all shadow-lg shadow-emerald-500/20 active:scale-95">
            {{ $t('nav.login') }}
          </router-link>
        </template>

        <SettingsModal :is-open="settingsOpen" @close="settingsOpen = false" />
      </div>

      <!-- Mobile Menu Toggle -->
      <button @click="mobileMenuOpen = !mobileMenuOpen" class="md:hidden text-muted p-2 hover:bg-surface rounded-lg transition-colors">
        <svg class="w-6 h-6" fill="none" stroke="currentColor" viewBox="0 0 24 24">
          <path v-if="!mobileMenuOpen" stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M4 6h16M4 12h16M4 18h16" />
          <path v-else stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M6 18L18 6M6 6l12 12" />
        </svg>
      </button>
    </div>

    <!-- Mobile Nav -->
    <transition enter-active-class="transition duration-200 ease-out" enter-from-class="opacity-0 -translate-y-4" enter-to-class="opacity-100 translate-y-0" leave-active-class="transition duration-150 ease-in" leave-from-class="opacity-100 translate-y-0" leave-to-class="opacity-0 -translate-y-4">
      <div v-if="mobileMenuOpen" class="md:hidden bg-background border-b border-border shadow-2xl">
        <div class="px-4 py-8 space-y-6">
          <!-- Welcome Message (Mobile) -->
          <div v-if="auth.isAuthenticated" class="pb-4 border-b border-border/50">
            <p class="text-xs font-bold text-muted uppercase tracking-widest mb-1">{{ $t('nav.welcome') }}</p>
            <p class="text-xl font-bold text-main">{{ auth.user?.firstName }} {{ auth.user?.lastName }}</p>
          </div>
          <router-link 
            v-for="link in navLinks" 
            :key="link.to" 
            :to="link.to"
            @click="mobileMenuOpen = false"
            class="block text-lg font-bold text-muted hover:text-main"
            active-class="text-emerald-500"
          >
            {{ $t(link.label) }}
          </router-link>
          <div class="pt-6 border-t border-border">
            <template v-if="auth.isAuthenticated">
              <button @click="settingsOpen = true; mobileMenuOpen = false" class="block w-full text-left text-lg font-bold text-muted mb-6">{{ $t('nav.settings') }}</button>
              <button @click="auth.logout(); mobileMenuOpen = false" class="block w-full text-left text-lg font-bold text-red-500">{{ $t('nav.logout') }}</button>
            </template>
            <template v-else>
              <router-link to="/login" @click="mobileMenuOpen = false" class="block w-full text-center px-4 py-4 bg-emerald-500 text-white font-bold rounded-2xl shadow-xl shadow-emerald-500/20">{{ $t('nav.login') }}</router-link>
            </template>
          </div>
        </div>
      </div>
    </transition>
  </nav>
</template>

<script setup lang="ts">
import { ref } from 'vue'
import { useAuthStore } from '@/stores/auth'
import SettingsModal from '@/components/layout/SettingsModal.vue'

const auth = useAuthStore()
const mobileMenuOpen = ref(false)
const settingsOpen = ref(false)

const navLinks = [
  { label: 'nav.home', to: '/' },
  { label: 'nav.chat', to: '/chat' },
  { label: 'nav.search', to: '/search' },
]
</script>
