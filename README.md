# Demo Exchange

Projeto destinado a criar um serviço para expor endpoints e realizar cotações de moeda estrangeira para o Real, contemplando os segmentos Varejo, Private e Personnalite.

## Imagens Docker
Para o projeto foram usadas as seguintes imagens:

Para o banco de dados, foi utilizado o MariaDB

`docker build -f sql/Dockerfile -t demo-exchange-db . `

O comando acima irá criar uma imagem com o banco de dados da aplicação criado

`docker run --name mariadb -e MYSQL_ROOT_PASSWORD=admin -d -p 3306:3306 demo-exchange-db`

**Comando acima irá expor a porta 3306 para utilizar instâncias do MariaDB**

**Recomendado utilizar o client DBeaver para administração do banco de dados https://dbeaver.io/**

Para o mecanismo de cache, foi utilizado Memcached

`docker run -p 11211:11211 -d memcached`

## Rodar as aplicaçãos com Docker Compose
A solução conta com a opção de rodar todos os serviços utilizando docker-compose:

Para iniciar todos os serviços você deve executar os seguintes comandos na raiz do projeto /demo-exchange: 

`docker-compose build --no-cache --force-rm --pull`

Comando que irá realizar o pull de todas as imagens relacionados ao serviço.

`docker-compose up -d`

Comando que irá iniciar todos os serviços com as mesmas portas do comandos para o docker descritos acima.
Obs: Você pode executar apenas esse comando, que o mesmo também irá fazer o pull das imagens caso não sejam localizadas na máquina.


## Swagger
O serviço contem a documentação baseada no Swagger, estrutura de software de código aberto que ajuda os desenvolvedores a projetar, criar, documentar e consumir serviços da Web RESTfu

**Url Swagger: http://localhost:5000/swagger/index.html**

## Packages
A aplicação faz uso dos seguintes packages (nuget):

**https://www.nuget.org/packages/EnyimMemcachedCore/ : Bliblioteca que abstrai utilização do Memcached**

**https://www.nuget.org/packages/MySqlConnector/ : Bliblioteca que abstrai para utilização do MariDB**
