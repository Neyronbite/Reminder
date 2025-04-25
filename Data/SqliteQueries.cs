using Data.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Data
{
    public class SqliteQueries : BaseSqliteQueries
    {
        /// <summary>
        /// Gets days of specified month
        /// </summary>
        /// <param name="month"></param>
        /// <param name="year"></param>
        /// <returns></returns>
        public async Task<List<Day>> GetDays(int month, int year)
        {
            var query = @"SELECT * FROM days
                WHERE month = @month AND year = @year";
            var parameters = new List<(string name, object value)>()
            {
                ("@year", year),
                ("@month", month)
            };

            var result = await Get<Day>(query, parameters: parameters);
            return result;
        }

        /// <summary>
        /// Gets events by specified day params
        /// </summary>
        /// <param name="month"></param>
        /// <param name="year"></param>
        /// <param name="dayNum"></param>
        /// <returns></returns>
        public async Task<List<Event>> GetEvents(int month, int year, int dayNum)
        {
            var query = @"SELECT * FROM events
                WHERE month = @month AND year = @year AND day_num = @day_num";
            var parameters = new List<(string name, object value)>()
            {
                ("@year", year),
                ("@month", month),
                ("@day_num", dayNum)
            };

            var result = await Get<Event>(query, parameters: parameters);
            return result;
        }

        /// <summary>
        /// Creates a new entity or updates an existing one based on its Id.
        /// </summary>
        /// <typeparam name="T">Type of entity</typeparam>
        /// <param name="entity">Entity to insert or update</param>
        /// <returns>The inserted or updated entity</returns>
        public virtual async Task<T> CreateOrUpdate<T>(T entity) where T : IEntity
        {
            if (entity.Id == 0)
            {
                return await Create(entity);
            }
            else
            {
                return await Update(entity);
            }
        }

        /// <summary>
        /// Deletes the entity from the database if it has a valid Id.
        /// </summary>
        /// <typeparam name="T">Type of the entity</typeparam>
        /// <param name="entity">Entity to delete</param>
        public virtual async Task DeleteIfExists<T>(T entity) where T : IEntity
        {
            if (entity.Id != 0)
            {
                await Delete(entity);
            }
        }

        /// <summary>
        /// Creating databases tables
        /// </summary>
        /// <returns></returns>
        protected override async Task CreateDb()
        {
            var sqlScript = @"
            CREATE TABLE IF NOT EXISTS days (
                id INTEGER PRIMARY KEY AUTOINCREMENT,
                day_num INTEGER NOT NULL,
                year INTEGER NOT NULL,
                month INTEGER NOT NULL,
                day_of_week INTEGER NOT NULL,
                title TEXT,
                notes TEXT
            );

            CREATE TABLE IF NOT EXISTS events (
                id INTEGER PRIMARY KEY AUTOINCREMENT,
                day_id INTEGER,
                day_num INTEGER NOT NULL,
                year INTEGER NOT NULL,
                month INTEGER NOT NULL,
                hour INTEGER NOT NULL,
                minute INTEGER NOT NULL,
                enabled INTEGER NOT NULL,
                triggered INTEGER NOT NULL,
                title TEXT NOT NULL,
                CONSTRAINT fk_days
                FOREIGN KEY (day_id)
                REFERENCES days(day_id)
            );";
            await Execute(sqlScript);
        }
    }
}
