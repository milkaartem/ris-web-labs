<script setup>
import { ref } from 'vue'

const loginForm = ref({
  login: '',
  password: '',
})

const notice = ref('')

function handleLoginSubmit() {
  notice.value =
    'Заготовка экрана входа: проверка логина и пароля будет подключена в ЛР11.'
}
</script>

<template>
  <main class="card">
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
</template>

<style scoped>
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

.field input {
  padding: 10px 12px;
  border: 1px solid var(--border);
  border-radius: 6px;
  background-color: #ffffff;
  color: var(--text);
}

.field input:focus {
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
