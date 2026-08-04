```mermaid
flowchart LR

LoginRequest

LoginRequest --> AuthenticationProviderFactory

AuthenticationProviderFactory --> MockAuthenticationProvider

MockAuthenticationProvider --> PKCEValidation

PKCEValidation --> ClaimsFactory

ClaimsFactory --> JwtService

JwtService --> AuthenticationResponse
```
