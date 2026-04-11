<template>
  <div class="space-y-8 animate-in fade-in duration-500">
    <!-- Header with Refresh -->
    <div class="flex items-center justify-between">
      <div>
        <h2 class="text-2xl font-bold text-main">{{ $t('app.name') }} Admin</h2>
        <p class="text-sm text-muted">{{ $t('admin.dashboard.subtitle') }}</p>
      </div>
      <button 
        @click="loadStats" 
        class="flex items-center space-x-2 px-4 py-2 bg-surface border border-border/50 rounded-xl text-sm font-bold text-muted hover:text-emerald-500 hover:border-emerald-500/30 transition-all active:scale-95"
      >
        <svg class="w-4 h-4" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M4 4v5h.582m15.356 2A8.001 8.001 0 004.582 9m0 0H9m11 11v-5h-.581m0 0a8.003 8.003 0 01-15.357-2m15.357 2H15" /></svg>
        <span>{{ $t('admin.dashboard.refresh') }}</span>
      </button>
    </div>

    <!-- Stats Grid -->
    <div class="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-4 gap-6">
      <div v-for="(stat, idx) in statsCards" :key="stat.label" class="bg-surface border border-border/50 p-6 rounded-3xl hover:border-emerald-500/30 transition-all group">
        <div class="flex items-center justify-between mb-4">
          <div class="w-12 h-12 rounded-2xl flex items-center justify-center transition-colors" :class="stat.bg">
            <!-- Dynamic Icon -->
            <svg v-if="idx === 0" class="w-6 h-6" :class="stat.color" fill="none" stroke="currentColor" stroke-width="2" viewBox="0 0 24 24"><path d="M12 4.354a4 4 0 110 5.292M15 21H3v-1a6 6 0 0112 0v1zm0 0h6v-1a6 6 0 00-9-5.197M13 7a4 4 0 11-8 0 4 4 0 018 0z"/></svg>
            <svg v-else-if="idx === 1" class="w-6 h-6" :class="stat.color" fill="none" stroke="currentColor" stroke-width="2" viewBox="0 0 24 24"><path d="M8 10h.01M12 10h.01M16 10h.01M9 16H5a2 2 0 01-2-2V6a2 2 0 012-2h14a2 2 0 012 2v8a2 2 0 01-2 2h-5l-5 5v-5z"/></svg>
            <svg v-else-if="idx === 2" class="w-6 h-6" :class="stat.color" fill="none" stroke="currentColor" stroke-width="2" viewBox="0 0 24 24"><path d="M13 10V3L4 14h7v7l9-11h-7z"/></svg>
            <svg v-else-if="idx === 3" class="w-6 h-6" :class="stat.color" fill="none" stroke="currentColor" stroke-width="2" viewBox="0 0 24 24"><path d="M17 8h2a2 2 0 012 2v6a2 2 0 01-2 2h-2v4l-4-4H9a1.994 1.994 0 01-1.414-.586m0 0L11 14h4a2 2 0 002-2V6a2 2 0 00-2-2H5a2 2 0 00-2 2v6a2 2 0 002 2h2v4l.586-.586z"/></svg>
          </div>
          <span class="text-[10px] font-black uppercase tracking-widest text-muted/50 group-hover:text-emerald-500 transition-colors">Uživo</span>
        </div>
        <div>
          <h3 class="text-3xl font-black text-main tabular-nums">{{ loading ? '...' : stat.value }}</h3>
          <p class="text-sm font-bold text-muted mt-1">{{ stat.label }}</p>
        </div>
      </div>
    </div>

    <div class="grid grid-cols-1 lg:grid-cols-3 gap-8">
      <!-- Top Topics -->
      <div class="lg:col-span-2 bg-surface border border-border/50 rounded-3xl p-8">
        <div class="flex items-center justify-between mb-8">
          <h3 class="text-xl font-bold text-main">{{ $t('admin.dashboard.popularTopics') }}</h3>
          <span class="px-3 py-1 bg-emerald-500/10 text-emerald-500 text-[10px] font-black uppercase tracking-widest rounded-full">AI Generisano</span>
        </div>
        
        <div v-if="loading" class="space-y-6">
          <div v-for="i in 5" :key="i" class="h-12 bg-border/20 animate-pulse rounded-xl"></div>
        </div>
        <div v-else-if="stats?.topTopics.length" class="space-y-6">
          <div v-for="(topic, idx) in stats.topTopics" :key="idx" class="relative">
            <div class="flex items-center justify-between mb-2">
              <span class="text-sm font-bold text-main">{{ topic.title }}</span>
              <span class="text-xs font-bold text-muted">{{ topic.count }} razgovora</span>
            </div>
            <div class="h-2 w-full bg-border/30 rounded-full overflow-hidden">
              <div 
                class="h-full bg-emerald-500 rounded-full transition-all duration-1000"
                :style="{ width: (topic.count / stats.topTopics[0].count * 100) + '%' }"
              ></div>
            </div>
          </div>
        </div>
        <div v-else class="text-center py-12 text-muted italic">Nema dostupnih podataka o temama.</div>
      </div>

      <!-- System Health / Quick actions -->
      <div class="bg-surface border border-border/50 rounded-3xl p-8">
        <h3 class="text-xl font-bold text-main mb-8">{{ $t('admin.dashboard.systemStatus') }}</h3>
        <div class="space-y-4">
          <div class="flex items-center justify-between p-4 bg-emerald-500/5 border border-emerald-500/10 rounded-2xl">
            <div class="flex items-center space-x-3">
              <div class="w-2 h-2 rounded-full bg-emerald-500 animate-pulse"></div>
              <span class="text-sm font-bold text-main">{{ $t('admin.dashboard.apiServer') }}</span>
            </div>
            <span class="text-[10px] font-black uppercase text-emerald-500">{{ $t('admin.dashboard.ok') }}</span>
          </div>
          <div class="flex items-center justify-between p-4 bg-emerald-500/5 border border-emerald-500/10 rounded-2xl">
            <div class="flex items-center space-x-3">
              <div class="w-2 h-2 rounded-full bg-emerald-500 animate-pulse"></div>
              <span class="text-sm font-bold text-main">{{ $t('admin.dashboard.aiEngine') }}</span>
            </div>
            <span class="text-[10px] font-black uppercase text-emerald-500">{{ $t('admin.dashboard.active') }}</span>
          </div>
          <div class="flex items-center justify-between p-4 bg-emerald-500/5 border border-emerald-500/10 rounded-2xl">
            <div class="flex items-center space-x-3">
              <div class="w-2 h-2 rounded-full bg-emerald-500 animate-pulse"></div>
              <span class="text-sm font-bold text-main">{{ $t('admin.dashboard.database') }}</span>
            </div>
            <span class="text-[10px] font-black uppercase text-emerald-500">{{ $t('admin.dashboard.connected') }}</span>
          </div>
        </div>
        
        <div class="mt-8 pt-8 border-t border-border/50">
          <p class="text-[10px] text-muted font-bold uppercase tracking-widest mb-4">Administrativne akcije</p>
          <div class="grid grid-cols-2 gap-3">
            <button class="p-3 bg-surface-light border border-border/50 rounded-xl text-[10px] font-bold text-muted hover:text-main transition-all">Clear Cache</button>
            <button class="p-3 bg-surface-light border border-border/50 rounded-xl text-[10px] font-bold text-muted hover:text-main transition-all">Export Report</button>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref, onMounted, computed } from 'vue'
import { api } from '@/lib/api'
import { useI18n } from 'vue-i18n'

const { t } = useI18n()

const stats = ref<any>(null)
const loading = ref(true)

const loadStats = async () => {
  loading.value = true
  try {
    stats.value = await api.admin.getStats()
  } catch (err) {
    console.error('Failed to load admin stats:', err)
  } finally {
    loading.value = false
  }
}

onMounted(loadStats)

const statsCards = computed(() => [
  { 
    label: t('admin.dashboard.totalUsers'), 
    value: stats.value?.totalUsers || 0,
    icon: { template: '<svg fill="none" stroke="currentColor" stroke-width="2" viewBox="0 0 24 24"><path d="M12 4.354a4 4 0 110 5.292M15 21H3v-1a6 6 0 0112 0v1zm0 0h6v-1a6 6 0 00-9-5.197M13 7a4 4 0 11-8 0 4 4 0 018 0z"/></svg>' },
    bg: 'bg-blue-500/10',
    color: 'text-blue-500'
  },
  { 
    label: t('admin.dashboard.newMessages'), 
    value: stats.value?.messages24h || 0,
    icon: { template: '<svg fill="none" stroke="currentColor" stroke-width="2" viewBox="0 0 24 24"><path d="M8 10h.01M12 10h.01M16 10h.01M9 16H5a2 2 0 01-2-2V6a2 2 0 012-2h14a2 2 0 012 2v8a2 2 0 01-2 2h-5l-5 5v-5z"/></svg>' },
    bg: 'bg-emerald-500/10',
    color: 'text-emerald-500'
  },
  { 
    label: t('admin.dashboard.activeUsers'), 
    value: stats.value?.activeUsers24h || 0,
    icon: { template: '<svg fill="none" stroke="currentColor" stroke-width="2" viewBox="0 0 24 24"><path d="M13 10V3L4 14h7v7l9-11h-7z"/></svg>' },
    bg: 'bg-amber-500/10',
    color: 'text-amber-500'
  },
  { 
    label: t('admin.dashboard.totalSessions'), 
    value: stats.value?.totalSessions || 0,
    icon: { template: '<svg fill="none" stroke="currentColor" stroke-width="2" viewBox="0 0 24 24"><path d="M17 8h2a2 2 0 012 2v6a2 2 0 01-2 2h-2v4l-4-4H9a1.994 1.994 0 01-1.414-.586m0 0L11 14h4a2 2 0 002-2V6a2 2 0 00-2-2H5a2 2 0 00-2 2v6a2 2 0 002 2h2v4l.586-.586z"/></svg>' },
    bg: 'bg-purple-500/10',
    color: 'text-purple-500'
  }
])
</script>
