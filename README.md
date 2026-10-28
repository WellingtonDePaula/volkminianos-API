# Volkminianos API

API back-end do sistema web da linha **Volkmann** (transporte público de Pomerode - SC). O sistema mapeia as paradas da linha e permite consultar a **tarifa entre dois pontos** de forma simples para passageiros e turistas.

Projeto desenvolvido no **IFC Campus Blumenau**.

## O que a API faz

- **Bairros:** cadastro dos bairros atendidos.
- **Pontos:** paradas com nome, endereço, coordenadas (latitude/longitude) e bairro, prontas para serem mostradas em um mapa.
- **Tarifas:** valor entre dois bairros (vale nos dois sentidos) e uma consulta direta de "quanto custa ir do ponto X ao ponto Y?".
- **Usuários:** login com JWT. Só administradores cadastram bairros, pontos, tarifas e outros usuários. As consultas são públicas.

## Tecnologias

- C# com ASP.NET Core (.NET 9)
- Entity Framework Core + MySQL 8 (driver Pomelo)
- Autenticação JWT e senhas com BCrypt
- Documentação interativa com Scalar (OpenAPI)
- Docker Compose para o banco de dados

## Pré-requisitos

Instale antes de começar:

| Ferramenta | Para quê | Como verificar |
|---|---|---|
| [.NET SDK 9](https://dotnet.microsoft.com/download/dotnet/9.0) | compilar e rodar a API | `dotnet --version` (deve começar com `9.`) |
| [Docker Desktop](https://www.docker.com/products/docker-desktop/) | subir o MySQL | `docker --version` |
| [Git](https://git-scm.com/) | baixar o repositório | `git --version` |

Instale também a ferramenta de migrations do Entity Framework (uma vez só):

```bash
dotnet tool install --global dotnet-ef
```

## Passo a passo para subir a API

### 1. Baixar o repositório

```bash
git clone https://github.com/<organizacao-ou-usuario>/Volkminianos-API.git
cd Volkminianos-API/Volkminianos-API
```

Todos os comandos abaixo são executados nessa pasta (a que contém o arquivo `Volkminianos-API.csproj` e o `docker-compose.yml`).

### 2. Subir o banco de dados

Abra o Docker Desktop e aguarde ele iniciar. Depois:

```bash
docker compose up -d
```

Isso cria um MySQL 8 em `localhost:3306` com o banco `VolkminianosDB` (usuário `root`, senha `123`). Na primeira vez, o MySQL leva alguns segundos para ficar pronto. Para acompanhar:

```bash
docker compose logs -f db
```

Quando aparecer `ready for connections`, aperte `Ctrl + C` para sair dos logs. O contêiner continua rodando.

### 3. Criar as tabelas (migrations)

```bash
dotnet ef database update
```

### 4. Configurar o administrador inicial

A API só tem rotas protegidas para cadastro, então é preciso existir um primeiro administrador. Ele é criado automaticamente na inicialização, a partir das configurações abaixo. Guarde os valores nos *user-secrets* (ficam na sua máquina e **não** vão para o Git):

```bash
dotnet user-secrets set "Admin:Email" "admin@exemplo.com"
dotnet user-secrets set "Admin:Senha" "troque-por-uma-senha-forte"
dotnet user-secrets set "Admin:Nome" "Administrador"
```

Atenção:

- A senha precisa ter **no mínimo 8 caracteres**, senão o login será recusado.
- O administrador só é criado se **ainda não existir nenhum** no banco. Para trocar a senha depois, use a API.

### 5. Rodar a API

```bash
dotnet run --launch-profile http
```

Quando aparecer `Now listening on: http://localhost:5212`, a API está no ar.

Para usar HTTPS (`https://localhost:7041`), rode uma vez `dotnet dev-certs https --trust` e use `dotnet run --launch-profile https`.

### 6. Conferir que funcionou

- Abra a documentação interativa: <http://localhost:5212/scalar/v1> (disponível só em ambiente de desenvolvimento).
- Ou teste uma rota pública no navegador: <http://localhost:5212/api/bairro> (deve responder `[]` na primeira vez).
- Faça o login com o administrador do passo 4:

```bash
curl -X POST http://localhost:5212/api/usuario/login \
  -H "Content-Type: application/json" \
  -d '{"email":"admin@exemplo.com","senha":"troque-por-uma-senha-forte"}'
```

A resposta traz um `token`. Nas rotas de administrador, envie-o no cabeçalho `Authorization: Bearer <token>`.

## Configurações opcionais

Todas podem ser definidas com `dotnet user-secrets set "<chave>" "<valor>"`, sem editar arquivos do repositório.

| Chave | Para quê | Padrão |
|---|---|---|
| `ConnectionStrings:DefaultConnection` | conexão com o MySQL | `Server=localhost;Database=VolkminianosDB;Uid=root;Pwd=123;Port=3306` |
| `Jwt:Chave` | chave que assina os tokens (mínimo 32 caracteres) | chave de exemplo do `appsettings.json` |
| `Jwt:ExpiracaoMinutos` | validade do token | `120` |
| `Cors:Origens:0` | endereço do front-end liberado para chamar a API (ex.: `http://localhost:5173`) | vazio = qualquer origem |

Se for publicar a API em algum servidor, **troque a chave JWT e a senha do banco**: os valores padrão do repositório servem só para desenvolvimento.

## Endpoints

Rotas marcadas como **público** não exigem login. As de **admin** exigem o token de um administrador.

| Recurso | Método e rota | Acesso |
|---|---|---|
| Usuário | `POST /api/usuario/login` | público |
| Usuário | `GET /api/usuario`, `GET /api/usuario/{id}`, `GET /api/usuario/por-email?email=` | logado |
| Usuário | `POST /api/usuario`, `PUT /api/usuario/{id}`, `DELETE /api/usuario/{id}` | admin |
| Bairro | `GET /api/bairro`, `GET /api/bairro/{id}` | público |
| Bairro | `POST /api/bairro`, `PUT /api/bairro/{id}`, `DELETE /api/bairro/{id}` | admin |
| Ponto | `GET /api/ponto`, `GET /api/ponto/{id}`, `GET /api/ponto/bairro/{bairroId}`, `GET /api/ponto/nome/{nome}` | público |
| Ponto | `POST /api/ponto`, `PUT /api/ponto/{id}`, `DELETE /api/ponto/{id}` | admin |
| Tarifa | `GET /api/tarifa`, `GET /api/tarifa/{id}` | público |
| Tarifa | `GET /api/tarifa/consulta?origemPontoId=1&destinoPontoId=2` | público |
| Tarifa | `POST /api/tarifa`, `PUT /api/tarifa/{id}`, `DELETE /api/tarifa/{id}` | admin |

Detalhes de cada corpo de requisição estão na documentação do Scalar.

### Como a tarifa funciona

- O valor é cadastrado **entre dois bairros** e vale nos dois sentidos (A→B é igual a B→A).
- Também é possível cadastrar tarifa dentro do mesmo bairro.
- A consulta recebe dois **pontos**, descobre o bairro de cada um e devolve o valor correspondente. Se não houver tarifa cadastrada para o par, a resposta é `404` com uma mensagem explicando.

## Testando

O arquivo `Volkminianos-API.http` contém um roteiro com dezenas de requisições em ordem, cada uma com o resultado esperado. Abra-o no Visual Studio 2022 ou no VS Code (extensão *REST Client*), ajuste as variáveis do topo (`@adminEmail` e `@adminSenha` devem ser os do passo 4) e execute de cima para baixo.

## Problemas comuns

| Sintoma | O que fazer |
|---|---|
| `dotnet ef` não é reconhecido | Rode `dotnet tool install --global dotnet-ef` e abra um terminal novo. |
| `Unable to connect to any of the specified MySQL hosts` | O Docker Desktop precisa estar aberto e o contêiner rodando (`docker compose ps`). Após subir, aguarde o `ready for connections`. |
| Porta 3306 já em uso | Há outro MySQL na máquina. Pare-o ou mude a porta em `docker-compose.yml` (ex.: `"3307:3306"`) e ajuste `Port=` na connection string. |
| O login do administrador não funciona | Confirme que a senha tem 8+ caracteres e que o banco já tinha as tabelas quando a API subiu (rode as migrations e reinicie a API). Veja os logs da API: uma mensagem avisa quando o administrador não pôde ser criado. |
| `401` nas rotas de admin | O token expirou (padrão: 2 horas) ou falta o cabeçalho `Authorization: Bearer <token>`. |
| `403` nas rotas de admin | O usuário logado não é administrador. |
| Aviso de HTTPS ao usar o perfil `http` | Pode ignorar: é só o redirecionamento para HTTPS que não está ativo nesse perfil. |

Para parar o banco: `docker compose down` (os dados são mantidos). Para apagar também os dados: `docker compose down -v`.

## Estrutura do projeto

```
Volkminianos-API/
├── Volkminianos-API.slnx
└── Volkminianos-API/
    ├── Controllers/       # rotas HTTP
    ├── DTOs/              # objetos de entrada e saída da API
    ├── Domain/            # contratos (interfaces) de repositórios e services
    ├── Infrastructure/    # implementações: repositories, services e seed do admin
    ├── Models/            # entidades do banco
    ├── Data/              # AppDbContext (Entity Framework)
    ├── Migrations/        # histórico de alterações do banco
    ├── Program.cs         # configuração da API (DI, JWT, CORS, OpenAPI)
    └── docker-compose.yml # MySQL para desenvolvimento
```

## Licença

Veja o arquivo [LICENSE](LICENSE).
