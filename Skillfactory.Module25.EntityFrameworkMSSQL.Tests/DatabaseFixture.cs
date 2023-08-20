using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Skillfactory.Module25.EntityFrameworkMSSQL;

namespace Skillfactory.Module25.EntityFrameworkMSSQL.Tests
{
    public class DatabaseFixture : IDisposable
    {
        public AppContext DbContext { get; private set; }
        public DatabaseFixture()
        {
            DbContext = new AppContext();
            DbContext.Database.EnsureDeleted();
            DbContext.Database.EnsureCreated();

            FakeData.CreateFakeData();

            DbContext.Categories.AddRange(FakeData.Categories);
            DbContext.Authors.AddRange(FakeData.Authors);
            DbContext.Users.AddRange(FakeData.Users);
            DbContext.Books.AddRange(FakeData.Books);

            DbContext.SaveChanges();
            // ... initialize data in the test database ...
        }

        public void Dispose()
        {
            DbContext.Dispose();
            // ... clean up test data from the database ...
        }
    }

    [CollectionDefinition("Database collection")]
    public class DatabaseCollection : ICollectionFixture<DatabaseFixture>
    {
        // This class has no code, and is never created. Its purpose is simply
        // to be the place to apply [CollectionDefinition] and all the
        // ICollectionFixture<> interfaces.
    }
}
