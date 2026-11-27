<script setup>
import { ref } from 'vue'

const newForm = ref({
  title: '',
  serviceTypeId: '',
  description: '',
})

const notice = ref('')

function handleCreateSubmit() {
  notice.value =
    'Заготовка формы создания: отправка данных через POST /api/appeals будет подключена в следующих лабораторных.'
}
</script>

<template>
  <main class="card">
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
