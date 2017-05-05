using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataMapper
{
    public class MyDbConnection : IDbConnection
    {
        public MyDbConnection()
        {
            if (String.IsNullOrEmpty(ConnectionString))
                _connection = new SqlConnection();
            else
            {
                _connection = new SqlConnection(ConnectionString);
                _connection.Open();
            }

        }

        IDbConnection _connection;
        public MyDbConnection(IDbConnection connection)
        {
            _connection = connection;
            _connection.Open();
        }

        //public static IDbConnection ConnectUsing(this IDbConnection connection)
        //{
        //    connection.Open();
        //    return connection;
        //}

        //public static IDbConnection ConnectUsing(this IDbConnection connection, string connectionString)
        //{
        //    connection.ConnectionString = connectionString;
        //    return ConnectUsing(connection);
        //}

        public static IDbConnection ConnectUsing(IDbConnection connection)
        {
            connection.Open();
            return connection;
        }

        public static IDbConnection ConnectUsing(string connectionString)
        {
            return ConnectUsing(new SqlConnection(connectionString));
        }

        public string ConnectionString { get => _connection.ConnectionString; set => _connection.ConnectionString = value; }

        public int ConnectionTimeout => _connection.ConnectionTimeout;

        public string Database => _connection.Database;

        public ConnectionState State => _connection.State;

        IDbTransaction IDbConnection.
            BeginTransaction()
        {
            return _connection.BeginTransaction();
        }

        IDbTransaction IDbConnection.
            BeginTransaction(IsolationLevel il)
        {
            return _connection.BeginTransaction(il); ;
        }

        void IDbConnection
            .ChangeDatabase(string databaseName)
        {
            _connection.ChangeDatabase(databaseName);
        }

        void IDbConnection
            .Open()
        {
            _connection.Open();
        }

        void IDbConnection
            .Close()
        {
            _connection.Close();
        }

        IDbCommand IDbConnection
            .CreateCommand()
        {
            return _connection.CreateCommand();
        }

        public IDbCommand CreateCommand(string requeteSelect)
        {
            IDbCommand command;
            (command = ((IDbConnection)this).CreateCommand()).CommandText = requeteSelect;
            return command;
        }

        void IDisposable
            .Dispose()
        {
            _connection.Dispose();
        }

    }

    public static class IDbConnectionExtensions 
    {
        public static IDbConnection ConnectUsing(this IDbConnection connection)
        {
            connection.Open();
            return connection;
        }

        public static IDbConnection ConnectUsing(this IDbConnection connection, string connectionString)
        {
            connection.ConnectionString = connectionString;
            return ConnectUsing(connection);
        }

        public static IDbCommand CreateCommand(this IDbConnection connection, string query)
        {
            IDbCommand command = connection.CreateCommand();
            command.CommandText = query;
            return command;
        }
    }
}
