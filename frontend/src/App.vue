<script setup>
import { ref } from 'vue'

const screen = ref('list') // 'list' | 'new' | 'login'
const notice = ref('')

const items = ref([
  {
    id: 1,
    number: 'ОБР-2026-0001',
    title: 'Выдача справки об обучении за 3 семестр',
    serviceTypeId: 1,
    description: 'Справка необходима для предоставления в пенсионный фонд.',
    status: 'New',
  },
  {
    id: 2,
    number: 'ОБР-2026-0002',
    title: 'Заявление на перевод в другую группу',
    serviceTypeId: 2,
    description: 'Прошу рассмотреть возможность перевода в группу ТРИС-1-23.',
    status: 'InProgress',
  },
  {
    id: 3,
    number: 'ОБР-2026-0003',
    title: 'Уточнение расчёта повышенной стипендии',
    serviceTypeId: 3,
    description: 'Прошу проверить учёт научных публикаций за прошлый семестр.',
    status: 'Closed',
  },
  {
    id: 4,
    number: 'ОБР-2026-0004',
    title: 'Повторная выдача дубликата студенческого билета',
    serviceTypeId: 4,
    description: 'Заявка подана повторно по ошибке.',
    status: 'Cancelled',
  },
])

const newForm = ref({
  title: '',
  serviceTypeId: '',
  description: '',
})

const loginForm = ref({
  login: '',
  password: '',
})

function setScreen(nextScreen) {
  screen.value = nextScreen
  notice.value = ''
}

function handleCreateSubmit() {
  notice.value =
    'Заготовка формы создания: отправка данных в API (POST /api/appeals) будет подключена в следующих лабораторных.'
}

function handleLoginSubmit() {
  notice.value =
    'Заготовка экрана входа: проверка логина и пароля будет подключена в ЛР11.'
}
</script>

<template>
  <div class="app-shell">
    <header class="app-header">
      <div class="brand">
        <span class="brand-title">Электронная приёмная деканата ИЦТЭ КГЭУ</span>
        <span class="brand-subtitle">Учёт обращений студентов</span>
      </div>

      <nav class="app-nav" aria-label="Переключение экранов">
        <button
          type="button"
          class="nav-btn"
          :class="{ active: screen === 'list' }"
          @click="setScreen('list')"
        >
          Список
        </button>
        <button
          type="button"
          class="nav-btn"
          :class="{ active: screen === 'new' }"
          @click="setScreen('new')"
        >
          Создать
        </button>
        <button
          type="button"
          class="nav-btn"
          :class="{ active: screen === 'login' }"
          @click="setScreen('login')"
        >
          Вход
        </button>
      </nav>
    </header>

    <main v-if="screen === 'list'" class="card">
      <h1>Список обращений</h1>
      <p class="lead">
        Реестр обращений студентов в деканат. В таблице показаны тестовые записи с допустимыми статусами системы.
      </p>

      <div class="table-wrap">
        <table class="appeals-table">
          <thead>
            <tr>
              <th>Номер</th>
              <th>Тема</th>
              <th>Услуга (serviceTypeId)</th>
              <th>Статус</th>
            </tr>
          </thead>
          <tbody>
            <tr v-for="item in items" :key="item.id">
              <td class="mono">{{ item.number }}</td>
              <td>{{ item.title }}</td>
              <td class="mono">{{ item.serviceTypeId }}</td>
              <td>
                <span class="status-badge" :data-status="item.status">
                  {{ item.status }}
                </span>
              </td>
            </tr>
          </tbody>
        </table>
      </div>
    </main>

    <main v-else-if="screen === 'new'" class="card">
      <h1>Создать обращение</h1>
      <p class="lead">
        Заполните три обязательных поля, соответствующие критерию 1 и контракту API.
      </p>

      <form class="app-form" @submit.prevent="handleCreateSubmit">
        <div class="field">
          <label for="title">Тема, 5–80 символов</label>
          <input
            id="title"
            v-model="newForm.title"
            name="title"
            type="text"
            placeholder="Например: Выдача справки об обучении за 3 семестр"
            autofocus
          />
        </div>

        <div class="field">
          <label for="serviceTypeId">Номер категории услуги (serviceTypeId)</label>
          <input
            id="serviceTypeId"
            v-model="newForm.serviceTypeId"
            name="serviceTypeId"
            type="number"
            min="1"
            placeholder="1 — справка, 2 — перевод, 3 — стипендия, 4 — документы"
          />
        </div>

        <div class="field">
          <label for="description">Описание, 10–500 символов</label>
          <textarea
            id="description"
            v-model="newForm.description"
            name="description"
            rows="4"
            placeholder="Подробно опишите суть обращения и цель запроса..."
          ></textarea>
        </div>

        <div class="form-actions">
          <button type="submit" class="btn-primary">Создать</button>
        </div>

        <p v-if="notice" class="notice">{{ notice }}</p>
      </form>
    </main>

    <main v-else class="card">
      <h1>Вход в систему</h1>
      <p class="lead">
        Заготовка экрана авторизации (роли: Студент, Заместитель декана, Сотрудник деканата).
      </p>

      <form class="app-form" @submit.prevent="handleLoginSubmit">
        <div class="field">
          <label for="login">Логин (корпоративная почта)</label>
          <input
            id="login"
            v-model="loginForm.login"
            name="login"
            type="text"
            placeholder="student@kgeu.ru"
            autofocus
          />
        </div>

        <div class="field">
          <label for="password">Пароль</label>
          <input
            id="password"
            v-model="loginForm.password"
            name="password"
            type="password"
            placeholder="Введите пароль"
          />
        </div>

        <div class="form-actions">
          <button type="submit" class="btn-primary">Войти</button>
        </div>

        <p v-if="notice" class="notice">{{ notice }}</p>
      </form>
    </main>
  </div>
</template>

<style scoped>
.app-shell {
  max-width: 960px;
  margin: 0 auto;
  padding: 24px 16px 48px;
}

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

.nav-btn {
  padding: 8px 14px;
  border: 1px solid var(--border);
  border-radius: 6px;
  background-color: var(--surface);
  color: var(--text);
  cursor: pointer;
  font-weight: 500;
}

.nav-btn:hover {
  border-color: var(--primary);
}

.nav-btn.active {
  background-color: var(--primary);
  border-color: var(--primary);
  color: #ffffff;
}

.card {
  padding: 24px;
  background-color: var(--surface);
  border: 1px solid var(--border);
  border-radius: 10px;
}

h1 {
  margin: 0 0 8px;
  font-size: 1.5rem;
  color: var(--text);
}

.lead {
  margin: 0 0 20px;
  color: var(--muted);
  font-size: 0.95rem;
}

.table-wrap {
  overflow-x: auto;
}

.appeals-table {
  width: 100%;
  border-collapse: collapse;
  text-align: left;
}

.appeals-table th,
.appeals-table td {
  padding: 12px;
  border-bottom: 1px solid var(--border);
  vertical-align: middle;
}

.appeals-table th {
  font-size: 0.875rem;
  font-weight: 600;
  color: var(--muted);
  background-color: #f8fafc;
}

.mono {
  font-family: ui-monospace, Consolas, monospace;
  font-size: 0.9rem;
}

.status-badge {
  display: inline-block;
  padding: 3px 10px;
  border-radius: 999px;
  font-size: 0.8125rem;
  font-weight: 600;
}

.status-badge[data-status='New'] {
  background-color: #dbeafe;
  color: #1e40af;
}

.status-badge[data-status='InProgress'] {
  background-color: #fef3c7;
  color: #92400e;
}

.status-badge[data-status='Closed'] {
  background-color: #dcfce7;
  color: #166534;
}

.status-badge[data-status='Cancelled'] {
  background-color: #fee2e2;
  color: #991b1b;
}

.app-form {
  max-width: 560px;
  display: flex;
  flex-direction: column;
  gap: 16px;
}

.field {
  display: flex;
  flex-direction: column;
  gap: 6px;
}

.field label {
  font-weight: 600;
  font-size: 0.95rem;
  color: var(--text);
}

.field input,
.field textarea {
  padding: 10px 12px;
  border: 1px solid var(--border);
  border-radius: 6px;
  background-color: #ffffff;
  color: var(--text);
}

.field input:focus,
.field textarea:focus {
  outline: 2px solid var(--primary);
  outline-offset: 1px;
  border-color: var(--primary);
}

.form-actions {
  margin-top: 4px;
}

.btn-primary {
  padding: 10px 18px;
  border: none;
  border-radius: 6px;
  background-color: var(--primary);
  color: #ffffff;
  font-weight: 600;
  cursor: pointer;
}

.btn-primary:hover {
  background-color: var(--primary-hover);
}

.notice {
  margin: 0;
  padding: 10px 12px;
  border-radius: 6px;
  background-color: #eff6ff;
  border: 1px solid #bfdbfe;
  color: #1e3a8a;
  font-size: 0.9rem;
}
</style>
