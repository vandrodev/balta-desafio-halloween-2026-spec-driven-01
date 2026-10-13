# Constituição do Projeto

## 1. Stack Tecnológica

- A solução deve ser implementada com .NET 10 usando C#.
- A API deve ser construída com Minimal API.
- A persistência deve utilizar SQLite e Entity Framework Core.
- Os testes automatizados devem ser escritos com xUnit.
- Essa stack representa uma escolha deliberada deste projeto, baseada nas recomendações do desafio, e não uma exigência original do enunciado.
- Qualquer substituição ou alteração da stack deve ser justificada tecnicamente, proposta formalmente e aprovada antes da implementação.

## 2. Arquitetura

- Os endpoints e handlers HTTP não devem acessar diretamente o banco de dados.
- As regras de negócio não devem residir nos endpoints nem nos handlers HTTP.
- A aplicação deve manter separação clara de responsabilidades entre o transporte HTTP, a execução dos casos de uso, as regras de negócio e a persistência.
- A separação de responsabilidades não exige que cada responsabilidade corresponda a uma camada, projeto ou conjunto de pastas independente.
- A organização concreta da arquitetura deve ser definida e justificada no `plan.md`, respeitando os princípios estabelecidos nesta constituição.
- A arquitetura deve permitir testar as regras de negócio de forma isolada e validar o comportamento dos endpoints por meio de testes de integração.

## 3. Qualidade e Testes

- Toda validação de entrada que resulte em erro de validação deve retornar HTTP 400 com uma resposta no formato ProblemDetails.
- A aplicação deve compilar sem erros ou warnings.
- Todos os testes automatizados devem passar antes da conclusão do desafio.
- A aplicação deve iniciar e executar sem erros.
- Toda regra de negócio deve possuir testes unitários.
- Todo endpoint deve possuir pelo menos um teste de integração que valide seu caminho feliz.
- Correções e alterações devem preservar a integridade da arquitetura e dos testes existentes.

## 4. Convenções

- O projeto deve seguir as convenções de nomenclatura e organização aplicáveis ao C#.
- Os nomes de classes, métodos, serviços e entidades devem ser claros e consistentes com o domínio do problema.
- A implementação deve priorizar clareza e simplicidade, evitando complexidade desnecessária.
- Os commits devem seguir o formato `tipo(escopo): descrição`, incluindo o identificador do requisito relacionado.
- O formato dos identificadores de requisitos deve ser consistente em toda a documentação do projeto, permitindo rastrear alterações até seus requisitos de origem.
- Os contratos de entrada e saída da API devem ser definidos na especificação funcional e implementados de forma consistente com ela.

## 5. Governança

- A constituição estabelece os princípios e as regras obrigatórias que orientam as decisões e as alterações do projeto.
- O `spec.md` deve definir o comportamento esperado do sistema, os requisitos funcionais, as regras de negócio observáveis e os critérios de aceitação.
- O `plan.md` deve definir as decisões técnicas, a organização da solução, o modelo de dados e a estratégia de implementação necessários para cumprir a especificação e respeitar esta constituição.
- O `tasks.md` deve organizar a execução em tarefas pequenas, ordenadas, rastreáveis e verificáveis, derivadas da especificação e do plano aprovados.
- Em caso de conflito entre o plano e a constituição, o plano deve ser ajustado para respeitar a constituição.
- Qualquer alteração na constituição deve ser proposta, justificada e aprovada antes de ser aplicada.
- Decisões que alterem a stack, a arquitetura, as dependências ou os critérios de qualidade devem ser documentadas e justificadas antes da implementação.
- Nenhuma dependência nova pode ser adicionada sem justificativa técnica registrada no `plan.md` e aprovação prévia.
- Requisitos ambíguos ou contraditórios que afetem o comportamento esperado ou a implementação devem ser esclarecidos na especificação funcional ou no plano técnico, conforme sua natureza, antes da implementação correspondente.
- O escopo da solução deve permanecer limitado ao desafio proposto.
- Mudanças que introduzam objetivos ou funcionalidades além do desafio original devem ser tratadas como expansão de escopo e aprovadas antes da implementação.

## 6. Escopo e Contexto Educacional

- O projeto tem finalidade educacional e de aprendizagem dos conceitos de Spec-Driven Development (SDD).
- A solução não deve ser considerada adequada para uso em produção.
- A criptografia de senhas não é um requisito obrigatório deste desafio educacional. Essa ausência de exigência não deve ser interpretada como uma proibição geral de implementar criptografia.
- Funcionalidades, dependências ou mecanismos adicionais que não sejam necessários para atender ao desafio devem ser justificados tecnicamente e aprovados antes de sua implementação.
