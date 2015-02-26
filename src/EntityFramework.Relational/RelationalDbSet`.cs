// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using Microsoft.Data.Entity.Infrastructure;
using Microsoft.Data.Entity.Utilities;

using Microsoft.Framework.DependencyInjection;

namespace Microsoft.Data.Entity.Relational
{
    public class RelationalDbSet<TEntity> where TEntity : class
    {
        private readonly DbSet<TEntity> _dbSet;
        private readonly RelationalDatabase _database;

        public RelationalDbSet([NotNull]DbSet<TEntity> dbSet)
        {
            Check.NotNull(dbSet, nameof(dbSet));

            _dbSet = dbSet;
            _database = (dbSet as IAccessor<IServiceProvider>).Service.GetService<RelationalDatabase>();
        }

        public virtual IEnumerable<TEntity> Query([NotNull]string sql, params object[] parameters)
        {


            throw new NotImplementedException();
        }

        public virtual IEnumerable<TEntity> UntrackedQuery([NotNull]string sql, params object[] parameters)
        {
            throw new NotImplementedException();
        }
    }
}
