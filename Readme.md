# API de Cliente

## Backend

- .Net CORE 5.0
- MYSQL, SQL, LOCALDB, SQLLITE, INMEMEORY, POSTGRESQL
- EF CORE
- Swagger

## Frontend

Frontend developded with angular 11

```bat
npm i -g @angular/cli
```

> [`Angular` - **Website** - https://angular.io/ ](https://angular.io/)

## Migrations

The app already comes with inital migration on Data project, but if you need to add migrations as shown below:

### Visual Studio

```
Add-Migration initial -s Api -p Data -c AppDbContext
Update-Database -s Api -p Data -c AppDbContext
```

### dotnet CLI

In case you don't have install ef tools, run on the console:

```
dotnet tool install --global dotnet-ef
```

```
dotnet ef migrations add migration_name -s Api -p Data -c AppDbContext
dotnet ef database update -s Api -p Data -c AppDbContext
```

## Databases

The app uses EntityframeworkCore as ORM, so you can choose one of those databases, fproviders

- SQL Server
- Local Db
- Sqlite
- PostgreSQL
- In Memory

You can edit the `appsettings.json` on Api project, by changing the DbType property to MYSQL, SQL, LOCALDB, SQLLITE, INMEMEORY, POSTGRESQL

and setting the related connection string

```JSON
{
  "AllowedHosts": "*",
  "AppSettings": {
    "ApiName": "API de cliente",
    "RequireHttpsMetadata": false,
    "DbType": "SQLLITE", //MYSQL, SQL, LOCALDB, SQLLITE, INMEMEORY, POSTGRESQL
    "ConnectionStrings": {
      "LocalDb": "Data Source=(localdb)\\.;Initial Catalog=ClientDb;Integrated Security=True;Trusted_Connection=True;",
      "Sql": "Server=.;Initial Catalog=ClientDb;Persist Security Info=False;User ID=sa;Password=sa;MultipleActiveResultSets=False;Connection Timeout=30;",
      "Mysql": "server=localhost,port=3306;ClientDb=jpproject;user=root;password=pa$$",
      "PostgreSql": "Server=localhost;Port=5432;Database=ClientDb;User Id=user;Password=pa$$;",
      "InMemory": "ClientDb",
      "Sqlite": "Data Source=../Data/DB/ClientDb.db"
    },
    "Cors": {
      "Origins": [ "http://localhost:4200", "http://localhost:4300" ]
    }
  }
}
```

## Run Frontend

From the folder src/Web, run;

```bat
npm i
ng serve -o --port=4200 --hmr
```

## Run Backend

From the folder src run:

```bat
dotnet build
dotnet watch run --urls=https://localhost:5001;http://localhost:5000"
```

## Unit Tests

The unit tests runs EF Core on Memory, you can find the tests on `Test` Folder

**Obs.:** The tests covers only the Repositories

# Contact

## Get in touch with [Adilson](http://adilson.almeidapedro.com.br)

- **Mobile** +55 11 9 9353-6732
- **WhatsApp** [+55 11 9 9353-6732](https://Api.whatsapp.com/send?phone=5511993536732&text=I%20want%20to%20receive%20more%20information%20about%20TUGON%20app%20model)
- **E-mail** [adilson@almeidapedro.com.br](mailto:adilson@almeidapedro.com.br)
- **Web** [devremoto.com.br](www.devremoto.com.br) / [www.tugon.com.br](www.tugon.com.br)
- **Resume** [adilson.almeidapedro.com.br](http://adilson.almeidapedro.com.br)
- **LinkedIn** [linkedin.com/in/adilsonpedro](https://linkedin.com/in/adilsonpedro)
- **Facebook** [facebook.com/DesenvolvedorRemoto](https://facebook.com/DesenvolvedorRemoto)
- **Skype** [fazsite](skype:fazsite?call)
- **Github** [github.com/devremoto](https://github.com/devremoto)
- **Twitter** [twitter.com/zumcoder](https://twitter.com/zumcoder)
