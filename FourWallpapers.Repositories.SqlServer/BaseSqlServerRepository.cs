using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading;
using System.Threading.Tasks;
using Dapper.Contrib.Extensions;
using FourWallpapers.Core.Database.Entities;
using FourWallpapers.Core.Database.Repositories;
using FourWallpapers.Core.Settings;

namespace FourWallpapers.Repositories.SqlServer
{
    public abstract class BaseSqlServerRepository<T> : IRepository<T> where T : BaseEntity, new()
    {
        protected readonly string ConnectionString;

        protected BaseSqlServerRepository(IDatabaseSettings db)
        {
            ConnectionString = db.ConnectionString;
        }

        protected BaseSqlServerRepository(string connectionString)
        {
            ConnectionString = connectionString;
        }

        public virtual async Task<Guid> AddAsync(T item, CancellationToken cancellationToken)
        {
            using (var dbConnection = Connection())
            {
                dbConnection.Open();
                item.Id = Guid.NewGuid();
                await dbConnection.InsertAsync(item);

                return item.Id;
            }
        }

        public virtual async Task<IEnumerable<T>> FindAllAsync(CancellationToken cancellationToken)
        {
            using (var dbConnection = Connection())
            {
                dbConnection.Open();
                return await dbConnection.GetAllAsync<T>();
            }
        }

        public virtual async Task<T> FindByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            using (var dbConnection = Connection())
            {
                dbConnection.Open();
                return await dbConnection.GetAsync<T>(id);
            }
        }

        public virtual async Task<bool> RemoveAsync(Guid id, CancellationToken cancellationToken)
        {
            using (var dbConnection = Connection())
            {
                dbConnection.Open();

                var result = await dbConnection.DeleteAsync(new T {Id = id});

                return result;
            }
        }

        public virtual async Task<bool> UpdateAsync(T item, CancellationToken cancellationToken)
        {
            using (var dbConnection = Connection())
            {
                dbConnection.Open();
                var results = await dbConnection.UpdateAsync(item);
                return results;
            }
        }

        protected IDbConnection Connection()
        {
            return new SqlConnection(ConnectionString);
        }
    }
}