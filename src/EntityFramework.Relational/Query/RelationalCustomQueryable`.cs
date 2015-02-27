// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Linq;
using System.Linq.Expressions;
using JetBrains.Annotations;
using Microsoft.Data.Entity.Utilities;
using Remotion.Linq;

namespace Microsoft.Data.Entity.Relational.Query
{    public class RelationalCustomQueryable<TEntity> : QueryableBase<TEntity>, IRelationalCustomQueryable
    {
        public string Sql { get; private set; }

        public RelationalCustomQueryable(string sql, [NotNull] IQueryProvider provider)
        : base(Check.NotNull(provider, nameof(provider)))
        {
            Sql = sql;
        }

        public RelationalCustomQueryable([NotNull] IQueryProvider provider, [NotNull] Expression expression)
        : base(
            Check.NotNull(provider, nameof(provider)),
            Check.NotNull(expression, nameof(expression)))
        {
        }

        public override string ToString()
        {
            return Sql;
        }
    }
}
