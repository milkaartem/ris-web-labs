-- Учебные данные справочника услуг деканата
INSERT INTO service_types (id, name, description, is_active) VALUES
    (1, 'Справка об обучении', 'Подготовка и выдача справки установленного образца', true),
    (2, 'Перевод в другую группу', 'Рассмотрение заявления о переводе внутри института', true),
    (3, 'Повышенная стипендия', 'Вопросы назначения и перерасчёта повышенной стипендии', true),
    (4, 'Дубликат студенческого билета', 'Оформление дубликата при утере документа', true);

-- Учебные пользователи (создатели, диспетчер и исполнители)
INSERT INTO users (id, full_name, email, role) VALUES
    (1, 'Югин Артем', 'artem.yugin@kgeu.ru', 'Student'),
    (2, 'Соколова Елена Сергеевна', 'sokolova.es@kgeu.ru', 'Staff'),
    (3, 'Галеев Марат Рафаэлевич', 'galeev.mr@kgeu.ru', 'ViceDean'),
    (4, 'Сафина Алина Ильнуровна', 'safina.ai@kgeu.ru', 'Student');

-- Учебные обращения во всех четырёх статусах: New, InProgress, Closed, Cancelled
INSERT INTO appeals (
    id,
    number,
    title,
    description,
    service_type_id,
    status,
    created_at,
    updated_at,
    created_by_user_id,
    assignee_user_id
) VALUES
    (
        1,
        'ОБР-2026-0001',
        'Выдача справки об обучении за 3 семестр',
        'Справка необходима для предоставления в пенсионный фонд.',
        1,
        'New',
        now() - interval '3 days',
        now() - interval '3 days',
        1,
        NULL
    ),
    (
        2,
        'ОБР-2026-0002',
        'Заявление на перевод в другую группу',
        'Прошу рассмотреть возможность перевода в группу ТРИС-1-23.',
        2,
        'InProgress',
        now() - interval '2 days',
        now() - interval '1 day',
        4,
        2
    ),
    (
        3,
        'ОБР-2026-0003',
        'Уточнение расчёта повышенной стипендии',
        'Прошу проверить учёт научных публикаций за прошлый семестр.',
        3,
        'Closed',
        now() - interval '5 days',
        now() - interval '1 day',
        1,
        2
    ),
    (
        4,
        'ОБР-2026-0004',
        'Повторная выдача дубликата студенческого билета',
        'Заявка подана повторно по ошибке, основной билет найден.',
        4,
        'Cancelled',
        now() - interval '4 days',
        now() - interval '3 days',
        4,
        NULL
    ),
    (
        5,
        'ОБР-2026-0005',
        'Справка об обучении для военкомата',
        'Требуется заверенная справка с гербовой печатью до пятницы.',
        1,
        'New',
        now() - interval '1 hour',
        now() - interval '1 hour',
        1,
        NULL
    );

-- Синхронизация счётчиков автоинкремента после явной вставки id
SELECT setval(pg_get_serial_sequence('service_types', 'id'), COALESCE(MAX(id), 1)) FROM service_types;
SELECT setval(pg_get_serial_sequence('users', 'id'), COALESCE(MAX(id), 1)) FROM users;
SELECT setval(pg_get_serial_sequence('appeals', 'id'), COALESCE(MAX(id), 1)) FROM appeals;
