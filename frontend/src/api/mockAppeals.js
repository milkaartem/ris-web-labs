const delay = (ms) => new Promise((resolve) => setTimeout(resolve, ms))

export const mockConfig = {
  force500: false,
}

const mockDatabase = [
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
]

let serverNextId = 5

export async function mockGetAppeals() {
  await delay(500)

  if (mockConfig.force500) {
    const errBody = { message: 'Внутренняя ошибка учебного mock-сервера' }
    console.warn('[MOCK без HTTP] GET /api/appeals -> 500 Internal Server Error', errBody)
    return {
      ok: false,
      status: 500,
      data: errBody,
    }
  }

  const list = mockDatabase.map((item) => ({ ...item }))
  console.info('[MOCK без HTTP] GET /api/appeals -> 200 OK', list)

  return {
    ok: true,
    status: 200,
    data: list,
  }
}

export async function mockCreateAppeal(body) {
  await delay(500)

  if (mockConfig.force500) {
    const errBody = { message: 'Внутренняя ошибка учебного mock-сервера' }
    console.warn('[MOCK без HTTP] POST /api/appeals -> 500 Internal Server Error', errBody)
    return {
      ok: false,
      status: 500,
      data: errBody,
    }
  }

  const title = typeof body?.title === 'string' ? body.title.trim() : ''
  const serviceTypeId = body?.serviceTypeId
  const description = typeof body?.description === 'string' ? body.description.trim() : ''

  const validationErrors = {}

  if (title.length < 5 || title.length > 80) {
    validationErrors.title = 'Тема: от 5 до 80 символов'
  }
  if (!Number.isInteger(serviceTypeId) || serviceTypeId <= 0 || serviceTypeId > 100) {
    validationErrors.serviceTypeId = 'Укажите существующий номер категории услуги (1–100)'
  }
  if (description.length < 10 || description.length > 500) {
    validationErrors.description = 'Описание: от 10 до 500 символов'
  }

  if (Object.keys(validationErrors).length > 0) {
    const errBody = { errors: validationErrors }
    console.warn('[MOCK без HTTP] POST /api/appeals -> 400 Bad Request', errBody)
    return {
      ok: false,
      status: 400,
      data: errBody,
    }
  }

  const id = serverNextId++
  const padded = String(id).padStart(4, '0')
  const created = {
    id,
    number: `ОБР-2026-${padded}`,
    title,
    serviceTypeId,
    description,
    status: 'New',
  }

  mockDatabase.push(created)
  console.info('[MOCK без HTTP] POST /api/appeals -> 201 Created', created)

  return {
    ok: true,
    status: 201,
    data: { ...created },
  }
}
