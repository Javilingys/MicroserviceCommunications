﻿# Catalog Service

### This service uses:

* Simple **one** layer (without repositories and services. All logic inside controller for simplisity)
* Simple **Anemic** models.
* **PostgreSQL** - Database
* **EF Core** such an ORM
* **AutoMapper** for map from Entity to DTO


## TODO:

- [ ] desctributed redis cache
- [ ] rabbitmq + masstransit while update product
- [ ] kaffka + masstransit while update product
- [ ] rest controller
- [ ] grpc
- [ ] polly
- [ ] ...