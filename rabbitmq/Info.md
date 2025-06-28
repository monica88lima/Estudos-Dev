🧪 Guia: Criar e Testar um Worker Service com .NET 8 e RabbitMQ
📦 1. Criar o projeto Worker
Crie um projeto do tipo Worker para rodar um serviço em segundo plano de forma contínua.
- Para que serve: executa tarefas automaticamente, como consumir uma fila, sem interface visual.
- Vai conter: a lógica de consumo da fila dentro do método ExecuteAsync.

🧰 2. Adicionar o pacote RabbitMQ.Client
Adicione a biblioteca cliente do RabbitMQ via NuGet.
- Por que: essa biblioteca permite se conectar, declarar filas, consumir e publicar mensagens.
- Vai conter: todas as interfaces e classes necessárias para configurar o canal de comunicação com o RabbitMQ.

⚙️ 3. Configurar a conexão no appsettings.json
Inclua informações como host, porta, login e nome da fila.
- Por que: centraliza os dados da configuração, facilita manutenção e evita hardcoding.
- Vai conter: seções de configuração como Host, Port, User, Password, e QueueName.

🔌 4. Criar uma classe responsável por consumir mensagens
Essa classe deve:
- Criar uma conexão com o RabbitMQ.
- Abrir um canal de comunicação.
- Declarar a fila (caso ela ainda não exista).
- Configurar um consumidor para escutar mensagens da fila.
- Processar as mensagens recebidas (ex: logar, salvar no banco, acionar outro serviço).
- Por que: separa a responsabilidade de leitura da fila, facilita testes e organização.
- Vai conter: lógica de conexão, canal (channel), e evento que reage às mensagens recebidas.

🎯 5. Configurar a execução no serviço principal (Worker)
Essa parte é onde o Worker chama o consumidor e mantém o serviço ativo.
- Por que: é o ponto de entrada da execução contínua.
- Vai conter: chamada ao método que inicia o consumo e monitora o cancelamento da aplicação.

🧱 6. Registrar os serviços na aplicação (Program.cs)
Adicione os serviços ao injetor de dependência (Dependency Injection).
- Por que: garante que o consumidor possa ser instanciado e usado corretamente pelo worker.
- Vai conter: configuração do host, e inclusão das classes de serviço.

🚀 7. Testar a aplicação
- Certifique-se de que o RabbitMQ está rodando (local ou em container).
- Acesse o painel (geralmente em http://localhost:15672) e crie a fila com o nome definido.
- Publique uma mensagem de teste na fila.
- Rode o Worker em modo debug e observe se a mensagem.
- Por que: valida se a integração está funcionando do começo ao fim.

