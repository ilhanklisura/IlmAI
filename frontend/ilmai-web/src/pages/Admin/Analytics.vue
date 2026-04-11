<template>
  <div class="space-y-8 animate-in fade-in slide-in-from-bottom-4 duration-500 pb-12">
    <!-- Header -->
    <div class="flex items-center justify-between">
      <div>
        <h2 class="text-2xl font-bold text-main">{{ $t('admin.analytics.title') }}</h2>
        <p class="text-sm text-muted">{{ $t('admin.analytics.subtitle') }}</p>
      </div>
    </div>

    <!-- Charts Grid -->
    <div class="grid grid-cols-1 lg:grid-cols-2 gap-8">
      <!-- Activity Area Chart -->
      <div class="bg-surface border border-border/50 rounded-3xl p-8 hover:border-emerald-500/30 transition-all">
        <div class="flex items-center justify-between mb-8">
          <h3 class="text-lg font-bold text-main">{{ $t('admin.analytics.messageActivity') }}</h3>
          <span class="text-[10px] font-black uppercase text-emerald-500 bg-emerald-500/10 px-2 py-1 rounded-lg">{{ $t('admin.analytics.period7Days') }}</span>
        </div>
        <div class="h-[300px] w-full">
          <div v-if="loading" class="h-full flex items-center justify-center text-muted italic">{{ $t('admin.analytics.loading') }}</div>
          <div v-else-if="error" class="h-full flex flex-col items-center justify-center space-y-2">
            <span class="text-xs text-red-500 font-medium">{{ error }}</span>
            <button @click="loadData" class="text-[10px] font-black uppercase text-emerald-500 hover:underline">{{ $t('admin.analytics.retry') }}</button>
          </div>
          <apexchart 
            v-else
            type="area" 
            height="100%" 
            :options="areaChartOptions" 
            :series="activitySeries"
          ></apexchart>
        </div>
      </div>

      <!-- User Growth Bar Chart -->
      <div class="bg-surface border border-border/50 rounded-3xl p-8 hover:border-emerald-500/30 transition-all">
        <div class="flex items-center justify-between mb-8">
          <h3 class="text-lg font-bold text-main">{{ $t('admin.analytics.userGrowth') }}</h3>
          <span class="text-[10px] font-black uppercase text-blue-500 bg-blue-500/10 px-2 py-1 rounded-lg">{{ $t('admin.analytics.growthTrend') }}</span>
        </div>
        <div class="h-[300px] w-full">
          <div v-if="loading" class="h-full flex items-center justify-center text-muted italic">{{ $t('admin.analytics.loading') }}</div>
          <div v-else-if="error" class="h-full flex flex-col items-center justify-center space-y-2">
            <span class="text-xs text-red-500 font-medium">{{ error }}</span>
            <button @click="loadData" class="text-[10px] font-black uppercase text-emerald-500 hover:underline">{{ $t('admin.analytics.retry') }}</button>
          </div>
          <apexchart 
            v-else
            type="bar" 
            height="100%" 
            :options="barChartOptions" 
            :series="userSeries"
          ></apexchart>
        </div>
      </div>

      <!-- Distribution / User Status -->
      <div class="bg-surface border border-border/50 rounded-3xl p-8 hover:border-emerald-500/30 transition-all lg:col-span-2">
        <h3 class="text-lg font-bold text-main mb-8">{{ $t('admin.analytics.verificationStatus') }}</h3>
        <div class="h-[300px] w-full flex items-center justify-center">
          <div v-if="loading" class="h-full flex items-center justify-center text-muted italic">{{ $t('admin.analytics.loading') }}</div>
          <div v-else-if="error" class="h-full flex flex-col items-center justify-center space-y-2">
            <span class="text-xs text-red-500 font-medium">{{ error }}</span>
            <button @click="loadData" class="text-[10px] font-black uppercase text-emerald-500 hover:underline">{{ $t('admin.analytics.retry') }}</button>
          </div>
          <apexchart 
            v-else
            type="donut" 
            height="100%"
            :options="donutChartOptions" 
            :series="donutSeries"
          ></apexchart>
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
const activityData = ref<any[]>([])
const donutSeries = ref<number[]>([0, 0])
const loading = ref(true)
const error = ref<string | null>(null)

const loadData = async () => {
  console.log('[Analytics] Starting data load...')
  loading.value = true
  error.value = null
  try {
    const res = await api.admin.getActivityAnalytics()
    console.log('[Analytics] Data received:', res)
    activityData.value = res.activity || []
    donutSeries.value = res.distribution || [0, 0]
  } catch (err: any) {
    console.error('[Analytics] Failed to load activity analytics:', err)
    error.value = t('admin.analytics.retry')
  } finally {
    loading.value = false
    console.log('[Analytics] Loading finished. State:', { loading: loading.value, dataLength: activityData.value.length })
  }
}

onMounted(loadData)

const activitySeries = computed(() => [{
  name: t('admin.analytics.messageActivity'),
  data: activityData.value.map(d => d.messages)
}])

const userSeries = computed(() => [{
  name: t('admin.analytics.userGrowth'),
  data: activityData.value.map(d => d.users)
}])

const areaChartOptions = computed(() => ({
  chart: { type: 'area', toolbar: { show: false }, background: 'transparent' },
  colors: ['#10b981'],
  stroke: { curve: 'smooth', width: 2 },
  fill: {
    type: 'gradient',
    gradient: { shadeIntensity: 1, opacityFrom: 0.4, opacityTo: 0, stops: [0, 90, 100] }
  },
  xaxis: {
    categories: activityData.value.map(d => d.date),
    axisBorder: { show: false },
    axisTicks: { show: false },
    labels: { style: { colors: '#94a3b8', fontSize: '10px', fontWeight: 700 } }
  },
  yaxis: { labels: { style: { colors: '#94a3b8', fontSize: '10px' } } },
  grid: { borderColor: 'rgba(148, 163, 184, 0.1)', strokeDashArray: 4 },
  tooltip: { theme: 'dark' }
}))

const barChartOptions = computed(() => ({
  chart: { type: 'bar', toolbar: { show: false }, background: 'transparent' },
  colors: ['#3b82f6'],
  plotOptions: { bar: { borderRadius: 6, columnWidth: '40%' } },
  xaxis: {
    categories: activityData.value.map(d => d.date),
    labels: { style: { colors: '#94a3b8', fontSize: '10px', fontWeight: 700 } }
  },
  grid: { show: false },
  tooltip: { theme: 'dark' }
}))

const donutChartOptions = computed(() => ({
  chart: { type: 'donut', background: 'transparent' },
  labels: [t('admin.analytics.verified'), t('admin.analytics.unverified')],
  colors: ['#10b981', '#ef4444'],
  stroke: { show: false },
  legend: { position: 'bottom', labels: { colors: '#94a3b8' } },
  dataLabels: { enabled: false },
  tooltip: { theme: 'dark' }
}))
</script>

<style>
.apexcharts-canvas { margin: 0 auto; }
</style>
