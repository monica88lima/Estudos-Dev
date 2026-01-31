# Lambda Function - Processamento de Eventos do SQS para SNS

## Sobre o Projeto

Esta Lambda faz parte de um fluxo de processamento de eventos. Ela é responsável por:

1. Receber eventos de um tópico do Amazon SQS.
2. Processar os dados recebidos, incluindo a normalização de datas e horas.
3. Publicar os dados processados em um tópico do Amazon SNS.

Essa funcionalidade é útil para integrar sistemas que utilizam o SQS como fila de mensagens e o SNS para notificação de eventos processados.

---

## Como Executar a Lambda

1. **Pré-requisitos**:
   - Python 3.10 ou superior instalado.
   - Instalar as dependências do projeto. Execute o comando:
     ```bash
     pip install -r requirements.txt
     ```

2. **Executar a Lambda localmente**:
   - O arquivo `event_mock.json` tem exemplo do que deve ser processado pela Lambda. Exemplo de conteúdo:
     ```json
     {
       "Records": [
         {
           "body": "{\"identificado\": \"123\", \"codigo\": \"BLOQ01\", \"databloqueio\": \"2023-10-01T15:40:00\", \"data_criacao\": \"2026-01-30T13:00:00\", \"hora_bloqueio\": \"15:40:00\"}"
         }
       ]
     }
     ```
   - Execute o seguinte comando para rodar a Lambda localmente:
     ```bash
     python lambda_function.py
     ```

---

## Como Executar os Testes

1. **Pré-requisitos**:
   - Certifique-se de que as dependências estão instaladas:
     ```bash
     pip install -r requirements.txt
     ```

2. **Executar os testes**:
   - Navegue até o diretório `tests`:
     ```bash
     cd tests
     ```
   - Execute os testes com o comando:
     ```bash
     pytest --gherkin-terminal-reporter
     ```

---

## Como Debugar o Projeto da Lambda

1. **Configurar o ambiente de desenvolvimento**:
   - Certifique-se de que o Python está configurado corretamente no seu editor de código (ex.: VS Code).
   - Instale as extensões necessárias, como a extensão de Python para o VS Code.


2. **Adicionar pontos de interrupção (breakpoints)**:
   - No arquivo `lambda_function.py`, adicione breakpoints clicando na lateral esquerda do editor de código (no VS Code).

3. **Executar o depurador**:
   - No VS Code, pressione `F5` para iniciar o depurador.
   - Certifique-se de que o arquivo `lambda_function.py` está configurado como o ponto de entrada no arquivo `launch.json`.

4. **Inspecionar variáveis e fluxo**:
   - Use o painel de depuração do VS Code para inspecionar variáveis e acompanhar o fluxo de execução da Lambda.

---

## Estrutura do Projeto

```
.
├── lambda_function.py       # Código principal da Lambda
├── event_mock.json          # Exemplo de evento para testes locais
├── requirements.txt         # Dependências do projeto
├── tests/                   # Diretório de testes
│   ├── test_steps.py        # Testes BDD com pytest-bdd
│   ├── tests_lambda.py      # Testes unitários da Lambda
│   └── features/            # Cenários BDD
│       ├── bloqueio.feature # Cenário de teste para eventos de bloqueio
│       └── desbloqueio.feature # Cenário de teste para eventos de desbloqueio
```

---

## Contato
