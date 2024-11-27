## Pre-Condition: Download and Install .NET 8 SDK  
You can download and install the .NET 8 SDK from the following link:  
[Download .NET 8 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)

## (API project)
### Step 1: Install packages  
Open your terminal and navigate to your project directory. Run the following commands to install the necessary packages:

1. **Microsoft.AspNetCore.OpenApi**
   ```bash
   dotnet add package Microsoft.AspNetCore.OpenApi --version 8.0.10
   ```

2. **Swashbuckle.AspNetCore**
   ```bash
   dotnet add package Swashbuckle.AspNetCore --version 6.6.2
   ```

3. **Entity Framework Core SQL Server 8.0.0**:
   ```bash
   dotnet add package Microsoft.EntityFrameworkCore.SqlServer --version 8.0.0
   ```

4. **Entity Framework Core Tools 8.0.0**:
   ```bash
   dotnet add package Microsoft.EntityFrameworkCore.Tools --version 8.0.0
   ```

5. **IdentityServer4.AspNetIdentity**:
   ```bash
   dotnet add package IdentityServer4.AspNetIdentity --version 4.0.0
   ```

6. **Microsoft.AspNetCore.Identity.EntityFrameworkCore**:
   ```bash
   dotnet add package Microsoft.AspNetCore.Identity.EntityFrameworkCore --version 8.0.0
   ```   

7. **Microsoft.AspNetCore.Authentication.JwtBearer**:
   ```bash
   dotnet add package Microsoft.AspNetCore.Authentication.JwtBearer --version 8.0.0
   ```  

8. **System.IdentityModel.Tokens.Jwt**:
   ```bash
   dotnet add package System.IdentityModel.Tokens.Jwt --version 8.0.0
   ```      

### Step 2: Update the Database Connection String  
To configure the database connection, open the `appsettings.json` file and update the connection string to align with your SQL Server instance. 

#### Connection String Explanation:
The connection string format consists of key-value pairs that define how your application connects to the database. Here’s an example connection string and a breakdown of its components:

```json
"ConnectionStrings": {
    "DefaultConnection": "Server=VNNOT0123\\MSSQLSERVER0123;Database=EcommerceDB;Trusted_Connection=True;TrustServerCertificate=True"
}
```

- **Server**: This specifies the SQL Server instance to connect to. In this example, `VNNOT0123\\MSSQLSERVER0123` refers to the server name and instance name.
- **Database**: This specifies the name of the database to use, which in this case is `EcommerceDB`.
- **Trusted_Connection**: Setting this to `True` enables Windows Authentication. If you use SQL Server Authentication, you need to provide a username and password instead.
- **TrustServerCertificate**: This setting is used to indicate whether the application should trust the server's SSL certificate. Set it to `True` if you want to ignore SSL certificate errors (generally not recommended for production environments).

### Step 3: Add class library
  ```bash
dotnet add reference ../../ClassLib/EcommerceLib/EcommerceLib.csproj
  ```

### Step 4: Build & Run the Project  
To start the application, use one of the following commands in your terminal:

- **Build the project**:
  ```bash
  dotnet build
  ```

- **Run the project**:
  ```bash
  dotnet run
  ```

  - **Run the project in watch mode & get Swagger API** (automatically restarts on file changes):
  ```bash
  dotnet watch run
  ```

Once the project is running, you can access it in your web browser at `https://localhost:<port>/`.

## (EcomerceWebMVC project)
### Step 1: Add class library
Open your terminal and navigate to your project directory. Run the following commands to add necessary class library:
  ```bash
dotnet add reference ../ClassLib/EcommerceLib/EcommerceLib.csproj
  ```

### Step 2: Build & Run the Project  
To start the application, use one of the following commands in your terminal:

- **Build the project**:
  ```bash
  dotnet build
  ```

- **Run the project**:
  ```bash
  dotnet run
  ```

- **Run the project in watch mode** (automatically restarts on file changes):
  ```bash
  dotnet watch run
  ```

Once the project is running, you can access it in your web browser at `https://localhost:<port>/`.

## (ecommerce_reactjs_admin project)
### Step 1: Install packages
Open your terminal and navigate to your project directory. Run the following commands to install the necessary packages:
  ```bash
npm install axios react-router-dom react-bootstrap bootstrap 
  ```

### Step 2: Build & Run the Project  
To start the application, use one of the following commands in your terminal:

- **Build the project**:
  ```bash
  npm start
  ```
Once the project is running, you can access it in your web browser at `https://localhost:<port>/`.

## (Sample API <API project>, UI <EcommerceWebMVC & ecommerce_reactjs_admin project>)
**Swagger API:**

<img width="583" alt="image" src="https://github.com/user-attachments/assets/9f650ae4-1adf-43a3-a0b1-76e343496d9a">

**Ecommerce - ASP.NET Core MVC:**

<img width="942" alt="image" src="https://github.com/user-attachments/assets/4da34ff9-0310-4f87-bda4-cb39ae05c69a">

**Ecommerce - ReactJS (for Admin):**

<img width="934" alt="image" src="https://github.com/user-attachments/assets/6fa00057-121f-4126-a055-312eb5bada30">



