namespace Domain.Settings
{

    public class AppSettings
	{
		public static string SECTION = "AppSettings";
		public Cors Cors { get; set; }
		public ConnectionStrings ConnectionStrings { get; set; }

        public DbType DbType { get; set; }
        public string ApiName { get; set; }
        public bool RequireHttpsMetadata { get; set; }
    }

	public class ConnectionStrings
	{
		public string MySql { get; set; }
		public string Sql { get; set; }
		public string LocalDb { get; set; }
		public string Sqlite { get; set; }
        public string InMemory { get; set; }
		public string PostgreSql { get; set; }
	}

	public class Cors
	{
		public string[] Origins { get; set; } = new[] { "http://localhost:4200" };
	}

	public enum DbType
    {		
		SQL,
		LOCALDB,
		SQLLITE,
		MYSQL,
		INMEMORY,
        POSTGRESQL,
    }
}
