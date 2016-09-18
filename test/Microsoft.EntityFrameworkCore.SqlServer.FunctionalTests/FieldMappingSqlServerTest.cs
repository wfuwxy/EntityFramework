// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using Microsoft.EntityFrameworkCore.Specification.Tests;
using Microsoft.EntityFrameworkCore.SqlServer.FunctionalTests.Utilities;
using Microsoft.Extensions.DependencyInjection;

namespace Microsoft.EntityFrameworkCore.SqlServer.FunctionalTests
{
    public class FieldMappingSqlServerTest
        : FieldMappingTestBase<SqlServerTestStore, FieldMappingSqlServerTest.FieldMappingSqlServerFixture>
    {
        public FieldMappingSqlServerTest(FieldMappingSqlServerFixture fixture)
            : base(fixture)
        {
        }

        public class FieldMappingSqlServerFixture : FieldMappingFixtureBase
        {
            private const string DatabaseName = "FieldMapping";

            private readonly IServiceProvider _serviceProvider;

            public FieldMappingSqlServerFixture()
            {
                _serviceProvider = new ServiceCollection()
                    .AddEntityFrameworkSqlServer()
                    .AddSingleton(TestSqlServerModelSource.GetFactory(OnModelCreating))
                    .BuildServiceProvider();
            }

            public override SqlServerTestStore CreateTestStore()
            {
                return SqlServerTestStore.GetOrCreateShared(DatabaseName, () =>
                    {
                        var optionsBuilder = new DbContextOptionsBuilder()
                            .UseSqlServer(SqlServerTestStore.CreateConnectionString(DatabaseName))
                            .UseInternalServiceProvider(_serviceProvider);

                        using (var context = new FieldMappingContext(optionsBuilder.Options))
                        {
                            context.Database.EnsureClean();
                            Seed(context);
                        }
                    });
            }

            public override DbContext CreateContext(SqlServerTestStore testStore)
            {
                var optionsBuilder = new DbContextOptionsBuilder()
                    .UseSqlServer(testStore.Connection)
                    .UseInternalServiceProvider(_serviceProvider);

                var context = new FieldMappingContext(optionsBuilder.Options);
                context.Database.UseTransaction(testStore.Transaction);

                return context;
            }
        }
    }
}
