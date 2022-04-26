<Query Kind="Expression">
  <Connection>
    <ID>15d71fa3-87a3-45ce-b63f-7cc8caac7b0a</ID>
    <NamingServiceVersion>2</NamingServiceVersion>
    <Persist>true</Persist>
    <Server>.\SQLEXPRESS</Server>
    <Database>LoggingDatabase</Database>
  </Connection>
</Query>

Logs
	.Where(x => x.Message.Contains("Application started"))
	.Select(x => new { x.Timestamp, x.ApplicationName, x.Message })
	.OrderByDescending (l => l.Timestamp)
	.Take (100)