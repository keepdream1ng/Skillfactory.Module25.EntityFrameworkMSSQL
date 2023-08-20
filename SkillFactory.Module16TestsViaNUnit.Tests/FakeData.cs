using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkillFactory.Module16TestsViaNUnit.Tests
{
    public static class FakeData
    {
        public static List<Category> Categories { get; set; }
        public static List<Author> Authors { get; set; }
        public static List<User> Users { get; set; }
        public static List<Book> Books { get; set; }

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
