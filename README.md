# EaSQL

EaSQL (/ˈiː.siːkwəl/) provides some quality of life functions that will make a developer's life easier when working with SQL queries.

It allows to perform SQL queries like this:

```
using IDbConnection dbConnection = /* obtain database connection */
int id = 42;

using IDataReader reader = dbConnection.RunQuery($"select * from users where user_id = {id}");
```

There are different extension methods for different scenarios:
The created command instance will contain the query string with all used parameters from the string as SQL parameters.

- `RunQuery`: allows to run a query against the database
- `RunCommand`: allows to run a single command (e.g. stored procedure call, update) against the database
- `GetSingleValue`: allows to run a query against the database that expects to return only a single result (e.g. count queries)

It also allows to define mappings between a data reader to a model type:

```
public class User
{
	public int Id { get; set; }
	public string Name { get; set; }
}

Mapper<User> mapper = new()
	.DefineMapping(u => u.Id, "id")
	.DefineMapping(u => u.Name, "user_name");

IDataReader reader = // perform database query

User user = mapper.ApplyMapping(new(), reader);
```