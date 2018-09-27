using System;

namespace Login
{
    internal class SqlDbConnection
    {
        public SqlDbConnection()
        {
        }

        public string ConnectionString { get; internal set; }

        internal void Open()
        {
            throw new NotImplementedException();
        }
    }
}