# Brass Loon Data Client

Brass Loon Data Client is a database interface utility.  Its primary function is de-serializing data sets into object data models.  Secondarily, it exposes database transaction access.  Use this access to add database requests in a transaction and control commits.  Version 1.0 only supports Microsoft SQL Server connections.

The component targets .Net Standard.

## Usage

### ISettings Interface

Everything the data client does relies on the ISettings interface.  Users must implement this interface so the data client can get a database connection string.

Remember, resolved/completed tasks can be created.

```
using System.Threading.Tasks;
⋮
return Task.FromResult<string>(this.ConnectionString)
```

Use the ISqlSettings interface when connecting to Azure SQL Server using access tokens.

### DbProviderFactory Class

The DbProviderFactory is a core component used to open connections and establish transactions.  It also includes a method to create IDataParameter instances.  I recommend using the IDbProviderFactory interface.

As of version 1.0, DbProvider only supports Microsoft SQL Server.  

When using a dependency injection container, register DbProviderFactory as IDbProviderFactory.

### GenericDataFactory Class

Use the GenericDataFactory class to de-serialize data into model objects.  Apply column mapping attributes to model class properties to direct the de-serialization process.

```
Task<IEnumerable<T>> GetData(ISettings settings, IDbProviderFactory providerFactory, string commandText, Func<T> createModelObject, IEnumerable<IDataParameter> parameters, CommandType commandType);
```
+ Generic type parameter T specifies the model class.
+ ISettings is used to get a connection string.
+ IDbProviderFactory is used to open a database connection.
+ commandText contains the SQL to execute.  When running stored procedures, this is the name of the stored procedure.
+ createModelObject is used to instantiate the model object.  This can be a simple lambda like "() => new Person()".
+ parameters is a collection of parameters to send with the database request.
+ commandType indicates whether the command is calling a stored procedure by name or is an SQL statement to be executed.  Defaults to stored procedure.

### Azure SQL Server

The data client does support Azure SQL server authenticating with access tokens.  In this case, use the ISqlSettings, ISqlTransactionHandler, and SqlClientProviderFactory variants.  The ISqlSettings interface includes a "GetAccessToken" method.  Users need to implement this method and return the token required for authentication.

The "GetAccessToken" method is where you might do something like the following.

```
using Azure.Core;
using Azure.Identity;
⋮
TokenRequestContext context = new TokenRequestContext(
    new[] { "https://database.windows.net//.default" }
    );
AccessToken token = await new DefaultAzureCredential()
    .GetTokenAsync(context);
``` 

## License

GNU General Public License 3