# dotnet-login-lista-tarefas
- Usado .Net8, Entity Framework  com SQLite
  
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
