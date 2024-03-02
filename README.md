# dotnet-login-lista-tarefas
- Usado .Net8, Identity Core e Entity Framework com SQLite
  
  Video no Youtube de demonstração: https://www.youtube.com/watch?v=MbVEaN_xoQU

# Observação para uso:
- Esse projeto foi feito com .Net8, para usa-lo é necessário ter instalado SDK .Net8

- O arquivo que esta na raiz "localDB.db" é o banco de dados SQLite sem dados.
  Mas se precisar criar novamente:
  "dotnet ef database update"
  É necessário que o ef esteja instalado no seu computador.

- Com o arquivo "localDB.db" existente, para rodar o projeto:
  "dotnet watch"

# Package usados:
- ORM Entity:
  
  Microsoft.EntityFrameworkCore
  Microsoft.EntityFrameworkCore.Design

- ORM Entity para SQLite:
  
  Microsoft.EntityFrameworkCore.Sqlite

- Login Jwt Bearer:
  
  Microsoft.AspNetCore.Authentication.JwtBearer

- Identy criação e gerencimaneto de senha:
  
  Microsoft.Extensions.Identity.Core

# Arquitetura de software usada:

A arquitetura de software usada nesse projeto é chamado arquitetura de software em camadas, com a utilização dos padrões Repository e Unit of Work. Esses padrões são frequentemente empregados em conjunto com o ASP.NET Core para promover a separação de preocupações e a modularidade do código.

Aqui está uma breve explicação de cada componente:

Interface: Normalmente, as interfaces são usadas para definir contratos entre diferentes partes do sistema. No contexto do ASP.NET Core, elas podem ser utilizadas para definir contratos para os repositórios, definindo métodos que serão implementados pelas classes concretas.

Repositórios (Repository): O padrão Repository é utilizado para isolar a lógica de acesso a dados do resto da aplicação. Ele fornece uma camada de abstração sobre o acesso a dados, permitindo que as operações de leitura e escrita sejam realizadas sem que o restante da aplicação precise conhecer os detalhes específicos do mecanismo de armazenamento de dados (por exemplo, banco de dados).

Controladores (Controllers): No contexto do ASP.NET Core, os controladores são responsáveis por receber as requisições HTTP, processá-las e retornar as respostas apropriadas. Eles atuam como intermediários entre as solicitações dos clientes e a lógica de negócios da aplicação.

Unit of Work: O padrão Unit of Work é utilizado para coordenar transações e operações relacionadas a múltiplos repositórios. Ele agrupa as operações em uma única unidade lógica, garantindo que todas as operações sejam executadas de forma consistente e que as transações sejam tratadas de maneira adequada.

Essa arquitetura, que utiliza interfaces, repositórios, controladores e o padrão Unit of Work, promove uma separação clara de responsabilidades e facilita a manutenção e evolução do sistema ao longo do tempo. Ela é amplamente adotada em projetos ASP.NET Core e é conhecida por sua capacidade de promover a modularidade, testabilidade e escalabilidade do código.
