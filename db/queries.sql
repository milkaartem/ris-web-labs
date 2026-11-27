-- 1. Фильтр и сортировка: обращения в статусе New от новых к старым
SELECT number, title, status, created_at
FROM appeals
WHERE status = 'New'
ORDER BY created_at DESC;

-- 2. JOIN: список обращений с названием услуги из справочника (не только service_type_id)
SELECT
    a.number,
    a.title,
    s.name AS service_type_name,
    u_author.full_name AS author_name,
    u_assignee.full_name AS assignee_name,
    a.status,
    a.created_at
FROM appeals AS a
JOIN service_types AS s ON s.id = a.service_type_id
JOIN users AS u_author ON u_author.id = a.created_by_user_id
LEFT JOIN users AS u_assignee ON u_assignee.id = a.assignee_user_id
ORDER BY a.created_at DESC;

-- 3. GROUP BY: сводка количества обращений по всем статусам
SELECT status, COUNT(*) AS count
FROM appeals
GROUP BY status
ORDER BY status;

-- 4. Проверка ограничения CHECK (выполнять вручную отдельно; должна завершиться отказом БД):
-- INSERT INTO appeals (number, title, description, service_type_id, status, created_by_user_id)
-- VALUES ('ОБР-2026-9999', 'Тест неверного статуса', 'Проверка ограничения CHECK на статус Done.', 1, 'Done', 1);
