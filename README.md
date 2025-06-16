# Admin Dashboard

Админ-панель с бэкендом на ASP.NET Core 8 и фронтендом на React + Vite, с реальной JWT аутентификацией и запуском через Docker Compose.

---

## Стек

- **Backend:** ASP.NET Core 8 Minimal API + SQLite (файл)  
- **Frontend:** React + Vite  
- **Аутентификация:** JWT токены, полноценный механизм логина  
- **Контейнеризация:** Docker + Docker Compose  

---

## Запуск

### 1. Клонировать репозиторий

```bash
git clone https://github.com/Fuipon/AdminDashboardFull
cd AdminDashboardFull
```
### 2. Запустить всё через Docker Compose
```bash
docker-compose up --build
```
Это поднимет одновременно backend и frontend.

Как это работает
Backend доступен по адресу: [http://localhost:5000](http://localhost:5000)

Frontend доступен по адресу: [http://localhost:5173](http://localhost:5173)

---

### API эндпойнты
POST /auth/login — логин, возвращает JWT токен

CRUD /clients 

GET /payments?take=5 — последние платежи (по умолчанию 5) (авторизация)

GET /rate — текущий курс токенов (авторизация)

POST /rate — обновление курса токенов (авторизация)

---

### Авторизация
Для доступа к защищённым эндпойнтам нужно передавать в заголовке
Authorization: Bearer <JWT_TOKEN>

Токен выдаётся при логине через /auth/login с передачей email и password.

---

Тесты запускаются через Vitest во фронтенде.

Чтобы запустить тесты локально:

```bash
cd admin-dashboard
npm install
npx vitest run
```
В CI используется GitHub Actions, где тесты запускаются автоматически.

---

### Данные для входа (тестовые)

Email: admin@mirra.dev
Password: admin123

Приятной проверки
