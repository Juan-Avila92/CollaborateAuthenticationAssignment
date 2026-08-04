The API is running on Azure App Service under this URL:

https://collaborateapi20260804132553-bnbybxgma6a7h8he.canadacentral-01.azurewebsites.net/swagger/index.html

You will see some endpoints to interact with.

The first one would be the Authentication Login which is a Post method. In the body you would need to add the tenanatId and email data:

```
Below you can see the JSON data you can use as body payloads.

  {
    "tenantId": "af67d479-c4b0-41d0-a4f5-d61603051f38",
    "email": "emily.davis@fabrikam.com"
  }

  {
    "tenantId": "1f45677f-29c5-4b35-892b-129c83b93b01",
    "email": "sarah.jones@caseware.com"
  }
```
Example:

<img width="1455" height="654" alt="image" src="https://github.com/user-attachments/assets/ca24728e-553f-4341-a9a5-742693ded969" />

RESPONSE:
```
{
  "succeeded": true,
  "message": "Authentication successful.",
  "userId": "15e5c989-83ad-4e57-8521-46603d54cd34",
  "tenantId": "af67d479-c4b0-41d0-a4f5-d61603051f38",
  "displayName": "Emily Davis",
  "email": "emily.davis@fabrikam.com",
  "userType": "External Client",
  "accessToken": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiIxNWU1Yzk4OS04M2FkLTRlNTctODUyMS00NjYwM2Q1NGNkMzQiLCJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9lbWFpbGFkZHJlc3MiOiJlbWlseS5kYXZpc0BmYWJyaWthbS5jb20iLCJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1lIjoiRW1pbHkgRGF2aXMiLCJodHRwOi8vc2NoZW1hcy5taWNyb3NvZnQuY29tL3dzLzIwMDgvMDYvaWRlbnRpdHkvY2xhaW1zL3JvbGUiOiJWaWV3ZXIiLCJwZXJtaXNzaW9uIjoiVmlld1JlcG9ydHMiLCJleHAiOjE3ODU4NzQzNDMsImlzcyI6IkNhc2V3YXJlLkNvbGxhYm9yYXRlIiwiYXVkIjoiQ29sbGFib3JhdGUuQXBpIn0.xxUtNxMwp5KHG5z4ej2GiDToIhJrLIqXhoEBcBAzgiQ",
  "expiresAtUtc": "2026-08-04T20:12:23.7549789Z"
}
```

The whole idea of this excercise is that the Authentication and Authorization act as orchestrator consumeing different IdPs. The access token have got different Claims information (such as roles, permission etc) that will be use along the application while the application user is logged in.
Once we have our access token, it means that the user is logged in.

______________________________________________________________________________________

Authorization

Now we have the current user access token and we are going to call the Authorization endpoint to know which roles and permissions the user has.

```
{
  "userId": "00000000-0000-0000-0000-000000000000",
  "tenantId": "00000000-0000-0000-0000-000000000000",
  "email": "emily.davis@fabrikam.com",
  "displayName": "Emily Davis",
  "roles": [
    "Viewer"
  ],
  "permissions": [
    "ViewReports"
  ]
}
```


