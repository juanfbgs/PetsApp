# Pets App Crud

## Usage

### Create a PostgreSQL container using Docker Compose

```yml
services:
  db:
    image: postgres:alpine
    environment:
      POSTGRES_USER: myUser
      POSTGRES_PASSWORD: mySecurePassword123
      POSTGRES_DB: Petsdb
    ports:
      - "5432:5432"
    volumes:
      # Format: [volume_name]:[path_inside_container]
      - pgdatapets:/var/lib/postgresql/data

# You must declare the named volume at the bottom of the file
volumes:
  pgdatapets:
```

```bash
docker compose up -d
```

### Clone project

```bash
git clone https://github.com/juanfbgs/PetsApp.git
cd PetsApp/PetsApp
```

### Restore dependencies

```bash
dotnet restore
```

### Development Secrets

```bash
dotnet user-secrets init

dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Host=localhost;Port=5432;Database=Petsdb;Username=myUser;Password=mySecurePassword123"
```

### Update Database

```bash
dotnet ef database update
```

### Run the application

```bash
dotnet run
```
