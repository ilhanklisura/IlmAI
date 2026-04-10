<template>
  <Teleport to="body">
    <Transition
      enter-active-class="transition duration-300 ease-out"
      enter-from-class="opacity-0 scale-95"
      enter-to-class="opacity-100 scale-100"
      leave-active-class="transition duration-200 ease-in"
      leave-from-class="opacity-100 scale-100"
      leave-to-class="opacity-0 scale-95"
    >
      <div v-if="show" class="fixed inset-0 z-[10001] flex items-center justify-center p-4 sm:p-6">
        <!-- Backdrop -->
        <div class="absolute inset-0 bg-slate-950/60 backdrop-blur-sm" @click="$emit('close')"></div>
        
        <!-- Modal Content -->
        <div 
          class="relative w-full max-w-lg border rounded-[2.5rem] shadow-2xl overflow-hidden animate-in fade-in zoom-in duration-300"
          :class="[isLight ? 'bg-white border-slate-200 text-slate-900' : 'bg-[#0f172a] border-white/10 text-white']"
        >
          <!-- Close Button -->
          <button 
            @click="$emit('close')"
            class="absolute top-6 right-6 p-2 rounded-full transition-all z-10"
            :class="[isLight ? 'bg-slate-100 text-slate-500 hover:text-slate-900' : 'bg-white/5 text-slate-400 hover:text-white']"
          >
            <svg class="w-5 h-5" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M6 18L18 6M6 6l12 12"></path></svg>
          </button>

          <div class="p-8 sm:p-10 text-center">
            <slot />
          </div>
        </div>
      </div>
    </Transition>
  </Teleport>
</template>

<script setup lang="ts">
import { computed } from 'vue'

defineProps<{
  show: boolean
}>()

defineEmits(['close'])

const isLight = computed(() => document.documentElement.classList.contains('light'))
</script>
