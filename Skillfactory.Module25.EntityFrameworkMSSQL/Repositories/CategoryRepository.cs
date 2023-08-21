using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skillfactory.Module25.EntityFrameworkMSSQL.Repositories
{
    public class CategoryRepository : BaseRepository
    {
        public CategoryRepository() : base() { }

        public CategoryRepository(AppContext appContext) : base(appContext) { }

        public int CreateCategory(string categoryName)
        {
            DbContext.Categories.Add(new Category { Name = categoryName});
            return DbContext.SaveChanges();
        }
        public Category GetCategoryById(int id)
        {
            return DbContext.Categories.SingleOrDefault(c => c.Id == id);
        }

        public List<Category> GetAllCategories()
        {
            return DbContext.Categories.ToList();
        }

        public int UpdateCategoryName(int categoryId, string categoryNewName)
        {
            DbContext.Categories.SingleOrDefault(c => c.Id == categoryId).Name = categoryNewName;
            return DbContext.SaveChanges();
        }

        public int DeleteCategory(int id)
        {
            int result;
            using (var transactionContext = DbContext.Database.BeginTransaction())
            {
                Category categoryToDelete = DbContext.Categories.SingleOrDefault(c =>c.Id == id);
                foreach (Book book in categoryToDelete.BooksInCategory)
                {
                    book.CategoryId = null;
                    book.Category = null;
                }
                categoryToDelete.BooksInCategory.Clear();
                DbContext.Categories.Remove(categoryToDelete);
                result = DbContext.SaveChanges();
                transactionContext.Commit();
            }
            return result;
        }
    }
}
