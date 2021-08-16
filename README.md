# .Net Core web api project created for technical evaluation at Virtualmind.

## Stack:

* .Net Core (5.0)
* Repository Pattern
* Automapper
* ILoger (Nlog)
* HttpClient factory
* Http Retry-Policy (Polly extension)
* SQL Server
* Swagger
* Fluent validations
* Separated into 2 microservices (ExchangeService and RateService)
  An api gateway like Ocelot can be added to handle both microservice's endpoints
* Health checks
* Unit testing with xUnit (Unit testing and integration testing)

# About using the user id:
I think adding the user id to the input model when making a currency purchase is ok for example when our service is being used by an external operator like an account manager, not for the user itself.

# Security improvements
Also, i would improve the security of the exchange endpoint by adding JWT authentication, this way our service will be used only by authenticated users or programs.
