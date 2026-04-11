<template>
  <div class="min-h-screen bg-transparent text-main font-sans selection:bg-emerald-500/30">
    <Navbar v-if="!isAdminRoute" />
    <main :class="{ 'pt-16': !isAdminRoute, 'pt-0': isAdminRoute }">
      <router-view v-slot="{ Component }">
        <transition 
          enter-active-class="transition duration-300 ease-out"
          enter-from-class="opacity-0 translate-y-4"
          enter-to-class="opacity-100 translate-y-0"
          leave-active-class="transition duration-200 ease-in"
          leave-from-class="opacity-100 translate-y-0"
          leave-to-class="opacity-0 translate-y-4"
          mode="out-in"
        >
          <component :is="Component" :key="$route.path" />
        </transition>
      </router-view>
    </main>
    
    <!-- Toast/Notification placeholder -->
    <Toast />
    <div id="notifications" class="fixed bottom-4 right-4 z-[100] space-y-2"></div>
  </div>
</template>

<script setup lang="ts">
import { onMounted, computed } from 'vue'
import { useRoute } from 'vue-router'
import { useAuthStore } from '@/stores/auth'
import Navbar from '@/components/layout/Navbar.vue'
import Toast from '@/components/ui/Toast.vue'

const auth = useAuthStore()
const route = useRoute()

const isAdminRoute = computed(() => route.path.startsWith('/admin'))

onMounted(async () => {
  if (auth.token) {
    await auth.fetchUser()
  }
})
</script>

<style>
/* Global transition for pages */
.page-enter-active,
.page-leave-active {
  transition: opacity 0.2s ease;
}

.page-enter-from,
.page-leave-to {
  opacity: 0;
}
</style>
