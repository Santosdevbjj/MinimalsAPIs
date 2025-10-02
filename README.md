## Desafio de Projeto:  Trabalhando com ASP.NET Minimals APIs

![GFTNet001](https://github.com/user-attachments/assets/53b03495-a48d-4248-a775-5de04bd979cc)


**Bootcamp GFT Start #7 .NET, ministrado pela DIO** 

--- 

**DESCRIÇÃO:**
Neste LAB, vamos criar uma API utilizando a técnica de Minimals APIs para o registro de veículos, ampliando suas funcionalidades ao incorporar administradores com regras JWT.

 Ao explorarmos o funcionamento da API, nos familiarizaremos com o uso do Swagger, além de trabalharmos com testes, garantindo a robustez e confiabilidade do sistema que estaremos desenvolvendo.


---

# Minimal API Vehicles 🚗

Uma aplicação de exemplo em **.NET 8 Minimal APIs** para gerenciamento de veículos, com autenticação de administradores, testes automatizados, integração com MySQL via Docker Compose e ambientes configurados por `.env`.

---

## 🚀 Versão Enxuta (Quickstart) 



### Rodando em Development
```bash
docker-compose --env-file .env.development up --build

```


Rodando em Test

docker-compose --env-file .env.test up --build



Rodando em Production

docker-compose --env-file .env.production up --build -d


Swagger: http://localhost:5000/swagger

Login seed: admin@local.test / Admin@123, editor@local.test / Editor@123



Exemplo para obter token:

curl -X POST http://localhost:5000/login \
  -H "Content-Type: application/json" \
  -d '{"email":"admin@local.test","password":"Admin@123"}'


---


**Versão Detalhada**

1. **Estrutura de Diretórios**

<img width="799" height="1307" alt="Screenshot_20251002-144625" src="https://github.com/user-attachments/assets/2d8bd492-88ef-428c-9f2c-03f6c0a6fc06" />




---

2. **Descrição dos Arquivos**

**docker-compose.yml**

Define serviços mysql e api.

Base para todos os ambientes.

Não contém credenciais sensíveis.


**docker-compose.override.yml**

Overwrite do compose para desenvolvimento.

Aponta .env corretos e faz bind de volumes locais.

Facilita hot reload em dev.


.env.development, .env.test, .env.production

Guardam credenciais e variáveis específicas por ambiente:

MYSQL_USER, MYSQL_PASSWORD

DB_HOST, DB_NAME

ASPNETCORE_ENVIRONMENT

JWT_SECRET



**src/MinimalApi.Vehicles/**

**Código principal da API:**

**Program.cs** → configura endpoints, middlewares, JWT, Swagger, DI, EF Core.

**Models/** → entidades (Vehicle, AdminUser).

**DTOs/** → objetos de entrada/saída (LoginRequest, VehicleDto, AdminCreateDto).

**Data/** → AppDbContext + DbSeed (seed de usuários).

**Services/** → lógica de autenticação (AuthService) e interface (IAuthService).

**Extensions/** → métodos auxiliares e configuração de serviços.



**src/MinimalApi.Vehicles.Tests/**

**Projeto de testes automatizados:**

**UnitTests/ → teste de entidades e regras de negócio.**

**IntegrationTests/** → testes de endpoints da API e integração com banco.



**.github/workflows/ci-cd.yml**

Pipeline GitHub Actions:

Build

**Testes unitários e integração**

(Opcional) Build Docker + push

Deploy (configurar secrets para produção)



.gitignore

Ignora bin/, obj/, .env*, publish/, arquivos temporários.



---

3. **Como rodar**

Development

docker-compose --env-file .env.development up --build

API em http://localhost:5000/swagger

DB exposto em porta configurada no .env

Seed inicial:

Admin: admin@local.test / Admin@123

Editor: editor@local.test / Editor@123



**Test**

docker-compose --env-file .env.test up --build --abort-on-container-exit

Executa testes automatizados e encerra os containers.


**Production**

docker-compose --env-file .env.production up -d --build

Modo detach (background)

Logs: docker-compose logs -f



---

4. **Migrations (EF Core)**

**Criar migration:**


dotnet ef migrations add InitialCreate -p src/MinimalApi.Vehicles -s src/MinimalApi.Vehicles

**Aplicar:**


dotnet ef database update -p src/MinimalApi.Vehicles -s src/MinimalApi.Vehicles

> O Program.cs já aplica db.Database.Migrate() automaticamente.




---

5. **Endpoints principais**

Método	Endpoint	Roles

GET	/	qualquer usuário
POST	/login	qualquer usuário
POST	/vehicles	Admin, Editor
GET	/vehicles	Admin, Editor
GET	/vehicles/{id}	Admin, Editor
PUT	/vehicles/{id}	Admin
DELETE	/vehicles/{id}	Admin
POST	/admins	Admin
GET	/admins	Admin



---

6. **Exemplos práticos (curl)**

**Login e obter token**

curl -X POST http://localhost:5000/login \
  -H "Content-Type: application/json" \
  -d '{"email":"admin@local.test","password":"Admin@123"}'

**Criar veículo**

curl -X POST http://localhost:5000/vehicles \
  -H "Content-Type: application/json" \
  -H "Authorization: Bearer <TOKEN>" \
  -d '{"make":"Toyota","model":"Corolla","vin":"JTDBR32E720012345","year":2020}'

**Listar veículos**

curl -H "Authorization: Bearer <TOKEN>" http://localhost:5000/vehicles


---

7. **Boas práticas**

Não versionar .env com segredos.

Usar HTTPS em produção.

Rotacionar JWT secret periodicamente.

Políticas CORS apenas para domínios autorizados.

Evitar migrations automáticas em produção.



---

8. **Troubleshooting rápido**

**Erro MySQL** → checar portas, DB_HOST, logs do container.

401 / Token inválido → verificar JWT_SECRET do .env.

**Swagger não aparece** → ASPNETCORE_ENVIRONMENT=Development.



---










