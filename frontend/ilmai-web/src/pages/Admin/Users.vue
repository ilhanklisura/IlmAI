<template>
  <div class="space-y-8 animate-in fade-in slide-in-from-bottom-4 duration-500">
    <!-- Header -->
    <div class="flex flex-col md:flex-row md:items-center justify-between gap-4">
      <div>
        <h2 class="text-2xl font-bold text-main">{{ $t('admin.users.title') }}</h2>
        <p class="text-sm text-muted">{{ $t('admin.users.searchPlaceholder', 'Upravljanje korisničkim nalozima i pristupom.') }}</p>
      </div>
      
      <!-- Search -->
      <div class="relative w-full md:w-80">
        <svg class="absolute left-4 top-1/2 -translate-y-1/2 w-4 h-4 text-muted" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M21 21l-6-6m2-5a7 7 0 11-14 0 7 7 0 0114 0z" /></svg>
        <input 
          v-model="searchQuery"
          type="text" 
          :placeholder="$t('admin.users.searchPlaceholder')"
          class="w-full pl-11 pr-4 py-3 bg-surface border border-border/50 rounded-2xl text-sm focus:border-emerald-500/50 focus:ring-4 focus:ring-emerald-500/5 transition-all outline-none"
        >
      </div>
    </div>

    <!-- Users Table -->
    <div class="bg-surface border border-border/50 rounded-3xl overflow-hidden shadow-sm">
      <div class="overflow-x-auto">
        <table class="w-full text-left border-collapse">
          <thead>
            <tr class="bg-border/10">
              <th class="px-6 py-4 text-[10px] font-black uppercase tracking-widest text-muted border-b border-border/50">{{ $t('auth.username') }}</th>
              <th class="px-6 py-4 text-[10px] font-black uppercase tracking-widest text-muted border-b border-border/50">{{ $t('verifyEmail.title', 'Status') }}</th>
              <th class="px-6 py-4 text-[10px] font-black uppercase tracking-widest text-muted border-b border-border/50">{{ $t('admin.logs.level', 'Uloge') }}</th>
              <th class="px-6 py-4 text-[10px] font-black uppercase tracking-widest text-muted border-b border-border/50">{{ $t('admin.logs.time', 'Pridružen') }}</th>
              <th class="px-6 py-4 text-[10px] font-black uppercase tracking-widest text-muted border-b border-border/50 text-right">{{ $t('admin.users.actions') }}</th>
            </tr>
          </thead>
          <tbody class="divide-y divide-border/50">
            <tr v-if="loading" v-for="i in 5" :key="i" class="animate-pulse">
              <td colspan="5" class="px-6 py-8"><div class="h-6 bg-border/20 rounded-lg"></div></td>
            </tr>
            <tr v-else-if="filteredUsers.length" v-for="user in filteredUsers" :key="user.id" class="hover:bg-emerald-500/[0.02] transition-colors group">
              <td class="px-6 py-5">
                <div class="flex items-center space-x-3">
                  <div class="w-10 h-10 rounded-xl bg-surface-light border border-border/50 flex items-center justify-center font-bold text-emerald-500">
                    {{ user.username[0].toUpperCase() }}
                  </div>
                  <div class="flex flex-col">
                    <span class="text-sm font-bold text-main">{{ user.fullName || user.username }}</span>
                    <span class="text-xs text-muted">{{ user.email }}</span>
                  </div>
                </div>
              </td>
              <td class="px-6 py-5">
                <div class="flex flex-col space-y-1">
                  <span 
                    class="px-2 py-1 rounded-full text-[9px] font-black uppercase tracking-wider w-fit flex items-center"
                    :class="isOnline(user) ? 'bg-emerald-500/10 text-emerald-500' : 'bg-slate-500/10 text-muted'"
                  >
                    <span v-if="isOnline(user)" class="w-1.5 h-1.5 rounded-full bg-emerald-500 mr-2 animate-pulse shadow-sm shadow-emerald-500/50"></span>
                    <span v-else class="w-1.5 h-1.5 rounded-full bg-slate-500/40 mr-2"></span>
                    {{ isOnline(user) ? $t('admin.users.statusOnline') : $t('admin.users.statusOffline') }}
                  </span>
                  <p v-if="!isOnline(user) && user.lastActiveAt" class="text-[9px] text-muted/60 font-medium ml-1">
                    {{ $t('admin.users.lastSeen') }} {{ formatLastSeen(user.lastActiveAt) }}
                  </p>
                  <span v-if="user.isEmailVerified" class="text-[9px] text-emerald-500/50 font-bold flex items-center mt-1">
                    <svg class="w-3 h-3 mr-1" fill="currentColor" viewBox="0 0 20 20"><path d="M10 18a8 8 0 100-16 8 8 0 000 16zm3.707-9.293a1 1 0 00-1.414-1.414L9 10.586 7.707 9.293a1 1 0 00-1.414 1.414l2 2a1 1 0 001.414 0l4-4z"/></svg>
                    {{ $t('admin.users.verified') }}
                  </span>
                  <span v-else class="text-[9px] text-red-500/50 font-bold flex items-center">
                    <svg class="w-3 h-3 mr-1" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M12 8v4m0 4h.01M21 12a9 9 0 11-18 0 9 9 0 0118 0z"/></svg>
                    {{ $t('admin.users.unverified') }}
                  </span>
                </div>
              </td>
              <td class="px-6 py-5">
                <div class="flex flex-wrap gap-1">
                  <span 
                    v-for="role in user.roles" 
                    :key="role"
                    class="px-2 py-0.5 bg-border/30 rounded-lg text-[10px] font-bold text-muted border border-border/50"
                  >
                    {{ role }}
                  </span>
                </div>
              </td>
              <td class="px-6 py-5 text-sm text-muted font-medium">
                {{ formatDate(user.createdAt) }}
              </td>
              <td class="px-6 py-5 text-right">
                <div class="flex items-center justify-end space-x-2">
                  <button 
                    @click="handleEditRole(user)"
                    class="p-2 rounded-xl text-emerald-500 hover:bg-emerald-500/10 transition-all"
                    :title="$t('admin.users.changeRole')"
                  >
                    <svg class="w-4 h-4" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M9 12l2 2 4-4m5.618-4.016A11.955 11.955 0 0112 2.944a11.955 11.955 0 01-8.618 3.04A12.02 12.02 0 003 9c0 5.591 3.824 10.29 9 11.622 5.176-1.332 9-6.03 9-11.622 0-1.042-.133-2.052-.382-3.016z" /></svg>
                  </button>
                  <button 
                    @click="handleResetPassword(user)"
                    class="p-2 rounded-xl text-amber-500 hover:bg-amber-500/10 transition-all"
                    :title="$t('admin.users.resetPasswordTooltip')"
                  >
                    <svg class="w-4 h-4" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M15 7a2 2 0 012 2m4 0a6 6 0 01-7.743 5.743L11 17H9v2H7v2H4a1 1 0 01-1-1v-2.586a1 1 0 01.293-.707l5.964-5.964A6 6 0 1121 9z" /></svg>
                  </button>
                  <button 
                    @click="toggleUser(user)"
                    class="px-3 py-1.5 rounded-xl text-[10px] font-black uppercase tracking-widest transition-all border"
                    :class="user.isActive ? 'text-red-500 border-red-500/20 hover:bg-red-500/10' : 'text-emerald-500 border-emerald-500/20 hover:bg-emerald-500/10'"
                  >
                    {{ user.isActive ? 'Blok' : 'Aktiv' }}
                  </button>
                </div>
              </td>
            </tr>
            <tr v-else>
              <td colspan="5" class="px-6 py-12 text-center text-muted italic">{{ $t('admin.users.noUsersFound') }}</td>
            </tr>
          </tbody>
        </table>
      </div>
    </div>

    <!-- Password Reset Success Modal -->
    <Teleport to="body">
      <div v-if="resetPasswordData" class="fixed inset-0 z-[10001] flex items-center justify-center p-4">
        <div class="absolute inset-0 bg-slate-950/60 backdrop-blur-sm" @click="resetPasswordData = null"></div>
        <div class="relative w-full max-w-sm bg-surface border border-border/50 rounded-3xl p-8 shadow-2xl animate-in fade-in zoom-in duration-300">
          <div class="w-16 h-16 bg-emerald-500/10 rounded-2xl flex items-center justify-center text-emerald-500 mx-auto mb-6">
            <svg class="w-8 h-8" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M15 7a2 2 0 012 2m4 0a6 6 0 01-7.743 5.743L11 17H9v2H7v2H4a1 1 0 01-1-1v-2.586a1 1 0 01.293-.707l5.964-5.964A6 6 0 1121 9z" /></svg>
          </div>
          <h3 class="text-xl font-bold text-main text-center mb-2">{{ $t('admin.users.passwordResetSuccess') }}</h3>
          <p class="text-xs text-muted text-center mb-6">{{ $t('admin.users.tempPasswordFor') }} <span class="text-main font-bold">{{ resetPasswordData.username }}</span> {{ $t('admin.users.is') }}</p>
          
          <div class="bg-surface-light border border-border/50 p-4 rounded-2xl mb-8 flex items-center justify-between group">
            <code class="text-xl font-black text-emerald-500 tracking-wider">{{ resetPasswordData.newPassword }}</code>
            <button @click="copyToClipboard(resetPasswordData.newPassword)" class="p-2 text-muted hover:text-emerald-500 transition-colors">
              <svg class="w-4 h-4" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M8 5H6a2 2 0 00-2 2v12a2 2 0 002 2h10a2 2 0 002-2v-1M8 5a2 2 0 002 2h2a2 2 0 002-2M8 5a2 2 0 012-2h2a2 2 0 012 2m0 0h2a2 2 0 012 2v3m2 4H10m0 0l3-3m-3 3l3 3" /></svg>
            </button>
          </div>

          <button @click="resetPasswordData = null" class="w-full py-4 bg-emerald-500 text-white font-bold rounded-2xl hover:bg-emerald-600 transition-all shadow-lg shadow-emerald-500/20">
            {{ $t('common.close') }}
          </button>
        </div>
      </div>

      <!-- Confirmation Modal -->
      <div v-if="showConfirmReset" class="fixed inset-0 z-[10001] flex items-center justify-center p-4">
        <div class="absolute inset-0 bg-slate-950/60 backdrop-blur-sm" @click="showConfirmReset = false"></div>
        <div class="relative w-full max-w-sm bg-surface border border-border/50 rounded-3xl p-8 shadow-2xl animate-in fade-in zoom-in duration-300">
          <div class="w-16 h-16 bg-amber-500/10 rounded-2xl flex items-center justify-center text-amber-500 mx-auto mb-6">
            <svg class="w-8 h-8" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M12 9v2m0 4h.01m-6.938 4h13.856c1.54 0 2.502-1.667 1.732-3L13.732 4c-.77-1.333-2.694-1.333-3.464 0L3.34 16c-.77 1.333.192 3 1.732 3z" /></svg>
          </div>
          <h3 class="text-xl font-bold text-main text-center mb-2">{{ $t('admin.users.confirmResetTitle') }}</h3>
          <p class="text-sm text-muted text-center mb-8">
            {{ $t('admin.users.confirmResetDesc') }} <span class="text-main font-bold">{{ userToReset?.username }}</span>?
          </p>
          
          <div class="flex gap-3">
            <button @click="showConfirmReset = false" class="flex-1 py-3 bg-surface border border-border/50 text-muted font-bold rounded-xl hover:bg-border/20 transition-all">
              {{ $t('common.cancel') }}
            </button>
            <button @click="confirmResetPassword" class="flex-1 py-3 bg-amber-500 text-white font-bold rounded-xl hover:bg-amber-600 transition-all shadow-lg shadow-amber-500/20">
              {{ $t('admin.users.resetBtn') }}
            </button>
          </div>
        </div>
      </div>

      <!-- Role Selection Modal -->
      <div v-if="showRoleModal" class="fixed inset-0 z-[10001] flex items-center justify-center p-4">
        <div class="absolute inset-0 bg-slate-950/60 backdrop-blur-sm" @click="showRoleModal = false"></div>
        <div class="relative w-full max-w-sm bg-surface border border-border/50 rounded-3xl p-8 shadow-2xl animate-in fade-in zoom-in duration-300">
          <div class="w-16 h-16 bg-emerald-500/10 rounded-2xl flex items-center justify-center text-emerald-500 mx-auto mb-6">
            <svg class="w-8 h-8" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="1.5" d="M9 12l2 2 4-4m5.618-4.016A11.955 11.955 0 0112 2.944a11.955 11.955 0 01-8.618 3.04A12.02 12.02 0 003 9c0 5.591 3.824 10.29 9 11.622 5.176-1.332 9-6.03 9-11.622 0-1.042-.133-2.052-.382-3.016z" /></svg>
          </div>
          <h3 class="text-xl font-bold text-main text-center mb-2">{{ $t('admin.users.changeRole') }}</h3>
          <p class="text-xs text-muted text-center mb-8">
            {{ $t('admin.users.selectRole') }} <span class="text-main font-bold">{{ userToEditRole?.username }}</span>
          </p>
          
          <div class="space-y-3">
            <button 
              @click="confirmChangeRole('admin')" 
              class="w-full py-4 bg-surface border border-emerald-500/30 text-emerald-500 font-bold rounded-2xl hover:bg-emerald-500/10 transition-all shadow-lg shadow-emerald-500/5 flex items-center justify-center space-x-3"
            >
              <svg class="w-5 h-5" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M9 12l2 2 4-4m5.618-4.016A11.955 11.955 0 0112 2.944a11.955 11.955 0 01-8.618 3.04A12.02 12.02 0 003 9c0 5.591 3.824 10.29 9 11.622 5.176-1.332 9-6.03 9-11.622 0-1.042-.133-2.052-.382-3.016z" /></svg>
              <span>{{ $t('admin.users.roleAdmin') }}</span>
            </button>
            <button 
              @click="confirmChangeRole('user')" 
              class="w-full py-4 bg-surface border border-border/50 text-muted font-bold rounded-2xl hover:bg-border/10 transition-all flex items-center justify-center space-x-3"
            >
              <svg class="w-5 h-5" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M16 7a4 4 0 11-8 0 4 4 0 018 0zM12 14a7 7 0 00-7 7h14a7 7 0 00-7-7z" /></svg>
              <span>{{ $t('admin.users.roleUser') }}</span>
            </button>
            <button 
              @click="showRoleModal = false" 
              class="w-full py-3 text-xs text-muted/60 font-bold hover:text-main transition-colors uppercase tracking-widest pt-4"
            >
              {{ $t('common.cancel') }}
            </button>
          </div>
        </div>
      </div>
    </Teleport>
  </div>
</template>

<script setup lang="ts">
import { ref, onMounted, computed } from 'vue'
import { api } from '@/lib/api'
import { useI18n } from 'vue-i18n'
import { useToastStore } from '@/stores/toast'

const { t } = useI18n()

const users = ref<any[]>([])
const loading = ref(true)
const searchQuery = ref('')
const resetPasswordData = ref<any>(null)
const showConfirmReset = ref(false)
const showRoleModal = ref(false)
const userToReset = ref<any>(null)
const userToEditRole = ref<any>(null)

const toast = useToastStore()

const loadUsers = async () => {
  loading.value = true
  try {
    users.value = await api.admin.getUsers()
  } catch (err) {
    console.error('Failed to load users:', err)
  } finally {
    loading.value = false
  }
}

const toggleUser = async (user: any) => {
  try {
    const res = await api.admin.toggleUser(user.id)
    user.isActive = res.isActive
  } catch (err) {
    console.error('Failed to toggle user status:', err)
  }
}

const handleResetPassword = (user: any) => {
  userToReset.value = user
  showConfirmReset.value = true
}

const confirmResetPassword = async () => {
  if (!userToReset.value) return
  
  try {
    const res = await api.admin.resetPassword(userToReset.value.id)
    resetPasswordData.value = {
      username: userToReset.value.username,
      newPassword: res.newPassword
    }
  } catch (err) {
    console.error('Failed to reset password:', err)
    toast.error(t('admin.users.resetFailed'))
  } finally {
    showConfirmReset.value = false
    userToReset.value = null
  }
}

const handleEditRole = (user: any) => {
  userToEditRole.value = user
  showRoleModal.value = true
}

const confirmChangeRole = async (roleName: string) => {
  if (!userToEditRole.value) return
  
  try {
    const res = await api.admin.updateUserRole(userToEditRole.value.id, roleName)
    userToEditRole.value.roles = res.roles
    toast.success(t('admin.users.roleUpdated'))
  } catch (err: any) {
    console.error('Failed to update role:', err)
    toast.error(err.message || t('admin.users.roleUpdateFailed'))
  } finally {
    showRoleModal.value = false
    userToEditRole.value = null
  }
}

const copyToClipboard = (text: string) => {
  navigator.clipboard.writeText(text)
  toast.success(t('admin.users.passwordCopied'))
}

const filteredUsers = computed(() => {
  if (!searchQuery.value) return users.value
  const q = searchQuery.value.toLowerCase()
  return users.value.filter(u => 
    u.username.toLowerCase().includes(q) || 
    u.email.toLowerCase().includes(q) ||
    u.fullName?.toLowerCase().includes(q)
  )
})

const formatDate = (dateStr: string) => {
  const d = new Date(dateStr)
  const day = d.getDate().toString().padStart(2, '0')
  const month = (d.getMonth() + 1).toString().padStart(2, '0')
  const year = d.getFullYear()
  return `${day}.${month}.${year}.`
}

const isOnline = (user: any) => {
  if (!user.lastActiveAt) return false
  
  // Ensure we treat the date as UTC by appending Z if missing
  let dateStr = user.lastActiveAt
  if (!dateStr.endsWith('Z') && !dateStr.includes('+')) {
    dateStr += 'Z'
  }
  
  const lastActive = new Date(dateStr).getTime()
  const now = new Date().getTime()
  
  // 5 minutes threshold (in ms)
  const diff = now - lastActive
  
  // Debug logging to console (will help users see what's happening)
  // console.log(`User ${user.username} diff: ${diff}ms`)
  
  return diff < 5 * 60 * 1000
}

const formatLastSeen = (dateStr: string) => {
  const lastActive = new Date(dateStr).getTime()
  const now = new Date().getTime()
  const diffMs = now - lastActive
  const diffMins = Math.floor(diffMs / 60000)
  const diffHours = Math.floor(diffMins / 6000)
  
  if (diffMins < 1) return t('admin.users.justNow')
  if (diffMins < 60) return `${diffMins} ${t('admin.users.mins')}`
  if (diffHours < 24) return `${diffHours} ${t('admin.users.hours')}`
  return formatDate(dateStr)
}

onMounted(loadUsers)
</script>
