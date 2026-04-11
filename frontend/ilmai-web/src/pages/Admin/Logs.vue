<template>
  <div class="space-y-6 animate-in fade-in slide-in-from-bottom-4 duration-500 pb-12">
    <!-- Header -->
    <div class="flex flex-col md:flex-row md:items-center justify-between gap-4">
      <div>
        <h2 class="text-2xl font-bold text-main px-2">{{ $t('admin.logs.title') }}</h2>
        <p class="text-sm text-muted px-2">{{ $t('admin.logs.subtitle') }}</p>
      </div>
      
      <div class="flex items-center space-x-3 bg-surface border border-border/50 p-2 rounded-2xl">
        <button 
          @click="downloadLogs('csv')"
          class="flex items-center space-x-2 px-4 py-2 bg-emerald-500/10 text-emerald-500 rounded-xl hover:bg-emerald-500 hover:text-white transition-all text-xs font-bold uppercase tracking-wider"
        >
          <svg class="w-4 h-4" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M12 10v6m0 0l-3-3m3 3l3-3m2 8H7a2 2 0 01-2-2V5a2 2 0 012-2h5.586a1 1 0 01.707.293l5.414 5.414a1 1 0 01.293.707V19a2 2 0 01-2 2z" /></svg>
          <span>{{ $t('admin.logs.downloadCsv') }}</span>
        </button>
        <button 
          @click="downloadLogs('txt')"
          class="flex items-center space-x-2 px-4 py-2 bg-muted/10 text-muted rounded-xl hover:bg-muted hover:text-white transition-all text-xs font-bold uppercase tracking-wider"
        >
          <svg class="w-4 h-4" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M9 12h6m-6 4h6m2 5H7a2 2 0 01-2-2V5a2 2 0 012-2h5.586a1 1 0 01.707.293l5.414 5.414a1 1 0 01.293.707V19a2 2 0 01-2 2z" /></svg>
          <span>{{ $t('admin.logs.downloadTxt') }}</span>
        </button>
      </div>
    </div>

    <!-- Filters & Search -->
    <div class="bg-surface border border-border/50 rounded-3xl p-6">
      <div class="flex flex-wrap items-center gap-4">
        <div class="flex-1 min-w-[200px]">
          <select 
            v-model="selectedLevel"
            @change="loadLogs"
            class="w-full bg-surface-light border border-border/50 text-main rounded-xl px-4 py-3 text-sm focus:outline-none focus:ring-2 focus:ring-emerald-500/20"
          >
            <option value="">{{ $t('admin.logs.allLevels') }}</option>
            <option value="Info" class="text-emerald-500">Info</option>
            <option value="Warning" class="text-amber-500">Warning</option>
            <option value="Error" class="text-red-500">Error</option>
          </select>
        </div>
        <button @click="loadLogs" class="p-3 bg-emerald-600 text-white rounded-xl hover:bg-emerald-700 transition-colors shadow-lg shadow-emerald-500/20">
          <svg class="w-5 h-5" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M4 4v5h.582m15.356 2A8.001 8.001 0 004.582 9m0 0H9m11 11v-5h-.581m0 0a8.003 8.003 0 01-15.357-2m15.357 2H15" /></svg>
        </button>
      </div>
    </div>

    <!-- Logs Table -->
    <div class="bg-surface border border-border/50 rounded-3xl overflow-hidden">
      <div class="overflow-x-auto">
        <table class="w-full text-left">
          <thead>
            <tr class="bg-surface-light border-b border-border/50">
              <th class="px-6 py-4 text-[10px] font-black uppercase tracking-widest text-muted">{{ $t('admin.logs.time') }}</th>
              <th class="px-6 py-4 text-[10px] font-black uppercase tracking-widest text-muted">{{ $t('admin.logs.level') }}</th>
              <th class="px-6 py-4 text-[10px] font-black uppercase tracking-widest text-muted">{{ $t('admin.logs.message') }}</th>
              <th class="px-6 py-4 text-[10px] font-black uppercase tracking-widest text-muted text-right">{{ $t('admin.logs.source') }}</th>
            </tr>
          </thead>
          <tbody class="divide-y divide-border/30">
            <tr v-for="log in logs" :key="log.id" class="hover:bg-emerald-500/5 transition-colors group">
              <td class="px-6 py-4 whitespace-nowrap">
                <span class="text-xs font-medium text-muted tabular-nums">{{ formatDate(log.createdAt) }}</span>
              </td>
              <td class="px-6 py-4">
                <span 
                  class="text-[9px] font-black uppercase tracking-widest px-2 py-1 rounded-md"
                  :class="{
                    'bg-emerald-500/10 text-emerald-500': log.level === 'Info',
                    'bg-amber-500/10 text-amber-500': log.level === 'Warning',
                    'bg-red-500/10 text-red-500': log.level === 'Error'
                  }"
                >
                  {{ log.level }}
                </span>
              </td>
              <td class="px-6 py-4">
                <p class="text-sm text-main font-medium line-clamp-1 group-hover:line-clamp-none transition-all cursor-default">
                  {{ log.message }}
                </p>
              </td>
              <td class="px-6 py-4 text-right">
                <span class="text-[10px] font-bold text-muted bg-border/20 px-2 py-1 rounded-lg">{{ log.source || 'N/A' }}</span>
              </td>
            </tr>
            <tr v-if="logs.length === 0 && !loading">
              <td colspan="4" class="px-6 py-12 text-center text-muted italic">Nema dostupnih logova za odabrane kriterije.</td>
            </tr>
          </tbody>
        </table>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref, onMounted } from 'vue'
import { api } from '@/lib/api'
import { useI18n } from 'vue-i18n'
import { useToastStore } from '@/stores/toast'

const { t } = useI18n()

const logs = ref<any[]>([])
const loading = ref(true)
const selectedLevel = ref('')

const loadLogs = async () => {
  loading.value = true
  try {
    logs.value = await api.admin.getLogs(selectedLevel.value)
  } catch (err) {
    console.error('Failed to load logs:', err)
  } finally {
    loading.value = false
  }
}

const downloadLogs = async (format: string) => {
  try {
    const url = api.admin.exportLogsUrl(format)
    const token = localStorage.getItem('ilmai_token')
    const response = await fetch(url, {
      headers: {
        'Authorization': `Bearer ${token}`
      }
    })
    
    if (!response.ok) throw new Error('Download failed')
    
    const blob = await response.blob()
    const downloadUrl = window.URL.createObjectURL(blob)
    const link = document.createElement('a')
    link.href = downloadUrl
    link.setAttribute('download', `logs-${new Date().toISOString().split('T')[0]}.${format}`)
    document.body.appendChild(link)
    link.click()
    link.remove()
    window.URL.revokeObjectURL(downloadUrl)
  } catch (err) {
    const toast = useToastStore()
    console.error('Download failed:', err)
    toast.error(t('admin.analytics.retry'))
  }
}

const formatDate = (dateStr: string) => {
  const d = new Date(dateStr)
  return d.toLocaleString('bs-BA', {
    day: '2-digit',
    month: '2-digit',
    year: 'numeric',
    hour: '2-digit',
    minute: '2-digit',
    second: '2-digit'
  })
}

onMounted(loadLogs)
</script>
