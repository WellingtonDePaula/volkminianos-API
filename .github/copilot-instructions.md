# Diretrizes para mensagens de commit (Uso com Copilot) - API Linhas de Ônibus (EF Core)

## Objetivo
- Garantir mensagens de commit consistentes para a API em ASP.NET Core da matéria de Desenvolvimento de Projetos.

## Convenção adotada
- Usar a convenção "Conventional Commits": `type(scope): descrição curta`.

## Tipos comuns
- `feat`    : nova funcionalidade (novos endpoints, regras de negócio, tabelas novas)
- `fix`     : correção de bug (erros em requisições, validações, problemas no banco)
- `docs`    : documentação (Swagger, comentários, README)
- `refactor`: melhoria de código sem alterar o comportamento (otimizar LINQ)
- `chore`   : alteração em pacotes NuGet, appsettings.json, configuração do DbContext ou novas migrations

## Escopos Comuns (Domínio de Linhas de Ônibus)
- `onibus`, `rotas`, `horarios`, `motoristas`, `paradas`, `db` (migrações/modelagem), `auth`.

## Regras Específicas para EF Core & Migrations (CRÍTICO)
- **IGNORAR detalhes de arquivos de Migration:** Ao detectar arquivos dentro da pasta `Migrations` (ex: `.Designer.cs`, `ModelSnapshot.cs`), o Copilot **NÃO** deve tentar ler ou narrar as mudanças internas do código gerado pela CLI do .NET. 
- Foque estritamente no objetivo da migração na vida real. Use o tipo `chore(db)` ou `feat(db)` dependendo do impacto.

## Regras úteis para o Copilot
- Gerar a mensagem obrigatoriamente em **Português (Brasil)**.
- Linha de cabeçalho com no máximo 50 caracteres, usando verbo no **infinitivo** (ex: Adicionar, Criar, Corrigir).
- No corpo (opcional), focar na regra de negócio da linha de ônibus. NÃO listar os arquivos `.cs` modificados.

## Exemplos
- `chore(db): adicionar migração para tabela de motoristas`
- `fix(db): corrigir relacionamento entre rotas e horários no DbContext`
- `feat(rotas): criar endpoint para listar itinerários por ônibus`
- `fix(horarios): corrigir validação de partida em duplicidade`
- `refactor(onibus): otimizar consulta LINQ usando AsNoTracking`
