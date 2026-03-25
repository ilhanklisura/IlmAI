<template>
  <Container class="py-12 space-y-8">
    <div class="max-w-3xl mx-auto space-y-4 text-center">
      <h1 class="text-4xl font-extrabold text-main tracking-tight animate-in fade-in slide-in-from-top-4 duration-500">{{ $t('search.title') }}</h1>
      <p class="text-muted font-medium opacity-80">{{ $t('search.subtitle') || 'Pretražujte baze podataka islamskih izvora (Kur\'an, Hadis, Tefsir)' }}</p>
      
      <div class="relative mt-6 group max-w-2xl mx-auto">
        <div class="absolute inset-y-0 left-0 pl-5 flex items-center pointer-events-none text-muted group-focus-within:text-emerald-500 transition-colors">
          <svg class="w-6 h-6" width="24" height="24" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M21 21l-6-6m2-5a7 7 0 11-14 0 7 7 0 0114 0z"></path></svg>
        </div>
        <input
          v-model="query"
          type="text"
          :placeholder="$t('search.placeholder')"
          @keyup.enter="handleSearch"
          class="w-full bg-surface border border-border rounded-2xl pl-14 pr-28 py-5 text-main text-lg placeholder-muted/40 focus:outline-none focus:ring-2 focus:ring-emerald-500 focus:border-transparent transition-all shadow-xl shadow-black/5"
        />
        <div class="absolute inset-y-0 right-2.5 flex items-center">
          <Button 
            :loading="loading" 
            size="md" 
            class="!rounded-xl h-11 px-6 shadow-lg shadow-emerald-500/20"
            @click="handleSearch"
          >
            {{ $t('search.button') || 'Pretraži' }}
          </Button>
        </div>
      </div>
    </div>

    <div v-if="loading" class="grid sm:grid-cols-2 lg:grid-cols-3 gap-6">
      <div v-for="i in 6" :key="i" class="h-48 bg-surface/50 border border-border rounded-2xl animate-pulse"></div>
    </div>

    <div v-else-if="results.length > 0" class="grid sm:grid-cols-2 lg:grid-cols-3 gap-6">
      <Card v-for="(res, idx) in results" :key="idx" class="hover:border-emerald-500/50 transition-all duration-300 group hover:shadow-xl hover:shadow-emerald-500/5">
        <template #header>
          <div class="flex items-center justify-between">
             <span class="text-[10px] font-bold uppercase tracking-widest text-emerald-500 bg-emerald-500/10 px-2 py-0.5 rounded border border-emerald-500/20">{{ res.source }}</span>
             <span class="text-[10px] text-muted font-bold opacity-60">{{ res.reference }}</span>
          </div>
        </template>
        <p class="text-main/90 line-clamp-4 leading-relaxed group-hover:text-main transition-colors">{{ res.content }}</p>
        <div v-if="res.metadata" class="mt-4 flex flex-wrap gap-2 pt-4 border-t border-border/50">
            <span v-for="(v, k) in res.metadata" :key="k" class="text-[9px] font-bold uppercase tracking-tight text-muted bg-background/50 border border-border px-2 py-0.5 rounded-lg">{{ k }}: {{ v }}</span>
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
</template>

<script setup lang="ts">
import { ref } from 'vue'
import { api } from '@/lib/api'
import { useI18n } from 'vue-i18n'
import Container from '@/components/layout/Container.vue'
import Card from '@/components/ui/Card.vue'
import Button from '@/components/ui/Button.vue'

const { locale } = useI18n()
const query = ref('')
const loading = ref(false)
const results = ref<any[]>([])
const searched = ref(false)

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
</script>
