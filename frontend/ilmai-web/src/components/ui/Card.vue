<template>
  <div class="bg-surface/50 border border-border rounded-xl overflow-hidden shadow-sm">
    <div v-if="$slots.header" class="px-6 py-4 border-b border-border">
      <slot name="header" />
    </div>
    <div class="px-6 py-4">
      <slot />
    </div>
    <div v-if="$slots.footer" class="px-6 py-4 border-t border-slate-700 bg-slate-800/30">
      <slot name="footer" />
    </div>
  </div>
</template>

<script setup lang="ts">
// No props defined for class to allow standard attribute fallthrough to the root <div>
</script>
