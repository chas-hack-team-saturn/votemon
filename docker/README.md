# helpful docker commands

## Down project (removes database)
```sh
docker compose down --volumes
```

## Up project with rebuild
```sh
docker compose up --detach --build --force-recreate
```
