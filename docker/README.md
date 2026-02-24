# setup .env
make sure there is a .env file at the root of the repo with MARIADB_PASSWORD

example .env content:
```ini
MARIADB_PASSWORD=asdf
```

oneline command:
```sh
echo "MARIADB_PASSWORD=asdf" > .env
```

# helpful docker commands

## Down project (removes database)
```sh
docker compose down --volumes
```

## Up project with rebuild
```sh
docker compose up --detach --build --force-recreate
```
