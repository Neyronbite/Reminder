namespace Data
{
    public static class StaticDb
    {
        public static SqliteQueries SqliteQueries
        {
            get
            {
                if (sqliteQueries == null)
                {
                    sqliteQueries = new SqliteQueries();
                }
                return sqliteQueries;
            }
        }
        private static SqliteQueries sqliteQueries;
    }
}
