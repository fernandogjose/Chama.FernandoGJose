using Chama.FernandoGJose.Domain.Share.Interfaces.SqlServerRepositories;
using System;
using System.Data;

namespace Chama.FernandoGJose.SqlServer.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        public IDbConnection Connection { get; }

        public IDbTransaction Transaction { get; private set; }

        public UnitOfWork(IDbConnection connection)
        {
            Connection = connection;
        }

        public void BeginTransaction()
        {
            if (Transaction == null)
            {
                if (Connection.State == ConnectionState.Closed) Connection.Open();
                Transaction = Connection.BeginTransaction(IsolationLevel.ReadCommitted);
            }
        }

        public void CommitTransaction()
        {
            if (Transaction != null)
            {
                Transaction.Commit();
                Dispose();
            }
        }

        public void Dispose()
        {
            if (Transaction != null)
            {
                Transaction.Dispose();
                Transaction = null;

                Connection.Close();
                Connection.Dispose();
            }

            GC.SuppressFinalize(this);
        }
    }
}
