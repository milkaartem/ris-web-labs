<template>
  <header class="app-header">
    <div class="brand">
      <span class="brand-title">Электронная приёмная деканата ИЦТЭ КГЭУ</span>
      <span class="brand-subtitle">Учёт обращений студентов</span>
    </div>

    <nav class="app-nav" aria-label="Навигация по экранам">
      <router-link to="/" class="nav-link" exact-active-class="active">
        Список
      </router-link>
      <router-link to="/new" class="nav-link" active-class="active">
        Создать
      </router-link>
      <router-link to="/login" class="nav-link" active-class="active">
        Вход
      </router-link>
    </nav>
  </header>
</template>

<style scoped>
.app-header {
  display: flex;
  flex-wrap: wrap;
  justify-content: space-between;
  align-items: center;
  gap: 16px;
  padding: 16px 20px;
  margin-bottom: 24px;
  background-color: var(--surface);
  border: 1px solid var(--border);
  border-radius: 10px;
}

.brand {
  display: flex;
  flex-direction: column;
}

.brand-title {
  font-size: 1.125rem;
  font-weight: 700;
  color: var(--primary);
}

.brand-subtitle {
  font-size: 0.875rem;
  color: var(--muted);
}

.app-nav {
  display: flex;
  gap: 8px;
}

.nav-link {
  padding: 8px 14px;
  border: 1px solid var(--border);
  border-radius: 6px;
  background-color: var(--surface);
  color: var(--text);
  text-decoration: none;
  font-weight: 500;
}

.nav-link:hover {
  border-color: var(--primary);
}

.nav-link.active {
  background-color: var(--primary);
  border-color: var(--primary);
  color: #ffffff;
}
</style>
