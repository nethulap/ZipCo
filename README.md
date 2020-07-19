# ZipCo

To run this project locally using .Net Core SDK Image and SQL Server on Linux Image Then download Docker Engine and Docker Compose on your OS.


## ZipPayApi

Perform the following things to run this api on localhost:8080
```bash
1. dotnet restore
2. dotnet build
3. docker-compose build
4. docker-compose up
```

Once everything is build and compile you can access the api at http://localhost:8080. 
for eg: to get list of users http://localhost:8080/api/user

### Api urls

1. To get List of users GET
```bash
   http://localhost:8080/api/user
```
2. To get a specific user GET 
```bash
   http://localhost:8080/api/user/1
```
3. To Create new user POST 
```bash
   http://localhost:8080/api/user
```
4. To get List of accounts GET 
```bash
   http://localhost:8080/api/account
```
5. To get specific account GET 
```bash
   http://localhost:8080/api/account/1
```
6. To Create new account POST 
```bash
   http://localhost:8080/api/account
```

## ZipPayApi.Tests
To run tests
```bash
dotnet test
```