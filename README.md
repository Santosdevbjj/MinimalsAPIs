## Desafio de Projeto:  Trabalhando com ASP.NET Minimals APIs

![GFTNet001](https://github.com/user-attachments/assets/53b03495-a48d-4248-a775-5de04bd979cc)


**Bootcamp GFT Start #7 .NET**

---

# Minimal API Vehicles 🚗

Uma aplicação de exemplo em **.NET 8 Minimal APIs** para gerenciamento de veículos, com autenticação de administradores, testes automatizados, integração com MySQL via Docker Compose e ambientes configurados por `.env`.

---

## 🚀 Versão Enxuta (Quickstart)

### Rodando em Development
```bash
docker-compose --env-file .env.development up --build



**Rodando em Test**


docker-compose --env-file .env.test up --build


**Rodando em Production**

docker-compose --env-file .env.production up --build -d

Swagger: http://localhost:5000/swagger

Login seed: admin@local.test / Admin@123, editor@local.test / Editor@123


**Exemplo para obter token:**

curl -X POST http://localhost:5000/login \
  -H "Content-Type: application/json" \
  -d '{"email":"admin@local.test","password":"Admin@123"}'


---


