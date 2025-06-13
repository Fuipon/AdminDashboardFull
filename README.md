
Admin Dashboard

Минимальный проект админ-панели с бэкендом на ASP.NET Core 8 и фронтендом на React + Vite.

---

Стек

- Backend: ASP.NET Core 8 Minimal API + SQLite (файл)
- Frontend: React + Vite
- Аутентификация: простой POST /auth/login с возвращением токена

---

Запуск

1. Запуск API

```bash
cd AdminDashboardApi
dotnet run
```

API стартует на [http://localhost:5000](http://localhost:5000)

2. Запуск фронтенда

В отдельном терминале:

```bash
cd admin-dashboard
npm install
npm run dev
```

Фронтенд стартует на [http://localhost:5173](http://localhost:5173)

---

API эндпойнты

- POST /auth/login — вход (email + password) → возвращает { token: "demo" }
- GET /clients — список всех клиентов
- GET /payments?take=5 — последние N платежей (по умолчанию 5)
- GET /rate — текущий курс токенов
- POST /rate — обновить курс токенов

---

Пример теста через Postman

Получить текущий курс

GET
```bash
http://localhost:5000/rate
```

---

Данные для входа

- Email: admin@mirra.dev  
- Password: admin123

---

Особенности

- Данные хранятся в SQLite файле
- Простая аутентификация с заглушкой токена
- Интерфейс с таблицей клиентов и блоком курса токенов
- Возможность обновления курса с фронтенда

---

Если возникнут вопросы — пишите!

Спасибо за внимание и приятной проверки )))))
