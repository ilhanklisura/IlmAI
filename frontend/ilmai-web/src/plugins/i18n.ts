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
  settings: { title: 'Postavke', theme: 'Tema', language: 'Jezik', notifications: 'Notifikacije', dark: 'Tamna', light: 'Svijetla', save: 'Spremi' },
  search: { 
    title: 'Pretraga', 
    placeholder: 'Pretražite islamske izvore...', 
    results: 'Rezultati', 
    noResults: 'Nema rezultata.',
    subtitle: 'Pretražujte baze podataka islamskih izvora (Kur\'an, Hadis, Tefsir)',
    button: 'Pretraži'
  },
  common: { save: 'Spremi', close: 'Zatvori' }
}

const en = {
  app: { name: 'IlmAI', subtitle: 'AI Islamic Knowledge Assistant' },
  nav: { home: 'Home', chat: 'Ask AI', quran: 'Quran', hadith: 'Hadith', search: 'Search', settings: 'Settings', login: 'Login', register: 'Register', logout: 'Logout', welcome: 'Welcome' },
  home: { 
    welcome: 'Welcome to IlmAI', 
    heroTitle: 'IlmAI —',
    heroSubtitle: 'Islamic Knowledge',
    description: 'Your AI assistant for Islamic knowledge. Ask a question and receive source-grounded answers from Quran, Hadith, and Tafsir.', 
    askButton: 'Ask a Question', 
    dailyTitle: 'Daily Ayah', 
    dailyHadithTitle: 'Daily Hadith' 
  },
  chat: { 
    placeholder: 'Ask a question about Islam...', 
    send: 'Send', 
    newChat: 'New Chat', 
    sessions: 'Sessions', 
    noAnswer: 'No answer found from available sources.', 
    sources: 'Sources', 
    thinking: 'IlmAI is thinking...',
    welcomeGreeting: 'Ask a question to IlmAI',
    disclaimer: 'IlmAI may provide inaccurate information. Check the sources.'
  },
  auth: { 
    username: 'Username', email: 'Email', password: 'Password', firstName: 'First Name', lastName: 'Last Name', 
    login: 'Login', register: 'Register', noAccount: "Don't have an account?", hasAccount: 'Already have an account?',
    firstNamePlaceholder: 'First Name', lastNamePlaceholder: 'Last Name'
  },
  settings: { title: 'Settings', theme: 'Theme', language: 'Language', notifications: 'Notifications', dark: 'Dark', light: 'Light', save: 'Save' },
  search: { 
    title: 'Search', 
    placeholder: 'Search Islamic sources...', 
    results: 'Results', 
    noResults: 'No results found.',
    subtitle: 'Search through Islamic source databases (Quran, Hadith, Tafsir)',
    button: 'Search'
  },
  common: { save: 'Save', close: 'Close' }
}

export default createI18n({
  legacy: false,
  locale: 'bs',
  fallbackLocale: 'en',
  messages: { bs, en }
})
