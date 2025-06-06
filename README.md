# EaSQL

EaSQL (/ˈiː.siːkwəl/) provides some quality of life functions that will make a developer's life easier when working with SQL queries.

It allows to perform SQL queries like this:

```
IDbConnection dbConnection = /* obtain database connection */
SqlQueryHandler queryHandler = new(dbConnection);

int id = 42;

IDbCommand command = queryHandler.CreateCommand($"select * from users where user_id = {id}");
```

The created command instance will contain the query string with all used parameters from the string as SQL parameters.

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