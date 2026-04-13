import { createI18n } from 'vue-i18n'

const bs = {
  app: { name: 'IlmAI', subtitle: 'AI Asistent za Islamsko Znanje' },
  nav: { home: 'Početna', chat: 'Pitaj AI', quran: "Kur'an", hadith: 'Hadis', search: 'Pretraga', settings: 'Postavke', login: 'Prijava', register: 'Registracija', logout: 'Odjava', welcome: 'Dobrodošli' },
  home: { 
    welcome: 'Dobrodošli u IlmAI', 
    heroTitle: 'IlmAI —',
    heroSubtitle: 'Islamsko Znanje',
    description: 'Vaš AI asistent za islamsko znanje. Postavite pitanje i dobijte odgovor zasnovan na Kur\'anu, hadisima i tefsiru.', 
    askButton: 'Postavi Pitanje', 
    dailyTitle: 'Dnevni Ajet', 
    dailyHadithTitle: 'Dnevni Hadis' 
  },
  chat: { 
    placeholder: 'Postavite pitanje o islamu...', 
    send: 'Pošalji', 
    newChat: 'Novi Razgovor', 
    sessions: 'Sesije', 
    noAnswer: 'Nema odgovora na osnovu dostupnih izvora.', 
    sources: 'Izvori', 
    thinking: 'IlmAI razmišlja...',
    welcomeGreeting: 'Postavite pitanje IlmAI-u',
    disclaimer: 'IlmAI može dati netačne informacije. Provjerite izvore.'
  },
  auth: { 
    username: 'Korisničko ime', email: 'Email', password: 'Lozinka', firstName: 'Ime', lastName: 'Prezime', 
    login: 'Prijavi se', register: 'Registruj se', noAccount: 'Nemate račun?', hasAccount: 'Već imate račun?',
    firstNamePlaceholder: 'Ime', lastNamePlaceholder: 'Prezime'
  },
  settings: { title: 'Postavke', theme: 'Tema', language: 'Jezik', notifications: 'Notifikacije', dark: 'Tamna', light: 'Svijetla', save: 'Spremi', changePassword: 'Promijeni lozinku', currentPassword: 'Trenutna lozinka', newPassword: 'Nova lozinka', confirmPassword: 'Potvrdi novu lozinku', passwordChanged: 'Lozinka uspješno promijenjena!', passwordChangeBtn: 'Promijeni lozinku' },
  search: { 
    title: 'Pretraga', 
    placeholder: 'Pretražite islamske izvore...', 
    results: 'Rezultati', 
    noResults: 'Nema rezultata.',
    subtitle: 'Pretražujte baze podataka islamskih izvora (Kur\'an, Hadis, Tefsir)',
    button: 'Pretraži',
    buttonAskAI: 'Pitaj AI',
    authModal: {
      title: 'Niste prijavljeni!',
      description: 'Prijavite se ili kreirajte nalog kako biste otključali sve mogućnosti IlmAI Chat asistenta.',
      login: 'Prijava',
      register: 'Registracija'
    }
  },
  verifyEmail: {
    title: 'Verifikacija Emaila',
    subtitle: 'Poslali smo 6-cifreni kod na vaš email',
    placeholder: 'Unesite kod',
    button: 'Potvrdi',
    resend: 'Pošalji ponovo',
    success: 'Email uspješno verifikovan!',
    error: 'Neispravan kod. Pokušajte ponovo.',
    backToLogin: 'Povratak na prijavu'
  },
  common: { save: 'Spremi', close: 'Zatvori', backToHome: 'Nazad na početnu' },
  admin: {
    nav: { 
      dashboard: 'Dashboard', 
      users: 'Korisnici', 
      analytics: 'Analitika', 
      logs: 'Sistemski Logovi',
      administration: 'Administracija',
      openDashboard: 'Otvori Admin Dashboard'
    },
    dashboard: { 
      subtitle: 'Sistemska statistika i pregled rada.',
      refresh: 'Osvježi',
      currentUser: 'Trenutni korisnik',
      totalUsers: 'Ukupno Korisnika', 
      activeUsers: 'Aktivni Korisnici (24h)', 
      totalMessages: 'Ukupno Poruka', 
      newMessages: 'Poruke (24h)',
      totalSessions: 'Ukupno Razgovora',
      popularTopics: 'Popularne Teme',
      systemStatus: 'Status Sistema',
      apiServer: 'API Server',
      aiEngine: 'AI Engine',
      database: 'Database',
      ok: 'U redu',
      active: 'Aktivan',
      connected: 'Povezan'
    },
    analytics: {
      title: 'Analitika & Grafikoni',
      subtitle: 'Dubinski uvid u aktivnost i rast IlmAI platforme.',
      messageActivity: 'Aktivnost Poruka',
      userGrowth: 'Novi Korisnici',
      verificationStatus: 'Status Verifikacije Korisnika',
      period7Days: 'Zadnjih 7 dana',
      growthTrend: 'Trend Rasta',
      verified: 'Verifikovani',
      unverified: 'Neverifikovani',
      loading: 'Učitavanje podataka...',
      retry: 'Pokušaj ponovo'
    },
    logs: {
      title: 'Sistemski Logovi',
      subtitle: 'Prati greške i važne događaje u realnom vremenu.',
      downloadCsv: 'Download CSV',
      downloadTxt: 'Download TXT',
      allLevels: 'Svi Nivoi',
      time: 'Vrijeme',
      level: 'Nivo',
      message: 'Poruka',
      source: 'Izvor',
      noLogs: 'Nema dostupnih logova.'
    },
    users: {
      title: 'Upravljanje Korisnicima',
      searchPlaceholder: 'Traži korisnika...',
      resetPassword: 'Reset lozinke',
      changeRole: 'Promijeni ulogu',
      selectRole: 'Odaberite novu ulogu za korisnika',
      roleAdmin: 'Administrator',
      roleUser: 'Korisnik',
      roleUpdated: 'Uloga uspješno promijenjena!',
      statusActive: 'Aktivan',
      statusBlocked: 'Blokiran',
      statusOnline: 'AKTIVAN',
      statusOffline: 'ODJAVLJEN',
      lastSeen: 'Viđen',
      verified: 'Verifikovan',
      unverified: 'Nije verifikovan',
      actions: 'Akcije'
    }
  }
}

const en = {
  app: { name: 'IlmAI', subtitle: 'AI Assistant for Islamic Knowledge' },
  nav: { home: 'Home', chat: 'Ask AI', quran: 'Quran', hadith: 'Hadith', search: 'Search', settings: 'Settings', login: 'Login', register: 'Register', logout: 'Logout', welcome: 'Welcome' },
  home: { 
    welcome: 'Welcome to IlmAI', 
    heroTitle: 'IlmAI —',
    heroSubtitle: 'Islamic Knowledge',
    description: 'Your AI assistant for Islamic knowledge. Ask a question and get answers based on Quran, Hadith, and Tafsir.', 
    askButton: 'Ask a Question', 
    dailyTitle: 'Daily Ayah', 
    dailyHadithTitle: 'Daily Hadith' 
  },
  chat: { 
    placeholder: 'Ask a question about Islam...', 
    send: 'Send', 
    newChat: 'New Chat', 
    sessions: 'Sessions', 
    noAnswer: 'No answer found based on available sources.', 
    sources: 'Sources', 
    thinking: 'IlmAI is thinking...',
    welcomeGreeting: 'Ask IlmAI a question',
    disclaimer: 'IlmAI can provide inaccurate information. Verify sources.'
  },
  auth: { 
    username: 'Username', email: 'Email', password: 'Password', firstName: 'First Name', lastName: 'Last Name', 
    login: 'Login', register: 'Register', noAccount: "Don't have an account?", hasAccount: 'Already have an account?',
    firstNamePlaceholder: 'First Name', lastNamePlaceholder: 'Last Name'
  },
  settings: { title: 'Settings', theme: 'Theme', language: 'Language', notifications: 'Notifications', dark: 'Dark', light: 'Light', save: 'Save', changePassword: 'Change password', currentPassword: 'Current password', newPassword: 'New password', confirmPassword: 'Confirm new password', passwordChanged: 'Password successfully changed!', passwordChangeBtn: 'Change password' },
  search: { 
    title: 'Search', 
    placeholder: 'Search Islamic sources...', 
    results: 'Results', 
    noResults: 'No results found.',
    subtitle: 'Search through Islamic sources databases (Quran, Hadith, Tafsir)',
    button: 'Search',
    buttonAskAI: 'Ask AI',
    authModal: {
      title: 'Not logged in!',
      description: 'Login or register to unlock all features of IlmAI Chat assistant.',
      login: 'Login',
      register: 'Register'
    }
  },
  verifyEmail: {
    title: 'Email Verification',
    subtitle: 'We sent a 6-digit code to your email',
    placeholder: 'Enter code',
    button: 'Verify',
    resend: 'Resend Code',
    success: 'Email successfully verified!',
    error: 'Invalid code. Please try again.',
    backToLogin: 'Back to Login'
  },
  common: { save: 'Save', close: 'Close', backToHome: 'Back to Home' },
  admin: {
    nav: { 
      dashboard: 'Dashboard', 
      users: 'Users', 
      analytics: 'Analytics', 
      logs: 'System Logs',
      administration: 'Administration',
      openDashboard: 'Open Admin Dashboard'
    },
    dashboard: { 
      subtitle: 'System statistics and performance overview.',
      refresh: 'Refresh',
      currentUser: 'Current user',
      totalUsers: 'Total Users', 
      activeUsers: 'Active Users (24h)', 
      totalMessages: 'Total Messages', 
      newMessages: 'Messages (24h)',
      totalSessions: 'Total Sessions',
      popularTopics: 'Popular Topics',
      systemStatus: 'System Status',
      apiServer: 'API Server',
      aiEngine: 'AI Engine',
      database: 'Database',
      ok: 'OK',
      active: 'Active',
      connected: 'Connected'
    },
    analytics: {
      title: 'Analytics & Charts',
      subtitle: 'Deep insight into IlmAI platform activity and growth.',
      messageActivity: 'Message Activity',
      userGrowth: 'New Users',
      verificationStatus: 'User Verification Status',
      period7Days: 'Last 7 Days',
      growthTrend: 'Growth Trend',
      verified: 'Verified',
      unverified: 'Unverified',
      loading: 'Loading data...',
      retry: 'Retry'
    },
    logs: {
      title: 'System Logs',
      subtitle: 'Monitor errors and important events in real-time.',
      downloadCsv: 'Download CSV',
      downloadTxt: 'Download TXT',
      allLevels: 'All Levels',
      time: 'Time',
      level: 'Level',
      message: 'Message',
      source: 'Source',
      noLogs: 'No logs available.'
    },
    users: {
      title: 'User Management',
      searchPlaceholder: 'Search user...',
      resetPassword: 'Reset password',
      changeRole: 'Change Role',
      selectRole: 'Select new role for the user',
      roleAdmin: 'Administrator',
      roleUser: 'User',
      roleUpdated: 'Role successfully changed!',
      statusActive: 'Active',
      statusBlocked: 'Blocked',
      statusOnline: 'ONLINE',
      statusOffline: 'OFFLINE',
      lastSeen: 'Seen',
      verified: 'Verified',
      unverified: 'Not verified',
      actions: 'Actions'
    }
  }
}

const savedLocale = localStorage.getItem('user-locale') || 'bs'

export default createI18n({
  legacy: false,
  locale: savedLocale,
  fallbackLocale: 'en',
  messages: { bs, en }
})
