using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skillfactory.Module25.EntityFrameworkMSSQL
{
    public class FakeData
    {
        public List<Category> Categories { get; set; }
        public void CreateFakeData()
        {
            CreateCategories();
        }

        private void CreateCategories()
        {
            string[] categoriesNames = new string[] { "Fiction", "Science", "History", "Romance" };
            foreach (string catName in categoriesNames)
            {
                Categories.Add(new Category
                {
                    Name = catName,
                    BooksInCategory = new List<Book>()
                });
            }
        }
    }
}
