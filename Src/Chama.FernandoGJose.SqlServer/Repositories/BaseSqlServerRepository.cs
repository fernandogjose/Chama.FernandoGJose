using Chama.FernandoGJose.Domain.Share.Interfaces.SqlServerRepositories;
using System;

namespace Chama.FernandoGJose.SqlServer.Repositories
{
    public abstract class BaseSqlServerRepository : IDisposable
    {
        protected readonly string _connectionString;
        protected readonly IUnitOfWork _unitOfWork;

        public BaseSqlServerRepository(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public void Dispose()
        {
            if (_unitOfWork != null)
            {
                _unitOfWork.Connection.Close();
                _unitOfWork.Connection.Dispose();
            }
        }
    }
}