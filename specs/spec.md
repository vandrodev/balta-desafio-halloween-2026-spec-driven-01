# Gerador de Senhas Fortes

## Problema

Aplicações e usuários precisam gerar senhas que atendam a critérios mínimos de composição e que possam ser recuperadas posteriormente por meio de um identificador único.

O desafio consiste em desenvolver uma API capaz de gerar senhas fortes, armazená-las para consulta posterior e permitir sua recuperação a partir de um GUID.

A solução deve atender aos requisitos funcionais definidos neste documento, mantendo as regras de negócio separadas dos mecanismos de comunicação HTTP e de persistência.

## Objetivo

Desenvolver uma API que permita:

- Gerar uma senha que atenda aos critérios mínimos definidos neste documento.
- Armazenar cada senha gerada para permitir sua recuperação posterior.
- Retornar um identificador único (GUID) associado à senha armazenada.
- Consultar uma senha previamente gerada utilizando o GUID correspondente.
- Informar erros de entrada e de consulta de maneira consistente e previsível.

O projeto tem finalidade educacional e visa praticar o desenvolvimento orientado por especificações (_Spec-Driven Development_ — SDD).

## Usuários

### USU-01 — Consumidor da API

Pessoa ou aplicação que utiliza os endpoints da API para gerar senhas e consultar senhas anteriormente geradas.

### USU-02 — Desenvolvedor do projeto

Pessoa responsável por implementar, testar e manter a solução de acordo com os requisitos funcionais, as regras de negócio e os critérios de aceite definidos nesta especificação.

## Histórias

### US-01 — Gerar uma senha forte

**Como** consumidor da API,  
**quero** solicitar a geração de uma senha forte,  
**para** obter uma senha que atenda aos critérios mínimos definidos pelo sistema e que possa ser recuperada posteriormente.

**Critérios de aceitação:**

- A solicitação deve gerar uma nova senha.
- A senha deve atender a todas as regras de negócio aplicáveis.
- A senha deve ser armazenada para consulta posterior.
- A operação deve retornar um GUID que identifique a senha armazenada.
- Uma falha que impeça a conclusão da operação não deve resultar em um identificador que represente uma senha inexistente.

### US-02 — Consultar uma senha pelo identificador

**Como** consumidor da API,  
**quero** consultar uma senha utilizando seu GUID,  
**para** recuperar uma senha gerada anteriormente.

**Critérios de aceitação:**

- Quando o GUID corresponder a uma senha armazenada, a API deve retornar a senha associada.
- Quando o GUID estiver malformado, a API deve informar um erro de validação.
- Quando o GUID for válido, mas não corresponder a uma senha armazenada, a API deve informar que o recurso não foi encontrado.

## Requisitos funcionais

### RF-01 — Gerar senha

A API deve disponibilizar uma operação que permita solicitar a geração de uma nova senha sem exigir parâmetros de entrada.

A senha gerada deve atender às regras de negócio estabelecidas nesta especificação.

### RF-02 — Persistir senha gerada

A API deve armazenar cada senha gerada de modo que ela possa ser recuperada posteriormente por meio de seu identificador.

A persistência deve manter a associação entre o GUID e a senha correspondente.

### RF-03 — Retornar identificador da senha

Após concluir com sucesso a geração e o armazenamento da senha, a API deve retornar um GUID que identifique o registro criado.

O identificador deve permitir a consulta posterior da senha correspondente.

### RF-04 — Consultar senha por GUID

A API deve disponibilizar uma operação que receba um GUID e retorne a senha associada a esse identificador, desde que exista um registro correspondente.

### RF-05 — Validar identificador recebido

A API deve validar o formato do identificador recebido na consulta.

Quando o identificador não estiver em um formato válido de GUID, a operação deve retornar HTTP 400 com uma resposta no formato ProblemDetails.

### RF-06 — Tratar senha não encontrada

Quando a consulta utilizar um GUID válido que não corresponda a uma senha armazenada, a API deve retornar HTTP 404.

A resposta não deve apresentar uma senha diferente daquela solicitada nem simular uma consulta bem-sucedida.

### RF-07 — Padronizar erros de validação

As falhas de validação de entrada devem retornar HTTP 400 e utilizar o formato ProblemDetails.

A resposta deve comunicar o problema de forma compreensível, sem expor detalhes internos de implementação.

## Regras de negócio

### RN-01 — Comprimento mínimo

Toda senha gerada deve possuir pelo menos 16 caracteres.

### RN-02 — Presença de caracteres especiais

Toda senha gerada deve conter pelo menos um caractere especial.

O conjunto exato de caracteres considerados especiais deverá ser definido no plano técnico antes da implementação, de maneira consistente com os testes.

### RN-03 — Ausência de espaços em branco

A senha gerada não pode conter espaços em branco.

### RN-04 — Persistência obrigatória

Toda senha gerada com sucesso deve ser armazenada para consulta posterior.

A geração só deve ser considerada concluída quando a senha puder ser recuperada por meio do GUID retornado.

### RN-05 — Identificação única

Cada senha armazenada deve possuir um GUID que permita identificar seu registro individualmente.

O identificador retornado deve corresponder à senha efetivamente armazenada.

### RN-06 — Associação entre identificador e senha

Uma consulta por GUID deve retornar exclusivamente a senha associada ao identificador informado.

A consulta não deve gerar uma nova senha nem alterar a senha armazenada.

### RN-07 — Consulta inexistente

Um GUID válido sem registro correspondente deve ser tratado como recurso não encontrado, e não como erro de formato.

### RN-08 — Não ampliação dos critérios mínimos

Os critérios obrigatórios de composição definidos pelo desafio são: comprimento mínimo de 16 caracteres, presença de caracteres especiais e ausência de espaços em branco.

A exigência adicional de letras maiúsculas, letras minúsculas ou números não deve ser introduzida como regra obrigatória sem uma decisão explícita no escopo do projeto.

## Casos de borda

### CB-01 — Falha na geração da senha

Se não for possível gerar uma senha que atenda aos critérios obrigatórios, a operação não deve ser considerada bem-sucedida nem retornar um GUID sem correspondência com uma senha armazenada.

O tratamento HTTP específico para uma falha interna desse tipo deverá ser definido no plano técnico.

### CB-02 — Falha no armazenamento

Se a senha não puder ser armazenada, a API não deve informar que a operação foi concluída com sucesso nem retornar um GUID utilizável para consultar um registro inexistente.

### CB-03 — GUID malformado

Uma consulta com identificador em formato inválido deve retornar HTTP 400 com ProblemDetails.

### CB-04 — GUID válido sem correspondência

Uma consulta com GUID sintaticamente válido, mas sem registro associado, deve retornar HTTP 404.

### CB-05 — Consulta de senha existente

Uma consulta com GUID correspondente a um registro armazenado deve retornar exatamente a senha associada a esse registro.

### CB-06 — Senhas geradas em solicitações distintas

Solicitações distintas de geração devem produzir registros identificáveis individualmente. Cada GUID retornado deve permitir recuperar a senha correspondente ao seu próprio registro.

A especificação não exige que o conteúdo das senhas seja necessariamente diferente em todas as solicitações, desde que os registros e seus identificadores sejam tratados corretamente.

### CB-07 — Persistência entre consultas

Depois que uma senha for armazenada com sucesso, uma consulta posterior pelo GUID retornado deve recuperar a senha correspondente, sem depender de a senha permanecer apenas na memória da operação anterior.

## Fora de escopo

Não fazem parte do escopo obrigatório deste desafio:

- Cadastro, autenticação ou gerenciamento de usuários.
- Login, controle de sessão ou autorização por usuário.
- Interface gráfica ou aplicação frontend.
- Recuperação de senhas por e-mail ou outros canais.
- Redefinição, atualização ou exclusão de senhas armazenadas.
- Classificação configurável de força de senha.
- Definição de políticas adicionais de composição além das estabelecidas nesta especificação.
- Criptografia obrigatória das senhas armazenadas.
- Recursos de segurança, auditoria, monitoramento ou infraestrutura voltados à operação em produção.
- Implantação em ambiente de produção.

O projeto tem finalidade exclusivamente educacional. A ausência de criptografia obrigatória é uma delimitação deste desafio, e não uma recomendação para sistemas reais que armazenem senhas.

## Critérios de aceite

### CA-01 — Geração bem-sucedida

**Dado que** a API esteja disponível,  
**quando** o consumidor solicitar a geração de uma senha,  
**então** a API deve gerar e armazenar uma senha válida e retornar um GUID correspondente ao registro criado.

### CA-02 — Comprimento mínimo atendido

**Dado que** uma senha tenha sido gerada com sucesso,  
**quando** seu conteúdo for verificado,  
**então** ela deve possuir pelo menos 16 caracteres.

### CA-03 — Caractere especial presente

**Dado que** uma senha tenha sido gerada com sucesso,  
**quando** seu conteúdo for verificado,  
**então** ela deve conter pelo menos um caractere especial.

### CA-04 — Ausência de espaços em branco

**Dado que** uma senha tenha sido gerada com sucesso,  
**quando** seu conteúdo for verificado,  
**então** ela não deve conter espaços em branco.

### CA-05 — Consulta de senha existente

**Dado que** uma senha tenha sido gerada e armazenada,  
**e que** seu GUID tenha sido retornado pela API,  
**quando** o consumidor consultar esse GUID,  
**então** a API deve retornar exatamente a senha originalmente associada ao identificador.

### CA-06 — Consulta com GUID inválido

**Dado que** o consumidor informe um identificador em formato inválido,  
**quando** solicitar a consulta da senha,  
**então** a API deve retornar HTTP 400 com uma resposta ProblemDetails.

### CA-07 — Consulta com GUID inexistente

**Dado que** o consumidor informe um GUID válido sem registro correspondente,  
**quando** solicitar a consulta da senha,  
**então** a API deve retornar HTTP 404.

### CA-08 — Falha de persistência

**Dado que** ocorra uma falha no armazenamento da senha,  
**quando** a operação de geração for executada,  
**então** a API não deve retornar uma resposta de sucesso que indique a criação de um registro inexistente.

### CA-09 — Consistência dos registros

**Dado que** duas ou mais solicitações de geração tenham sido concluídas com sucesso,  
**quando** cada GUID retornado for consultado,  
**então** cada consulta deve recuperar a senha associada ao respectivo identificador.

### CA-10 — Testes das regras de negócio

Todas as regras de negócio devem possuir testes unitários que verifiquem seus comportamentos obrigatórios.

### CA-11 — Testes de integração dos endpoints

Cada endpoint deve possuir pelo menos um teste de integração que valide seu caminho feliz.

### CA-12 — Validação dos erros de entrada

As falhas de validação de entrada devem ser verificadas por testes e retornar HTTP 400 com ProblemDetails.

### CA-13 — Definição de pronto do projeto

O desafio será considerado concluído quando:

- A aplicação compilar sem erros ou warnings.
- Todos os testes automatizados passarem.
- A aplicação iniciar e executar sem erros.
- Os requisitos funcionais e as regras de negócio desta especificação forem atendidos.
