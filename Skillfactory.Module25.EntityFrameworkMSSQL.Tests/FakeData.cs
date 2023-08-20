using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Skillfactory.Module25.EntityFrameworkMSSQL; 

namespace Skillfactory.Module25.EntityFrameworkMSSQL.Tests
{
    public static class FakeData
    {
        public static List<Category> Categories { get; set; } = new List<Category>();
        public static List<Author> Authors { get; set; } = new List<Author>();
        public static List<User> Users { get; set; } = new List<User>();
        public static List<Book> Books { get; set; } = new List<Book>();

        private static DataGenerator _data = new DataGenerator();
        public static void CreateFakeData(int multiplier = 1)
        {
            CreateCategories();
            Authors.AddRange(_data.authorFaker.Generate(20 * multiplier));
            Users.AddRange(_data.userFaker.Generate(50 * multiplier));
            Books.AddRange(_data.bookFaker.Generate(200 * multiplier));
        }

        private static void CreateCategories()
        {
            string[] categoriesNames = new string[] { "Fiction", "Science", "History", "Romance" };
            var categories = categoriesNames.Select(c => new Category { Name = c });
            Categories.AddRange(categories);
        }
    }
}
