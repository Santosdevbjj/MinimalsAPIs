## Desafio de Projeto:  Trabalhando com ASP.NET Minimals APIs

![GFTNet001](https://github.com/user-attachments/assets/19323aba-1ed5-40ee-9a9f-0f42f578b800)


**Bootcamp GFT Start #7 .NET**

---

📘 Minimal API Vehicles

Este repositório contém uma Minimal API em ASP.NET Core 8 para gerenciamento de veículos, incluindo autenticação com usuários administradores e testes automatizados (unitários e de integração).

A solução foi organizada com foco em Clean Architecture e boas práticas de desenvolvimento em microserviços.


---

📂 Estrutura de Diretórios

minimal-api-vehicles/
├─ src/
│  ├─ MinimalApi.Vehicles/              # Projeto principal da API
│  │  ├─ MinimalApi.Vehicles.csproj     # Arquivo de configuração do projeto
│  │  ├─ Program.cs                     # Ponto de entrada da API
│  │  ├─ appsettings.json               # Configurações globais
│  │  ├─ appsettings.Development.json   # Configurações específicas para dev
│  │  ├─ Dockerfile                     # Dockerfile da API
│  │  ├─ Models/                        # Entidades do domínio
│  │  │  ├─ AdminUser.cs
│  │  │  └─ Vehicle.cs
│  │  ├─ DTOs/                          # Objetos de transferência de dados
│  │  │  ├─ LoginRequest.cs
│  │  │  ├─ VehicleDto.cs
│  │  │  └─ AdminCreateDto.cs
│  │  ├─ Data/                          # Acesso a dados e seed inicial
│  │  │  ├─ AppDbContext.cs
│  │  │  └─ DbSeed.cs
│  │  ├─ Services/                      # Regras de negócio e autenticação
│  │  │  ├─ IAuthService.cs
│  │  │  └─ AuthService.cs
│  │  └─ Extensions/                    # Extensões e injeção de dependência
│  │     └─ ServiceExtensions.cs
│  └─ MinimalApi.Vehicles.Tests/        # Projeto de testes automatizados
│     ├─ MinimalApi.Vehicles.Tests.csproj
│     ├─ UnitTests/
│     │  └─ AdminUserTests.cs
│     └─ IntegrationTests/
│        └─ VehiclesApiTests.cs
├─ .env.development                     # Variáveis de ambiente (dev)
├─ .env.test                            # Variáveis de ambiente (testes)
├─ .env.production                      # Variáveis de ambiente (produção)
├─ .gitignore                           # Arquivos ignorados pelo Git
├─ docker-compose.yml                   # Configuração de containers
├─ docker-compose.override.yml          # Overrides para desenvolvimento
├─ README.md                            # Este arquivo
└─ .github/workflows/ci-cd.yml          # Pipeline CI/CD com GitHub Actions


---

⚙️ Descrição dos Arquivos Importantes

🔹 Projeto Principal (src/MinimalApi.Vehicles)

Program.cs → Define os endpoints, configura middlewares e inicializa a API.

appsettings.json → Configurações padrão do projeto (ex.: JWT, conexão com banco).

Dockerfile → Contém as instruções para criar a imagem da API.

Models/ → Entidades principais (ex.: Vehicle, AdminUser).

DTOs/ → Objetos para entrada/saída de dados (ex.: LoginRequest, VehicleDto).

Data/ → Contexto de banco (AppDbContext) e dados iniciais (DbSeed).

Services/ → Contém a lógica de autenticação (AuthService).

Extensions/ → Extensões para configuração de serviços (injeção de dependência).


🔹 Projeto de Testes (src/MinimalApi.Vehicles.Tests)

UnitTests/ → Testes unitários isolados de regras de negócio.

IntegrationTests/ → Testes que verificam o funcionamento da API com banco em memória/container.


🔹 Arquivos de Configuração

.env.development → Variáveis de ambiente para desenvolvimento.

.env.test → Variáveis de ambiente para execução de testes.

.env.production → Variáveis de ambiente para produção.

docker-compose.yml → Define serviços como API e Postgres.

docker-compose.override.yml → Ajustes específicos para desenvolvimento (hot reload, mounts locais).

.github/workflows/ci-cd.yml → Pipeline para build, testes e deploy automático.



---

🌍 Variáveis de Ambiente

Cada ambiente tem seu próprio .env.

📌 .env.development

ASPNETCORE_ENVIRONMENT=Development
DOTNET_USE_POLLING_FILE_WATCHER=1
DOTNET_HOST_PATH=/usr/share/dotnet/dotnet
DOTNET_RUNNING_IN_CONTAINER=true

DB_HOST=vehicles-db
DB_PORT=5432
DB_NAME=vehicles_dev
DB_USER=postgres
DB_PASSWORD=postgres

📌 .env.test

ASPNETCORE_ENVIRONMENT=Test
DB_HOST=vehicles-db-test
DB_PORT=5432
DB_NAME=vehicles_test
DB_USER=postgres
DB_PASSWORD=postgres

📌 .env.production

ASPNETCORE_ENVIRONMENT=Production
DB_HOST=vehicles-db
DB_PORT=5432
DB_NAME=vehicles_prod
DB_USER=postgres
DB_PASSWORD=supersecret


---

🐳 Executando os Ambientes

🔹 Desenvolvimento

docker-compose --env-file .env.development up --build

➡️ API disponível em http://localhost:5000

🔹 Testes

docker-compose --env-file .env.test up --build --abort-on-container-exit

➡️ Executa os testes automatizados e encerra os containers ao final.

🔹 Produção

docker-compose --env-file .env.production up -d --build

➡️ Sobe a aplicação em modo detached (background).


---

✅ Pipeline CI/CD

O arquivo .github/workflows/ci-cd.yml garante:

1. Build da aplicação.


2. Execução dos testes unitários e de integração.


3. Build da imagem Docker.


4. Deploy automático no ambiente configurado.




---





