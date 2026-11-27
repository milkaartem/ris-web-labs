<script setup>
import { ref } from 'vue'
import { useRouter } from 'vue-router'
import { createAppeal } from '../api/appeals.js'

const router = useRouter()

const form = ref({
  title: '',
  serviceTypeId: 0,
  description: '',
})

const errors = ref({
  title: '',
  serviceTypeId: '',
  description: '',
})

const apiError = ref('')
const isSubmitting = ref(false)

async function onCreate() {
  if (isSubmitting.value) {
    return
  }

  errors.value = {
    title: '',
    serviceTypeId: '',
    description: '',
  }
  apiError.value = ''

  const title = form.value.title.trim()
  const serviceTypeId = form.value.serviceTypeId
  const description = form.value.description.trim()

  let hasError = false

  if (title.length < 5 || title.length > 80) {
    errors.value.title = 'Тема: от 5 до 80 символов'
    hasError = true
  }

  if (!Number.isInteger(serviceTypeId) || serviceTypeId <= 0) {
    errors.value.serviceTypeId = 'Укажите целое число больше нуля'
    hasError = true
  }

  if (description.length < 10 || description.length > 500) {
    errors.value.description = 'Описание: от 10 до 500 символов'
    hasError = true
  }

  if (hasError) {
    return
  }

  isSubmitting.value = true

  try {
    await createAppeal({
      title,
      serviceTypeId,
      description,
    })

    form.value = {
      title: '',
      serviceTypeId: 0,
      description: '',
    }

    router.push('/')
  } catch (err) {
    apiError.value = err.message || 'Не удалось создать обращение'
    const serverErrors = err.data?.errors
    if (serverErrors) {
      if (serverErrors.title) errors.value.title = serverErrors.title
      if (serverErrors.serviceTypeId) errors.value.serviceTypeId = serverErrors.serviceTypeId
      if (serverErrors.description) errors.value.description = serverErrors.description
    }
  } finally {
    isSubmitting.value = false
  }
}
</script>

<template>
  <main class="card">
    <h1>Создать обращение</h1>
    <p class="lead">
      Заполните три обязательных поля. Запрос отправляется через <code>createAppeal</code>; номер и статус приходят из ответа.
    </p>

    <form class="app-form" novalidate @submit.prevent="onCreate">
      <div class="field">
        <label for="title">Тема, 5–80 символов</label>
        <input
          id="title"
          v-model="form.title"
          name="title"
          type="text"
          placeholder="Например: Выдача справки об обучении за 3 семестр"
          autofocus
        />
        <p v-if="errors.title" class="field-error">{{ errors.title }}</p>
      </div>

      <div class="field">
        <label for="serviceTypeId">Номер категории услуги (serviceTypeId)</label>
        <input
          id="serviceTypeId"
          v-model.number="form.serviceTypeId"
          name="serviceTypeId"
          type="number"
          placeholder="1 — справка, 2 — перевод, 3 — стипендия, 4 — документы"
        />
        <p v-if="errors.serviceTypeId" class="field-error">
          {{ errors.serviceTypeId }}
        </p>
      </div>

      <div class="field">
        <label for="description">Описание, 10–500 символов</label>
        <textarea
          id="description"
          v-model="form.description"
          name="description"
          rows="4"
          placeholder="Подробно опишите суть обращения и цель запроса..."
        ></textarea>
        <p v-if="errors.description" class="field-error">
          {{ errors.description }}
        </p>
      </div>

      <p v-if="apiError" class="api-error" role="alert">{{ apiError }}</p>

      <div class="form-actions">
        <button type="submit" class="btn-primary" :disabled="isSubmitting">
          {{ isSubmitting ? 'Создаём…' : 'Создать' }}
        </button>
      </div>
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

.field-error {
  margin: 0;
  color: #b91c1c;
  font-size: 0.875rem;
  font-weight: 500;
}

.api-error {
  margin: 0;
  padding: 10px 12px;
  border: 1px solid #fecaca;
  border-radius: 6px;
  background-color: #fef2f2;
  color: #991b1b;
  font-weight: 500;
  font-size: 0.92rem;
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

.btn-primary:hover:not(:disabled) {
  background-color: var(--primary-hover);
}

.btn-primary:disabled {
  opacity: 0.65;
  cursor: not-allowed;
}
</style>
