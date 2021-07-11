using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace DbHelper
{
    public interface IDbAccessHelper
    {
        SqlConnection GetConnection();
        Task<IEnumerable<T>> ReadDataAsync<T>(string query, object parameters = null, SqlTransaction transaction = null) where T : class;
        Task<int> InsertDataAsync(string query, object parameters, SqlTransaction transaction = null);
        Task<object> GetScalarAsync<T>(string query, object parameters, SqlTransaction transaction = null) where T: IConvertible;
        Task<int> UpdateAsync(string query, object parameters, SqlTransaction transaction = null);
        Task<int> DeleteAsync(string query, object parameters, SqlTransaction transaction = null);
    }
}
