# Como iniciar o projeto localmente

Este documento apresenta os passos necessários para baixar o projeto do GitHub, configurar o ambiente local, preparar o banco de dados PostgreSQL e executar a API.

> Atenção: este projeto utiliza uma string de conexão com o PostgreSQL. A senha utilizada no seu computador deve permanecer somente no seu ambiente local e não deve ser enviada para o GitHub.

---

## 1. Pré-requisitos

Antes de iniciar, verifique se estão instalados:

- Git;
- .NET SDK 10;
- PostgreSQL;
- pgAdmin ou outra ferramenta para executar comandos SQL;
- Visual Studio Code;
- extensão C# Dev Kit;
- Postman, para testar as rotas da API.

Para verificar o .NET:

```bash
dotnet --version
```

O resultado deverá iniciar com:

```text
10.0
```

Para verificar o Git:

```bash
git --version
```

---

## 2. Clonar o projeto

Abra o terminal no diretório em que deseja armazenar o projeto e execute:

```bash
git clone URL_DO_REPOSITORIO
```

Exemplo:

```bash
git clone https://github.com/usuario/repositorio.git
```

Depois, entre na pasta do projeto:

```bash
cd LojaApi
```

Abra o projeto no Visual Studio Code:

```bash
code .
```

---

## 3. Restaurar as dependências do projeto

Como o projeto foi obtido pelo GitHub, não é necessário instalar manualmente cada biblioteca que já estiver registrada no arquivo `.csproj`.

Execute:

```bash
dotnet restore
```

Esse comando lê o arquivo do projeto e baixa automaticamente os pacotes necessários.

Neste projeto, uma das principais dependências é o `Npgsql`, utilizado para realizar a comunicação entre o C# e o PostgreSQL.

Para conferir os pacotes instalados:

```bash
dotnet package list
```

> Se o `Npgsql` já estiver registrado no arquivo `.csproj`, não execute novamente `dotnet package add Npgsql`. O `dotnet restore` é suficiente para recuperar a dependência.

---

## 4. Criar o banco de dados

O projeto utiliza o banco:

```text
loja_info
```

No PostgreSQL, crie um banco de dados com esse nome.

No pgAdmin:

1. conecte-se ao servidor PostgreSQL;
2. clique com o botão direito em `Databases`;
3. escolha `Create`;
4. escolha `Database`;
5. informe o nome:

```text
loja_info
```

6. salve.

---

## 5. Executar o script inicial do banco

Dentro do projeto existe o arquivo:

```text
Database/init.sql
```

Abra esse arquivo e execute seu conteúdo no banco `loja_info`.

O script cria e popula inicialmente as tabelas:

```text
produto
venda
item_venda
```

> Atenção: o script possui comandos `DROP TABLE IF EXISTS`. Portanto, ao executá-lo novamente, as tabelas existentes serão removidas e recriadas.

---

## 6. Configurar a string de conexão

A aplicação precisa saber como acessar o PostgreSQL instalado no seu computador.

Crie ou configure o arquivo:

```text
appsettings.Development.json
```

Exemplo:

```json
{
  "ConnectionStrings": {
    "Postgres": "Host=localhost;Port=5432;Database=loja_info;Username=postgres;Password=SUA_SENHA"
  }
}
```

Substitua:

```text
SUA_SENHA
```

pela senha do PostgreSQL do seu computador.

Exemplo:

```json
{
  "ConnectionStrings": {
    "Postgres": "Host=localhost;Port=5432;Database=loja_info;Username=postgres;Password=admin"
  }
}
```

### Atenção com dados sensíveis

Nunca envie para o GitHub:

- senhas;
- tokens;
- chaves de acesso;
- strings de conexão com dados reais;
- qualquer outra credencial.

O arquivo `appsettings.Development.json` deve permanecer apenas no ambiente local quando possuir informações sensíveis.

Verifique se o `.gitignore` contém:

```gitignore
appsettings.Development.json
```

O professor pode disponibilizar uma string de conexão apenas como referência didática. Cada desenvolvedor deverá configurar sua própria senha local.

---

## 7. Compilar o projeto

Antes de executar a API, compile o projeto:

```bash
dotnet build
```

Se tudo estiver correto, o terminal deverá informar que a compilação foi concluída com sucesso.

---

## 8. Executar a API

Execute:

```bash
dotnet run
```

O projeto está configurado para utilizar:

```text
http://localhost:5050
```

O terminal deverá apresentar algo semelhante a:

```text
Now listening on: http://localhost:5050
```

Mantenha esse terminal aberto enquanto estiver utilizando a API.

---

## 9. Testar a conexão com o banco

Com a API em execução, abra o Postman e envie:

```http
GET http://localhost:5050/api/conexao/testar
```

Resposta esperada:

```json
{
  "mensagem": "Conexão realizada com sucesso."
}
```

Se ocorrer erro, confira principalmente:

- se o PostgreSQL está iniciado;
- se o banco `loja_info` existe;
- se o script `init.sql` foi executado;
- se o usuário PostgreSQL está correto;
- se a senha está correta;
- se a porta do PostgreSQL é `5432`;
- se o arquivo `appsettings.Development.json` foi criado corretamente.

---

## 10. Testar a listagem de vendas

No Postman:

```http
GET http://localhost:5050/api/vendas
```

Resposta esperada:

```text
200 OK
```

com uma lista JSON contendo as vendas cadastradas.

---

## 11. Testar a listagem de produtos

Caso a rota de produtos já esteja implementada:

```http
GET http://localhost:5050/api/produtos
```

Resposta esperada:

```text
200 OK
```

com os produtos cadastrados no banco.

---

## Resumo dos comandos

```bash
git clone URL_DO_REPOSITORIO
cd LojaApi
dotnet restore
dotnet build
dotnet run
```

O fluxo completo para preparar o projeto é:

```text
GitHub
   ↓
git clone
   ↓
dotnet restore
   ↓
Criar banco loja_info
   ↓
Executar Database/init.sql
   ↓
Configurar appsettings.Development.json
   ↓
dotnet build
   ↓
dotnet run
   ↓
Testar no Postman
```

## Problemas comuns

### Erro informando que a conexão Postgres não foi configurada

Verifique se existe a seção:

```json
"ConnectionStrings": {
  "Postgres": "..."
}
```

e se o arquivo está sendo carregado no ambiente `Development`.

### Erro de autenticação no PostgreSQL

Normalmente ocorre quando o usuário ou a senha da string de conexão estão incorretos.

### Erro informando que o banco não existe

Confirme se foi criado:

```text
loja_info
```

### Erro indicando que uma tabela não existe

Execute novamente o arquivo:

```text
Database/init.sql
```

no banco correto.

### A API inicia, mas não responde em `localhost:5050`

Confira o arquivo:

```text
Properties/launchSettings.json
```

e confirme a porta configurada para o projeto.
