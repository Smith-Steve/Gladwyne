using Microsoft.Data;
using Microsoft.Data.SqlClient;
using Gladwyne.API.Data.Interfaces;
using Microsoft.Data.SqlClient;
using System.Data;

namespace Gladwyne.API.Data.DataProviders
{
    public sealed class SqlDataProvider : Gladwyne.API.Data.Interfaces.IDataProvider
    {
        private readonly string connectionString;
        private readonly IConfiguration _configuration;

        public SqlDataProvider(string connectionString, IConfiguration configuration)
        {
            this.connectionString = connectionString;
            _configuration = configuration;
        }

        // Get Connectio
        public void ExecuteCmd(string storedProc, Action<SqlParameterCollection> inputParamMapper,
            Action<IDataReader, short> map, Action<SqlParameterCollection> returnParameters = null,
            Action<SqlCommand> cmdModifier = null
        )
        {
            if(map == null) throw new NullReferenceException("Object Mapper Is Required");

            //defining Data
            SqlDataReader reader = null; 
            SqlCommand command = null;
            SqlConnection connection = null;
            short result = 0;
            try
            {
                using (connection = GetConnection())
                {
                    //Test For Connection
                    if (connection != null)
                    {
                        //We're testing for the connection state.
                        //If the connection is not open, we open it.
                        if(connection.State != ConnectionState.Open) connection.Open();

                        //We're generating our command.
                        command = GetCommand(connection);
                    }
                }
            }
            finally
            {
                if(reader != null && !reader.IsClosed)
                {
                    reader.Close();
                }
                if(connection != null && connection.State != ConnectionState.Open)
                {
                    connection.Close();
                }
            }
        }

        //Execute Non-Query
        public int ExecuteNonQuery(string storedProc, Action<SqlParameterCollection> paramMapper, Action<SqlParameterCollection> returnParameters = null)
        {
            //Parameters
            // string - storedProc (The Stored Procedure)
            // ParamMapper - parameters.
            // returnedParameters.
            SqlCommand command = null;
            SqlConnection connection = null;
            try
            {
                using(connection = GetConnection())
                {
                    if(connection != null)
                    {
                        if(connection.State != ConnectionState.Open)
                        {
                            connection.Open();
                        }

                        command = GetCommand(connection, storedProc, paramMapper);
                        if(command != null)
                        {
                            int returnValue = command.ExecuteNonQuery();

                            if(connection.State != ConnectionState.Closed)
                            {
                                connection.Close();
                            }

                            if(returnParameters != null)
                            {
                                returnParameters(command.Parameters);
                            }
                            return returnValue;
                        }
                    }
                }
            }
            finally
            {
                if(connection != null && connection.State != ConnectionState.Closed)
                {
                    connection.Close();
                }
            }
            return -1;
        }
        //End - Execute Non-Query
        private SqlConnection GetConnection()
        {
            return new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));
        }

        //Execute Command

        // Sql Get Command
        private SqlCommand GetCommand(SqlConnection connection, string cmdText = null, Action<SqlParameterCollection> paramMapper = null)
        {
            SqlCommand command = null;
            //if connection is open.
            if (connection != null) command = connection.CreateCommand();

            if(command != null)
            {
                if(!String.IsNullOrEmpty(cmdText))
                {
                    command.CommandText = cmdText;
                    command.CommandType = CommandType.StoredProcedure;
                }

                //test for paramMapper
                if(paramMapper != null) paramMapper(command.Parameters);
            }

            //return
            return command;
        }

        private IDbCommand GetCommand(IDbConnection conn, string cmdText = null, Action<IDataParameterCollection> paramMapper = null)
        {
            IDbCommand cmd = null;

            if (conn != null)
                cmd = conn.CreateCommand();

            if (cmd != null)
            {
                if (!String.IsNullOrEmpty(cmdText))
                {
                    cmd.CommandText = cmdText;
                    cmd.CommandType = CommandType.StoredProcedure;
                }

                if (paramMapper != null)
                    paramMapper(cmd.Parameters);
            }

            return cmd;
        }
    }
}