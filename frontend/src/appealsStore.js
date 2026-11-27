import { ref } from 'vue'

export const items = ref([
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
