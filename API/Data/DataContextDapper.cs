using System.Data;
using Dapper;
using Microsoft.Data.SqlClient;

namespace Gladwyne.API.Data
{
    class DataContextDapper
    {
        private readonly IConfiguration _configuration;
        public DataContextDapper(IConfiguration configuration)
        {
            //Data Context Constructor.
            _configuration = configuration;
        }

        //Load Data - Get All
        public IEnumerable<T> LoadData<T>(string sql)
        {
            IDbConnection dbConnection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));
            return dbConnection.Query<T>(sql);
        }

        //Load Data Single
        public T LoadDataSingle<T>(string sql)
        {
            IDbConnection dbConnection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));
            return dbConnection.QuerySingle<T>(sql);
        }

        //
        public bool ExecuteSql(string sql)
        {
            IDbConnection dbConnection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));
            return dbConnection.Execute(sql) > 0;
        }

        public int ExecuteSqlWithRowCount(string sql)
        {
            IDbConnection dbConnection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));
            return dbConnection.Execute(sql);
        }

        public bool ExecuteSqlWithParameters(string sql, List<SqlParameter> parameters)
        {
            SqlCommand commandWithParameters = new SqlCommand(sql);
            foreach(SqlParameter parameter in parameters)
            {
                commandWithParameters.Parameters.Add(parameter);
            }
            SqlConnection dbConnection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));
            dbConnection.Open();
            commandWithParameters.Connection = dbConnection;

            int rowsAffected = commandWithParameters.ExecuteNonQuery();
            dbConnection.Close();

            return rowsAffected > 0;
        }
    }
}