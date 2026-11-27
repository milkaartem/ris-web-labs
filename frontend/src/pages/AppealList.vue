<script setup>
import AppealTable from '../components/AppealTable.vue'
import { items } from '../appealsStore.js'
</script>

<template>
  <main class="card">
    <h1>Список обращений</h1>
    <p class="lead">
      Реестр обращений студентов в деканат. Данные поступают из общего хранилища <code>appealsStore.js</code>.
    </p>

    <AppealTable :items="items" />
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
</style>
