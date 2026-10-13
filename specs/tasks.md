# Tarefas do projeto

## Visão geral

Este documento organiza a execução do desafio em tarefas pequenas, ordenadas e verificáveis, derivadas da especificação funcional e do plano técnico aprovados. A sequência foi planejada para preservar a separação entre regra de negócio, transporte HTTP, persistência e documentação gerada pela própria API.

## T-01 — Inicializar a solução e a estrutura mínima

**Objetivo:** preparar a base da solução para desenvolvimento e testes.

**Entregáveis:**

- criação da solução .NET 10 com C#;
- projeto de API com Minimal API;
- projeto de testes com xUnit;
- estrutura inicial organizada para evolução.

**Critérios de aceite:**

- a solução compila no estado inicial;
- os projetos da API e dos testes estão configurados;
- a estrutura inicial está pronta para receber as camadas de domínio, aplicação e persistência.

## T-02 — Configurar persistência e infraestrutura base

**Objetivo:** habilitar o armazenamento mínimo do desafio usando SQLite e Entity Framework Core.

**Entregáveis:**

- pacote/configuração do SQLite;
- DbContext configurado;
- entidade de persistência para o registro da senha;
- infraestrutura mínima para leitura e escrita de registros.

**Critérios de aceite:**

- o contexto de dados está funcional;
- o banco é inicializado corretamente em ambiente local;
- a entidade representa a associação entre GUID e senha.

## T-03 — Definir a regra de negócio da senha

**Objetivo:** centralizar a validação da senha em uma regra de negócio consistente e testável.

**Entregáveis:**

- regra para comprimento mínimo de 16 caracteres;
- regra para presença de ao menos um caractere especial;
- regra para ausência de espaços em branco;
- regra para presença de números, se mantida como requisito explícito;
- validação encapsulada fora do endpoint HTTP.

**Critérios de aceite:**

- senhas válidas passam sem erro;
- senhas inválidas são rejeitadas antes da persistência;
- a regra de negócio não depende do transporte HTTP.

## T-04 — Implementar geração e armazenamento

**Objetivo:** gerar uma senha válida e persistir corretamente o registro associado ao GUID.

**Entregáveis:**

- geração de senha em conformidade com a regra de negócio;
- criação de GUID para o registro;
- persistência da senha e do identificador;
- garantia de que o GUID retornado corresponda ao dado salvo.

**Critérios de aceite:**

- uma geração bem-sucedida cria um registro persistido;
- o identificador retornado é válido;
- a senha persistida pode ser recuperada posteriormente.

## T-05 — Implementar consulta por GUID

**Objetivo:** permitir a recuperação da senha através do identificador único.

**Entregáveis:**

- validação do GUID recebido;
- busca do registro correspondente;
- retorno de 400 para GUID inválido;
- retorno de 404 para GUID válido sem registro;
- retorno da senha associada quando o registro existe.

**Critérios de aceite:**

- GUID válido e existente retorna a senha correta;
- GUID inválido retorna 400 com ProblemDetails;
- GUID válido e inexistente retorna 404.

## T-06 — Expor os endpoints HTTP com contrato explícito

**Objetivo:** expor os fluxos da API por meio de endpoints HTTP sem misturar transporte e regra de negócio.

**Entregáveis:**

- endpoint POST /passwords;
- endpoint GET /passwords/{id};
- resposta de sucesso com GUID ou senha conforme o contrato definido;
- respostas de erro consistentes com ProblemDetails;
- separação clara entre endpoint, caso de uso e persistência.

**Critérios de aceite:**

- os endpoints respondem conforme o contrato estabelecido;
- erros de validação retornam HTTP 400;
- recursos inexistentes retornam HTTP 404.

## T-07 — Habilitar documentação nativa via OpenAPI

**Objetivo:** gerar a documentação da API de forma nativa no ASP.NET Core usando OpenAPI.

**Entregáveis:**

- configuração da geração de documentação OpenAPI;
- exposição dos endpoints e schemas reais da API;
- integração dos contratos de entrada e saída da aplicação;
- documentação refletindo o comportamento implementado.

**Critérios de aceite:**

- a API gera documentação OpenAPI corretamente;
- os endpoints e DTOs aparecem na documentação;
- a especificação implementada está rastreável por meio da própria API.

## T-08 — Conectar Swagger UI para leitura da especificação

**Objetivo:** disponibilizar a documentação para leitura humana e validação técnica via Swagger UI.

**Entregáveis:**

- configuração do Swagger UI;
- acesso à documentação da API em ambiente local;
- visualização dos endpoints, parâmetros, respostas e erros;
- revisão da API por meio da interface Swagger.

**Critérios de aceite:**

- o Swagger UI está disponível e funcional;
- a API pode ser consultada e validada visualmente pela documentação gerada;
- o consumidor consegue entender os contratos da API sem ler o código-fonte.

## T-09 — Cobrir testes unitários

**Objetivo:** validar cada regra de negócio isoladamente.

**Entregáveis:**

- testes para comprimento mínimo;
- testes para caractere especial;
- testes para ausência de espaços em branco;
- testes para presença de números, se esse critério for adotado;
- testes para rejeição de entradas inválidas.

**Critérios de aceite:**

- todas as regras de negócio possuem testes unitários;
- casos válidos e inválidos são cobertos;
- os testes refletem a regra definida na especificação e no plano.

## T-10 — Cobrir testes de integração

**Objetivo:** validar o comportamento real da API pelos endpoints.

**Entregáveis:**

- teste de geração com sucesso;
- teste de consulta com GUID existente;
- teste de GUID inválido;
- teste de GUID inexistente;
- teste da estrutura da resposta esperada.

**Critérios de aceite:**

- cada endpoint possui pelo menos um teste de integração;
- os status HTTP e corpos esperados são validados;
- o caminho feliz e os principais erros são cobertos.

## T-11 — Validar qualidade final e documentação

**Objetivo:** confirmar que a solução atende aos critérios de pronto do projeto e que a documentação está consistente.

**Entregáveis:**

- execução da suíte completa de testes;
- verificação de build sem erros ou warnings;
- validação da aplicação em execução;
- conferência da documentação OpenAPI e do Swagger UI.

**Critérios de aceite:**

- todos os testes automatizados passam;
- a aplicação compila e inicia sem erro;
- a documentação gerada pela API é consistente com os contratos implementados.

## Dependências

- T-01 precede todas as demais.
- T-02 precede a persistência e a geração.
- T-03 precede T-04 e T-09.
- T-04 precede T-05 e T-10.
- T-05 precede T-06 e T-10.
- T-06 precede T-07 e T-08.
- T-07 e T-08 são concluídas após a API estar funcional e servem como validação da documentação.
- T-09 e T-10 precedem a validação final em T-11.

## Resultado esperado

Ao final da execução desta sequência, o projeto cumprirá a especificação funcional, respeitará a constituição de arquitetura e qualidade, e a API estará documentada nativamente por OpenAPI com leitura e revisão por Swagger UI.
