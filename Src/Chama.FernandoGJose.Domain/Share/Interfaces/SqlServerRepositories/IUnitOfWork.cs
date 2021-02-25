using System;
using System.Data;

namespace Chama.FernandoGJose.Domain.Share.Interfaces.SqlServerRepositories
{
    public interface IUnitOfWork : IDisposable
    {
        void BeginTransaction();

        void CommitTransaction();

        IDbConnection Connection { get; }

        IDbTransaction Transaction { get; }
    }
}
