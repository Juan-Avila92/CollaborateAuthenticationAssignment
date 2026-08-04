```mermaid
flowchart LR

AuthenticationController

AuthenticationController --> AuthenticationService

AuthenticationService --> AuthenticationProviderFactory

AuthenticationProviderFactory --> IAuthenticationProvider

IAuthenticationProvider --> MockAuthenticationProvider

AuthenticationService --> IPkceService

AuthenticationService --> IClaimsFactory

AuthenticationService --> IJwtService

AuthenticationService --> IApplicationUserRepository

AuthenticationService --> IAuthenticationProviderRepository
```
