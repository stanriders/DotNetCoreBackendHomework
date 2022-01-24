# DotNetCoreBackendHomework

Небольшое тестовое задание для бэкендера-дотнетчика.

В репозитории лежит незаконченный WebAPI-проект, который представляет собой CRUD над сущностью задачи (todo item).

API должен уметь отдавать в ответе список всех сущностей или конкретную, добавлять новую сущность или обновлять существующую. Ожидаемые контракты см. ниже.

## Что нужно сделать обязательно:

- реализовать недостающие методы в API и исправить ошибки, если есть
- реализовать "ленивое" обновление сущности (через `MassTransit`, работающий в `InMemory`-режиме)
- написать `Dockerfile` для упаковки проекта в контейнер
- добавить файл с SQL-скриптами для создания нужной структуры БД
- покрыть тестами обработчики запросов

## Что можно сделать:

- написать `docker-compose.yml`, в котором будут описаны контейнеры с Postgres (нужная структура БД должна создаваться автоматически) и данным сервисом
- заменить `InMemory`-вариант `MassTransit` на `RabbitMQ`-вариант
- прикрутить `m2m`-аутентификацию через `IdentityServer4`
- покрыть тестами storage-сервисы

## Ожидаемые эндпоинты и примеры запросов

### Получить список сущностей

```
GET /todoItems
```

```json
{
  "items": [
    {
      "id": "0495b9a7-8c03-4efb-957c-8db9167cab11",
      "title": "Выполнить тестовое задание",
      "isCompleted": true
    }
  ]
}
```

### Получить конкретную сущность

```
GET /todoItems/0495b9a7-8c03-4efb-957c-8db9167cab11
```

```json
{
  "id": "0495b9a7-8c03-4efb-957c-8db9167cab11",
  "title": "Выполнить тестовое задание",
  "isCompleted": true
}
```

### Добавить новую сущность

```
POST /todoItems

{
  "title": "Выполнить тестовое задание",
  "isCompleted": true
}
```

```json
{
  "id": "0495b9a7-8c03-4efb-957c-8db9167cab11"
}
```

### Обновить существующую сущность

```
PUT /todoItems/0495b9a7-8c03-4efb-957c-8db9167cab11

{
  "title": "Выполнить тестовое задание",
  "isCompleted": true
}
```

```
{
  // empty
}
```

### Ответ в случае ошибок


```json
{
  "userMessage": "Задача не существует",
  "errorCode": "NotFound"
}
```

Примеры кодов ошибок:

- `NotFound`: сущность не найдена
- `InternalError`: внутренняя ошибка
- `InvalidModel`: некорректная модель запроса