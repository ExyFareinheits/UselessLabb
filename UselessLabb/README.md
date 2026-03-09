# Інструкції для запуску проекту

## Крок 1: Відновлення пакетів
```bash
dotnet restore
```

## Крок 2: Створення міграцій
```bash
dotnet ef migrations add InitialCreate
```

## Крок 3: Застосування міграцій (створення БД)
```bash
dotnet ef database update
```

## Крок 4: Додавання сторінок Identity (необхідно для авторизації)
```bash
dotnet add package Microsoft.VisualStudio.Web.CodeGeneration.Design
dotnet tool install --global dotnet-aspnet-codegenerator
dotnet aspnet-codegenerator identity --useDefaultUI
```

## Крок 5: Запуск додатку
```bash
dotnet run
```

## Функціональність:

### ✅ Моделі:
- **Book** - книги з полями: Title, Author, ISBN, PublishDate, Price, Genre, Publisher, Description, CoverImage
- **Genre** - жанри книг
- **Publisher** - видавництва

### ✅ CRUD операції:
- Повний CRUD для всіх трьох моделей
- Index (список), Create (створення), Edit (редагування), Delete (видалення), Details (деталі для книг)

### ✅ Авторизація:
- ASP.NET Core Identity
- Всі операції створення, редагування та видалення доступні лише авторизованим користувачам
- Атрибут `[Authorize]` на відповідних сторінках

### ✅ Валідація:
- Валідація на сервері (DataAnnotations)
- Валідація на клієнті (jQuery Validation)
- Required, StringLength, Range, Email, Phone, URL, RegularExpression

### ✅ UI компоненти:
- **DatePicker** - для вибору дат (PublishDate, FoundedDate)
- **Select List** - випадаючі списки для жанрів та видавництв
- **File Uploader** - завантаження обкладинок книг
- **Modal** - модальні вікна для підтвердження видалення

### ✅ Додатково:
- Сторінка "Про Автора" в головному меню
- Seed data для початкового наповнення БД
- Bootstrap 5 для UI
- Bootstrap Icons
- Responsive дизайн

## Примітки:
- База даних: SQL Server LocalDB
- Рядок підключення в `appsettings.json`
- Завантажені файли зберігаються в `/wwwroot/uploads/covers/`
