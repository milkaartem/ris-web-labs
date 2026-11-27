# Информационная система учёта обращений студентов в деканат («Электронный деканат ИЦТЭ КГЭУ»)

Учебный проект по дисциплине **«РИС и Web»** (Б1.В.ДЭ.01.02.02).  
Казанский государственный энергетический университет (КГЭУ) · Инженеры/бакалавры · 2026/2027 учебный год.

- **Студент:** Югин Артем
- **Группа:** ТРИС-2-23
- **Тема №9:** «Обращения в деканат» («Электронная приёмная деканата ИЦТЭ КГЭУ»)
- **Статус темы:** **утверждено** (преподаватель принял на защите ЛР1)
- **Стек технологий:** Vue 3 + ASP.NET Core + PostgreSQL

---

## Описание проекта

Система предназначена для цифровизации процесса подачи, распределения и обработки официальных обращений и заявлений студентов деканатом Института цифровых технологий и экономики (ИЦТЭ) КГЭУ.

### Основные сущности и процесс:
1. **Студент** формирует обращение по конкретной академической услуге из справочника (заказ справки об обучении, перевод, задолженности, стипендия).
2. **Заместитель декана** распределяет поступившие обращения и назначает ответственного сотрудника деканата отдельным действием (`PATCH /api/appeals/{id}/assignee`).
3. **Сотрудник деканата** принимает закреплённое за ним обращение в работу (`InProgress`) и после решения переводит в статус `Closed` либо `Cancelled` (`PATCH /api/appeals/{id}/status`).

---

## Запуск клиентского приложения (Frontend)

Для запуска клиентской части требуется установленный **Node.js (LTS)**.

```bash
cd frontend
copy .env.example .env    # для Windows (или cp .env.example .env)
npm install
npm run dev
```

После запуска открыть адрес Vite, обычно `http://localhost:5173`.
Переменная `VITE_API_BASE_URL=http://localhost:5000` задаётся в локальном `frontend/.env` (файл исключён из Git).

Доступные маршруты клиента (`vue-router`):
- `http://localhost:5173/` — список обращений;
- `http://localhost:5173/new` — форма создания нового обращения;
- `http://localhost:5173/login` — вход в систему.

---

## Запуск серверного приложения (Backend)

Для запуска серверной части требуется установленный **.NET 8 SDK**.

```bash
cd backend
dotnet restore
dotnet run
```

Основные адреса в режиме `Development`:
- API: `http://localhost:5000`
- Swagger: `http://localhost:5000/swagger` (или `http://localhost:5000/swagger/index.html`)
- Проверка живости процесса: `GET http://localhost:5000/health` (`200`, `ok`)
- Сведения о приложении и среде: `GET http://localhost:5000/info`
- Маршруты обращений:
  - `GET /api/appeals` (список, пустой список — `200 []`)
  - `GET /api/appeals/{id}` (одна карточка, `404` если нет)
  - `POST /api/appeals` (создание, `201` или `400`)
  - `PATCH /api/appeals/{id}/assignee` (назначение исполнителя, `200` / `404` / `409`)
  - `PATCH /api/appeals/{id}/status` (смена статуса, `200` / `400` / `404` / `409`)

**Соглашения для работы с API (ЛР8):**
- Пользователь передаётся через заголовок `X-User-Id` (по умолчанию при создании — `1`; для перевода в `InProgress`/`Closed` должен совпадать с `assigneeUserId`).
- Для учебного воспроизведения `500` в среде `Development` можно передать заголовок `X-Simulate-Error: true` на `POST /api/appeals`.
- Данные обращений хранятся в оперативной памяти сервера и сбрасываются после перезапуска (`Ctrl+C` $\to$ `dotnet run`).
- Для проверки запросов импортируйте коллекцию [`docs/api-collection/appeals.postman_collection.json`](docs/api-collection/appeals.postman_collection.json) в Postman или Insomnia (базовый URL: `http://localhost:5000`).

---

## Структура репозитория и документации

```text
.
├── README.md                      # Данное описание проекта и команды запуска
├── .gitignore                     # Исключения (node_modules/, dist/, .env и др.)
├── docs/
│   ├── project-proposal.md        # [ЛР1] Заявка на тему и самопроверка по 12 критериям
│   ├── lab-01-requirements.md     # [ЛР1] Техническое задание (ТЗ) на разработку системы
│   ├── architecture.md            # [ЛР2] Трёхузловая архитектурная схема и описание взаимодействия
│   ├── data-model.md              # [ЛР2] ER-диаграмма, словарь терминов, ключи (PK, FK) и проверка 3НФ
│   ├── api-contract.md            # [ЛР2] Спецификация REST API (2 независимых PATCH, JSON-примеры)
│   ├── requirements-matrix.md     # [ЛР2] Трассировочная матрица критериев ЛР1 к модели и API
│   ├── api-collection/
│   │   └── appeals.postman_collection.json # [ЛР8] Коллекция Postman / Insomnia
│   └── reports/
│       ├── lab-02.md              # [ЛР2] Отчёт по лабораторной работе №2 и ответы на вопросы защиты
│       ├── lab-03.md              # [ЛР3] Отчёт по лабораторной работе №3 и чеклист семантики
│       ├── lab-04.md              # [ЛР4] Отчёт по лабораторной работе №4 (компоненты и маршруты)
│       ├── lab-05.md              # [ЛР5] Отчёт по лабораторной работе №5 (успех, отказ, F5)
│       ├── lab-06.md              # [ЛР6] Отчёт по лабораторной работе №6 (API, mock и 3 отказа)
│       ├── lab-07.md              # [ЛР7] Отчёт по лабораторной работе №7 (каркас Web API и DI)
│       └── lab-08.md              # [ЛР8] Отчёт по лабораторной работе №8 (REST, валидация и лог)
├── backend/                       # [ЛР7–ЛР8] Сервер на ASP.NET Core 8 Web API (порт 5000)
│   ├── backend.csproj
│   ├── Program.cs                 # DI, Swagger, /health, /info
│   ├── appsettings.json
│   ├── appsettings.Development.json
│   ├── Properties/
│   │   └── launchSettings.json    # Профиль http://localhost:5000
│   ├── Controllers/
│   │   └── AppealsController.cs   # Эндпоинты /api/appeals (GET, POST, 2 PATCH)
│   ├── Dtos/
│   │   ├── CreateAppealRequest.cs
│   │   ├── AssignAppealRequest.cs
│   │   ├── UpdateAppealStatusRequest.cs
│   │   └── AppealResponse.cs
│   ├── Models/
│   │   └── Appeal.cs              # Сущность обращения
│   ├── Services/
│   │   ├── IAppealService.cs      # Интерфейс сервиса
│   │   └── AppealService.cs       # Правила процесса и хранение в памяти
│   └── Data/                      # Папка слоя доступа к данным
└── frontend/                      # [ЛР3–ЛР6] Клиентский SPA на Vue 3 + Vite + vue-router (JS)
    ├── .env.example               # Пример конфигурации VITE_API_BASE_URL=http://localhost:5000
    ├── index.html
    ├── package.json
    ├── package-lock.json
    ├── vite.config.js
    ├── public/
    └── src/
        ├── main.js
        ├── style.css
        ├── App.vue                # AppHeader + router-view
        ├── router.js              # Маршруты /, /new, /login
        ├── appealsStore.js        # Общее реактивное состояние списка
        ├── api/
        │   ├── appeals.js         # Клиент API (getAppeals, createAppeal, USE_MOCK, fetch)
        │   └── mockAppeals.js     # Учебный mock-сервер (задержка 500 мс, 200/201/400/500)
        ├── components/
        │   ├── AppHeader.vue      # Шапка с <router-link>
        │   └── AppealTable.vue    # Таблица (получает :items через props)
        └── pages/
            ├── AppealList.vue     # Страница / (loading | empty | data | error)
            ├── AppealNew.vue      # Страница /new (submit, disabled, обработка 400/500/сети)
            └── Login.vue          # Страница /login
```

---

## Статус выполнения лабораторных работ

| Лабораторная работа | Название | Статус | Документы / Код | Фиксация в Git |
|:-------------------:|:---------|:------:|:----------------|:---------------|
| **ЛР 1** | Анализ, требования и выбор темы | **Принято** | [`project-proposal.md`](docs/project-proposal.md), [`lab-01-requirements.md`](docs/lab-01-requirements.md) | [`lr1`](https://github.com/milkaartem/ris-web-labs/tree/lr1) (`b8b142b`) |
| **ЛР 2** | Архитектура и модель данных | **Принято** | [`architecture.md`](docs/architecture.md), [`data-model.md`](docs/data-model.md), [`api-contract.md`](docs/api-contract.md), [`requirements-matrix.md`](docs/requirements-matrix.md), [`lab-02.md`](docs/reports/lab-02.md) | [`lr2`](https://github.com/milkaartem/ris-web-labs/tree/lr2) (`8f97e50`) |
| **ЛР 3** | Vue: каркас и три экрана | **Готово к сдаче** | [`frontend/`](frontend/), [`lab-03.md`](docs/reports/lab-03.md) | [`lr3`](https://github.com/milkaartem/ris-web-labs/tree/lr3) (`75a2280`) |
| **ЛР 4** | Vue: компоненты и маршруты | **Готово к сдаче** | [`frontend/src/`](frontend/src/), [`lab-04.md`](docs/reports/lab-04.md) | [`lr4`](https://github.com/milkaartem/ris-web-labs/tree/lr4) (`c0c70c0`) |
| **ЛР 5** | Vue: форма и список в памяти | **Готово к сдаче** | [`appealsStore.js`](frontend/src/appealsStore.js), [`AppealNew.vue`](frontend/src/pages/AppealNew.vue), [`lab-05.md`](docs/reports/lab-05.md) | [`lr5`](https://github.com/milkaartem/ris-web-labs/tree/lr5) (`d620e2f`) |
| **ЛР 6** | Vue: клиент API и mock | **Готово к сдаче** | [`api/appeals.js`](frontend/src/api/appeals.js), [`api/mockAppeals.js`](frontend/src/api/mockAppeals.js), [`lab-06.md`](docs/reports/lab-06.md) | [`lr6`](https://github.com/milkaartem/ris-web-labs/tree/lr6) (`21d4e08`) |
| **ЛР 7** | Каркас Web API (ASP.NET Core) | **Готово к сдаче** | [`backend/`](backend/), [`lab-07.md`](docs/reports/lab-07.md) | [`lr7`](https://github.com/milkaartem/ris-web-labs/tree/lr7) (`dc26a3a`) |
| **ЛР 8** | REST: заявки и валидация | **Готово к сдаче** | [`AppealsController.cs`](backend/Controllers/AppealsController.cs), [`appeals.postman_collection.json`](docs/api-collection/appeals.postman_collection.json), [`lab-08.md`](docs/reports/lab-08.md) | [`lr8`](https://github.com/milkaartem/ris-web-labs/tree/lr8) (`main`) |

> **Примечание для проверки предыдущих работ:** исходный каркас ЛР3 (`v-if` в одном `App.vue`) зафиксирован на теге [`lr3`](https://github.com/milkaartem/ris-web-labs/tree/lr3) (`75a2280`), версия ЛР4 с роутером — на теге [`lr4`](https://github.com/milkaartem/ris-web-labs/tree/lr4) (`c0c70c0`), версия ЛР5 с локальной записью в память — на теге [`lr5`](https://github.com/milkaartem/ris-web-labs/tree/lr5) (`d620e2f`), клиент API с mock (ЛР6) — на теге [`lr6`](https://github.com/milkaartem/ris-web-labs/tree/lr6) (`21d4e08`), каркас Web API (ЛР7) — на теге [`lr7`](https://github.com/milkaartem/ris-web-labs/tree/lr7) (`dc26a3a`). В актуальной ветке `main` выполнена 8 лабораторная работа.
