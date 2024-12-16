-- Users

DO $$
DECLARE
    i INT;
BEGIN
    FOR i IN 1..100 LOOP
        INSERT INTO Users (FirstName, LastName, MiddleName, PhoneNumber, Email, Telegram, VK)
        VALUES (
            ('Имя' || i), -- Генерация уникального имени
            ('Фамилия' || i), -- Генерация уникальной фамилии
            ('Отчество' || i), -- Генерация уникального отчества
            ('+7' || (1000000000 + random() * 899999999)::BIGINT), -- Генерация случайного номера телефона
            ('user' || i || '@example.com'), -- Генерация уникального Email
            ('@user' || i), -- Генерация уникального Telegram-ника
            ('vk.com/user' || i) -- Генерация уникальной ссылки VK
        );
    END LOOP;
END $$;

-- Staff, CrewMembers, Owners

DO $$
DECLARE
    userIds INT[] := ARRAY(SELECT UserId FROM Users ORDER BY RANDOM() LIMIT 100); -- Получаем 100 уникальных UserId
    i INT := 1; -- Индекс для userIds
BEGIN
    -- Создаём записи для CrewMembers
    FOR i IN 1..20 LOOP
        INSERT INTO CrewMembers (Name, UserId)
        VALUES (
            ('Member' || i), -- Генерация имени члена отряда
            userIds[i] -- Получаем UserId без повторений
        );
    END LOOP;

    -- Создаём записи для Staff
    FOR i IN 21..50 LOOP
        INSERT INTO Staff (UserId, DateEmpl)
        VALUES (
            userIds[i], -- Получаем следующий UserId без повторений
            NOW() - (random() * 100)::INT * INTERVAL '1 day' -- Случайная дата найма за последние 100 дней
        );
    END LOOP;

    -- Создаём записи для Owners
    FOR i IN 51..100 LOOP
        INSERT INTO Owners (UserId)
        VALUES (
            userIds[i] -- Получаем следующий UserId без повторений
        );
    END LOOP;
END $$;

-- Organization + linking

DO $$
DECLARE
    ownerIds INT[] := ARRAY(SELECT OwnerId FROM Owners ORDER BY RANDOM() LIMIT 50); -- Получаем 50 уникальных OwnerId
    orgId INT; -- Для хранения текущего OrganizationId
BEGIN
    -- Создаём 10 организаций с уникальными владельцами
    FOR i IN 1..10 LOOP
        INSERT INTO Organizations (Name, Type, OwnerId)
        VALUES (
            'Organization ' || i, -- Генерация имени организации
            CASE WHEN random() < 0.5 THEN 'Commercial' ELSE 'Non-Profit' END, -- Тип организации
            ownerIds[i] -- Уникальный владелец организации
        )
        RETURNING OrganizationId INTO orgId; -- Сохраняем OrganizationId

        -- Привязываем 4 дополнительных владельцев к этой организации
        FOR j IN 1..4 LOOP
            UPDATE Owners
            SET OrganizationId = orgId
            WHERE OwnerId = ownerIds[10 + (i - 1) * 4 + j];
        END LOOP;
    END LOOP;
END $$;

-- Contracts

DO $$
DECLARE
    orgIds INT[] := ARRAY(SELECT OrganizationId FROM Organizations);
    contractCount INT;
    i INT;
    minPrice NUMERIC := 1000; -- Минимальная цена
    maxPrice NUMERIC := 100000; -- Максимальная цена
BEGIN
    FOR i IN 1..ARRAY_LENGTH(orgIds, 1) LOOP
        contractCount := 1 + (random() < 0.5)::INT;

        FOR j IN 1..contractCount LOOP
            INSERT INTO Contracts (OrganizationId, OwnerId, IsLegalEntity, SignDate, Address, Comment, Price)
            VALUES (
                orgIds[i],
                NULL,
                TRUE,
                NOW() - (random() * 365)::INT * INTERVAL '1 day',
                'Address ' || orgIds[i] || '-' || j,
                'Contract for organization ' || orgIds[i],
                minPrice + random() * (maxPrice - minPrice) -- Генерация случайной цены
            );
        END LOOP;
    END LOOP;
END $$;

-- Alarms

DO $$
DECLARE
    contractIds INT[] := ARRAY(SELECT ContractId FROM Contracts ORDER BY RANDOM() LIMIT 5); -- Получаем случайные 5 ContractId
    alarmCount INT; -- Количество записей для каждого контракта
    i INT; -- Индекс для цикла
BEGIN
    -- Создаём записи для выбранных контрактов
    FOR i IN 1..ARRAY_LENGTH(contractIds, 1) LOOP
        -- Генерируем случайное количество записей (от 2 до 4) для каждого контракта
        alarmCount := 2 + (random() * 3)::INT;

        FOR j IN 1..alarmCount LOOP
            INSERT INTO Alarms (ResultType, DateTime, Comment, ContractId)
            VALUES (
                (1 + random() * 3)::INT, -- ResultType: случайное значение от 1 до 3
                NOW() - (random() * 365)::INT * INTERVAL '1 day', -- Случайная дата за последний год
                'Alarm for contract ' || contractIds[i] || ' - ' || j, -- Уникальный комментарий
                contractIds[i] -- Привязка к контракту
            );
        END LOOP;
    END LOOP;
END $$;
