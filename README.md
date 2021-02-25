### Project
- **Desenvolvido por:** Fernando José
- **Descrição:** Chama's test

### Patters and Technologies used
#### BackEnd
- Net Core 3.1
- C#
- DDD
- CQRS
- MediatR
- MediatR Pipeline (ValidatorPipeline)
- API
- Swagger
- Dapper
- UnitOfWork
- IoC
- Redis for Cache
- Service Bus 
- Azure Function with triggers to service bus queue
- SendGrid for e-mail
- SqlServer
- ExceptionMiddleware: Responsible for catching and handling all solution's errors

#### Unit Test
- XUnit
- Moq
- Bogus (faker)

#### Database and Cache
- SQL Server
- Redis (cache)

### Configuration

#### Prerequisites
- Net Core 3.1

### ToDo
Improvements to be made

#### BackEnd
- Separate the databases. One for Commands (SqlServer) and one for Queries (Cosmos)
- Create CircuitBreaker policy
- Create HealthCheck
- Create retry policy with Polly
- Create separate solutions to have unique responsibilities. One solution for the API and one for each Worker, as well as the entire project structure, think about microservices.
- Create an error log, can be the serilog or think of a strategy with kibana