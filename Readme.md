## Green Maps
- Siga os passos para o build do projeto: (VS Code)
1. Faça o clone do projeto
2. Instale o SDK .NET 5 na sua máquina (se já estiver instalado pule este passo)
3. Instale a ferramenta EF com o .NET CLI (se já estiver instalado pule este passo)
5. Abra o arquivo appsettings.json e altere a "DefaultConnection" com a sua string de conexão de um banco SQL Server
6. Rode o commando <code>dotnet ef database update</code>
7. Digite o comando <code>dotnet restore</code>
8. Digite o comando <code>dotnet build</code>
9. Rode o projeto <code>dotnet watch run</code>