import { mockCreateAppeal, mockGetAppeals } from './mockAppeals.js'

// Один переключатель источника данных:
// true  — учебный mock (работает без запущенного C#-сервера)
// false — реальные HTTP-запросы через fetch на VITE_API_BASE_URL
export const USE_MOCK = true

const baseUrl = import.meta.env?.VITE_API_BASE_URL

export class ApiError extends Error {
  constructor(message, { status = null, data = null, isNetworkError = false } = {}) {
    super(message)
    this.name = 'ApiError'
    this.status = status
    this.data = data
    this.isNetworkError = isNetworkError
  }
}

function buildHttpError(status, data) {
  if (status === 400) {
    return new ApiError('Проверьте поля', { status, data })
  }
  if (status === 500) {
    return new ApiError('Сервер не смог обработать запрос', { status, data })
  }
  return new ApiError(`Ошибка запроса (HTTP ${status})`, { status, data })
}

async function parseJsonSafe(response) {
  const text = await response.text()
  if (!text) return null
  try {
    return JSON.parse(text)
  } catch {
    return null
  }
}

export async function getAppeals() {
  if (USE_MOCK) {
    const result = await mockGetAppeals()
    if (!result.ok) {
      throw buildHttpError(result.status, result.data)
    }
    return result.data
  }

  let response
  try {
    response = await fetch(`${baseUrl}/api/appeals`)
  } catch {
    throw new ApiError('Нет связи с сервером. Проверьте, что приложение запущено', {
      isNetworkError: true,
    })
  }

  const data = await parseJsonSafe(response)
  if (!response.ok) {
    throw buildHttpError(response.status, data)
  }

  return Array.isArray(data) ? data : []
}

export async function createAppeal(body) {
  if (USE_MOCK) {
    const result = await mockCreateAppeal(body)
    if (!result.ok) {
      throw buildHttpError(result.status, result.data)
    }
    return result.data
  }

  let response
  try {
    response = await fetch(`${baseUrl}/api/appeals`, {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify(body),
    })
  } catch {
    throw new ApiError('Нет связи с сервером. Проверьте, что приложение запущено', {
      isNetworkError: true,
    })
  }

  const data = await parseJsonSafe(response)
  if (!response.ok) {
    throw buildHttpError(response.status, data)
  }

  return data
}
