// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Linq;
using JetBrains.Annotations;
using Microsoft.Data.Entity.Infrastructure;
using Microsoft.Data.Entity.Query;
using Microsoft.Data.Entity.Relational.Query;
using Microsoft.Data.Entity.Storage;
using Microsoft.Data.Entity.Utilities;
using Microsoft.Framework.DependencyInjection;

namespace Microsoft.Data.Entity.Relational
{
    public class RelationalDbSet<TEntity> where TEntity : class
    {
        private readonly IServiceProvider _serviceProvider;

        public RelationalDbSet([NotNull]DbSet<TEntity> dbSet)
        {
            Check.NotNull(dbSet, nameof(dbSet));

            _serviceProvider = ((IAccessor<IServiceProvider>)dbSet).Service;
        }

        public virtual IQueryable<TEntity> Query([NotNull]string sql, params object[] parameters)
        {
            return new RelationalCustomQueryable<TEntity>(sql,
                new RelationalCustomQueryProvider(
                    _serviceProvider.GetRequiredServiceChecked<DbContextService<DbContext>>(),
                    _serviceProvider.GetRequiredServiceChecked<DbContextService<DataStore>>(),
                    _serviceProvider.GetRequiredServiceChecked<ICompiledQueryCache>()));
        }
    }
}
