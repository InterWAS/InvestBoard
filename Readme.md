<div align="center" style="display:grid;place-items:center;">
<h1>InvestBoard</h1>

<h4>Painel de Investimentos com Perfil de Risco Dinâmico</h4>

</div>

## Objetivo

API que analisa o comportamento financeiro do cliente e ajusta automaticamente seu perfil de risco, sugerindo produtos de investimento como CDBs, LCIs, LCAs, Tesouro Direto, Fundos, etc.

## Características

- Recebe um envelope JSON, via chamada à API, contendo solicitação de simulação de investimentos
- Consulta um conjunto de informações parametrizadas em uma tabela de banco de dados SQLite
- Valida os dados de entrada da API com base nos parâmetros de produtos retornados no banco de dados
- Filtra qual produto se adequa aos parâmetros de entrada
- Realiza cálculos para as simulações de cada tipo de investimento
- Retorna um envelope JSON contendo o nome do produto validado, e o resultado da simulação
- Persiste em banco local a simulação realizada
- Endpoint para retornar todas as simulações realizadas.
- Endpoint para retornar os valores simulados para cada produto em cada dia.
- Endpoint para retornar dados de telemetria com volumes e tempos de resposta para cada serviço.
- Motor de Recomendação utilizando algoritmo simples baseado em volume de investimentos, frequência de movimentações, preferência por liquidez ou rentabilidade e usando  pontuação para definir perfil (Conservador, Moderado e Agressivo)
- Autenticação em Keycloak utilizando OAuth2 e JWT 
- Execução via container
- Testes unitários e integração

## Instalação

- Requer .NET 10

