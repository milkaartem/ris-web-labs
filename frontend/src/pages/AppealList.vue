<script setup>
import { onMounted, ref } from 'vue'
import { getAppeals } from '../api/appeals.js'
import { items, setItems } from '../appealsStore.js'
import AppealTable from '../components/AppealTable.vue'

// 'loading' | 'empty' | 'data' | 'error'
const screen = ref('loading')
const errorMessage = ref('')

async function loadList() {
  screen.value = 'loading'
  errorMessage.value = ''

  try {
    const list = await getAppeals()
    setItems(list)
    screen.value = list.length === 0 ? 'empty' : 'data'
  } catch (err) {
    errorMessage.value = err.message || 'Не удалось загрузить список обращений'
    screen.value = 'error'
  }
}

onMounted(() => {
  loadList()
})
</script>

<template>
  <main class="card">
    <h1>Список обращений</h1>
    <p class="lead">
      Реестр обращений студентов в деканат. Данные загружаются через модуль <code>src/api/appeals.js</code>.
    </p>

    <p v-if="screen === 'loading'" class="state-box">Загрузка…</p>

    <p v-else-if="screen === 'empty'" class="state-box">Записей пока нет</p>

    <AppealTable v-else-if="screen === 'data'" :items="items" />

    <div v-else class="error-box" role="alert">
      <p class="error-text">{{ errorMessage }}</p>
      <button type="button" class="btn-retry" @click="loadList">Повторить</button>
    </div>
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

.state-box {
  margin: 0;
  padding: 20px;
  border: 1px dashed var(--border);
  border-radius: 8px;
  color: var(--muted);
  text-align: center;
}

.error-box {
  display: flex;
  flex-wrap: wrap;
  justify-content: space-between;
  align-items: center;
  gap: 12px;
  padding: 16px;
  border: 1px solid #fecaca;
  border-radius: 8px;
  background-color: #fef2f2;
}

.error-text {
  margin: 0;
  color: #991b1b;
  font-weight: 500;
}

.btn-retry {
  padding: 8px 14px;
  border: 1px solid #dc2626;
  border-radius: 6px;
  background-color: #ffffff;
  color: #991b1b;
  font-weight: 600;
  cursor: pointer;
}

.btn-retry:hover {
  background-color: #fee2e2;
}
</style>
