﻿using OzonCard.Context.Interfaces;

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
