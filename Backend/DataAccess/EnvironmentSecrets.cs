namespace DataAccess
{
    public class EnvironmentSecrets
    {
        private string _dbConnectionString = "Server=(localdb)\\mssqllocaldb;Database=TFE;Trusted_Connection=True;MultipleActiveResultSets=true";

        public string getDbConnectionString()
        {
            return _dbConnectionString;
        }
    }
}
