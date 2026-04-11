<template>
  <div class="search-page-container min-h-screen flex flex-col relative bg-background overflow-x-hidden">
    <!-- Lava Lamp Background -->
    <div class="lava-container">
      <div class="blob blob-1"></div>
      <div class="blob blob-2"></div>
      <div class="blob blob-3"></div>
    </div>

    <Container class="pt-32 pb-24 space-y-8 relative z-10 flex-grow">
      <div class="w-full mx-auto space-y-4 text-center">
        <h1 class="text-4xl font-extrabold text-main tracking-tight animate-in fade-in slide-in-from-top-4 duration-500">{{ $t('search.title') }}</h1>
        <p class="text-muted font-medium opacity-80">{{ $t('search.subtitle') || 'Pretražujte baze podataka islamskih izvora (Kur\'an, Hadis, Tefsir)' }}</p>
        
        <div class="relative mt-6 group w-full max-w-sm sm:max-w-3xl md:max-w-5xl lg:max-w-7xl xl:max-w-[1400px] mx-auto transition-transform duration-300 focus-within:scale-[1.01]">
          <div class="absolute inset-y-0 left-0 pl-5 flex items-center pointer-events-none text-muted group-focus-within:text-emerald-500 transition-colors">
            <svg class="w-6 h-6" width="24" height="24" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M21 21l-6-6m2-5a7 7 0 11-14 0 7 7 0 0114 0z"></path></svg>
          </div>
          <input
            v-model="query"
            type="text"
            :placeholder="$t('search.placeholder')"
            @keyup.enter="handleSearch"
            class="w-full bg-surface border border-border rounded-2xl pl-12 pr-14 sm:pl-14 sm:pr-32 md:pr-40 py-4 sm:py-5 text-base sm:text-lg md:text-xl text-main placeholder-muted/40 focus:outline-none focus:ring-2 focus:ring-emerald-500 focus:border-transparent transition-all shadow-xl shadow-black/5"
          />
          <div class="absolute inset-y-0 right-2 w-12 sm:w-auto flex items-center justify-center">
            <Button 
              :loading="loading" 
              size="md" 
              class="!rounded-xl h-10 w-10 sm:h-11 sm:w-auto sm:min-w-[140px] md:min-w-[160px] sm:px-6 md:px-10 flex items-center justify-center !p-0 sm:!p-auto"
              @click="handleSearch"
            >
              <svg class="w-5 h-5 flex-shrink-0" :class="{ 'sm:mr-2': true }" fill="none" stroke="currentColor" viewBox="0 0 24 24" xmlns="http://www.w3.org/2000/svg"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2.5" d="M21 21l-6-6m2-5a7 7 0 11-14 0 7 7 0 0114 0z"></path></svg>
              <span class="hidden sm:inline font-bold">{{ $t('search.button') || 'Pretraži' }}</span>
            </Button>
          </div>
        </div>
      </div>

      <div v-if="loading" class="grid sm:grid-cols-2 lg:grid-cols-3 gap-6">
        <div v-for="i in 6" :key="i" class="h-48 bg-surface/50 border border-border rounded-2xl animate-pulse"></div>
      </div>

      <div v-else-if="results.length > 0" class="grid sm:grid-cols-2 lg:grid-cols-3 gap-6 animate-in fade-in slide-in-from-bottom-4 duration-500">
        <Card v-for="(res, idx) in results" :key="idx" class="hover:border-emerald-500/50 transition-all duration-300 group hover:shadow-xl hover:shadow-emerald-500/5 flex flex-col">
          <template #header>
            <div class="flex items-center justify-between">
               <span class="text-[10px] font-bold uppercase tracking-widest text-emerald-500 bg-emerald-500/10 px-2 py-0.5 rounded border border-emerald-500/20">{{ res.documentTitle ? res.documentTitle.split(' ')[0] : (res.source || 'IZVOR') }}</span>
               <span class="text-[10px] text-muted font-bold opacity-60">{{ res.documentTitle || res.reference }}</span>
            </div>
          </template>
          <p class="text-main/90 line-clamp-4 leading-relaxed group-hover:text-main transition-colors flex-grow">{{ res.content }}</p>
          
          <div class="mt-6 flex flex-col items-center space-y-4 pt-4 border-t border-border/50">
             <div v-if="res.metadata" class="flex flex-wrap justify-center gap-2">
                <span v-for="(v, k) in res.metadata" :key="k" class="text-[9px] font-bold uppercase tracking-tight text-muted bg-background/50 border border-border px-2 py-0.5 rounded-lg">{{ k }}: {{ v }}</span>
             </div>
             
             <button 
               @click="handleAskAI(res.content)"
               class="flex items-center justify-center space-x-2 py-2.5 px-6 rounded-xl border border-emerald-500/20 bg-emerald-500/5 text-[11px] font-bold text-emerald-500 hover:bg-emerald-500 hover:text-white transition-all duration-300 uppercase tracking-wider w-full sm:w-auto"
             >
               <svg class="w-4 h-4" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2.5" d="M5 3v4M3 5h4M6 17v4m-2-2h4m5-16l2.286 6.857L21 12l-7.714 2.143L11 21l-2.286-6.857L1 12l7.714-2.143L11 3z"></path></svg>
               <span>{{ $t('search.buttonAskAI') || 'Pitaj AI' }}</span>
             </button>
          </div>
        </Card>
      </div>

      <div v-else-if="searched" class="text-center py-24 animate-in fade-in zoom-in duration-500">
        <div class="w-20 h-20 rounded-3xl bg-surface border border-border flex items-center justify-center mx-auto text-muted/30 mb-6 shadow-inner">
          <svg class="w-10 h-10" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M9.172 9.172a4 4 0 015.656 0M9 10h.01M15 10h.01M21 12a9 9 0 11-18 0 9 9 0 0118 0z"></path></svg>
        </div>
        <p class="text-muted text-lg font-medium">{{ $t('search.noResults') }}</p>
      </div>
    </Container>
    
    <!-- Auth Modal -->
    <Modal :show="showAuthModal" @close="showAuthModal = false">
      <div class="space-y-6">
        <div class="w-20 h-20 bg-emerald-500/10 rounded-[2rem] flex items-center justify-center mx-auto mb-2 group-hover:scale-110 transition-transform duration-500">
          <svg class="w-10 h-10 text-emerald-500" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M12 15v2m-6 4h12a2 2 0 002-2v-6a2 2 0 00-2-2H6a2 2 0 00-2 2v6a2 2 0 002 2zm10-10V7a4 4 0 00-8 0v4h8z"></path></svg>
        </div>
        
        <div class="space-y-2">
          <h3 class="text-2xl font-bold text-white tracking-tight">{{ $t('search.authModal.title') }}</h3>
          <p class="text-muted leading-relaxed max-w-xs mx-auto text-sm">{{ $t('search.authModal.description') }}</p>
        </div>

        <div class="grid grid-cols-1 gap-3 pt-4">
          <Button class="w-full !rounded-2xl h-12 shadow-lg shadow-emerald-500/10" @click="$router.push('/login')">
            {{ $t('search.authModal.login') }}
          </Button>
          <button 
            @click="$router.push('/register')"
            class="w-full h-12 rounded-2xl border border-white/5 bg-white/5 hover:bg-white/10 text-white font-bold transition-all"
          >
            {{ $t('search.authModal.register') }}
          </button>
        </div>
      </div>
    </Modal>

    <!-- Footer -->
    <footer class="mt-auto py-8 text-center border-t border-border/10 bg-surface/10 backdrop-blur-xl relative z-10 transition-all duration-500">
      <p class="text-[10px] font-bold text-muted/40 uppercase tracking-[0.3em]">
        &copy; {{ new Date().getFullYear() }} IlmAI — {{ $t('home.heroSubtitle') }}
      </p>
    </footer>
  </div>
</template>

<script setup lang="ts">
import { ref } from 'vue'
import { useRouter } from 'vue-router'
import { api } from '@/lib/api'
import { useI18n } from 'vue-i18n'
import { useAuthStore } from '@/stores/auth'
import Container from '@/components/layout/Container.vue'
import Card from '@/components/ui/Card.vue'
import Button from '@/components/ui/Button.vue'
import Modal from '@/components/ui/Modal.vue'

const router = useRouter()
const { locale } = useI18n()
const authStore = useAuthStore()

const query = ref('')
const loading = ref(false)
const results = ref<any[]>([])
const searched = ref(false)
const showAuthModal = ref(false)

const handleSearch = async () => {
  if (!query.value.trim()) return
  loading.value = true
  searched.value = true
  try {
    results.value = await api.search.query({ query: query.value, language: locale.value })
  } catch (err) {
    console.error('Search failed:', err)
  } finally {
    loading.value = false
  }
}

const handleAskAI = (content: string) => {
  if (!authStore.isAuthenticated) {
    showAuthModal.value = true
    return
  }
  router.push({ name: 'chat', query: { prompt: content } })
}
</script>
