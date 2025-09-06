🎬 Filmes API
Uma API REST construída em ASP.NET Core + SQLite para gerenciar filmes, gêneros, classificações e sinopses. Totalmente containerizada com Docker e pronta para rodar em qualquer ambiente!

🚀 Funcionalidades
- CRUD de Filmes
- CRUD de Gêneros
- Documentação interativa com Swagger
- Banco de dados SQLite persistente
- Docker-ready para rodar em qualquer máquina

🧰 Pré-requisitos
- Docker instalado Download

📦 Como executar com Docker

1. Clone o repositório
git clone 

2. Build da imagem
docker build -t filmes-api .

3. Execute o container com Swagger ativado
´´´
   docker run -d -p 5000:5000 \
  --name filmes-api-container \
  -e ASPNETCORE_ENVIRONMENT=Development \
  filmes-api
´´´ 

📄 Acessar a API
Abra o navegador em:
http://localhost:5000/swagger/index.html

![Swagger funcionando](https://github.com/monica88lima/Estudos-Dev/blob/main/docker/API.png)


🗃️ Banco de dados SQLite
- O banco é criado automaticamente na imagem (filmes.db)
- Migrations são aplicadas no iniciar da aplicação
