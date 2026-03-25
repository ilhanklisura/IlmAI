<template>
  <div class="h-[100dvh] flex flex-col relative overflow-hidden bg-background">
    <!-- Lava Lamp Background -->
    <div class="lava-container">
      <div class="blob blob-1"></div>
      <div class="blob blob-2"></div>
      <div class="blob blob-3"></div>
    </div>
    
    <!-- Content Wrapper -->
    <div class="relative z-10 flex-grow flex flex-col items-center justify-center px-4 py-6">
      <Container class="w-full max-w-6xl flex flex-col items-center">
        
        <!-- Hero Section -->
        <!-- FIX: veći razmak ispod -->
        <section class="text-center space-y-4 mb-16">
          <h1 class="text-4xl md:text-6xl font-extrabold text-main tracking-tight">
            {{ $t('home.heroTitle') }} <span class="text-emerald-500">{{ $t('home.heroSubtitle') }}</span>
          </h1>
          <p class="text-md md:text-lg text-muted max-w-xl mx-auto leading-relaxed">
            {{ $t('home.description') }}
          </p>

          <div class="flex items-center justify-center gap-4 pt-4">
            <Button size="lg" class="shadow-xl shadow-emerald-500/20 px-8" @click="$router.push('/chat')">
              {{ $t('nav.chat') }}
            </Button>
            <Button variant="secondary" size="lg" class="px-8" @click="$router.push('/search')">
              {{ $t('nav.search') }}
            </Button>
          </div>
        </section>

        <!-- Dual Daily Section -->
        <!-- FIX: dodatni margin top -->
        <div class="grid md:grid-cols-2 gap-8 w-full max-w-5xl items-stretch mt-6">
          
          <!-- Ayah Card -->
          <Card class="text-left border-border/10 hover:border-emerald-500/30 transition-all duration-700 group relative backdrop-blur-2xl bg-surface/20 flex flex-col h-full overflow-hidden h-auto">
            
            <div class="absolute -top-10 -right-10 w-40 h-40 bg-emerald-500/10 rounded-full blur-3xl group-hover:bg-emerald-500/20 transition-all duration-1000"></div>
            
            <!-- FIX: ikona skroz u ćošku -->
            <div class="absolute top-0 right-1 p-0 z-0 pointer-events-none text-emerald-500/20 group-hover:text-emerald-500/40 transition-all duration-700 transform">
              <svg width="64" height="64" class="w-13 h-13" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                <path stroke-linecap="round" stroke-linejoin="round" stroke-width="0.3" d="M12 6.253v13m0-13C10.832 5.477 9.246 5 7.5 5S4.168 5.477 3 6.253v13C4.168 18.477 5.754 18 7.5 18s3.332.477 4.5 1.253m0-13C13.168 5.477 14.754 5 16.5 5c1.747 0 3.332.477 4.5 1.253v13C19.832 18.477 18.247 18 16.5 18c-1.746 0-3.332.477-4.5 1.253"></path>
              </svg>
            </div>

            <template #header>
              <h3 class="text-[10px] font-black text-emerald-500 uppercase tracking-[0.2em] flex items-center justify-center w-full">
                <span class="w-2 h-2 rounded-full bg-emerald-500 mr-2 animate-pulse"></span>
                {{ $t('home.dailyTitle') }}
              </h3>
            </template>

            <div v-if="dailyData?.ayah" class="flex flex-col pt-4">
              
              <!-- FIX: stabilno centriranje -->
              <div class="flex flex-col items-center text-center px-6 space-y-3">
                <p class="text-3xl md:text-4xl font-serif text-emerald-500/80 leading-normal dir-rtl" v-if="dailyData.ayah.textArabic">
                  {{ dailyData.ayah.textArabic }}
                </p>

                <div class="space-y-1">
                  <p class="text-lg md:text-xl font-sans font-semibold text-main leading-relaxed" style="font-family: var(--font-sans) !important;">
                    "{{ dailyData.ayah.textTranslation }}"
                  </p>
                </div>
              </div>

              <!-- FIX: footer centriran -->
              <div class="mt-auto pt-6 flex flex-col items-center justify-center text-[10px] font-black text-muted/30 uppercase tracking-[0.2em] border-t border-border/5 space-y-1">
                <span class="hover:text-emerald-500/70 transition-colors">
                  {{ dailyData.ayah.surahName }} {{ dailyData.ayah.ayahReference }}
                </span>
                <span class="opacity-70">QURAN AL-KAREEM</span>
              </div>
            </div>
          </Card>

          <!-- Hadith Card -->
          <Card class="text-left border-border/10 hover:border-emerald-500/30 transition-all duration-700 group relative backdrop-blur-2xl bg-surface/20 flex flex-col h-full overflow-hidden h-auto">
            
            <div class="absolute -top-10 -right-10 w-40 h-40 bg-blue-500/10 rounded-full blur-3xl group-hover:bg-blue-500/20 transition-all duration-1000"></div>
            
            <!-- FIX: ikona skroz u ćošku -->
            <div class="absolute top-0 right-1 p-0 z-0 pointer-events-none text-emerald-500/20 group-hover:text-emerald-500/40 transition-all duration-700 transform">
              <svg width="64" height="64" class="w-13 h-13" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                <path stroke-linecap="round" stroke-linejoin="round" stroke-width="0.3" d="M8 10h.01M12 10h.01M16 10h.01M9 16H5a2 2 0 01-2-2V6a2 2 0 012-2h14a2 2 0 012 2v8a2 2 0 01-2 2h-3l-4 4z"></path>
              </svg>
            </div>

            <template #header>
              <h3 class="text-[10px] font-black text-emerald-500 uppercase tracking-[0.2em] flex items-center justify-center w-full">
                <span class="w-2 h-2 rounded-full bg-emerald-500 mr-2 animate-pulse"></span>
                {{ $t('home.dailyHadithTitle') }}
              </h3>
            </template>

            <div v-if="dailyData?.hadith" class="flex flex-col pt-4">
              
              <!-- FIX: centriranje -->
              <div class="flex-grow flex flex-col justify-center items-center text-center px-6 space-y-4">
                <p class="text-3xl md:text-4xl font-serif text-emerald-500/80 leading-normal dir-rtl" v-if="dailyData.hadith.textArabic">
                  {{ dailyData.hadith.textArabic }}
                </p>

                <div class="space-y-1">
                  <p class="text-lg md:text-xl font-sans font-semibold text-main leading-relaxed" style="font-family: var(--font-sans) !important;">
                    "{{ dailyData.hadith.textTranslation }}"
                  </p>
                </div>
              </div>

              <!-- FIX: footer centriran -->
              <div class="mt-auto pt-6 flex flex-col items-center justify-center text-[10px] font-black text-muted/30 uppercase tracking-[0.2em] border-t border-border/5 space-y-1">
                <span class="hover:text-emerald-500/70 transition-colors">
                  {{ dailyData.hadith.hadithCollection }} #{{ dailyData.hadith.hadithNumber }}
                </span>
                <span class="opacity-70">MUHAMMAD ﷺ</span>
              </div>
            </div>
          </Card>

        </div>
      </Container>
    </div>

    <!-- Footer -->
    <footer class="relative z-10 py-4 text-center border-t border-border/10 bg-surface/5 backdrop-blur-sm">
      <p class="text-[10px] font-bold text-muted/40 uppercase tracking-[0.3em]">
        &copy; {{ new Date().getFullYear() }} IlmAI — {{ $t('home.heroSubtitle') }}
      </p>
    </footer>
  </div>
</template>

<script setup lang="ts">
import { ref, onMounted, watch } from 'vue'
import { api } from '@/lib/api'
import { useI18n } from 'vue-i18n'
import Container from '@/components/layout/Container.vue'
import Button from '@/components/ui/Button.vue'
import Card from '@/components/ui/Card.vue'

const { locale } = useI18n()
const dailyData = ref<any>(null)

const fetchDaily = async () => {
  try {
    dailyData.value = await api.daily.get(locale.value)
  } catch (err) {
    console.error('Failed to fetch daily data:', err)
  }
}

onMounted(() => fetchDaily())
watch(locale, () => fetchDaily())
</script>