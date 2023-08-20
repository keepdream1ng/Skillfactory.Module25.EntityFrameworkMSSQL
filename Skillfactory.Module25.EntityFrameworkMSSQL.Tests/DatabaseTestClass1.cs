using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skillfactory.Module25.EntityFrameworkMSSQL.Tests
{
    [Collection("Database collection")]
    public class DatabaseTestClass1
    {
        DatabaseFixture fixture;

        public DatabaseTestClass1(DatabaseFixture fixture)
        {
            this.fixture = fixture;
        }

        [Fact]
        public void DummyTest()
        {
            Assert.True(true);
        }
    }
}
