# Plano técnico

## Contexto

Este projeto é um app educacional para praticar Spec-Driven Development. O objetivo é construir uma API simples com dois fluxos principais: gerar uma senha forte e consultar uma senha armazenada usando um GUID.

A solução deve cumprir os requisitos da especificação funcional sem ampliar o escopo do desafio. A implementação deve priorizar clareza e rastreabilidade, com regras de negócio separadas do transporte HTTP e da persistência.

## Arquitetura

A estrutura da solução será organizada em camadas com responsabilidades claras:

- Camada de transporte: endpoints HTTP, model binding e respostas de erro.
- Camada de aplicação: orquestração de casos de uso e validação de regras de negócio.
- Camada de persistência: acesso ao SQLite via Entity Framework Core.
- Camada de domínio: regras e modelos mínimos necessários ao problema.

Os endpoints não acessarão o banco diretamente. A lógica de regra de negócio não ficará no endpoint nem no handler HTTP. As regras serão centralizadas em serviços de aplicação, com persistência encapsulada por repositório/EF Core.

## Decisões

### 1. Stack adotada

- .NET 10 com C#
- Minimal API
- SQLite com Entity Framework Core
- xUnit para testes automatizados

Essa escolha está alinhada com a recomendação do desafio e com a constituição do projeto. A stack foi adotada como decisão técnica do projeto por ser simples, adequada ao exercício e compatível com a necessidade de persistência e testes.

### 2. Persistência mínima

- Será usado SQLite para manter o armazenamento local e simples.
- A persistência será mínima e focada no requisito do desafio: associar um GUID a uma senha gerada.
- Não será adicionada complexidade de autenticação, auditoria, migração avançada ou infraestrutura de produção.

### 3. Regras de negócio e testes

- A senha gerada deve obedecer à política mínima do desafio: pelo menos 16 caracteres, ao menos 1 caractere especial, ausência de espaços em branco e presença de números.
- As regras de negócio serão testadas unitariamente, cobrindo cada regra individualmente.
- Gerações inválidas devem ser rejeitadas antes da persistência.
- Os endpoints terão testes de integração para o caminho feliz.
- Os erros de validação seguirão o padrão do desafio, retornando 400 com ProblemDetails.
- Casos de recurso inexistente retornarão 404.

### 4. Escopo e limites

- O projeto é educativo e não deve ser tratado como solução de produção.
- Não será implementada criptografia obrigatória, autenticação ou recursos de segurança de produção.
- Qualquer dependência adicional exige justificativa técnica e deve ser registrada no plano antes da implementação.

## Modelo de dados

O modelo será mínimo e suficiente para atender ao desafio:

- PasswordRecord
  - Id: Guid
  - Password: string
  - CreatedAt: DateTime (opcional, apenas se for útil para rastreio e auditoria interna)

A modelagem deve garantir que:

- cada senha gerada tenha um identificador único;
- o identificador seja usado para recuperar a senha persistida;
- o valor persistido seja exatamente a senha gerada e validada.

Não será criado modelo de usuário, autenticação, auditoria de acesso ou estrutura de dados além do necessário ao requisito.

## Contratos

### Endpoint 1 — Gerar senha

- Método: POST
- Rota: /passwords
- Descrição: gera uma nova senha forte, persiste sua associação com um GUID e retorna o identificador criado.
- Resposta de sucesso: 200 OK ou 201 Created
- Conteúdo: objeto com o identificador gerado, por exemplo:
  - { "id": "<guid>" }
- Resposta de erro de validação: 400 com ProblemDetails

### Endpoint 2 — Consultar senha

- Método: GET
- Rota: /passwords/{id}
- Descrição: consulta a senha associada ao GUID informado.
- Resposta de sucesso: 200 OK
- Conteúdo: objeto contendo a senha associada, por exemplo:
  - { "id": "<guid>", "password": "..." }
- Resposta de GUID inválido: 400 com ProblemDetails
- Resposta de recurso inexistente: 404 Not Found

### Regras de contrato

- O endpoint de criação não exige entrada de dados do cliente.
- O endpoint de consulta exige um GUID válido.
- A API deve retornar um GUID válido após geração bem-sucedida.
- A consulta de um GUID existente deve retornar exatamente a senha armazenada.
- A consulta com GUID inválido deve retornar 400 com ProblemDetails.
- A consulta com GUID válido sem registro correspondente deve retornar 404.
- A API deve evitar expor detalhes internos da implementação em mensagens de erro.
- O formato de erro deve seguir ProblemDetails, conforme exigência do desafio.

## Riscos

### 1. Ambiguidade sobre caracteres especiais

O desafio exige presença de caracteres especiais, mas não define o conjunto considerado especial. Para manter consistência entre regra e teste, o plano técnico adotará o conjunto explícito abaixo como critério mínimo:

- !@#$%^&\*()-\_=+[]{};:,.<>/?|~`

Qualquer senha gerada deve possuir pelo menos um caractere pertencente a esse conjunto.

### 2. Escopo de segurança

O projeto é educacional e não deve ser tratado como solução de produção. A ausência de criptografia e de mecanismos de segurança avançados é intencional. Isso reduz risco de escopo, mas exige disciplina para não acrescentar recursos sem justificativa.

### 3. Regras de qualidade

Como o desafio exige testes unitários e de integração, o principal risco é deixar a regra de negócio e o contrato HTTP mal definidos. A solução deve manter a validação de senha e a persistência alinhadas com a especificação e com os testes.

### 4. Dependências extras

Qualquer biblioteca nova além da stack recomendada deve ser evitada. A justificativa deve ser explicitada no plano antes da implementação para respeitar a governança do projeto.

## Encerramento do plano

Este plano define a implementação mínima necessária para atender ao desafio com qualidade, sem adicionar recursos que não sejam necessários ao problema. A execução do projeto deve seguir a especificação funcional, respeitar a constituição e manter rastreabilidade entre requisitos, decisões técnicas e tarefas.
