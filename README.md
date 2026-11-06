# АИС «Онлайн-библиотека вуза»

Учебный проект по МДК 02.02 (вариант 4). Каталог электронных книг, выдача прав доступа к ним и полнотекстовый поиск.

## Паспорт артефактов проекта (ПР8)

| Категория | Артефакт | Где лежит |
|---|---|---|
| Архитектурный | Диаграмма контейнеров C4 Level 2 | [docs/architecture/c4-containers.puml](docs/architecture/c4-containers.puml) |
| Контрактный | OpenAPI-спецификация Library.Catalog API | [docs/openapi/library-catalog.yaml](docs/openapi/library-catalog.yaml) |
| Кодовый / тестовый | xUnit-тест `LoanServiceTests.IssueAccess_WhenLicenseLimitReached_ReturnsConflict` | [tests/Library.Tests/LoanServiceTests.cs](tests/Library.Tests/LoanServiceTests.cs) |
| Управленческий | Бэклог спринта 3 «Полнотекстовый поиск» | [docs/backlog/sprint-3.md](docs/backlog/sprint-3.md) |

Процессные протоколы описаны в [CONTRIBUTING.md](CONTRIBUTING.md), чек-лист проверки кода – в [шаблоне Pull Request](.github/pull_request_template.md), архитектурное решение – в [ADR-001](docs/adr/ADR-001-elasticsearch-fulltext-search.md).

## Структура репозитория (ПР9)

```
src/Library.Core            бизнес-логика
src/Library.Infrastructure  доступ к данным и внешним сервисам
src/Library.UI              веб-клиент
tests/Library.Tests         модульные тесты
docs/                       архитектура, OpenAPI, ADR, бэклог
```
