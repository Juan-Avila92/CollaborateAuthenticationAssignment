```mermaid
flowchart LR

JWT

JWT --> ValidateToken

ValidateToken --> ClaimsPrincipal

ClaimsPrincipal --> AuthorizationService

AuthorizationService --> UserRepository

AuthorizationService --> RoleRepository

RoleRepository --> PermissionRepository

AuthorizationService --> AuthorizationInfoResponse
```
