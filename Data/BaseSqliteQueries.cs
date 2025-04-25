using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SQLite;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data.Entities;
using System.IO;
using Data.Helpers;

namespace Data
{
    public abstract class BaseSqliteQueries
    {
        protected string DbPath { get; set; }
        protected string ConnectionString { get; set; }

        protected BaseSqliteQueries()
        {
            // Define DB file path
            var documents = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            DbPath = Path.Combine(documents, "reminder.db");

            // Connection string
            ConnectionString = $"Data Source={DbPath};Version=3;";

            // Create file if it doesn't exist
            if (!File.Exists(DbPath))
            {
                SQLiteConnection.CreateFile(DbPath);
            }
            // And create db tables
            Task.WaitAll(CreateDb());
        }

        /// <summary>
        /// Deletes entity from db
        /// </summary>
        /// <typeparam name="T">Type of entity</typeparam>
        /// <param name="entity">Entity that will be deleted</param>
        /// <returns></returns>
        public virtual async Task Delete<T>(T entity) where T : IEntity
        {
            var type = typeof(T);
            string delQuery = $"DELETE FROM {type.Name.PascalToSnake()}s WHERE id = @id";


            var parameters = new List<(string name, object value)>
            {
                ("@id", entity.Id)
            };

            await Execute(delQuery, parameters);
        }

        /// <summary>
        /// Updates enitity
        /// </summary>
        /// <typeparam name="T">Type of entity</typeparam>
        /// <param name="entity">Entity that will be updated</param>
        /// <returns></returns>
        public virtual async Task<T> Update<T>(T entity) where T : IEntity
        {
            // example
            // UPDATE table_name
            // SET column1 = value1, column2 = value2, ...
            // WHERE condition;
            try
            {
                // main query stringbulder that will be executed
                var query = new StringBuilder();
                var parameters = new List<(string name, object value)>();

                // getting type of entity and properties
                var type = typeof(T);
                var propertyInfos = type.GetProperties();

                // getting everything needed for query
                var props = propertyInfos.Skip(1).ToArray(); // skip the ID
                var columnNames = props.Select(p => p.Name.PascalToSnake());
                var equals = columnNames.Select(c => $"{c} = @{c}");

                // build the full insert
                query.Append($"UPDATE {type.Name.PascalToSnake()}s ");
                query.Append($"\n SET {string.Join(", ", equals)}");
                query.Append($"\nWHERE id = @id");

                parameters = props.Select(p => ("@" + p.Name.PascalToSnake(), p.GetValue(entity))).ToList();
                parameters.Add(("@id", entity.Id)); // Add the ID parameter separately for clarity

                // finaly executing query and updating id
                await ExecQueryAsync<object>(query.ToString(), s => s.ExecuteScalarAsync, parameters: parameters);

                return entity;
            }
            catch (Exception e)
            {
                // TODO log
                throw e;
            }
        }

        /// <summary>
        /// Inserts entity into db
        /// </summary>
        /// <typeparam name="T">Type of entity</typeparam>
        /// <param name="entity">Entity that will be inserted</param>
        /// <returns></returns>
        public virtual async Task<T> Create<T>(T entity) where T : IEntity
        {
            // example
            // INSERT INTO table_name (column1, column2, column3, ...)
            // VALUES(value1, value2, value3, ...);
            try
            {
                // main query stringbulder that will be executed
                var query = new StringBuilder();
                var parameters = new List<(string name, object value)>();

                // getting type of entity and properties
                var type = typeof(T);
                var propertyInfos = type.GetProperties();

                // getting everything needed for query
                var props = propertyInfos.Skip(1).ToArray(); // skip the ID
                var columnNames = props.Select(p => p.Name.PascalToSnake());
                var paramNames = columnNames.Select(n => "@" + n);

                // build the full insert
                query.Append($"INSERT INTO {type.Name.PascalToSnake()}s ");
                query.Append($"({string.Join(", ", columnNames)}) ");
                query.Append($"\nVALUES ({string.Join(", ", paramNames)});");
                // append last_insert_rowid()
                query.Append("\nSELECT last_insert_rowid();");

                parameters = props.Select(p => ("@" + p.Name.PascalToSnake(), p.GetValue(entity))).ToList();

                // finaly executing query and updating id
                await ExecQueryAsync<object>(query.ToString(), s => s.ExecuteScalarAsync, parameters: parameters, workWithResult: async r =>
                {
                    entity.Id = Convert.ToInt64(r);
                });

                return entity;
            }
            catch (Exception e)
            {
                // TODO log this part
                throw e;
            }
        }

        /// <summary>
        /// Function that designed to execute read queries and return list of entities
        /// </summary>
        /// <typeparam name="T">Entity type</typeparam>
        /// <param name="query">Read query as string</param>
        /// <returns></returns>
        public virtual async Task<List<T>> Get<T>(string query,
            Func<DbDataReader, T> mapper = null,
            List<(string name, object value)> parameters = null) where T : IEntity, new()
        {
            var entities = new List<T>();

            await ExecQueryAsync<DbDataReader>(query, c => c.ExecuteReaderAsync, async reader =>
            {
                try
                {
                    while (await reader.ReadAsync())
                    {
                        if (mapper != null)
                        {
                            var entityMapped = mapper(reader);
                            entities.Add(entityMapped);
                            continue;
                        }

                        // Getting current row values
                        var dataRecord = (IDataRecord)reader;
                        object[] recordValues = new object[reader.FieldCount];
                        dataRecord.GetValues(recordValues);

                        // Creating an instance of T and getting it Type to fill properties
                        var entity = new T();
                        var entityType = entity.GetType();
                        var entityProps = entityType.GetProperties();

                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            var columnName = reader.GetName(i);
                            var prop = entityProps.FirstOrDefault(p => p.Name.Equals(columnName.SnakeToPascal(), StringComparison.OrdinalIgnoreCase));
                            if (prop != null && recordValues[i] != DBNull.Value)
                            {
                                var value = Convert.ChangeType(recordValues[i], prop.PropertyType);
                                prop.SetValue(entity, value);
                            }
                        }

                        entities.Add(entity);
                    }
                }
                catch (Exception e)
                {
                    //TODO log
                    throw e;
                }
            }, parameters: parameters);

            return entities;
        }

        /// <summary>
        /// Function that designed to execute create/update/delete queries without returning anything
        /// </summary>
        /// <param name="query">Create/update/dlete query as string</param>
        /// <param name="parameters"></param>
        /// <param name="workWithCommand"></param>
        /// <returns></returns>
        protected internal virtual async Task Execute(string query,
            List<(string name, object value)> parameters = null,
            Func<SQLiteCommand, Task> workWithCommand = null)
        {
            await ExecQueryAsync<int>(query, c => c.ExecuteNonQueryAsync, parameters: parameters, workWithCommand: workWithCommand);
        }

        /// <summary>
        /// Base quering function
        /// </summary>
        /// <typeparam name="T">Type that will be returned by query</typeparam>
        /// <param name="query">Query as string</param>
        /// <param name="asyncChooseFunction">Choose function, ExecuteReaderAsync, ExecuteNonQueryAsync ...</param>
        /// <param name="workWithResult">Delegate that will be invoced after db provides result</param>
        /// <param name="parameters">Parameters for query</param>
        /// <param name="workWithCommand">Invokes before main query call. Provides SQLiteCommand to you, so you can change something</param>
        /// <returns></returns>
        protected internal virtual async Task ExecQueryAsync<T>(string query,
            Func<SQLiteCommand, Func<Task<T>>> asyncChooseFunction,
            Func<T, Task> workWithResult = null,
            List<(string name, object value)> parameters = null,
            Func<SQLiteCommand, Task> workWithCommand = null)
        {
            try
            {
                // opening connection to db
                using (var connection = new SQLiteConnection(ConnectionString))
                {
                    connection.Open();

                    using (var command = new SQLiteCommand(query, connection))
                    {
                        // invoking command delegate
                        if (workWithCommand != null)
                        {
                            await workWithCommand(command);
                        }

                        // setting parameters
                        if (parameters != null)
                        {
                            foreach (var par in parameters)
                            {
                                command.Parameters.AddWithValue(par.name, par.value);
                            }
                        }

                        // identifying function and calling
                        var c = asyncChooseFunction(command);
                        var result = await c();

                        // invoking result callback delegate
                        if (workWithResult != null)
                        {
                            await workWithResult(result);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                // TODO Log
                throw e;
            }
        }

        /// <summary>
        /// Creating databases tables
        /// </summary>
        /// <returns></returns>
        protected abstract Task CreateDb();
    }
}
