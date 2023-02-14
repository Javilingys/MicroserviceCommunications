# Catalog Service

### This service uses:

* Simple **one** layer (without repositories and services. All logic inside controller for simplisity)
* Simple **Anemic** models.
* **PostgreSQL** - Database
* **EF Core** such an ORM
* **AutoMapper** for map from Entity to DTO

## TODO:

- [x] Map to DTO
- [ ] Post Method (Create Product) 
- [ ] desctributed redis cache
- [ ] rabbitmq + masstransit on create product
- [ ] kaffka + masstransit on create product
- [ ] rabbitmq + masstransit on update product
- [ ] kaffka + masstransit on update product
- [ ] rest controller
- [ ] grpc
- [x] polly
- [ ] Health Checks
- [ ] ...