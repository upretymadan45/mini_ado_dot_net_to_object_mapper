using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace DbHelper
{
    public class DbAccessHelper : IDbAccessHelper
    {
        private readonly string connString = @"Server = localhost; user=sa; password=password; database=adodotnettoobjectmapper";
        public SqlConnection GetConnection()
        {
            return new SqlConnection(connString);
        }

        public async Task<IEnumerable<T>> ReadDataAsync<T>(string query, object parameters = null, SqlTransaction transaction=null) where T:class
        {
            try
            {
                using (var conn = GetConnection())
                {
                    conn.Open();
                    var command = new SqlCommand(query, conn, transaction);
                    AddParameters(parameters, command);

                    var reader = await command.ExecuteReaderAsync();
                    List<T> dataList = await MapDataToObject<T>(reader);

                    return dataList;
                }
            }
            catch (Exception) { throw; }
        }
        public async Task<int> InsertDataAsync(string query, object parameters, SqlTransaction transaction = null)
        {
            return await ExecuteNonQueryAsync(query, parameters, transaction);
        }
        public async Task<object> GetScalarAsync<T>(string query, object parameters, SqlTransaction transaction = null) where T: IConvertible
        {
            try
            {
                using (var conn = GetConnection())
                {
                    conn.Open();
                    var command = new SqlCommand(query, conn, transaction);
                    AddParameters(parameters, command);

                    var insertedId = await command.ExecuteScalarAsync();
                    return (T)Convert.ChangeType(insertedId, typeof(T));
                }
            }
            catch (Exception) { throw; }
        }

        public async Task<int> UpdateAsync(string query, object parameters, SqlTransaction transaction = null)
        {
            return await ExecuteNonQueryAsync(query, parameters, transaction);
        }

        public async Task<int> DeleteAsync(string query, object parameters, SqlTransaction transaction = null)
        {
            return await ExecuteNonQueryAsync(query, parameters, transaction);
        }
        private async Task<List<T>> MapDataToObject<T>(SqlDataReader reader) where T : class
        {
            var dataList = new List<T>();
            var props = typeof(T).GetProperties();

            while (await reader.ReadAsync())
            {
                var tObject = Activator.CreateInstance(typeof(T));
                foreach (var prop in props)
                {
                    try
                    {
                        prop.SetValue(tObject, reader[prop.Name]);
                    }
                    catch (Exception)
                    {
                        continue;
                    }
                }

                dataList.Add(tObject as T);
            }

            return dataList;
        }

        private void AddParameters(object parameters, SqlCommand command)
        {
            if (parameters != null)
            {
                var param = parameters.GetType().GetProperties();
                foreach (var p in param)
                {
                    command.Parameters.AddWithValue($"@{p.Name}", p.GetValue(parameters));
                }
            }
        }

        private async Task<int> ExecuteNonQueryAsync(string query, object parameters, SqlTransaction transaction = null)
        {
            try
            {
                using (var conn = GetConnection())
                {
                    conn.Open();
                    var command = new SqlCommand(query, conn, transaction);
                    AddParameters(parameters, command);

                    return await command.ExecuteNonQueryAsync();
                }
            }
            catch (Exception) { throw; }
        }
    }
}
