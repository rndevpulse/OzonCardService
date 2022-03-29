using Microsoft.EntityFrameworkCore.Migrations;
using OzonCard.Common;
using OzonCard.Context.Interfaces;
using OzonCard.Data.Enums;

namespace OzonCard.Context.Repositories
{
    public abstract class BaseRepository
    {
        protected string ConnectionString { get; }
        protected IRepositoryContextFactory ContextFactory { get; }
        public BaseRepository(string connectionString, IRepositoryContextFactory contextFactory)
        {
            ConnectionString = connectionString;
            ContextFactory = contextFactory;
        }

       

    }
}
